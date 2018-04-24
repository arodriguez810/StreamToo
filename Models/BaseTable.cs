using System;
using System.Collections.Generic;

namespace Admin.Models
{
    public partial class BaseTable
    {
        public BaseTable()
        {
            this.BaseColumns = new List<BaseColumn>();
            this.BaseTableContrains = new List<BaseTableContrain>();
        }

        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public bool active { get; set; }
        public virtual ICollection<BaseColumn> BaseColumns { get; set; }
        public virtual ICollection<BaseTableContrain> BaseTableContrains { get; set; }
    }
}
