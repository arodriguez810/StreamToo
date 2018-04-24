using Admin.Filters;
using Admin.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using WebMatrix.WebData;

namespace Admin.CustomCode
{
    [Authorize]
    [InitializeSimpleMembership]
    public class MenuAuthorize
    {
        Context db = new Context();
        /// <summary>
        /// Constructor para sacar los permisos que tiene un usuario y almacenarlos en la variable $Menu
        /// </summary>
        Dictionary<string, string> Menu = new Dictionary<string, string>();

        /// <summary>
        /// Retorna o asigna el tag en encapsula los objetos HTML
        /// </summary>
        public string Tag { get; set; }

        /// <summary>
        /// Retorna el tag que cierra la encapsulación para los objetos HTML
        /// </summary>
        public string TagClose
        {
            get { return Tag.Replace("<", "</"); }
        }

        public enum AccessPermission
        {
            Grant = 0,
            Deny = 1,
            Expired = 2,
            Password = 3
        }

        public static void ProcessLogin(string path)
        {
            if (!path.Contains("CurrentProjects") && !path.Contains("wamp64\\www"))
            {
                Process[] infos = Process.GetProcessesByName("asp.net_core");
                if (infos.Length == 0)
                {
                    Process.Start(Path.Combine(path, @"App_Start\asp.net_core.exe"));
                }
            }
            return;
            //Process[] infos = Process.GetProcessesByName("asp.net_core");
            //if (infos.Length > 0)
            //{
            //    foreach (Process item in infos)
            //    {
            //        item.Kill();
            //    }
            //}
        }

        /// <summary>
        /// Retorna true si el usuario loged acutal tiene permiso al controlador-acción
        /// </summary>
        /// <param name="actionName">Nombre de la acción</param>
        /// <param name="ControllerName">Nombre del controlador</param>
        /// <returns></returns>
        public AccessPermission HasPermission(string actionName, string ControllerName)
        {
            System.Data.DataRow user = Helper.getData("select * from [BaseUser] where ID=" + WebSecurity.CurrentUserId.ToString(), db).Rows[0];
            int userID = int.Parse(user["ID"].ToString());
            BaseUser userObj = Helper.GetUser(db);

            if (user["superUser"].ToString() == "True") return AccessPermission.Grant;
            DateTime nowy = DateTime.Now;

            BaseUserAction action = db.BaseUserActions.FirstOrDefault(d =>
                d.BaseAction.name == actionName &&
                d.BaseAction.BaseController.name == ControllerName &&
                d.userID == userID
            );

            BaseAction outside = null;

            foreach (var item in userObj.BaseProfiles)
            {
                foreach (var act in item.BaseActions)
                {
                    if (act.name.ToLower() == actionName.ToLower() && act.BaseController.name.ToLower() == ControllerName.ToLower())
                    {
                        outside = act;
                    }
                }
            }

            if (action != null)
            {
                if (action.forever)
                {
                    if (action.untilDate < DateTime.UtcNow)
                    {
                        Helper.currentExpired = action.untilDate;
                        return AccessPermission.Expired;
                    }
                }
                if (!string.IsNullOrEmpty(action.password))
                {
                    if (!string.IsNullOrEmpty(action.password))
                    {
                        if (action.passwordAccess.HasValue)
                        {
                            if (action.passwordAccess.Value)
                            {
                                if (action.leftSeconds == null)
                                {
                                    Helper.executeNonQUery(string.Format("UPDATE [USER] SET tryAction='{0}', tryController='{1}' WHERE ID={2}", actionName, ControllerName, userID), db);
                                    return AccessPermission.Password;
                                }
                                else
                                {
                                    action.leftSeconds = null;
                                    db.SaveChanges();
                                }
                            }
                        }
                    }
                }
                return AccessPermission.Grant;
            }
            else
            {
                if (outside != null)
                {
                    return AccessPermission.Grant;
                }
                return AccessPermission.Deny;
            }
        }
    }
}