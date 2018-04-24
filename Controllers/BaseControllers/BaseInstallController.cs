using Admin.BaseModels.ViewModels;
using System.Data.Entity;
using System.Web.Mvc;


namespace Admin.Controllers
{
    [AllowAnonymous]
    public class BaseInstallController : BaseController
    {

        public ActionResult Index()
        {
            Install install = new Install();
            return View(install);
        }

        [HttpPost]
        public JsonResult Index(Admin.Models.BaseController model)
        {
            if (model.id != 0)
            {
                if (ModelState.IsValid)
                {
                    db.Entry(model).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
            else
            {
                db.BaseControllers.Add(model);
                db.SaveChanges();
            }
            return Json(model.id);
        }
    }
}
