using System;
using System.Collections.Generic;

namespace Admin.Models
{
    public partial class BaseWidget
    {
        public BaseWidget()
        {
            this.BaseProfiles = new List<BaseProfile>();
            this.BaseUsers = new List<BaseUser>();
            this.BaseGraphs = new List<BaseGraph>();
            this.BaseUsers1 = new List<BaseUser>();
            this.BaseProfiles1 = new List<BaseProfile>();
        }

        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public Nullable<int> actionID { get; set; }
        public int width { get; set; }
        public string actionContent { get; set; }
        public string html { get; set; }
        public bool hasRange { get; set; }
        public virtual BaseAction BaseAction { get; set; }
        public virtual ICollection<BaseProfile> BaseProfiles { get; set; }
        public virtual ICollection<BaseUser> BaseUsers { get; set; }
        public virtual ICollection<BaseGraph> BaseGraphs { get; set; }
        public virtual ICollection<BaseUser> BaseUsers1 { get; set; }
        public virtual ICollection<BaseProfile> BaseProfiles1 { get; set; }
    }
}
