using System.Collections.Generic;
using RecomendationForStartups.ServiceModel.Types;
using ServiceStack;
using ServiceStack.DataAnnotations;

namespace RecomendationForStartups.ServiceModel
{
    [Route("/Parameters", Verbs ="GET", Summary = "��������� �������� ����������")]
    public class GetParameters : IReturn<GetParameters.GetParametersResponse>
    {
        [Description("��������� ��������� �������� ����������")]
        public class GetParametersResponse
        {
            [Description("������ ����������")]
            public List<ParameterDefinition> Parameters { get; set; }
        }
    }
    
    [Route("/Recommendation", Verbs = "POST", Summary = "��������� ������������ �� ����������")]
    public class GetRecommendation : IReturn<GetRecommendation.GetRecommendationResponse>
    {
        [ApiMember(Description = "������ ����������")]
        public List<ParameterValue> Parameters { get; set; }
        
        [Description("��������� ��������� ������������ �� ����������")]
        public class GetRecommendationResponse
        {
            [Description("������ ������������")]
            public List<RecommendationInfo> Recommendations { get; set; }

        }
    }

    [Route("/Recommendation/{RecommendationId}", Verbs = "PUT", Summary = "���������� ������ ������������ �� ����������")]
    public class UpdateRecommendation : IReturn
    {
        [ApiMember(Description = "������ ����������", IsRequired = true)]
        public List<ParameterValue> Parameters { get; set; }

        [Description("������������� ������������")]
        public string RecommendationId { get; set; }
    }

    [Route("/Parameters/History", Verbs = "GET", Summary = "��������� ������� ���������� ����������")]
    public class GetParametersHistory : IReturn<GetParametersHistory.GetParametersHistoryResponse>
    {
        [Description("��������� ��������� ������� ���������� ����������")]
        public class GetParametersHistoryResponse
        {
            [Description("������ ���������� ����������")]
            public List<ParametersCombination> Combinations { get; set; }
        }
    }
}
