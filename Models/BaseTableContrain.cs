using System;
using System.Collections.Generic;

namespace Admin.Models
{
    public partial class BaseTableContrain
    {
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public int basetable_table { get; set; }
        public string query { get; set; }
        public bool active { get; set; }
        public virtual BaseTable BaseTable { get; set; }
    }
}
