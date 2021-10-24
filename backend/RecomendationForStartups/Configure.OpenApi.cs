using ServiceStack;
using ServiceStack.Api.OpenApi;

namespace RecomendationForStartups
{
    public class ConfigureOpenApi : IConfigureAppHost
    {
        public void Configure(IAppHost appHost)
        {
            appHost.Plugins.Add(new OpenApiFeature());
        }
    }
}