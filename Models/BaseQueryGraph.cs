using System;
using System.Collections.Generic;

namespace Admin.Models
{
    public partial class BaseQueryGraph
    {
        public BaseQueryGraph()
        {
            this.BaseGraphs = new List<BaseGraph>();
        }

        public int id { get; set; }
        public string name { get; set; }
        public string query { get; set; }
        public virtual ICollection<BaseGraph> BaseGraphs { get; set; }
    }
}
