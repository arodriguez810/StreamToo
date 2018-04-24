using Admin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace Admin.CustomCode
{
    public class ControllerActionProvider
    {
        Context db = new Context();

        public List<string> GetActionNames(string controllerName)
        {
            List<string> actions = new List<string>();
            var types =
                from a in AppDomain.CurrentDomain.GetAssemblies()
                from t in a.GetTypes()
                where typeof(IController).IsAssignableFrom(t) &&
                        string.Equals(
                        (controllerName.EndsWith("Controller") ? controllerName : controllerName+"Controller"),
                        t.Name,
                        StringComparison.OrdinalIgnoreCase)
                select t;

            //var controllers = Assembly.GetExecutingAssembly().GetExportedTypes().Where(t => typeof(ControllerBase).IsAssignableFrom(t)).Select(t => t);

            var controllerType = types.FirstOrDefault();

            if (controllerType == null)
            {
                return Enumerable.Empty<string>().ToList();
            }

            

            foreach (ActionDescriptor a in new ReflectedControllerDescriptor(controllerType)
                .GetCanonicalActions())
            {
                if (!a.GetCustomAttributes(typeof(AllowAnonymousAttribute), false).Any())
                {
                    actions.Add(a.ActionName);
                }
            }

            return actions;
        }

        public List<string> GetControllers()
        {
            List<string> controllerNames = new List<string>();
            var types =
                from a in AppDomain.CurrentDomain.GetAssemblies()
                from t in a.GetTypes()
                where typeof(IController).IsAssignableFrom(t)
                select t;

            foreach(var c in types)
            {
                controllerNames.Add(c.Name);
            }
            return controllerNames;
        }

        private static List<System.Type> GetSubClasses<T>()
        {
            return Assembly.GetCallingAssembly().GetTypes()
                .Where(
                    type => type.IsSubclassOf(typeof(T))
                ).ToList();
        }

        public List<string> GetControllerNames()
        {
            List<string> controllerNames = new List<string>();
            GetSubClasses<System.Web.Mvc.Controller>().ForEach(
                type => controllerNames.Add(type.Name));
            return controllerNames;
        }
    }
}