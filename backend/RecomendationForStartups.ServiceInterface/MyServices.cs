using System;
using Raven.Client.Documents.Session;
using ServiceStack;
using RecomendationForStartups.ServiceModel;

namespace RecomendationForStartups.ServiceInterface
{
    public class MyServices : Service
    {
        public IAsyncDocumentSession RavenSession { get; set; }

        public GetParameters.GetParametersResponse Get(GetParameters request)
        {
            var response = new GetParameters.GetParametersResponse();
            return response;
        }

        public GetRecommendation.GetRecommendationResponse Post(GetRecommendation request)
        {
            var response = new GetRecommendation.GetRecommendationResponse();
            return response;
        }

        public void Put(UpdateRecommendation request)
        {

        }

        public GetParametersHistory.GetParametersHistoryResponse Get(GetParametersHistory request)
        {
            var response = new GetParametersHistory.GetParametersHistoryResponse();
            return response;
        }
    }
}
