using ServiceStack.DataAnnotations;

namespace RecomendationForStartups.ServiceModel.Types
{
    public enum ParameterType
    {
        [Description("����� �� ���������")]
        NumberRange,

        [Description("���� �������� �� �������������")]
        OneAcceptable,

        [Description("��������� �������� �� �������������")]
        MultiAcceptable
    }
}