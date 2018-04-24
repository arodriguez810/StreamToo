using System;
using System.Collections.Generic;

namespace Admin.Models
{
    public partial class BaseGraph
    {
        public BaseGraph()
        {
            this.BaseTypesGraphs = new List<BaseTypesGraph>();
            this.BaseUsers = new List<BaseUser>();
            this.BaseProfiles = new List<BaseProfile>();
            this.BaseWidgets = new List<BaseWidget>();
        }

        public int id { get; set; }
        public Nullable<int> queryGraphID { get; set; }
        public string name { get; set; }
        public string title { get; set; }
        public virtual BaseQueryGraph BaseQueryGraph { get; set; }
        public virtual ICollection<BaseTypesGraph> BaseTypesGraphs { get; set; }
        public virtual ICollection<BaseUser> BaseUsers { get; set; }
        public virtual ICollection<BaseProfile> BaseProfiles { get; set; }
        public virtual ICollection<BaseWidget> BaseWidgets { get; set; }
    }
}
