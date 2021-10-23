using System.Collections.Generic;
using RecomendationForStartups.ServiceModel.Types;
using ServiceStack;
using ServiceStack.DataAnnotations;

namespace RecomendationForStartups.ServiceModel
{
    [Route("/Parameters", Verbs ="GET", Summary = "Получение описания параметров")]
    public class GetParameters : IReturn<GetParameters.GetParametersResponse>
    {
        [Description("Результат получения описания параметров")]
        public class GetParametersResponse
        {
            [Description("Список параметров")]
            public List<ParameterDefinition> Parameters { get; set; }
        }
    }
    
    [Route("/Recommendation", Verbs = "POST", Summary = "Получение рекомендаций по параметрам")]
    public class GetRecommendation : IReturn<GetRecommendation.GetRecommendationResponse>
    {
        [ApiMember(Description = "Список параметров")]
        public List<ParameterValue> Parameters { get; set; }
        
        [Description("Результат получения рекомендаций по параметрам")]
        public class GetRecommendationResponse
        {
            [Description("Список рекомендаций")]
            public List<RecommendationInfo> Recommendations { get; set; }

        }
    }

    [Route("/Recommendation/{RecommendationId}", Verbs = "PUT", Summary = "Дообучение модели рекомендаций по параметрам")]
    public class UpdateRecommendation : IReturn
    {
        [ApiMember(Description = "Список параметров", IsRequired = true)]
        public List<ParameterValue> Parameters { get; set; }

        [Description("Идентификатор рекомендации")]
        public string RecommendationId { get; set; }
    }

    [Route("/Parameters/History", Verbs = "GET", Summary = "Получение истории комбинаций параметров")]
    public class GetParametersHistory : IReturn<GetParametersHistory.GetParametersHistoryResponse>
    {
        [Description("Результат получения истории комбинаций параметров")]
        public class GetParametersHistoryResponse
        {
            [Description("Список комбинаций параметров")]
            public List<ParametersCombination> Combinations { get; set; }
        }
    }
}
