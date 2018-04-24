using System;
using System.Collections.Generic;

namespace Admin.Models
{
    public partial class VWBaseRelation
    {
        public string parent { get; set; }
        public string child { get; set; }
        public string fk { get; set; }
        public string type { get; set; }
    }
}
