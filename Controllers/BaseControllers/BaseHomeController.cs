using Admin.BaseModels.ViewModels;
using Admin.CustomCode;
using Admin.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Web;
using System.Web.Mvc;
using WebMatrix.WebData;

namespace Admin.Controllers
{
    [CustomAuthorize]
    public class BaseHomeController : BaseController
    {

        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            System.Diagnostics.Debug.WriteLine(System.Threading.Thread.CurrentThread.CurrentCulture.Name);
            base.OnActionExecuted(filterContext);
        }

        [AllowAnonymous]
        public ActionResult Index()
        {
            #region  
            //if (Request.IsLocal)
            //    Helper.Autenticate(Server.MapPath("~"));
            #endregion
            if (WebSecurity.IsAuthenticated)
                return View();
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }
        [AllowAnonymous]
        public ActionResult blank(string action, string controller)
        {
            if (WebSecurity.IsAuthenticated)
                return View();
            else
            {
                return RedirectToAction(action, controller);
            }
        }
        [IsView]
        public ActionResult Dashboard()
        {
            ViewBag.db = db;
            return PartialView(db);
        }
        [HttpPost]
        public JsonResult Dashboard(int widSelect)
        {
            try
            {
                var us = db.BaseUsers.Find(WebSecurity.CurrentUserId);
                if (us != null)
                {
                    if (widSelect > 0)
                    {
                        int? val = widSelect;
                        us.widgetsToShow = val;
                        db.SaveChanges();
                    }
                }
                else
                {
                    return Json("Usuario Incorrecto");
                }

            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }

            return Json("Ok");
        }

        [AllowAnonymous]
        public JsonResult ReadStyles()
        {
            BaseConfiguration config = new BaseConfiguration();
            string[] styles =
            {
                "css/font-awesome.min.css",
                "css/smartadmin-production.css",
                "css/your_style.css",
                $"css/{config.css}.css",
                "js/plugin/datetimepicker/bootstrap-datetimepicker.css",
                "js/plugin/contextmenu/jquery.contextMenu.css",
                "js/plugin/contextmenu/theme-fixes.css",
                "css/font.css"
            };
            return new JsonResult { Data = styles, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

    }
}
