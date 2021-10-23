using ServiceStack.DataAnnotations;

namespace RecomendationForStartups.ServiceModel.Types
{
    [Description("Рекомендация")]
    public class RecommendationInfo
    {
        [Description("Идентификатор рекомендации")]
        public string Id{ get; set; }

        [Description("Название")]
        public string Description{ get; set; }

        [Description("Процент совпадения")]
        public double MatchPercentage { get; set; }
    }
}