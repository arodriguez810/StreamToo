using System.Web.Mvc;


namespace Admin.Controllers
{
    [AllowAnonymous]
    public class BaseFormGeneratorController : BaseController
    {

        public ActionResult Index()
        {
            return PartialView();
        }
    }
}
