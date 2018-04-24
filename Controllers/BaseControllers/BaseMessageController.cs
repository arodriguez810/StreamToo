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
    public class BaseMessageController : BaseController
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
            ViewBag.tableName = "BaseMessage";
            return PartialView(new BaseMessage());
        }
        [IsView]
        public ActionResult Form(int id = 0)
        {
            if (id == 0)
                return PartialView(new BaseMessage());
            else
            {
                BaseMessage model = db.BaseMessages.Find(id);
                return PartialView(model);
            }
        }
        [HttpPost]
        [ValidateInput(false)]
        public JsonResult Save(BaseMessage model)
        {
            try
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
                    db.BaseMessages.Add(model);
                    db.SaveChanges();
                }
                if (Request.Files.Count > 0)
                {
                }
                db.SaveChanges();
                return Json(new { id = model.id, MessageSucess = "That Se Message saved successfully." });
            }
            catch (Exception ex)
            {
                return Json(new { Message = Helper.ModeralException(ex).Replace("@table", "Se Message") });
            }
        }
        public JsonResult Delete(int id = 0)
        {
            try
            {
                BaseMessage model = db.BaseMessages.Find(id);
                if (model != null)
                {
                    db.BaseMessages.Remove(model);
                    db.SaveChanges();
                    return Json(new { id = model.id, MessageSucess = "That Se Message deleted successfully." }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { Message = "This record no longer exists" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Message = Helper.ModeralException(ex).Replace("@table", "Se Message") }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult data(FormCollection form, string filter = "")
        {
            JsonCsTableHelper table = new JsonCsTableHelper(db, form, Request.QueryString);
            table.TableName = "[BaseMessage]";
            table.PrimaryKey = "id";
            table.ReplacedText.Add("id", "Id");
            table.ReplacedText.Add("code", "Code");
            table.ReplacedText.Add("message", "Message");
            table.ReplacedText.Add("messageCategory", "Message Category");
            table.SearchCondition = " id like '%{0}%' OR code like '%{0}%' OR message like '%{0}%' OR messageCategory like '%{0}%' ";
            table.VarName = "csBaseMessage";
            table.ReferenceText = "Message";
            table.Links.Add(new CsTableLink() { ButtonType = CsTableLinkType.button, Href = URLHelper.getActionUrl("BaseMessage", "Form"), Title = "Edit", Css = "btn btn-warning btn-circle refreshTable", Icon = BaseUIIconText.fa.fa_fa_pencil });
            table.Links.Add(new CsTableLink() { ButtonType = CsTableLinkType.deleteButton, Href = URLHelper.getActionUrl("BaseMessage", "Delete"), Title = "Delete", Css = "btn btn-danger btn-circle refreshTable", Icon = BaseUIIconText.fa.fa_fa_trash_o });
            table.buildDataTable(filter);
            foreach (DataRow item in table.Data.Rows)
            {
                if (item["messageCategory"] != null)
                {
                    if (item["messageCategory"].ToString() != "")
                    {
                        int messageCategory = int.Parse(item["messageCategory"].ToString());
                        item["messageCategory"] = db.BaseMessageCategories.FirstOrDefault(d => d.id == messageCategory).name;
                    }
                    else
                        item["messageCategory"] = "N/A";
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
            wb.Worksheets.Add(data, "BaseMessage");
            using (var memoryStream = new MemoryStream())
            {
                wb.SaveAs(memoryStream);
                byte[] byteInfo = memoryStream.ToArray();
                return File(byteInfo, System.Net.Mime.MediaTypeNames.Application.Octet, String.Format("BaseMessage BackOffice {0}.xlsx", DateTime.Now.ToShortDateString()));
            }
        }
    }
}
