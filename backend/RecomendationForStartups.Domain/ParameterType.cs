using System.ComponentModel;

namespace RecomendationForStartups.Domain
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