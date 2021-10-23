using System;
using System.Collections.Generic;
using ServiceStack.DataAnnotations;

namespace RecomendationForStartups.ServiceModel.Types
{
    public class ParametersCombination
    {
        [Description("������ ����������")]
        public List<ParameterValue> Parameters { get; set; }

        [Description("���� ��������")]
        public DateTimeOffset Created { get; set; }
    }
}