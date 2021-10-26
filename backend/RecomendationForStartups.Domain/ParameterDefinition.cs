using System.Collections.Generic;
using System.ComponentModel;

namespace RecomendationForStartups.Domain
{
    [Description("Список параметров")]
    public class ParameterDefinition
    {
        [Description("Идентификатор параметра")]
        public string Id { get; set; }

        [Description("Описание параметра")]
        public string Description { get; set; }

        [Description("Тип параметра")]
        public ParameterType Type { get; set; }

        [Description("Список допустимых значений")]
        public Dictionary<string, string> AcceptableValues { get; set; }
    }
}