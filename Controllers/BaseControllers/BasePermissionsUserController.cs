using Admin.BaseClass;
using Admin.BaseClass.App;
using Admin.CustomCode;
using Admin.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;
namespace Admin.Controllers
{
    [CustomAuthorize]
    public class BasePermissionsUserController : BaseController
    {
        //
        // GET: /BasePermissions/
        [IsView]
        public ActionResult Index(int id = 0)
        {
            var UserData = db.BaseUsers.Find(id);
            if (UserData != null)
            {
                return PartialView(UserData);
            }
            else { return HttpNotFound(); }
        }

        [HttpPost]
        public ActionResult SaveData(FormCollection form)
        {
            int userID = int.Parse(form["ID"]);
            var us = db.BaseUsers.Find(userID);
            bool firsttime = false;
            int increment = 0;
            List<BaseUserAction> luserActions = new List<BaseUserAction>();
            foreach (var f in form.AllKeys)
            {
              
                if (!firsttime)
                {
                    Helper.executeNonQUery("Delete from dbo.BaseWidgetUser where UserID = " + us.ID, db);
                    Helper.executeNonQUery("Delete from dbo.BaseUserMenu where userID = " + us.ID, db);
                    Helper.executeNonQUery("Delete from dbo.BaseUserProfile where userID = " + us.ID, db);
                    var beforeDelete = db.BaseUserActions.Where(x => x.userID == us.ID);
                    foreach (var item in beforeDelete)
                    {
                        luserActions.Add(new BaseUserAction()
                        {
                            actionID = item.actionID,
                            userID = item.userID,
                            forever = item.forever,
                            untilDate = item.untilDate,
                            password = item.password,
                            passwordAccess = item.passwordAccess,
                            leftSeconds = item.leftSeconds
                        });
                    }
                    Helper.executeNonQUery("Delete from dbo.BaseUserAction where userID = " + us.ID, db);

                    Helper.executeNonQUery("Delete from dbo.BaseUserGraph where UserID = " + us.ID, db);
                    firsttime = true;
                }
                db = new Context();

                //ELEMENTEO DEL MENU
                if (f.Contains("MenuID_"))
                {
                    string sp = f.Split('_')[1];
                    if (sp != "")
                    {
                        //inserts
                        int id = int.Parse(sp);
                        BaseMenu m = db.BaseMenus.Find(id);
                        BaseUserMenu bpm = new BaseUserMenu();
                        bpm.menuID = m.id;
                        bpm.userID = us.ID;
                        bpm.noOrder = increment;
                        db.BaseUserMenus.Add(bpm);
                        increment++;
                        db.SaveChanges();
                    }
                }
                else if (f.Contains("ActionID_"))
                {
                    string sp = f.Split('_')[1];
                    if (sp != "")
                    {
                        //inserts
                        int id = int.Parse(sp);

                        BaseUserAction ac = new BaseUserAction();
                        var existOnList = luserActions.FirstOrDefault(x => x.userID == us.ID && x.actionID == id);

                        ac.actionID = (existOnList != null) ? existOnList.actionID : id;
                        ac.userID = (existOnList != null) ? existOnList.userID : us.ID;
                        ac.forever = (existOnList != null) ? existOnList.forever : true;
                        ac.untilDate = (existOnList != null) ? existOnList.untilDate : null;
                        ac.password = (existOnList != null) ? existOnList.password : null;
                        ac.passwordAccess = (existOnList != null) ? existOnList.passwordAccess : null;
                        ac.leftSeconds = (existOnList != null) ? existOnList.leftSeconds : null;

                        db.BaseUserActions.Add(ac);
                        db.SaveChanges();
                    }
                }
                else if (f.Contains("WidgetID_"))
                {
                    string sp = f.Split('_')[1];
                    if (sp != "")
                    {
                        //inserts
                        int id = int.Parse(sp);
                        Helper.executeNonQUery(string.Format("insert into dbo.WidgetUser " +
                                               "(UserID,WidgetID) VALUES({0},{1})", us.ID, id), db);
                        //BaseWidget a = pp.BaseWidgets.Find(id);
                        //ii.BaseWidgets1.Add(a);
                        //pp.SaveChanges();
                    }
                }
                else if (f.Contains("GraphID_"))
                {
                    string sp = f.Split('_')[1];
                    if (sp != "")
                    {
                        //inserts
                        int id = int.Parse(sp);
                        Helper.executeNonQUery(string.Format("insert into dbo.UserGraph " +
                                               "(GraphsID,UserID) VALUES({1},{0})", us.ID, id), db);
                        //Graph a = db.Graphs.Find(id);
                        //us.Graphs.Add(a);
                        //db.SaveChanges();
                    }
                }
            }
            db = new Context();
            if (!string.IsNullOrEmpty(form["userProfileID"]))
            {
                foreach (var item in form["userProfileID"].Split(','))
                {
                    int i = int.Parse(item);
                    BaseProfile p = db.BaseProfiles.Find(i);
                    if (p != null)
                    {
                        //us.BaseProfiles.Add(p);
                        //db.SaveChanges();
                        Helper.executeNonQUery(string.Format("insert into dbo.BaseUserProfile " +
                                               "(profileID,userID) VALUES({1},{0})", us.ID, i), db);
                    }
                }
            }
            //db.SaveChanges();

            string parameters = "?id=" + us.ID;
            string url = URLHelper.getUrl("BasePermissionsUser", "Index") + parameters;
            return Redirect(url);
        }

        public ActionResult actionsProperties(int actionid = 0, int userid = 0)
        {
            var actiondata = db.BaseActions.Find(actionid);
            ViewBag.userid = userid;
            return PartialView(actiondata);
        }
        [HttpPost]
        public JsonResult actionsProperties(FormCollection form)
        {
            try
            {
                int idaction = int.Parse(form["id"]);
                int user = int.Parse(form["userid"]);
                var actiondata = db.BaseActions.Find(idaction);
                if (actiondata != null)
                {
                    var k = form["forever"].Split(',')[0];
                    var s = form["passwordAccess"].Split(',')[0];
                    bool pp = Convert.ToBoolean(k);

                    var exis = db.BaseUserActions.FirstOrDefault(x => x.userID == user && x.actionID == actiondata.id);
                    if (exis != null)
                    {
                        if (form["password"].ToString() == Permission.defaultShowPassword)
                        {
                            exis.password = Helper.getData("SELECT password from [BaseUserAction] where actionID=" + exis.actionID, db).Rows[0][0].ToString();
                        }
                        else
                        {
                            exis.password = Permission.CalculateMD5Hash(form["password"]);
                        }

                        exis.forever = pp;
                        exis.passwordAccess = Convert.ToBoolean(s);
                        exis.untilDate = Convert.ToDateTime(form["untilDate"]);
                        //exis.leftSeconds = int.Parse(form["leftSeconds"]);

                        exis.actionID = actiondata.id;
                        exis.userID = user;
                    }
                    else
                    {
                        BaseUserAction bu = new BaseUserAction();
                        bu.forever = pp;
                        bu.password = Permission.CalculateMD5Hash(form["password"]);
                        bu.passwordAccess = Convert.ToBoolean(s);
                        bu.untilDate = Convert.ToDateTime(form["untilDate"]);
                        bu.leftSeconds = int.Parse(form["leftSeconds"]);

                        bu.actionID = actiondata.id;
                        bu.userID = user;

                        db.BaseUserActions.Add(bu);
                    }
                    db.SaveChanges();
                }
                return Json("Ok");
            }
            catch (Exception ex)
            {
                return Json(new { Message = "Error " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult ListaAcciones(int idUser = 0)
        {
            var UserData = db.BaseUsers.Find(idUser);
            return PartialView(UserData);
        }
        public ActionResult ListaMenus(int idUser = 0)
        {
            var UserData = db.BaseUsers.Find(idUser);
            return PartialView(UserData);
        }
        public ActionResult ListaWidgets(int idUser = 0)
        {
            var UserData = db.BaseUsers.Find(idUser);
            return PartialView(UserData);
        }

    }
}
