using System.Linq;

namespace Admin.Models
{
    public partial class BaseUser
    {
        public string FullName
        {
            get { return this.name + " " + this.lastName; }
        }

        public string PimageUrl
        {
            get { return string.IsNullOrEmpty(this.imageUrl) ? "defaultImage.png" : this.imageUrl; }
        }

        public string Emails
        {
            get { return this.username; }
        }

        public BaseUserAction getOneBaseUserAction(string actionName, string controllerName)
        {
            return this.BaseUserActions.FirstOrDefault(d => d.BaseAction.name == actionName && d.BaseAction.BaseController.name == controllerName.Replace("Controller", ""));
        }
    }
}
