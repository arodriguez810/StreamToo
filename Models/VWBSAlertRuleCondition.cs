using System;
using System.Collections.Generic;

namespace Admin.Models
{
    public partial class VWBSAlertRuleCondition
    {
        public int id { get; set; }
        public string entity { get; set; }
        public string from { get; set; }
        public string rule { get; set; }
    }
}
