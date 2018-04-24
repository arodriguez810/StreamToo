using System;
using System.Collections.Generic;

namespace Admin.Models
{
    public partial class VWBaseNiiFk
    {
        public string parent { get; set; }
        public string child { get; set; }
        public string parentColumn { get; set; }
        public string parentChild { get; set; }
    }
}
