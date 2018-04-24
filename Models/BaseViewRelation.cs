using System;
using System.Collections.Generic;

namespace Admin.Models
{
    public partial class BaseViewRelation
    {
        public int id { get; set; }
        public string tableFrom { get; set; }
        public string fkColumn { get; set; }
        public string pkColumn { get; set; }
        public string pkColumnName { get; set; }
        public string tableTo { get; set; }
        public string customLabel { get; set; }
        public Nullable<bool> hide { get; set; }
    }
}
