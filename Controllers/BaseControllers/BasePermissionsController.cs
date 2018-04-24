using Admin.BaseClass;
using Admin.CustomCode;
using Admin.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Admin.Controllers
{
    [CustomAuthorize]
    public class BasePermissionsController : BaseController
    {
        //
        // GET: /BasePermissions/
        [IsView]
        public ActionResult Index(int id = 0)
        {
            var perfilData = db.BaseProfiles.Find(id);
            if (perfilData != null)
            {
                return PartialView(perfilData);
            }
            else { return HttpNotFound(); }
        }

        [IsView]
        public ActionResult Index2(int id = 0)
        {
            var perfilData = db.BaseProfiles.Find(id);
            if (perfilData != null)
            {
                return PartialView(perfilData);
            }
            else { return HttpNotFound(); }
        }

        [HttpPost]
        public ActionResult SaveData(FormCollection form)
        {
            int idProfile = int.Parse(form["id"]);
            var pro = db.BaseProfiles.Find(idProfile);
            bool firsttime = false;
            int increment = 0;
            foreach (var f in form.AllKeys)
            {
                if (!firsttime)
                {
                    Helper.executeNonQUery("Delete from dbo.BaseWidgetUserProfile where ProfileID = " + pro.id, db);
                    Helper.executeNonQUery("Delete from dbo.BaseProfileMenu where userProfileID = " + pro.id, db);
                    Helper.executeNonQUery("Delete from dbo.BaseProfileAction where profileID = " + pro.id, db);
                    Helper.executeNonQUery("Delete from dbo.BaseUserProfileGraph where ProfileID = " + pro.id, db);
                    firsttime = true;
                }

                //ELEMENTEO DEL MENU
                if (f.Contains("MenuID_"))
                {
                    string sp = f.Split('_')[1];
                    if (sp != "")
                    {
                        //inserts
                        int id = int.Parse(sp);
                        BaseMenu m = db.BaseMenus.Find(id);
                        BaseProfileMenu bpm = new BaseProfileMenu();
                        bpm.menuID = m.id;
                        bpm.userProfileID = pro.id;
                        bpm.noOrder = increment;
                        db.BaseProfileMenus.Add(bpm);
                        increment++;
                    }
                }
                else if (f.Contains("ActionID_"))
                {
                    string sp = f.Split('_')[1];
                    if (sp != "")
                    {
                        //inserts
                        int id = int.Parse(sp);
                        BaseAction a = db.BaseActions.Find(id);
                        pro.BaseActions.Add(a);
                    }
                }
                else if (f.Contains("WidgetID_"))
                {
                    string sp = f.Split('_')[1];
                    if (sp != "")
                    {
                        //inserts
                        int id = int.Parse(sp);
                        BaseWidget a = db.BaseWidgets.Find(id);
                        pro.BaseWidgets1.Add(a);
                    }
                }
                else if (f.Contains("GraphID_"))
                {
                    string sp = f.Split('_')[1];
                    if (sp != "")
                    {
                        //inserts
                        int id = int.Parse(sp);
                        BaseGraph a = db.BaseGraphs.Find(id);
                        pro.BaseGraphs.Add(a);
                    }
                }
            }
            db.SaveChanges();

            string parameters = "?id=" + pro.id;
            string url = URLHelper.getUrl("BasePermissions", "Index2") + parameters;
            return Redirect(url);
        }

        public ActionResult SaveActions(FormCollection form)
        {
            int idProfile = int.Parse(form["id"]);
            var pro = db.BaseProfiles.Find(idProfile);
            bool firsttime = false;

            foreach (var f in form.AllKeys)
            {
                if (!firsttime)
                {
                    Helper.executeNonQUery("Delete from dbo.BaseProfileAction where profileID = " + pro.id, db);
                    firsttime = true;
                }

                if (f.Contains("ActionID_"))
                {
                    string sp = f.Split('_')[1];
                    if (sp != "")
                    {
                        //inserts
                        int id = int.Parse(sp);
                        BaseAction a = db.BaseActions.Find(id);
                        pro.BaseActions.Add(a);
                    }
                }
            }
            db.SaveChanges();

            string parameters = "?id=" + pro.id;
            string url = URLHelper.getUrl("BasePermissions", "Index2") + parameters;
            return Redirect(url);
        }

        public ActionResult SaveMenus(FormCollection form)
        {
            int idProfile = int.Parse(form["id"]);
            var pro = db.BaseProfiles.Find(idProfile);
            bool firsttime = false;
            int increment = 0;

            foreach (var f in form.AllKeys)
            {
                if (!firsttime)
                {
                    Helper.executeNonQUery("Delete from dbo.BaseProfileMenu where userProfileID = " + pro.id, db);
                    firsttime = true;
                }

                //ELEMENTEO DEL MENU
                if (f.Contains("MenuID_"))
                {
                    string sp = f.Split('_')[1];
                    if (sp != "")
                    {
                        //inserts
                        int id = int.Parse(sp);
                        BaseMenu m = db.BaseMenus.Find(id);
                        BaseProfileMenu bpm = new BaseProfileMenu();
                        bpm.menuID = m.id;
                        bpm.userProfileID = pro.id;
                        bpm.noOrder = increment;
                        db.BaseProfileMenus.Add(bpm);
                        increment++;
                    }
                }
            }
            db.SaveChanges();

            string parameters = "?id=" + pro.id;
            string url = URLHelper.getUrl("BasePermissions", "Index2") + parameters;
            return Redirect(url);
        }

        public ActionResult ListaAcciones(int idProfile = 0)
        {
            var perfilData = db.BaseProfiles.Find(idProfile);
            return PartialView(perfilData);
        }

        public ActionResult ListaMenus(int idProfile = 0)
        {
            var perfilData = db.BaseProfiles.Find(idProfile);
            return PartialView(perfilData);
        }

        public ActionResult ListaMenus2(int idProfile = 0)
        {
            var perfilData = db.BaseProfiles.Find(idProfile);

            StringBuilder html = new StringBuilder();

            html.Append("<div class=''>");
            html.Append("<input id='search' type='text' class='form-control pull-right buscador-tabs' placeholder='Search' />");
            html.Append("<ul class='list-group list-group-root well' id='menuListGroup'>");

            MenusHtml(perfilData, html: html);

            html.Append("</ul>");
            html.Append("</div>");

            string realHtml = html.ToString();

            ViewBag.Html = realHtml;

            return PartialView(perfilData);
        }

        public ActionResult ListaAcciones2(int idProfile = 0)
        {
            var perfilData = db.BaseProfiles.Find(idProfile);

            StringBuilder html = new StringBuilder();

            html.Append("<div class=''>");
            html.Append("<input id='search2' type='text' class='form-control pull-right buscador-tabs' placeholder='Search' />");
            html.Append("<ul class='list-group list-group-root well' id='actionListGroup'>");

            ActionsHtml(perfilData, html: html);

            html.Append("</ul>");
            html.Append("</div>");

            string realHtml = html.ToString();

            ViewBag.Html = realHtml;

            return PartialView(perfilData);
        }

        public ActionResult ListaWidgets(int idProfile = 0)
        {
            var perfilData = db.BaseProfiles.Find(idProfile);
            return PartialView(perfilData);
        }

        #region Html Helpers

        private void MenusHtml(BaseProfile model, BaseUser userMind = null, List<BaseMenu> allUserMenus = null, List<BaseMenu> menus = null, StringBuilder html = null, bool isNullMenuId = true, int parentId = 0)
        {
            if (html == null)
            {
                html = new StringBuilder();
            }

            if (userMind == null)
            {
                userMind = Helper.GetUser(db);
            }

            if (allUserMenus == null)
            {
                var fromDirect = db.BaseUserMenus.Where(d => d.userID == userMind.ID).ToList();

                foreach (var item in fromDirect)
                {
                    allUserMenus.Add(item.BaseMenu);
                }

                foreach (var item in userMind.BaseProfiles)
                {
                    foreach (var items in item.BaseProfileMenus)
                    {
                        allUserMenus.Add(items.BaseMenu);
                    }
                }

                if (allUserMenus == null)
                {
                    allUserMenus = new List<BaseMenu>();
                }
            }

            if (menus == null)
            {
                menus = db.BaseMenus
                    .Where(q => q.hidden.HasValue && !q.hidden.Value)
                    .OrderBy(x => x.noOrder)
                    .ToList();

                if (menus == null)
                {
                    menus = new List<BaseMenu>();
                }
            }

            foreach (var item in menus)
            {
                if (allUserMenus.Where(d => d.id == item.id).Count() > 0 || userMind.superUser)
                {
                    if (item.menuID == null || !isNullMenuId)
                    {
                        string tituloMenu = (string.IsNullOrEmpty(item.realTitle)) ? item.realTitle : item.realText;

                        var hijos = item.BaseMenu1.Where(x => x.menuID == item.id).ToList();
                        var hijosAny = hijos.Any();

                        string check = "";
                        if (item.BaseProfileMenus.FirstOrDefault(x => x.menuID == item.id && x.userProfileID == model.id) != null)
                        {
                            check = "checked=\"checked\"";
                        }

                        if (parentId == 0)
                        {
                            html.AppendFormat("<li class='list-group-item canSearch width-100 titulos-tab' data-id='{0}'>", item.id);
                        }
                        else
                        {
                            html.AppendFormat("<li class='list-group-item canSearch width-100' data-parent='{0}'>", parentId);
                        }

                        html.AppendFormat("<input type='checkbox' name='MenuID_{0}' id='MenuID_{0}' {1} class='ckx' /> {2}", item.id, check, tituloMenu);
                        html.Append("</li>");

                        if (hijosAny)
                        {
                            html.Append("<ul class='list-group'>");
                            MenusHtml(model, userMind, allUserMenus, hijos, html, false, item.id);
                            html.Append("</ul>");
                        }
                    }
                }
            }
        }

        private void ActionsHtml(BaseProfile model, BaseUser userMind = null, List<Models.BaseController> allControllers = null, StringBuilder html = null)
        {
            if (html == null)
            {
                html = new StringBuilder();
            }

            if (userMind == null)
            {
                userMind = Helper.GetUser(db);
            }

            if (allControllers == null)
            {
                allControllers = db.BaseControllers.Where(d => !string.IsNullOrEmpty(d.infoName)).OrderBy(d => d.infoName).ToList();

                if (allControllers == null)
                {
                    allControllers = new List<Models.BaseController>();
                }
            }

            foreach (var item in allControllers)
            {
                string tituloController = (item.infoName != "") ? item.infoName : item.name;

                var children = item.BaseActions;
                var anyChildren = children.Any();

                html.AppendFormat("<li class='list-group-item canSearch width-100 titulos-tab' data-id='{0}'>{1}</li>", item.id, tituloController);

                if (anyChildren)
                {
                    html.AppendFormat("<ul class='list-group' data-parent='{0}'>", item.id);

                    html.Append("<li class='list-group-item width-100'>");

                    html.AppendFormat("<input type='checkbox' onclick='checkFuntion(\"{0}\", this)' class='ckxact' /> Marcar todo", item.id);

                    html.Append("</li>");

                    foreach (var child in children)
                    {
                        if (new MenuAuthorize().HasPermission(child.name, child.BaseController.name) == MenuAuthorize.AccessPermission.Grant)
                        {
                            string check = "";

                            if (model.BaseActions.FirstOrDefault(x => x.id == child.id) != null)
                            {
                                check = "checked='checked'";
                            }

                            string displayName = ((child.displayName != "") ? child.displayName : child.name);
                            html.Append("<li class='list-group-item width-100'>");

                            html.AppendFormat("<input type='checkbox' name='ActionID_{0}' id='ActionID_{0}' {1} class='ckx classCheck{2}' /> {3}", child.id, check, item.id, displayName);

                            html.Append("</li>");
                        }
                    }

                    html.Append("</ul>");
                }
            }
        }

        #endregion
    }
}