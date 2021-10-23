using ServiceStack.DataAnnotations;

namespace RecomendationForStartups.ServiceModel.Types
{
    [Description("������������")]
    public class RecommendationInfo
    {
        [Description("������������� ������������")]
        public string Id{ get; set; }

        [Description("��������")]
        public string Description{ get; set; }

        [Description("������� ����������")]
        public double MatchPercentage { get; set; }
    }
}