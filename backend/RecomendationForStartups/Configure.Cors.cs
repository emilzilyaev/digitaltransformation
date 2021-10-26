using ServiceStack;

namespace RecomendationForStartups
{
    public class ConfigureCors : IConfigureAppHost
    {
        public void Configure(IAppHost appHost)
        {
            appHost.Plugins.Add(new CorsFeature());
        }
    }
}