using Admin.CustomCode;
using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Admin.Models
{

    [MetadataType(typeof(BaseMenuMetadata))]
    public partial class BaseMenu
    {
        public bool isVisible()
        {
            return Helper.GetUser(new Context()).BaseUserMenus.FirstOrDefault(d => d.menuID == this.id) != null;
        }

        public string hreftext
        {
            get
            {
                return
                    this.BaseAction != null ?
                    (this.directLink ? this.href : this.BaseAction.BaseController.name + "/" + this.BaseAction.name) :
                    "#";
            }
        }

        public string realTitle
        {
            get { return this.title; }
        }

        public string realText
        {
            get { return this.text; }
        }

        public string realBadgeText
        {
            get { return this.badgeText; }
        }

    }

    public class MenuJson
    {
        public int id { get; set; }
        public List<MenuJson> children { get; set; }
    }

    public partial class BaseMenuMetadata
    {
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
        public virtual BaseAction BaseAction { get; set; }
        public virtual ICollection<BaseMenu> BaseMenu1 { get; set; }
        public virtual BaseMenu BaseMenu2 { get; set; }
        public virtual ICollection<BaseUserMenu> BaseUserMenus { get; set; }
    }
}
