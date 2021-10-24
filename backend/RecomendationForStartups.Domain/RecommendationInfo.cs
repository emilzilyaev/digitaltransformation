using System.ComponentModel;

namespace RecomendationForStartups.Domain
{
    [Description("Рекомендация")]
    public class Recommendation
    {
        [Description("Идентификатор рекомендации")]
        public string Id{ get; set; }

        [Description("Название")]
        public string Description{ get; set; }
    }
}