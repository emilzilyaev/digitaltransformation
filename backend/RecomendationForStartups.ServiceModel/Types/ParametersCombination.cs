using System;
using System.Collections.Generic;
using ServiceStack.DataAnnotations;

namespace RecomendationForStartups.ServiceModel.Types
{
    public class ParametersCombination
    {
        [Description("Список параметров")]
        public List<ParameterValue> Parameters { get; set; }

        [Description("Дата создания")]
        public DateTimeOffset Created { get; set; }
    }
}