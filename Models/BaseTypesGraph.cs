using System;
using System.Collections.Generic;

namespace Admin.Models
{
    public partial class BaseTypesGraph
    {
        public BaseTypesGraph()
        {
            this.BaseGraphs = new List<BaseGraph>();
        }

        public int id { get; set; }
        public string name { get; set; }
        public virtual ICollection<BaseGraph> BaseGraphs { get; set; }
    }
}
