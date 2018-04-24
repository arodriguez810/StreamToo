using System;
using System.Collections.Generic;

namespace Admin.Models
{
    public partial class BaseDynamicColumnList
    {
        public int listID { get; set; }
        public string name { get; set; }
        public bool show { get; set; }
        public int orderNum { get; set; }
        public virtual BaseDynamicList BaseDynamicList { get; set; }
    }
}
