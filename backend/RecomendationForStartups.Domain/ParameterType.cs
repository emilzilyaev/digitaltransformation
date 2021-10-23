using System.ComponentModel;

namespace RecomendationForStartups.Domain
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