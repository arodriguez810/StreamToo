using Admin.BaseClass;
using Admin.BaseClass.UI;
using Admin.CustomCode;
using Admin.Models;
using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace Admin.Controllers
{
    public class BaseViewRelationController : BaseController
    {
        [IsView]
        public ActionResult Index(int id = 0, string from = "")
        {
            List<BaseDynamicColumnList> bDynamicColumn = db.BaseDynamicColumnLists.ToList();
            List<BaseDynamicList> bDynamicList = db.BaseDynamicLists.ToList();
            ViewBag.column = bDynamicColumn;
            ViewBag.dynamicList = bDynamicList;
            ViewBag.from = from;
            ViewBag.tableName = "BaseViewRelation";
            return PartialView(new BaseViewRelation());
        }
        [IsView]
        public ActionResult Form(int id = 0, string from = "")
        {
            ViewBag.relations = db.VWISRElations.Where(d => d.PK_Table == "BaseViewRelation").ToList();
            ViewBag.from = from;
            if (id == 0)
                return PartialView(new BaseViewRelation());
            else
            {
                BaseViewRelation model = db.BaseViewRelations.Find(id);
                return PartialView(model);
            }
        }
        [HttpPost]
        [ValidateInput(false)]
        public JsonResult Save(BaseViewRelation model)
        {
            try
            {
                if (model.id != 0)
                {
                    BoolString validation = model.BeforeEdit(db);
                    if (validation.BoolValue)
                        return Json(new { Message = validation.StringValue });
                    db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    validation = model.AfterEdit(db);
                    if (validation.BoolValue)
                        return Json(new { Message = validation.StringValue });
                }
                else
                {
                    BoolString validation = model.BeforeSave(db);
                    if (validation.BoolValue)
                        return Json(new { Message = validation.StringValue });
                    db.BaseViewRelations.Add(model);
                    db.SaveChanges();
                    validation = model.AfterSave(db);
                    if (validation.BoolValue)
                        return Json(new { Message = validation.StringValue });
                }
                return Json(new { id = model.id, MessageSucess = "That View Relation saved successfully." });
            }
            catch (Exception ex)
            {
                return Json(new { Message = Helper.ModeralException(ex).Replace("@table", "View Relation") });
            }
        }
        public JsonResult Delete(int id = 0)
        {
            try
            {
                BaseViewRelation model = db.BaseViewRelations.Find(id);
                if (model != null)
                {
                    BoolString validation = model.BeforeDelete(db);
                    if (validation.BoolValue)
                        return Json(new { Message = validation.StringValue }, JsonRequestBehavior.AllowGet);
                    db.BaseViewRelations.Remove(model);
                    db.SaveChanges();
                    validation = model.AfterDelete(db);
                    if (validation.BoolValue)
                        return Json(new { Message = validation.StringValue }, JsonRequestBehavior.AllowGet);
                    return Json(new { id = model.id, MessageSucess = "That View Relation deleted successfully." }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { Message = "This record no longer exists" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Message = Helper.ModeralException(ex).Replace("@table", "View Relation") }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult data(FormCollection form, string from = "", string filter = "")
        {
            JsonCsTableHelper table = new JsonCsTableHelper(db, form, Request.QueryString);
            table.TableName = "[BaseViewRelation]";
            table.PrimaryKey = "id";
            string[] hides = table.hideColumns;
            List<string> tohide = hides.ToList();
            //tohide.Add("column");
            //tohide.Remove("creationDate");
            string extraRelation = "";
            if (!string.IsNullOrEmpty(from))
            {
                string[] relation = from.Split(':');
                table.AdditionalWhere = $"{relation[0]}='{relation[1]}'";
                extraRelation = $"?from={from}";
                tohide.Add(relation[0]);
            }
            table.hideColumns = tohide.ToArray();
            table.ReplacedText.Add("id", ("Id"));
            table.ReplacedText.Add("tableFrom", ("Table From"));
            table.ReplacedText.Add("fkColumn", ("Fk Column"));
            table.ReplacedText.Add("pkColumn", ("Pk Column"));
            table.ReplacedText.Add("pkColumnName", ("Pk Column Name"));
            table.ReplacedText.Add("tableTo", ("Table To"));
            table.SearchCondition = " id like '%{0}%'  OR  tableFrom like '%{0}%'  OR  fkColumn like '%{0}%'  OR  pkColumn like '%{0}%'  OR  pkColumnName like '%{0}%'  OR  tableTo like '%{0}%' ";
            table.VarName = "csBaseViewRelation";
            table.ReferenceText = "seViewRelation";
            table.Links.Add(new CsTableLink() { ButtonType = URLHelper.buttonType("BaseViewRelation"), Href = URLHelper.getActionUrlRelation("BaseViewRelation", "Form"), Title = "Edit", Css = "btn btn-warning btn-circle refreshTable", Icon = BaseUIIconText.fa.fa_fa_pencil, Extra = extraRelation });
            table.Links.Add(new CsTableLink() { ButtonType = CsTableLinkType.deleteButton, Href = URLHelper.getActionUrl("BaseViewRelation", "Delete"), Title = "Delete", Css = "btn btn-danger btn-circle refreshTable", Icon = BaseUIIconText.fa.fa_fa_trash_o });
            table.buildDataTable(filter);
            foreach (DataRow item in table.Data.Rows)
            {
            }
            table.buildDefaultJson();
            table.buildLinks();
            return Json(table.Json, JsonRequestBehavior.AllowGet);
        }
        public FileResult getCSV(string from = "")
        {
            FormCollection form = new FormCollection();
            foreach (string key in Request.QueryString.AllKeys)
                if (key != "limit" && key != "offset")
                    form.Add(key, Request.QueryString[key]);
            var wb = new XLWorkbook();
            JsonCsTableHelper table = new JsonCsTableHelper(db, form, Request.QueryString);
            form.Remove("limit");
            form.Remove("offset");
            table.TableName = "[BaseViewRelation]";
            table.PrimaryKey = "id";
            string[] hides = table.hideColumns;
            List<string> tohide = hides.ToList();
            //tohide.Add("column");
            //tohide.Remove("creationDate");
            if (!string.IsNullOrEmpty(from))
            {
                string[] relation = from.Split(':');
                table.AdditionalWhere = $"{relation[0]}='{relation[1]}'";
                tohide.Add(relation[0]);
            }
            table.hideColumns = tohide.ToArray();
            table.ReplacedText.Add("id", ("Id"));
            table.ReplacedText.Add("tableFrom", ("Table From"));
            table.ReplacedText.Add("fkColumn", ("Fk Column"));
            table.ReplacedText.Add("pkColumn", ("Pk Column"));
            table.ReplacedText.Add("pkColumnName", ("Pk Column Name"));
            table.ReplacedText.Add("tableTo", ("Table To"));
            table.SearchCondition = " id like '%{0}%'  OR  tableFrom like '%{0}%'  OR  fkColumn like '%{0}%'  OR  pkColumn like '%{0}%'  OR  pkColumnName like '%{0}%'  OR  tableTo like '%{0}%' ";
            table.VarName = "csBaseViewRelation";
            table.ReferenceText = "seViewRelation";
            table.Links.Add(new CsTableLink() { ButtonType = CsTableLinkType.button, Href = URLHelper.getActionUrl("BaseViewRelation", "Form"), Title = "Edit", Css = "btn btn-warning btn-circle refreshTable", Icon = BaseUIIconText.fa.fa_fa_pencil });
            table.Links.Add(new CsTableLink() { ButtonType = CsTableLinkType.deleteButton, Href = URLHelper.getActionUrl("BaseViewRelation", "Delete"), Title = "Delete", Css = "btn btn-danger btn-circle refreshTable", Icon = BaseUIIconText.fa.fa_fa_trash_o });
            if (Request.QueryString.AllKeys.Contains("filter"))
                table.buildDataTable(Request.QueryString["filter"]);
            else
                table.buildDataTable("");
            foreach (DataRow item in table.Data.Rows)
            {
            }
            wb.Worksheets.Add(table.Data, "View Relation");
            using (var memoryStream = new MemoryStream())
            {
                wb.SaveAs(memoryStream);
                byte[] byteInfo = memoryStream.ToArray();
                return File(byteInfo, System.Net.Mime.MediaTypeNames.Application.Octet, String.Format("View Relation Viya Spark {0}.xlsx", DateTime.Now.ToShortDateString()));
            }
        }
    }
}
