using ServiceStack.DataAnnotations;

namespace RecomendationForStartups.ServiceModel.Types
{
    public enum ParameterType
    {
        [Description("Число из диапазона")]
        NumberRange,

        [Description("Одно значение из фиксированных")]
        OneAcceptable,

        [Description("Несколько значений из фиксированных")]
        MultiAcceptable
    }
}