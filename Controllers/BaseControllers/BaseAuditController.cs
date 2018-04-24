using Admin.BaseClass;
using Admin.BaseClass.UI;
using Admin.CustomCode;
using Admin.Models;
using Newtonsoft.Json;
using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebMatrix.WebData;


namespace Admin.Controllers
{
    public class BaseAuditController : BaseController
    {
        [IsView]
        public ActionResult Index(int id = 0, string from = "")
        {
            List<BaseDynamicColumnList> bDynamicColumn = db.BaseDynamicColumnLists.ToList();
            List<BaseDynamicList> bDynamicList = db.BaseDynamicLists.ToList();
            ViewBag.column = bDynamicColumn;
            ViewBag.dynamicList = bDynamicList;
            ViewBag.from = from;
            ViewBag.tableName = "BaseAudit";
            return PartialView(new BaseAudit());
        }
        public JsonResult data(FormCollection form, string from = "", string filter = "")
        {
            JsonCsTableHelper table = new JsonCsTableHelper(db, form, Request.QueryString);
            table.TableName = "[BaseAudit]";
            table.PrimaryKey = "Execute at";
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
            table.ReplacedText.Add("Execute at", ("Execute at"));
            table.ReplacedText.Add("Entity", ("Entity"));
            table.ReplacedText.Add("RecordID", ("Record"));
            table.ReplacedText.Add("User", ("User"));
            table.ReplacedText.Add("Action", ("Action"));
            table.SearchCondition = " Execute at like '%{0}%'  OR  Entity like '%{0}%'  OR  RecordID like '%{0}%'  OR  User like '%{0}%'  OR  Action like '%{0}%' ";
            table.VarName = "csBaseAudit" + from.Split(':')[0];
            table.ReferenceText = "BaseAudit";
            table.Links.Add(new CsTableLink() { ButtonType = URLHelper.buttonType("BSExample"), Href = URLHelper.getActionUrlRelation("BSExample", "Form"), Title = "Edit", Css = "btn btn-warning btn-circle refreshTable", Icon = BaseUIIconText.fa.fa_fa_pencil, Extra = extraRelation });
            table.buildDataTable(filter);
            foreach (DataRow item in table.Data.Rows)
            {
                if (item["RecordID"] != null)
                {
                    item["RecordID"] = $"<button data-loading-text=\"<i class='fa fa-refresh fa-spin'></i>\" title=\"{item["RecordID"]}\" class=\"btn btn-warning btn-circle refreshTable baseDynamicModal\" data-action=\"{URLHelper.getActionUrl(item["Entity"].ToString(), "Form") + "/" + item["RecordID"]}\">{item["RecordID"]}</button>";
                }
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
            table.TableName = "[BaseAudit]";
            table.PrimaryKey = "Execute at";
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
            table.ReplacedText.Add("Execute at", ("Execute at"));
            table.ReplacedText.Add("Entity", ("Entity"));
            table.ReplacedText.Add("RecordID", ("Record"));
            table.ReplacedText.Add("User", ("User"));
            table.ReplacedText.Add("Action", ("Action"));
            table.SearchCondition = " Execute at like '%{0}%'  OR  Entity like '%{0}%'  OR  RecordID like '%{0}%'  OR  User like '%{0}%'  OR  Action like '%{0}%' ";
            table.VarName = "csBaseAudit";
            table.ReferenceText = "BaseAudit";
            if (Request.QueryString.AllKeys.Contains("filter"))
                table.buildDataTable(Request.QueryString["filter"]);
            else
                table.buildDataTable("");
            foreach (DataRow item in table.Data.Rows)
            {

            }
            wb.Worksheets.Add(table.Data, "Audit");
            using (var memoryStream = new MemoryStream())
            {
                wb.SaveAs(memoryStream);
                byte[] byteInfo = memoryStream.ToArray();
                return File(byteInfo, System.Net.Mime.MediaTypeNames.Application.Octet, String.Format("Audit BUZZSPY {0}.xlsx", DateTime.Now.ToShortDateString()));
            }
        }
        [HttpGet]
        [AllowAnonymous]
        public JsonResult getAll(FormCollection form, string columns = "['*']", string @operator = "AND")
        {
            List<string> where = new List<string>();
            foreach (string item in Request.QueryString) if (!form.AllKeys.Contains(item)) form.Add(item, Request.QueryString[item]);
            if (form.Count > 0)
                foreach (string item in form.AllKeys)
                    where.Add($"{item}='{form[item]}'");
            List<string> sel = JsonConvert.DeserializeObject<List<string>>(columns);
            string whereCondition = where.Count > 0 ? " where " + string.Join($" {@operator} ", where) : "";
            DataTable data = Helper.getData($"SELECT [{string.Join("],[", sel)}] FROM [BaseAudit] {whereCondition} ".Replace("[*]", "*"), db);
            List<string> ncolumns = new List<string>();
            foreach (DataColumn item in data.Columns)
                ncolumns.Add(item.ColumnName);
            List<object> rows = new List<object>();
            foreach (DataRow item in data.Rows)
            {
                List<object> adds = item.ItemArray.ToList();
                rows.Add(adds.ToArray());
            }
            return Json(new { columns = ncolumns, rows = rows, count = data.Rows.Count }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        [AllowAnonymous]
        public JsonResult get(int id)
        {
            BaseAudit model = db.BaseAudits.Find(id);
            if (model != null)
                return Json(model, JsonRequestBehavior.AllowGet);
            else
                return Json(new { Message = "This record no longer exists" }, JsonRequestBehavior.AllowGet);
        }
        [HttpPut]
        [AllowAnonymous]
        public JsonResult create(BaseAudit model)
        {
            BoolString validation = model.BeforeSave(db);
            if (validation.BoolValue)
                return Json(new { Message = validation.StringValue });
            db.BaseAudits.Add(model);
            db.SaveChanges();
            validation = model.AfterSave(db);
            if (validation.BoolValue)
                return Json(new { Message = validation.StringValue });
            return Json(new { id = model.Execute_at, MessageSucess = "That Audit saved successfully." });
        }
        [HttpPost]
        [AllowAnonymous]
        public JsonResult update(int id, BaseAudit model)
        {
            BoolString validation = model.BeforeEdit(db);
            if (validation.BoolValue)
                return Json(new { Message = validation.StringValue });
            db.Entry(model).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            validation = model.AfterEdit(db);
            if (validation.BoolValue)
                return Json(new { Message = validation.StringValue });
            return Json(new { id = model.Execute_at, MessageSucess = "That Audit saved successfully." });
        }
        [HttpDelete]
        [AllowAnonymous]
        public JsonResult remove(int id = 0)
        {
            try
            {
                BaseAudit model = db.BaseAudits.Find(id);
                if (model != null)
                {
                    BoolString validation = model.BeforeDelete(db);
                    if (validation.BoolValue)
                        return Json(new { Message = validation.StringValue }, JsonRequestBehavior.AllowGet);
                    db.BaseAudits.Remove(model);
                    db.SaveChanges();
                    validation = model.AfterDelete(db);
                    if (validation.BoolValue)
                        return Json(new { Message = validation.StringValue }, JsonRequestBehavior.AllowGet);
                    return Json(new { id = model.Execute_at, MessageSucess = "That Audit deleted successfully." }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { Message = "This record no longer exists" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Message = Helper.ModeralException(ex).Replace("@table", "Audit") }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
