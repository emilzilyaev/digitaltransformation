using System.Collections.Generic;
using System.ComponentModel;

namespace RecomendationForStartups.Domain
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