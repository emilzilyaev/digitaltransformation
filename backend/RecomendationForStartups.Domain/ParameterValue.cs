using System.Collections.Generic;
using System.ComponentModel;

namespace RecomendationForStartups.Domain
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