using System.Collections.Generic;
using ServiceStack.DataAnnotations;

namespace RecomendationForStartups.ServiceModel.Types
{
    [Description("Значение параметра")]
    public class ParameterValue
    {
        [Description("Идентификатор параметра")]
        public string Id{ get; set; }

        [Description("Список значений")]
        public List<string> Values { get; set; }
    }
}