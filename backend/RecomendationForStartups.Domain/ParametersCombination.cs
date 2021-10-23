using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace RecomendationForStartups.Domain
{
    public class ParametersCombination
    {
        [Description("�������������")]
        public string Id { get; set; }

        [Description("������ ����������")]
        public List<ParameterValue> Parameters { get; set; }

        [Description("���� ��������")]
        public DateTimeOffset Created { get; set; }
    }
}