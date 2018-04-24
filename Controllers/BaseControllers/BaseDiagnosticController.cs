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
    public class BaseDiagnosticController : BaseController
    {
        [IsView]
        public ActionResult Index(int id = 0, string from = "")
        {
            List<BaseDynamicColumnList> bDynamicColumn = db.BaseDynamicColumnLists.ToList();
            List<BaseDynamicList> bDynamicList = db.BaseDynamicLists.ToList();
            ViewBag.column = bDynamicColumn;
            ViewBag.dynamicList = bDynamicList;
            ViewBag.from = from;
            ViewBag.tableName = "BaseDiagnostic";
            return PartialView(new BaseDiagnostic());
        }
        public JsonResult data(FormCollection form, string from = "", string filter = "")
        {
            JsonCsTableHelper table = new JsonCsTableHelper(db, form, Request.QueryString);
            table.TableName = "[BaseDiagnostic]";
            table.PrimaryKey = "TableOrColumn";
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
            table.ReplacedText.Add("TableOrColumn", ("Table Or Column"));
            table.ReplacedText.Add("Input", ("Input"));
            table.ReplacedText.Add("endWithS", ("End With S"));
            table.ReplacedText.Add("isKeyWord", ("Is Key Word"));
            table.ReplacedText.Add("hasUnderScore", ("Has Under Score"));
            table.ReplacedText.Add("correctNameRelation", ("Correct Name Relation"));
            table.ReplacedText.Add("noHasPrimaryKey", ("No Has Primary Key"));
            table.ReplacedText.Add("hasActiveField", ("Has Active Field"));
            table.SearchCondition = " TableOrColumn like '%{0}%'  OR  Input like '%{0}%'  OR  endWithS like '%{0}%'  OR  isKeyWord like '%{0}%'  OR  hasUnderScore like '%{0}%'  OR  correctNameRelation like '%{0}%'  OR  noHasPrimaryKey like '%{0}%'  OR  hasActiveField like '%{0}%' ";
            table.VarName = "csBaseDiagnostic" + from.Split(':')[0];
            table.ReferenceText = "BaseDiagnostic";
            table.buildDataTable(filter);
            foreach (DataRow item in table.Data.Rows)
            {
            }
            table.buildDefaultJson();
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
            table.TableName = "[BaseDiagnostic]";
            table.PrimaryKey = "TableOrColumn";
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
            table.ReplacedText.Add("TableOrColumn", ("Table Or Column"));
            table.ReplacedText.Add("Input", ("Input"));
            table.ReplacedText.Add("endWithS", ("End With S"));
            table.ReplacedText.Add("isKeyWord", ("Is Key Word"));
            table.ReplacedText.Add("hasUnderScore", ("Has Under Score"));
            table.ReplacedText.Add("correctNameRelation", ("Correct Name Relation"));
            table.ReplacedText.Add("noHasPrimaryKey", ("No Has Primary Key"));
            table.ReplacedText.Add("hasActiveField", ("Has Active Field"));
            table.SearchCondition = " TableOrColumn like '%{0}%'  OR  Input like '%{0}%'  OR  endWithS like '%{0}%'  OR  isKeyWord like '%{0}%'  OR  hasUnderScore like '%{0}%'  OR  correctNameRelation like '%{0}%'  OR  noHasPrimaryKey like '%{0}%'  OR  hasActiveField like '%{0}%' ";
            table.VarName = "csBaseDiagnostic";
            table.ReferenceText = "BaseDiagnostic";
            if (Request.QueryString.AllKeys.Contains("filter"))
                table.buildDataTable(Request.QueryString["filter"]);
            else
                table.buildDataTable("");
            foreach (DataRow item in table.Data.Rows)
            {
            }
            wb.Worksheets.Add(table.Data, "Diagnostic");
            using (var memoryStream = new MemoryStream())
            {
                wb.SaveAs(memoryStream);
                byte[] byteInfo = memoryStream.ToArray();
                return File(byteInfo, System.Net.Mime.MediaTypeNames.Application.Octet, String.Format("Diagnostic BUZZSPY {0}.xlsx", DateTime.Now.ToShortDateString()));
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
            DataTable data = Helper.getData($"SELECT [{string.Join("],[", sel)}] FROM [BaseDiagnostic] {whereCondition} ".Replace("[*]", "*"), db);
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
            BaseDiagnostic model = db.BaseDiagnostics.Find(id);
            if (model != null)
                return Json(model, JsonRequestBehavior.AllowGet);
            else
                return Json(new { Message = "This record no longer exists" }, JsonRequestBehavior.AllowGet);
        }
    }
}
