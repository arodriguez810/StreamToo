using System;
using System.Collections.Generic;

namespace Admin.Models
{
    public partial class VWGraphListView
    {
        public int id { get; set; }
        public string name { get; set; }
        public string title { get; set; }
        public string query { get; set; }
        public int queryGraphID { get; set; }
    }
}
