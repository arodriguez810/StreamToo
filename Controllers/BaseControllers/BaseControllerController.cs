using Admin.BaseClass;
using Admin.BaseClass.UI;
using Admin.CustomCode;
using Admin.Models;
using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace Admin.Controllers
{
    public class BaseControllerController : BaseController
    {
        [IsView]
        public ActionResult Index()
        {
            //Para obtener columnas apagadas por Default
            List<BaseDynamicColumnList> bDynamicColumn = db.BaseDynamicColumnLists.ToList();
            List<BaseDynamicList> bDynamicList = db.BaseDynamicLists.ToList();
            ViewBag.column = bDynamicColumn;
            ViewBag.dynamicList = bDynamicList;
            //Nombre de la tabla o vista
            ViewBag.tableName = "BaseController";
            return PartialView(new Admin.Models.BaseController());
        }
        [IsView]
        public ActionResult Form(int id = 0)
        {
            if (id == 0)
                return PartialView(new Admin.Models.BaseController());
            else
            {
                Admin.Models.BaseController model = db.BaseControllers.Find(id);
                return PartialView(model);
            }
        }
        [HttpPost]
        public JsonResult Save(Admin.Models.BaseController model)
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
        public JsonResult Delete(int id = 0)
        {
            Admin.Models.BaseController model = db.BaseControllers.Find(id);
            if (model != null)
            {
                db.BaseControllers.Remove(model);
                db.SaveChanges();
            }
            return Json("ok", JsonRequestBehavior.AllowGet);
        }
        public JsonResult data(FormCollection form, string filter = "")
        {
            JsonCsTableHelper table = new JsonCsTableHelper(db, form, Request.QueryString);
            table.TableName = "[BaseController]";
            table.PrimaryKey = "id";
            table.ReplacedText.Add("id", "Id");
            table.ReplacedText.Add("name", "Name");
            table.ReplacedText.Add("fullName", "Full Name");
            table.ReplacedText.Add("infoName", "Info Name");
            table.ReplacedText.Add("Discriminator", "Discriminator");
            table.SearchCondition = " id like '%{0}%' OR name like '%{0}%' OR fullName like '%{0}%' OR infoName like '%{0}%' OR Discriminator like '%{0}%' ";
            table.VarName = "csBaseController";
            table.ReferenceText = "seController";
            table.Links.Add(new CsTableLink() { ButtonType = CsTableLinkType.link, Href = URLHelper.getUrl("BaseController", "Form"), Title = "Edit", Css = "btn btn-warning btn-circle refreshTable", Icon = BaseUIIconText.fa.fa_fa_pencil });
            table.Links.Add(new CsTableLink() { ButtonType = CsTableLinkType.deleteButton, Href = URLHelper.getActionUrl("BaseController", "Delete"), Title = "Delete", Css = "btn btn-danger btn-circle refreshTable", Icon = BaseUIIconText.fa.fa_fa_trash_o });
            table.buildDataTable(filter);
            table.buildDefaultJson();
            table.buildLinks();
            return Json(table.Json, JsonRequestBehavior.AllowGet);
        }
        public FileResult getCSV()
        {
            HttpRequestBase req = this.Request; //Parametros GET
            string consulta = crearConsulta(req); //String que almacena toda la consulta

            var wb = new XLWorkbook();
            DataTable data = Helper.getData(consulta, db);
            wb.Worksheets.Add(data, "BaseController");
            using (var memoryStream = new MemoryStream())
            {
                wb.SaveAs(memoryStream);
                byte[] byteInfo = memoryStream.ToArray();
                return File(byteInfo, System.Net.Mime.MediaTypeNames.Application.Octet, String.Format("BaseController BackOffice {0}.xlsx", DateTime.Now.ToShortDateString()));
            }
        }
    }
}
