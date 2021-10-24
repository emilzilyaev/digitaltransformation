using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace RecomendationForStartups.Domain
{
    public class ParametersCombination
    {
        [Description("Идентификатор")]
        public string Id { get; set; }

        [Description("Список параметров")]
        public List<ParameterValue> Parameters { get; set; }

        [Description("Дата создания")]
        public DateTimeOffset Created { get; set; }
    }
}