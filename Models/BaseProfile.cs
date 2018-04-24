using System;
using System.Collections.Generic;

namespace Admin.Models
{
    public partial class BaseProfile
    {
        public BaseProfile()
        {
            this.BaseWebSocketChannelBlackListProfiles = new List<BaseWebSocketChannelBlackListProfile>();
            this.BaseProfileMenus = new List<BaseProfileMenu>();
            this.BaseActions = new List<BaseAction>();
            this.BaseWidgets = new List<BaseWidget>();
            this.BaseUsers = new List<BaseUser>();
            this.BaseGraphs = new List<BaseGraph>();
            this.BaseWidgets1 = new List<BaseWidget>();
        }

        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public Nullable<System.DateTime> creationDate { get; set; }
        public Nullable<int> defaultActionID { get; set; }
        public virtual BaseAction BaseAction { get; set; }
        public virtual ICollection<BaseWebSocketChannelBlackListProfile> BaseWebSocketChannelBlackListProfiles { get; set; }
        public virtual ICollection<BaseProfileMenu> BaseProfileMenus { get; set; }
        public virtual ICollection<BaseAction> BaseActions { get; set; }
        public virtual ICollection<BaseWidget> BaseWidgets { get; set; }
        public virtual ICollection<BaseUser> BaseUsers { get; set; }
        public virtual ICollection<BaseGraph> BaseGraphs { get; set; }
        public virtual ICollection<BaseWidget> BaseWidgets1 { get; set; }
    }
}
