using Admin.Models;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using WebMatrix.WebData;

namespace Admin.CustomCode
{
    public class CustomAuthorize : AuthorizeAttribute
    {
        Context db;
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);

            db = new Context();
            if (WebSecurity.IsAuthenticated)
            {
                var a = db.BaseActions
                    .FirstOrDefault(ac => ac.name == filterContext.ActionDescriptor.ActionName);

                var descriptor = filterContext.ActionDescriptor;
                var controller = descriptor.ControllerDescriptor.ControllerName;
                var action = descriptor.ActionName;

                bool skipAuthorization = filterContext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), inherit: true) || filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(AllowAnonymousAttribute), inherit: true);

                if (skipAuthorization)
                    return;
                skipAuthorization = !filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(CustomAuthorize), true);
                if (skipAuthorization)
                    return;
                //skipAuthorization = !filterContext.ActionDescriptor.IsDefined(typeof(IsViewAttribute), true);
                //if (skipAuthorization)
                //    return;
                skipAuthorization = filterContext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true);

                if (skipAuthorization)
                    return;

                Admin.CustomCode.MenuAuthorize.AccessPermission permission = new MenuAuthorize().HasPermission(descriptor.ActionName, descriptor.ControllerDescriptor.ControllerName);

                if (descriptor.ActionName == "Index" && descriptor.ControllerDescriptor.ControllerName == "BaseHome")
                {
                    permission = MenuAuthorize.AccessPermission.Grant;
                }
                if (permission == MenuAuthorize.AccessPermission.Grant)
                {
                    return;
                }
                else if (permission == MenuAuthorize.AccessPermission.Deny)
                {
                    if (((ReflectedActionDescriptor)filterContext.ActionDescriptor).MethodInfo.ReturnType == typeof(JsonResult))
                    {
                        filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary { { "controller", "BaseUnauthorize" }, { "action", "ErrorUnauthorizedJson" } });
                    }
                    else
                    {
                        filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary { { "controller", "BaseUnauthorize" }, { "action", "ErrorUnauthorized" } });
                    }
                }
                else if (permission == MenuAuthorize.AccessPermission.Expired)
                {
                    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary { { "controller", "BaseUnauthorize" }, { "action", "Expired" } });
                }
                else if (permission == MenuAuthorize.AccessPermission.Password)
                {
                    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary { { "controller", "BaseUnauthorize" }, { "action", "needPassword" }, { "actionName", "needPassword" } });
                }
            }
            else
            {
                filterContext.Result = new RedirectToRouteResult(
                     new RouteValueDictionary
                 {
                     { "controller", "Account" },
                     { "action", "Login" }
                 });
            }
        }
    }
}