using System;
using System.Collections.Generic;

namespace Admin.Models
{
    public partial class BaseUserStatu
    {
        public BaseUserStatu()
        {
            this.BaseUsers = new List<BaseUser>();
        }

        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public bool canLogin { get; set; }
        public Nullable<int> defaultAction { get; set; }
        public string message { get; set; }
        public virtual BaseAction BaseAction { get; set; }
        public virtual ICollection<BaseUser> BaseUsers { get; set; }
    }
}
