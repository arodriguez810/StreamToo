using System;
using System.Collections.Generic;

namespace Admin.Models
{
    public partial class VWBSAlertTransaction
    {
        public int id { get; set; }
        public string Creator { get; set; }
        public string Status { get; set; }
        public string comment { get; set; }
        public Nullable<int> alertID { get; set; }
    }
}
