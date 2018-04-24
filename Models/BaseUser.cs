using System;
using System.Collections.Generic;

namespace Admin.Models
{
    public partial class BaseUser
    {
        public BaseUser()
        {
            this.BaseLogs = new List<BaseLog>();
            this.BaseWebSocketChannelBlackListUsers = new List<BaseWebSocketChannelBlackListUser>();
            this.BaseUserActions = new List<BaseUserAction>();
            this.BaseUserMenus = new List<BaseUserMenu>();
            this.BaseUserSessions = new List<BaseUserSession>();
            this.BaseGraphs = new List<BaseGraph>();
            this.BaseProfiles = new List<BaseProfile>();
            this.BaseWidgets = new List<BaseWidget>();
            this.BaseWidgets1 = new List<BaseWidget>();
        }

        public int ID { get; set; }
        public string name { get; set; }
        public Nullable<int> userStatusID { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public bool superUser { get; set; }
        public Nullable<System.DateTime> createDate { get; set; }
        public Nullable<int> createUser { get; set; }
        public string imageUrl { get; set; }
        public string lastName { get; set; }
        public string address { get; set; }
        public string phone { get; set; }
        public Nullable<int> defaultActionID { get; set; }
        public string cookie { get; set; }
        public Nullable<int> widgetsToShow { get; set; }
        public string tryAction { get; set; }
        public string tryController { get; set; }
        public Nullable<bool> superAdminShowHiddenMenu { get; set; }
        public string token { get; set; }
        public Nullable<int> employeeType_Type { get; set; }
        public Nullable<int> office_office { get; set; }
        public bool isAppUser { get; set; }
        public virtual BaseAction BaseAction { get; set; }
        public virtual ICollection<BaseLog> BaseLogs { get; set; }
        public virtual ICollection<BaseWebSocketChannelBlackListUser> BaseWebSocketChannelBlackListUsers { get; set; }
        public virtual ICollection<BaseUserAction> BaseUserActions { get; set; }
        public virtual ICollection<BaseUserMenu> BaseUserMenus { get; set; }
        public virtual BaseUserStatu BaseUserStatu { get; set; }
        public virtual ICollection<BaseUserSession> BaseUserSessions { get; set; }
        public virtual ICollection<BaseGraph> BaseGraphs { get; set; }
        public virtual ICollection<BaseProfile> BaseProfiles { get; set; }
        public virtual ICollection<BaseWidget> BaseWidgets { get; set; }
        public virtual ICollection<BaseWidget> BaseWidgets1 { get; set; }
    }
}
