using System;
using System.IO;
using System.Reflection;
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
                //var session = documentStore.OpenSession();
                //var haveAnyChat = session.Query<Chat>().Any();
                //if (!haveAnyChat)
                //{
                //    var firstChat = new Chat()
                //    {
                //        Id = Guid.NewGuid().ToString(),
                //        ChatName = "SkillBoxChat",
                //        ChatType = ChatType.Public,
                //        OwnerId = ""
                //    };
                //    session.Store(firstChat);
                //    session.SaveChanges();
                //}

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
