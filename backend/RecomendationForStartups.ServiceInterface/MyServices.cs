using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;
using ServiceStack;
using RecomendationForStartups.ServiceModel;
using RecomendationForStartups.ServiceModel.Types;

namespace RecomendationForStartups.ServiceInterface
{
    public class MyServices : Service
    {
        public IAsyncDocumentSession RavenSession { get; set; }
        public IMapper Mapper { get; set; }

        public async Task<GetParameters.GetParametersResponse> Get(GetParameters request)
        {
            var response = new GetParameters.GetParametersResponse();
            var parameters = await RavenSession.Query<Domain.ParameterDefinition>().ToListAsync();
            response.Parameters = parameters.Select(Mapper.Map<ParameterDefinition>).ToList();
            return response;
        }

        public async Task<GetRecommendation.GetRecommendationResponse> Post(GetRecommendation request)
        {
            var response = new GetRecommendation.GetRecommendationResponse();
            //TODO тут обращаемся к нейронке
            var random = new Random();
            var result = new Dictionary<string, double>();
            for (int i = 0; i < 119; i++)
            {
                result[i.ToString()] = random.NextDouble();
            }
            var services = await RavenSession.Query<Domain.Recommendation>().ToListAsync();
            response.Recommendations = services.Select(Mapper.Map<RecommendationInfo>)
                .Select(info =>
                {
                    if (result.TryGetValue(info.Id, out var match))
                    {
                        info.MatchPercentage = match;
                    }
                    else
                    {
                        info.MatchPercentage = 0;
                    }
                    return info;
                })
                .OrderByDescending(info => info.MatchPercentage)
                .ToList();

            var combination = new Domain.ParametersCombination();
            combination.Parameters = request.Parameters?.Select(Mapper.Map<Domain.ParameterValue>).ToList();
            var key =new StringBuilder();
            if (combination.Parameters!= null)
            {
                foreach (var parameter in combination.Parameters.OrderBy(value => value.Id))
                {
                    key.Append(parameter.Id);
                    foreach (var value in parameter.Values.OrderBy(s => s))
                    {
                        key.Append(value);
                    }
                }
            }
            combination.Id = key.ToString();
            combination.Created = DateTimeOffset.UtcNow;
            await RavenSession.StoreAsync(combination);
            await RavenSession.SaveChangesAsync();
            return response;
        }

        public async Task Put(UpdateRecommendation request)
        {

        }

        public async Task<GetParametersHistory.GetParametersHistoryResponse> Get(GetParametersHistory request)
        {
            var response = new GetParametersHistory.GetParametersHistoryResponse();
            var history = await RavenSession.Query<Domain.ParametersCombination>().ToListAsync();
            response.Combinations = history.Select(Mapper.Map<ParametersCombination>).ToList();
            return response;
        }
    }
}
