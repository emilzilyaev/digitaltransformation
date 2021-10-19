using System;
using ServiceStack;
using RecomendationForStartups.ServiceModel;

namespace RecomendationForStartups.ServiceInterface
{
    public class MyServices : Service
    {
        public object Any(Hello request)
        {
            return new HelloResponse { Result = $"Hello, {request.Name}!" };
        }
    }
}
