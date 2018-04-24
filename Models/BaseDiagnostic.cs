using System;
using System.Collections.Generic;

namespace Admin.Models
{
    public partial class BaseDiagnostic
    {
        public string TableOrColumn { get; set; }
        public string Input { get; set; }
        public string endWithS { get; set; }
        public string isKeyWord { get; set; }
        public string hasUnderScore { get; set; }
        public string correctNameRelation { get; set; }
        public string noHasPrimaryKey { get; set; }
        public string hasActiveField { get; set; }
    }
}
