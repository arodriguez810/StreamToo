using System;
using System.Collections.Generic;

namespace Admin.Models
{
    public partial class VWBaseProfile
    {
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string displayName { get; set; }
    }
}
