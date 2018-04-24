using System;
using System.Collections.Generic;

namespace Admin.Models
{
    public partial class VWBaseDynamicList
    {
        public int id { get; set; }
        public string name { get; set; }
        public string cookieKey { get; set; }
        public string Save_Columns { get; set; }
        public string Export { get; set; }
        public bool enableControl { get; set; }
    }
}
