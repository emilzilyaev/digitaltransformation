using System.ComponentModel;

namespace RecomendationForStartups.Domain
{
    [Description("������������")]
    public class Recommendation
    {
        [Description("������������� ������������")]
        public string Id{ get; set; }

        [Description("��������")]
        public string Description{ get; set; }
    }
}