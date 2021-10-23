using System.Collections.Generic;
using ServiceStack.DataAnnotations;

namespace RecomendationForStartups.ServiceModel.Types
{
    [Description("�������� ���������")]
    public class ParameterValue
    {
        [Description("������������� ���������")]
        public string Id{ get; set; }

        [Description("������ ��������")]
        public List<string> Values { get; set; }
    }
}