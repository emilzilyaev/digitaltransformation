using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ServiceStack;
using ServiceStack.DataAnnotations;
using Raven.Client.Documents;
using Raven.Client.Documents.Conventions;
using Raven.Client.Documents.Operations.Revisions;
using Raven.Client.Documents.Session;
using Raven.Client.ServerWide;
using Raven.Client.ServerWide.Operations;
using Raven.Embedded;
using RecomendationForStartups.Domain;

namespace RecomendationForStartups
{
    public static class StartupExtensions
    {
        public static void ConfigureRevisions(this IDocumentStore store)
        {
            store.Maintenance.Send(new ConfigureRevisionsOperation(new RevisionsConfiguration
            {
                Default = new RevisionsCollectionConfiguration
                {
                    Disabled = false,
                    PurgeOnDelete = false,
                }
            }));
        }

        public static bool IsRunning(this EmbeddedServer instance)
        {
            // HACK: check RavenDB running state using private field to avoid multiple starts
            // https://github.com/ravendb/ravendb/blob/release/v4.2/src/Raven.Embedded/EmbeddedServer.cs#L45-L46
            const BindingFlags bindingFlags = BindingFlags.NonPublic | BindingFlags.Instance;
            var result = instance.GetType().GetField("_serverTask", bindingFlags)?.GetValue(instance);
            return result != null;
        }
    }

    public class ConfigureRavenDb : IConfigureServices
    {
        IConfiguration Configuration { get; }
        public ConfigureRavenDb(IConfiguration configuration) => Configuration = configuration;

        public void Configure(IServiceCollection services)
        {
            services.AddSingleton<IHaveVersions, DotNetCorePackageList>();
            services.AddSingleton<DotNetVersionHelper>();
            services.AddSingleton<DatabaseRecord>(serviceProvider =>
            {
                var dbName = Configuration.GetSection("RavenDb:DatabaseRecord").GetValue<string>("DatabaseName");
                var dbRecord = new DatabaseRecord(dbName);
                return dbRecord;
            });
            services.AddSingleton<DatabaseOptions>(serviceProvider =>
            {
                var dbRecord = serviceProvider.GetRequiredService<DatabaseRecord>();
                var dbOptions = new DatabaseOptions(dbRecord);

                //������ ������ ������ ����� ���������
                dbOptions.Conventions = new DocumentConventions
                {
                    FindCollectionName = type => type.Name,
                };

                dbOptions.Conventions.FindIdentityProperty = p => {
                    var attr = p.DeclaringType.FirstAttribute<IndexAttribute>(); // Allow overriding 'Id' Identity property
                    return attr != null
                        ? p.Name == attr.Name
                        : p.Name == "Id";
                };

                return dbOptions;
            });
            services.AddSingleton<ServerOptions>(serviceProvider =>
            {
                var environment = serviceProvider.GetRequiredService<IHostEnvironment>();
                var serverOptions = Configuration.GetSection("RavenDb:ServerOptions").Get<ServerOptions>();

                //���� ���������� ������ DotNet ��� ������� �������� RavenDB
                try
                {
                    serverOptions.FrameworkVersion = serviceProvider.GetRequiredService<DotNetVersionHelper>()
                        .GetNearestDotNetVersion(serverOptions.FrameworkVersion);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }

                //�������� ���� ������� ����������, ��� ��� RavenDB � Embeddeed ������ ����������� ����� ��������� ������
                //������������� ��� �������� � ������ ����, ������ ����� ������ � ����� Temp
                //����� ����� �������� ���������� ����������� ����
                var path = environment.ContentRootPath;
                //���������� ���� � ����
                serverOptions.DataDirectory = Path.Combine(path, serverOptions.DataDirectory);
                //���������� ���� � �����
                serverOptions.LogsPath = Path.Combine(path, serverOptions.LogsPath);

                return serverOptions;
            });
            services.AddSingleton<EmbeddedServer>(serviceProvider =>
            {
                if (!EmbeddedServer.Instance.IsRunning())
                {
                    var serverOptions = serviceProvider.GetRequiredService<ServerOptions>();
                    EmbeddedServer.Instance.StartServer(serverOptions);
                }

                return EmbeddedServer.Instance;
            });
            services.AddSingleton<IDocumentStore>(serviceProvider =>
            {
                //��������� ������
                var embeddedServer = serviceProvider.GetRequiredService<EmbeddedServer>();

                var databaseOptions = serviceProvider.GetRequiredService<DatabaseOptions>();

                //�������� ����, ���� �� ����������
                var documentStore = embeddedServer.GetDocumentStore(databaseOptions);
                documentStore.Initialize();

                //�������� ��������� ����
                var databaseRecord = documentStore.Maintenance.Server.Send(new GetDatabaseRecordOperation(documentStore.Database));

                //���� �������� ������� ���
                if (databaseRecord.Revisions == null)
                {
                    documentStore.ConfigureRevisions();
                }
                var session = documentStore.OpenSession();
                var haveAnyParameterDefinition = session.Query<ParameterDefinition>().Any();
                if (!haveAnyParameterDefinition)
                {
                    Console.WriteLine("ParameterDefinition not found!");
                    var dataCatalog = new DirectoryInfo("Data");
                    if (dataCatalog.Exists)
                    {
                        Console.WriteLine($"{dataCatalog.FullName} is exist");
                        dataCatalog = dataCatalog.GetDirectories().First(i => i.Name == "Parameters");
                        foreach (var fileInfo in dataCatalog.EnumerateFiles())
                        {
                            Console.WriteLine($"Read {fileInfo.FullName} reading...");
                            var lines = fileInfo.ReadAllText().Split("\n").Select(s => s.Trim())
                                .Where(s => !string.IsNullOrWhiteSpace(s)).ToList();
                            var nameSplit = fileInfo.Name.Split(".");
                            var parameterDefinition = new ParameterDefinition()
                            {
                                Id = nameSplit[0],
                                AcceptableValues = lines,
                                Type = nameSplit[1] == "1"? ParameterType.OneAcceptable: ParameterType.MultiAcceptable,
                            };

                            Console.WriteLine($"Read {fileInfo.FullName} parsed {lines.Count}");
                            session.Store(parameterDefinition);
                        }
                        session.SaveChanges();
                    }
                }

                var haveAnyRecommendation = session.Query<Recommendation>().Any();
                if (!haveAnyRecommendation)
                {
                    var dataCatalog = new DirectoryInfo("Data");
                    if (dataCatalog.Exists)
                    {
                        Console.WriteLine($"{dataCatalog.FullName} is exist");
                        dataCatalog = dataCatalog.GetDirectories().First(i => i.Name == "Recommendation");
                        foreach (var fileInfo in dataCatalog.EnumerateFiles())
                        {
                            Console.WriteLine($"Read {fileInfo.FullName} reading...");
                            var lines = fileInfo.ReadAllText().Split("\n").Select(s => s.Trim())
                                .Where(s => !string.IsNullOrWhiteSpace(s)).ToList();
                            foreach (var line in lines)
                            {
                                var match = Regex.Match(line, "(?<id>\\d+)\\s+(?<name>.+)");
                                var recommendation = new Recommendation
                                {
                                    Id = match.Groups["id"].Value,
                                    Description = match.Groups["name"].Value.Trim()
                                };
                                session.Store(recommendation);
                            }
                        }
                        session.SaveChanges();
                    }
                }

                return documentStore;
            });
            services.AddTransient<IDocumentSession>(serviceProvider =>
            {
                var session = serviceProvider
                    .GetRequiredService<IDocumentStore>()
                    .OpenSession();
                return session;
            });
            services.AddTransient<IAsyncDocumentSession>(serviceProvider =>
            {
                var session = serviceProvider
                    .GetRequiredService<IDocumentStore>()
                    .OpenAsyncSession();
                return session;
            });
        }
    }    
}
