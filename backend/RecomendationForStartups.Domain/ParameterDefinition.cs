using System.Collections.Generic;
using System.ComponentModel;

namespace RecomendationForStartups.Domain
{
    [Description("Список параметров")]
    public class ParameterDefinition
    {
        [Description("Идентификатор параметра")]
        public string Id { get; set; }

        [Description("Тип параметра")]
        public ParameterType Type { get; set; }
        
        [Description("Список допустимых значений")]
        public List<string> AcceptableValues { get; set; }
    }
}