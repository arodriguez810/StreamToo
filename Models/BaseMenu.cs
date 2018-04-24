using System;
using System.Collections.Generic;

namespace Admin.Models
{
    public partial class BaseMenu
    {
        public BaseMenu()
        {
            this.BaseMenu1 = new List<BaseMenu>();
            this.BaseProfileMenus = new List<BaseProfileMenu>();
            this.BaseUserMenus = new List<BaseUserMenu>();
        }

        public int id { get; set; }
        public Nullable<int> menuID { get; set; }
        public string title { get; set; }
        public int noOrder { get; set; }
        public string href { get; set; }
        public Nullable<int> actionID { get; set; }
        public bool directLink { get; set; }
        public string css { get; set; }
        public string icon { get; set; }
        public string text { get; set; }
        public string badgeQuery { get; set; }
        public string badgeText { get; set; }
        public string badgeColor { get; set; }
        public string backGroundColor { get; set; }
        public bool translatable { get; set; }
        public Nullable<bool> hidden { get; set; }
        public virtual BaseAction BaseAction { get; set; }
        public virtual ICollection<BaseMenu> BaseMenu1 { get; set; }
        public virtual BaseMenu BaseMenu2 { get; set; }
        public virtual ICollection<BaseProfileMenu> BaseProfileMenus { get; set; }
        public virtual ICollection<BaseUserMenu> BaseUserMenus { get; set; }
    }
}
