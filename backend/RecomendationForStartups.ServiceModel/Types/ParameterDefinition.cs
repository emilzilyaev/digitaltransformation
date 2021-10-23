using System.Collections.Generic;
using ServiceStack.DataAnnotations;

namespace RecomendationForStartups.ServiceModel.Types
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