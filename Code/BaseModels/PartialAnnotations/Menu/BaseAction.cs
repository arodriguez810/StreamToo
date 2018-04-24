using Admin.CustomCode;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;


namespace Admin.Models
{
    [MetadataType(typeof(BaseActionMetadata))]
    public partial class BaseAction
    {
        public bool hasPermission()
        {
            return Helper.GetUser(new Context()).BaseUserActions.FirstOrDefault(d => d.actionID == this.id) != null;
        }

    }
    public partial class BaseActionMetadata
    {

        public int id { get; set; }
        public int controllerID { get; set; }
        public string name { get; set; }
        public string displayName { get; set; }
        public virtual BaseController BaseController { get; set; }
        public virtual ICollection<BaseMenu> BaseMenus { get; set; }
        public virtual ICollection<BaseUserAction> BaseUserActions { get; set; }
    }
}
