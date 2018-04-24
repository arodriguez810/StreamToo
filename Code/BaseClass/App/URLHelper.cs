using Admin.CustomCode;
using Admin.Models;
using System;
using System.Data;
using System.Web.Mvc;
using System.Web.Routing;

namespace Admin.BaseClass
{
    public static class URLHelper
    {
        

        public static string getUrl(string controller = "", string action = "")
        {
            controller = controller == "" ? Helper.currentController : controller;
            action = action == "" ? Helper.currentAction : action;

            return string.Format("{2}/BaseHome#{0}/{1}", controller, action, System.Web.Hosting.HostingEnvironment.ApplicationVirtualPath);
        }

        public static string getAbsoluteUrl(string controller = "", string action = "", params string[] paramsn)
        {
            controller = controller == "" ? Helper.currentController : controller;
            action = action == "" ? Helper.currentAction : action;
            Uri request = System.Web.HttpContext.Current.Request.Url;
            string host = string.Format("{2}://{0}{1}", request.Host, request.Port != 80 ? ":" + request.Port : "", request.Scheme);
            string paramsString = "";
            if (paramsn != null)
                foreach (string item in paramsn)
                {
                    paramsString += string.Format("{0}&", item);
                }
            return string.Format(host + "{2}/BaseHome#{0}/{1}{3}", controller, action, System.Web.Hosting.HostingEnvironment.ApplicationVirtualPath, (!string.IsNullOrEmpty(paramsString) ? "?" + paramsString : ""));
        }

        public static string getAbsoluteUrlNoHome(string controller = "", string action = "", params string[] paramsn)
        {
            controller = controller == "" ? Helper.currentController : controller;
            action = action == "" ? Helper.currentAction : action;
            Uri request = System.Web.HttpContext.Current.Request.Url;
            string host = string.Format("{2}://{0}{1}", request.Host, request.Port != 80 ? ":" + request.Port : "", request.Scheme);
            string paramsString = "";
            if (paramsn != null)
                foreach (string item in paramsn)
                {
                    paramsString += string.Format("{0}&", item);
                }
            return string.Format(host + "{2}/{0}/{1}{3}", controller, action, System.Web.Hosting.HostingEnvironment.ApplicationVirtualPath, (!string.IsNullOrEmpty(paramsString) ? "?" + paramsString : ""));
        }

        public static string getActionUrl(string controller = "", string action = "")
        {
            controller = controller == "" ? Helper.currentController : controller;
            action = action == "" ? Helper.currentAction : action;
            return string.Format("{2}/{0}/{1}", controller, action, System.Web.Hosting.HostingEnvironment.ApplicationVirtualPath);
        }

        public static string getActionUrlWithBaseHome(string controller = "", string action = "")
        {
            controller = controller == "" ? Helper.currentController : controller;
            action = action == "" ? Helper.currentAction : action;
            return string.Format("BaseHome#{0}/{1}", controller, action);
        }

        public static CsTableLinkType buttonType(string controller = "")
        {
            DataTable data = Helper.getData($"select COUNT(*) from VWISRElations where PK_Table='{controller}'", new Context());
            if (data.Rows[0][0].ToString() == "0")
                return CsTableLinkType.button;
            return CsTableLinkType.link;
        }

        public static string getActionUrlRelation(string controller = "", string action = "")
        {
            DataTable data = Helper.getData($"select COUNT(*) from VWISRElations where PK_Table='{controller}'", new Context());
            if (data.Rows[0][0].ToString() == "0")
            {
                controller = controller == "" ? Helper.currentController : controller;
                action = action == "" ? Helper.currentAction : action;
                return string.Format("{2}/{0}/{1}", controller, action, System.Web.Hosting.HostingEnvironment.ApplicationVirtualPath);
            }
            else
            {
                return getUrl(controller, action);
            }
        }

        public static string getActionUrl(string controller = "", string action = "", string[] paramsn = null)
        {
            controller = controller == "" ? Helper.currentController : controller;
            action = action == "" ? Helper.currentAction : action;

            string paramsString = "";
            if (paramsn != null)
            {
                foreach (string item in paramsn)
                {
                    paramsString += string.Format("{0}&", item);
                }
            }
            return string.Format("{2}/{0}/{1}{3}", controller, action, System.Web.Hosting.HostingEnvironment.ApplicationVirtualPath, (!string.IsNullOrEmpty(paramsString) ? "?" + paramsString : ""));
        }

        public static string getCurrentUrl(ControllerContext context)
        {
            string actionName = context.RouteData.Values["action"].ToString();
            string controllerName = context.RouteData.Values["controller"].ToString();
            return string.Format("{2}/BaseHome#{0}/{1}", controllerName.Replace("Controller", ""), actionName, System.Web.Hosting.HostingEnvironment.ApplicationVirtualPath);
        }

        internal static string getActionUrl(BaseAction action)
        {
            return string.Format("{2}/BaseHome#{0}/{1}", action.BaseController.name, action.name, System.Web.Hosting.HostingEnvironment.ApplicationVirtualPath);
        }
    }
}