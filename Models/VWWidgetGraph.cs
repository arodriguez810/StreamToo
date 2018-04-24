using System;
using System.Collections.Generic;

namespace Admin.Models
{
    public partial class VWWidgetGraph
    {
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string url { get; set; }
        public string Graph { get; set; }
    }
}
