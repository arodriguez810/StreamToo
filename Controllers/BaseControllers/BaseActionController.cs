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
    public class BaseActionController : BaseController
    {
        [IsView]
        public ActionResult Index(int id = 0)
        {
            //Para obtener columnas apagadas por Default
            List<BaseDynamicColumnList> bDynamicColumn = db.BaseDynamicColumnLists.ToList();
            List<BaseDynamicList> bDynamicList = db.BaseDynamicLists.ToList();
            ViewBag.column = bDynamicColumn;
            ViewBag.dynamicList = bDynamicList;
            //Nombre de la tabla o vista
            ViewBag.tableName = "BaseAction";
            return PartialView(new BaseAction() { controllerID = id });
        }
        [IsView]
        public ActionResult Form(int id = 0, int controllerID = 0)
        {
            if (id == 0)
                return PartialView(new BaseAction() { controllerID = controllerID });
            else
            {
                BaseAction model = db.BaseActions.Find(id);
                return PartialView(model);
            }
        }
        [HttpPost]
        public JsonResult Save(BaseAction model)
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
                db.BaseActions.Add(model);
                db.SaveChanges();
            }
            return Json(model.id);
        }
        public JsonResult Delete(int id = 0)
        {
            BaseAction model = db.BaseActions.Find(id);
            if (model != null)
            {
                db.BaseActions.Remove(model);
                db.SaveChanges();
            } return Json("ok", JsonRequestBehavior.AllowGet);
        }
        public JsonResult data(FormCollection form, int controllerIDT = 0)
        {
            JsonCsTableHelper table = new JsonCsTableHelper(db, form, Request.QueryString);
            table.TableName = "[BaseAction]";
            table.PrimaryKey = "id";
            string[] hides = table.hideColumns;
            List<string> tohide = hides.ToList();
            tohide.Add("controllerID");
            table.hideColumns = tohide.ToArray();
            if (controllerIDT != 0)
                table.AdditionalWhere = " controllerID=" + controllerIDT;
            table.ReplacedText.Add("id", "Id");
            table.ReplacedText.Add("controllerID", "Controller I D");
            table.ReplacedText.Add("name", "Name");
            table.ReplacedText.Add("displayName", "Display Name");
            table.ReplacedText.Add("isSystem", "Is System");
            table.SearchCondition = " id like '%{0}%' OR controllerID like '%{0}%' OR name like '%{0}%' OR displayName like '%{0}%' OR isSystem like '%{0}%' OR";
            table.VarName = "csBaseAction";
            table.ReferenceText = "seAction";
            table.Links.Add(new CsTableLink() { ButtonType = CsTableLinkType.button, Href = URLHelper.getActionUrl("BaseAction", "Form"), Title = "Edit", Css = "btn btn-warning btn-circle refreshTable", Icon = BaseUIIconText.fa.fa_fa_pencil });
            table.Links.Add(new CsTableLink() { ButtonType = CsTableLinkType.deleteButton, Href = URLHelper.getActionUrl("BaseAction", "Delete"), Title = "Delete", Css = "btn btn-danger btn-circle refreshTable", Icon = BaseUIIconText.fa.fa_fa_trash_o });
            table.buildDataTable();
            foreach (DataRow item in table.Data.Rows)
            {
                if (item["isSystem"] != null)
                {
                    item["isSystem"] = (item["isSystem"].ToString() == "1" || item["isSystem"].ToString() == "True") ? "Yes" : "No";
                }
            }
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
            wb.Worksheets.Add(data, "BaseAction");
            using (var memoryStream = new MemoryStream())
            {
                wb.SaveAs(memoryStream);
                byte[] byteInfo = memoryStream.ToArray();
                return File(byteInfo, System.Net.Mime.MediaTypeNames.Application.Octet, String.Format("BaseAction BackOffice {0}.xlsx", DateTime.Now.ToShortDateString()));
            }
        }
    }
}
