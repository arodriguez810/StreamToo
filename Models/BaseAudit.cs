using System;
using System.Collections.Generic;

namespace Admin.Models
{
    public partial class BaseAudit
    {
        public System.DateTime Execute_at { get; set; }
        public string Entity { get; set; }
        public string RecordID { get; set; }
        public string User { get; set; }
        public string Action { get; set; }
    }
}
