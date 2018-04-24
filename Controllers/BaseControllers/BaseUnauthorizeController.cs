using Admin.BaseClass;
using Admin.BaseClass.App;
using Admin.CustomCode;
using System.Linq;
using System.Web.Mvc;
using Admin.Models;

namespace Admin.Controllers
{

    public class BaseUnauthorizeController : BaseController
    {
        //
        // GET: /Unauthorize/

        public ActionResult UserStatus()
        {
            return View();
        }

        public ActionResult ErrorUnauthorized()
        {
            return PartialView();
        }

        public JsonResult ErrorUnauthorizedJson()
        {
            return Json(new { Message = "Sorry,  you do not have permission for this option." }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Expired()
        {
            return PartialView();
        }

        public ActionResult needPassword()
        {
            return PartialView();
        }

        [HttpPost]
        public ActionResult needPassword(FormCollection form)
        {
            BaseUser user = Helper.GetUser(db);
            BaseUserAction action = user.getOneBaseUserAction(user.tryAction, user.tryController);
            if (action != null)
            {
                if (form["password"] != null)
                {
                    string password = Permission.CalculateMD5Hash(form["password"]);
                    if (action.password.ToUpper() == password.ToString())
                    {
                        BaseUserAction Baction = db.BaseUserActions.FirstOrDefault(d => d.actionID == action.actionID && d.userID == user.ID);
                        if (Baction != null)
                        {
                            Baction.leftSeconds = 1;
                            db.SaveChanges();
                        }
                        return Redirect(URLHelper.getUrl(user.tryController, user.tryAction));
                    }
                    else
                    {
                        GlobalsViewBag.Add("error", "Contraseña Incorrecta.");
                        return Redirect(URLHelper.getCurrentUrl(this.ControllerContext));
                    }
                }
            }
            else
            {
                return Redirect(URLHelper.getCurrentUrl(this.ControllerContext));
            }
            return Redirect(URLHelper.getCurrentUrl(this.ControllerContext));
        }

    }
}
