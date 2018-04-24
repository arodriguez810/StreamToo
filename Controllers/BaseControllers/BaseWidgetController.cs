using Admin.BaseClass;
using Admin.BaseClass.UI;
using Admin.CustomCode;
using Admin.Models;
using System.Data.Entity;
using System.Web.Mvc;

namespace Admin.Controllers
{
    public class BaseWidgetController : BaseController
    {
        //
        // GET: /BaseWidget/

        public ActionResult Index()
        {
            return PartialView(new BaseWidget());
        }
        public ActionResult Form(int id = 0)
        {
            if (id == 0)
                return PartialView(new BaseWidget());
            else
            {
                BaseWidget model = db.BaseWidgets.Find(id);
                return PartialView(model);
            }
        }
        [HttpPost]
        public JsonResult Save(BaseWidget model)
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
                db.BaseWidgets.Add(model);
                db.SaveChanges();
            }
            return Json(model.id);
        }
        public JsonResult Delete(int id = 0)
        {
            BaseWidget model = db.BaseWidgets.Find(id);
            if (model != null)
            {
                db.BaseWidgets.Remove(model);
                db.SaveChanges();
            } return Json("ok", JsonRequestBehavior.AllowGet);
        }
        public JsonResult data(FormCollection form)
        {
            JsonCsTableHelper table = new JsonCsTableHelper(db, form, Request.QueryString);
            table.TableName = "[BaseWidget]";
            table.PrimaryKey = "id";
            table.SearchCondition = " id like '%{0}%' OR name like '%{0}%' OR description like '%{0}%' OR actionID like '%{0}%' OR width like '%{0}%' OR actionContent like '%{0}%' OR html like '%{0}%' OR";
            table.VarName = "csBaseWidget";
            table.ReferenceText = "BaseWidget";
            table.Links.Add(new CsTableLink() { ButtonType = CsTableLinkType.button, Href = URLHelper.getActionUrl("BaseWidget", "Form"), Title = "Edit", Css = "btn btn-warning btn-circle refreshTable", Icon = BaseUIIconText.fa.fa_fa_pencil });
            table.Links.Add(new CsTableLink() { ButtonType = CsTableLinkType.deleteButton, Href = URLHelper.getActionUrl("BaseWidget", "Delete"), Title = "Delete", Css = "btn btn-danger btn-circle refreshTable", Icon = BaseUIIconText.fa.fa_fa_trash_o });
            table.buildDataTable();
            table.buildDefaultJson();
            table.buildLinks();
            return Json(table.Json, JsonRequestBehavior.AllowGet);
        }

    }
}
