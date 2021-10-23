using System.Collections.Generic;
using ServiceStack.DataAnnotations;

namespace RecomendationForStartups.ServiceModel.Types
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