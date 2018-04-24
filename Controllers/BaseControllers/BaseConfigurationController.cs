using Admin.BaseModels.ViewModels;
using Admin.CustomCode;
using System.IO;
using System.Web;
using System.Web.Mvc;
using WebMatrix.WebData;

namespace Admin.Controllers
{
    [CustomAuthorize]
    public class BaseConfigurationController : BaseController
    {
        public ActionResult Index()
        {
            return PartialView(new BaseConfiguration());
        }

        [HttpPost]
        public JsonResult Save(BaseConfiguration model)
        {

            HttpPostedFileBase file = null;
            if (Request.Files.Count > 0)
            {
                file = Request.Files["logo"];
                if (file != null && file.ContentLength > 0)
                    file.SaveAs(Server.MapPath("~/Uploads/ApplicationLogos/default.png"));

                file = Request.Files["favicon"];
                if (file != null && file.ContentLength > 0)
                    file.SaveAs(Server.MapPath("~/Uploads/ApplicationLogos/favicon.png"));
            }
            try
            {
                BaseConfiguration config = new BaseConfiguration();
                config = model;
                return Json(new { MessageSucess = "Saved Successfully" });
            }
            catch (System.Exception ex)
            {
                return Json(new { Message = ex.Message });
            }

        }

        public JsonResult TestEmail()
        {
            try
            {
                BaseConfiguration model = new BaseConfiguration();
                SendMail mail = new SendMail();
                mail.Body = $"Test from {model.appName}";
                mail.Subject = $"Test from {model.appName}";
                mail.To.Add(db.BaseUsers.Find(WebSecurity.CurrentUserId).username);
                mail.Send();
                return Json(new { MessageSucess = "Test sent successfully" }, JsonRequestBehavior.AllowGet);
            }
            catch (System.Exception ex)
            {
                return Json(new { Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}

