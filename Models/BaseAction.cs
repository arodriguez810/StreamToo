using System;
using System.Collections.Generic;

namespace Admin.Models
{
    public partial class BaseAction
    {
        public BaseAction()
        {
            this.BaseMenus = new List<BaseMenu>();
            this.BaseProfiles = new List<BaseProfile>();
            this.BaseUserActions = new List<BaseUserAction>();
            this.BaseWidgets = new List<BaseWidget>();
            this.BaseUsers = new List<BaseUser>();
            this.BaseUserStatus = new List<BaseUserStatu>();
            this.BaseProfiles1 = new List<BaseProfile>();
        }

        public int id { get; set; }
        public int controllerID { get; set; }
        public string name { get; set; }
        public string displayName { get; set; }
        public bool isSystem { get; set; }
        public virtual BaseController BaseController { get; set; }
        public virtual ICollection<BaseMenu> BaseMenus { get; set; }
        public virtual ICollection<BaseProfile> BaseProfiles { get; set; }
        public virtual ICollection<BaseUserAction> BaseUserActions { get; set; }
        public virtual ICollection<BaseWidget> BaseWidgets { get; set; }
        public virtual ICollection<BaseUser> BaseUsers { get; set; }
        public virtual ICollection<BaseUserStatu> BaseUserStatus { get; set; }
        public virtual ICollection<BaseProfile> BaseProfiles1 { get; set; }
    }
}
