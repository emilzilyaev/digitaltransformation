using System.Collections.Generic;
using System.ComponentModel;

namespace RecomendationForStartups.Domain
{
    [Description("������ ����������")]
    public class ParameterDefinition
    {
        [Description("������������� ���������")]
        public string Id { get; set; }

        [Description("��� ���������")]
        public ParameterType Type { get; set; }
        
        [Description("������ ���������� ��������")]
        public List<string> AcceptableValues { get; set; }
    }
}