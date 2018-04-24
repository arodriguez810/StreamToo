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
    public class BaseColumnController : BaseController
    {
[IsView]
        public ActionResult Index(int id = 0, string from = "")
        {
             List<BaseDynamicColumnList> bDynamicColumn = db.BaseDynamicColumnLists.ToList();
             List<BaseDynamicList> bDynamicList = db.BaseDynamicLists.ToList();
             ViewBag.column = bDynamicColumn;
             ViewBag.dynamicList = bDynamicList;
             ViewBag.from = from;
             ViewBag.tableName = "BaseColumn";
             return PartialView(new BaseColumn());
        }
[IsView]
        public ActionResult Form(int id=0, string from = "")
        {
ViewBag.relations = db.VWISRElations.Where(d => d.PK_Table == "BaseColumn").ToList();
            ViewBag.from = from;
            if (id == 0)
                return PartialView(new BaseColumn());
            else
            {
                BaseColumn model = db.BaseColumns.Find(id);
                return PartialView(model);
            }
        }
[HttpPost]
[ValidateInput(false)]
        public JsonResult Save(BaseColumn model, FormCollection form)
        {
try
            {
            if (model.id != 0)
            {
                    BoolString validation = model.BeforeEdit(db);
                    if (validation.BoolValue)
                        return Json(new { Message = validation.StringValue });
                    db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                    validation = model.BeforeSave(db);
                    if (validation.BoolValue)
                        return Json(new { Message = validation.StringValue });
                    validation = model.BeforeEdit(db);
                
                    if (validation.BoolValue)
                        return Json(new { Message = validation.StringValue });
                    db.SaveChanges();
                    validation = model.AfterSave(db);
                
                    if (validation.BoolValue)
                        return Json(new { Message = validation.StringValue });
                    validation = model.AfterEdit(db);
                
                    if (validation.BoolValue)
                        return Json(new { Message = validation.StringValue });
            }
            else
            {
                 BoolString validation = model.BeforeSave(db);
                 if (validation.BoolValue)
                    return Json(new { Message = validation.StringValue });
                 validation = model.BeforeCreate(db);
                 if (validation.BoolValue)
                    return Json(new { Message = validation.StringValue });
                
                db.BaseColumns.Add(model);
                db.SaveChanges();
                validation = model.AfterSave(db);
                if (validation.BoolValue)
                    return Json(new { Message = validation.StringValue });
                validation = model.AfterCreate(db);
                if (validation.BoolValue)
                    return Json(new { Message = validation.StringValue });
            }
                return Json(new { id = model.id, MessageSucess = "That Column saved successfully." });
            }
            catch (Exception ex)
            {
                return Json(new { Message = Helper.ModeralException(ex).Replace("@table", "Column") });
            }
            }
public JsonResult ToggleActive(int id = 0)
        {
            try
            {
	BaseColumn model = db.BaseColumns.Find(id);
                if (model != null)
                {
                    if (model.active)
                    {
                        BoolString validation = model.BeforeInactive(db);
                        if (validation.BoolValue)
                            return Json(new { Message = validation.StringValue }, JsonRequestBehavior.AllowGet);
                          model.active = !model.active;
                        db.SaveChanges();
                        validation = model.AfterInactive(db);
                        if (validation.BoolValue)
                            return Json(new { Message = validation.StringValue }, JsonRequestBehavior.AllowGet);
                        return Json(new { id = model.id, MessageSucess = "That Column Inactive successfully." }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        BoolString validation = model.BeforeActive(db);
                        if (validation.BoolValue)
                            return Json(new { Message = validation.StringValue }, JsonRequestBehavior.AllowGet);
                model.active = !model.active;
                        db.SaveChanges();
                        validation = model.AfterActive(db);
                        if (validation.BoolValue)
                            return Json(new { Message = validation.StringValue }, JsonRequestBehavior.AllowGet);
                        return Json(new { id = model.id, MessageSucess = "That Column Active successfully." }, JsonRequestBehavior.AllowGet);
                    }
                }
                return Json(new { Message = "This record no longer exists" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Message = Helper.ModeralException(ex).Replace("@table", "Column") }, JsonRequestBehavior.AllowGet);
            }
        }
public JsonResult Delete(int id = 0)
{
try
 {
	BaseColumn model = db.BaseColumns.Find(id);
if(model != null){                    BoolString validation = model.BeforeDelete(db);
                    if (validation.BoolValue)
                        return Json(new { Message = validation.StringValue }, JsonRequestBehavior.AllowGet);
	db.BaseColumns.Remove(model);
	db.SaveChanges();
                    validation = model.AfterDelete(db);
                
                    if (validation.BoolValue)
                        return Json(new { Message = validation.StringValue }, JsonRequestBehavior.AllowGet);
                return Json(new { id = model.id, MessageSucess = "That Column deleted successfully." }, JsonRequestBehavior.AllowGet);
            }
return Json(new { Message = "This record no longer exists" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Message = Helper.ModeralException(ex).Replace("@table", "Column") }, JsonRequestBehavior.AllowGet);
            }
            }
public JsonResult data(FormCollection form, string from = "", string filter = "")
{
	JsonCsTableHelper table = new JsonCsTableHelper(db, form, Request.QueryString);
	table.TableName = "[BaseColumn]";
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
table.ReplacedText.Add("basetable_table", ("Table")); 
table.ReplacedText.Add("columnName", ("Column Name")); 
table.ReplacedText.Add("dataType", ("Data Type")); 
table.ReplacedText.Add("allowNull", ("Allow Null")); 
table.ReplacedText.Add("defaultValue", ("Default Value")); 
table.ReplacedText.Add("ordinalPosition", ("Ordinal Position")); 
table.ReplacedText.Add("characterMaximumLength", ("Character Maximum Length")); 
table.ReplacedText.Add("numericPrecision", ("Numeric Precision")); 
table.ReplacedText.Add("numericScale", ("Numeric Scale")); 
table.ReplacedText.Add("isIndex", ("Is Index")); 
table.ReplacedText.Add("isUnique", ("Is Unique")); 
table.ReplacedText.Add("active", ("Active")); 
table.ReplacedText.Add("referenceTable", ("Reference Table")); 
table.ReplacedText.Add("referenceField", ("Reference Field")); 
	table.SearchCondition = " id like '%{0}%'  OR  (select top 1 name from [BaseTable] where id=basetable_table) like '%{0}%'  OR  columnName like '%{0}%'  OR  dataType like '%{0}%'  OR  allowNull like '%{0}%'  OR  defaultValue like '%{0}%'  OR  ordinalPosition like '%{0}%'  OR  characterMaximumLength like '%{0}%'  OR  numericPrecision like '%{0}%'  OR  numericScale like '%{0}%'  OR  isIndex like '%{0}%'  OR  isUnique like '%{0}%'  OR  active like '%{0}%'  OR  referenceTable like '%{0}%'  OR  referenceField like '%{0}%' ";
	table.VarName = "csBaseColumn" + from.Split(':')[0];
	table.ReferenceText = "seColumn";
	table.Links.Add(new CsTableLink() { ButtonType = URLHelper.buttonType("BaseColumn"), Href = URLHelper.getActionUrlRelation("BaseColumn", "Form"), Title = "Edit", Css = "btn btn-warning btn-circle refreshTable", Icon = BaseUIIconText.fa.fa_fa_pencil, Extra = extraRelation });
table.Links.Add(new CsTableLink() { ButtonType = CsTableLinkType.functionButton, Href = URLHelper.getActionUrl("BaseColumn", "ToggleActive"), Title = "Active/Inactive", Css = "btn btn-warning btn-circle refreshTable", Icon = BaseUIIconText.fa.fa_fa_asterisk });
	table.Links.Add(new CsTableLink() { ButtonType = CsTableLinkType.deleteButton, Href = URLHelper.getActionUrl("BaseColumn", "Delete"), Title = "Delete", Css = "btn btn-danger btn-circle refreshTable", Icon = BaseUIIconText.fa.fa_fa_trash_o });
table.buildDataTable(filter);
	foreach (DataRow item in table.Data.Rows) 
{ 
	if (item["basetable_table"] != null) 
{ 
 if (item["basetable_table"].ToString() != "")
 {
    int basetable_table = int.Parse(item["basetable_table"].ToString()); 
    item["basetable_table"] = db.BaseTables.FirstOrDefault(d => d.id == basetable_table).name; 
 } 
else
     item["basetable_table"] = "Not Set"; 
} 
	if (item["allowNull"] != null) 
{ 
    item["allowNull"] = (item["allowNull"].ToString() == "1" || item["allowNull"].ToString() == "True") ? ("Yes") : ("No"); 
} 
	if (item["isIndex"] != null) 
{ 
    item["isIndex"] = (item["isIndex"].ToString() == "1" || item["isIndex"].ToString() == "True") ? ("Yes") : ("No"); 
} 
	if (item["isUnique"] != null) 
{ 
    item["isUnique"] = (item["isUnique"].ToString() == "1" || item["isUnique"].ToString() == "True") ? ("Yes") : ("No"); 
} 
	if (item["active"] != null) 
{ 
    item["active"] = (item["active"].ToString() == "1" || item["active"].ToString() == "True") ? ("Yes") : ("No"); 
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
	table.TableName = "[BaseColumn]";
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
table.ReplacedText.Add("basetable_table", ("Basetable_table")); 
table.ReplacedText.Add("columnName", ("Column Name")); 
table.ReplacedText.Add("dataType", ("Data Type")); 
table.ReplacedText.Add("allowNull", ("Allow Null")); 
table.ReplacedText.Add("defaultValue", ("Default Value")); 
table.ReplacedText.Add("ordinalPosition", ("Ordinal Position")); 
table.ReplacedText.Add("characterMaximumLength", ("Character Maximum Length")); 
table.ReplacedText.Add("numericPrecision", ("Numeric Precision")); 
table.ReplacedText.Add("numericScale", ("Numeric Scale")); 
table.ReplacedText.Add("isIndex", ("Is Index")); 
table.ReplacedText.Add("isUnique", ("Is Unique")); 
table.ReplacedText.Add("active", ("Active")); 
table.ReplacedText.Add("referenceTable", ("Reference Table")); 
table.ReplacedText.Add("referenceField", ("Reference Field")); 
	table.SearchCondition = " id like '%{0}%'  OR  (select top 1 name from [BaseTable] where id=basetable_table) like '%{0}%'  OR  columnName like '%{0}%'  OR  dataType like '%{0}%'  OR  allowNull like '%{0}%'  OR  defaultValue like '%{0}%'  OR  ordinalPosition like '%{0}%'  OR  characterMaximumLength like '%{0}%'  OR  numericPrecision like '%{0}%'  OR  numericScale like '%{0}%'  OR  isIndex like '%{0}%'  OR  isUnique like '%{0}%'  OR  active like '%{0}%'  OR  referenceTable like '%{0}%'  OR  referenceField like '%{0}%' ";
	table.VarName = "csBaseColumn";
	table.ReferenceText = "seColumn";
	table.Links.Add(new CsTableLink() { ButtonType = CsTableLinkType.button, Href = URLHelper.getActionUrl("BaseColumn", "Form"), Title = "Edit", Css = "btn btn-warning btn-circle refreshTable", Icon = BaseUIIconText.fa.fa_fa_pencil });
	table.Links.Add(new CsTableLink() { ButtonType = CsTableLinkType.deleteButton, Href = URLHelper.getActionUrl("BaseColumn", "Delete"), Title = "Delete", Css = "btn btn-danger btn-circle refreshTable", Icon = BaseUIIconText.fa.fa_fa_trash_o });
if (Request.QueryString.AllKeys.Contains("filter"))
                table.buildDataTable(Request.QueryString["filter"]);
            else
                table.buildDataTable("");
	foreach (DataRow item in table.Data.Rows) 
{ 
	if (item["basetable_table"] != null) 
{ 
 if (item["basetable_table"].ToString() != "")
 {
    int basetable_table = int.Parse(item["basetable_table"].ToString()); 
    item["basetable_table"] = db.BaseTables.FirstOrDefault(d => d.id == basetable_table).name; 
 } 
else
     item["basetable_table"] = "Not Set"; 
} 
	if (item["allowNull"] != null) 
{ 
    item["allowNull"] = (item["allowNull"].ToString() == "1" || item["allowNull"].ToString() == "True") ? ("Yes") : ("No"); 
} 
	if (item["isIndex"] != null) 
{ 
    item["isIndex"] = (item["isIndex"].ToString() == "1" || item["isIndex"].ToString() == "True") ? ("Yes") : ("No"); 
} 
	if (item["isUnique"] != null) 
{ 
    item["isUnique"] = (item["isUnique"].ToString() == "1" || item["isUnique"].ToString() == "True") ? ("Yes") : ("No"); 
} 
	if (item["active"] != null) 
{ 
    item["active"] = (item["active"].ToString() == "1" || item["active"].ToString() == "True") ? ("Yes") : ("No"); 
} 
} 
	wb.Worksheets.Add(table.Data, "Column");
	using (var memoryStream = new MemoryStream())
	{
		wb.SaveAs(memoryStream);
		byte[] byteInfo = memoryStream.ToArray();
		return File(byteInfo, System.Net.Mime.MediaTypeNames.Application.Octet, String.Format("Column BUZZSPY {0}.xlsx", DateTime.Now.ToShortDateString()));
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
            DataTable data = Helper.getData($"SELECT [{string.Join("],[", sel)}] FROM [BaseColumn] {whereCondition} ".Replace("[*]", "*"), db);
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
            BaseColumn model = db.BaseColumns.Find(id);
            if (model != null)
                return Json(model, JsonRequestBehavior.AllowGet);
            else
                return Json(new { Message = "This record no longer exists" }, JsonRequestBehavior.AllowGet);
        }
        [HttpPut]
        [AllowAnonymous]
        public JsonResult create(BaseColumn model)
        {
            BoolString validation = model.BeforeSave(db);
            if (validation.BoolValue)
                return Json(new { Message = validation.StringValue });
            db.BaseColumns.Add(model);
            db.SaveChanges();
            validation = model.AfterSave(db);
            if (validation.BoolValue)
                return Json(new { Message = validation.StringValue });
            return Json(new { id = model.id, MessageSucess = "That Column saved successfully." });
        }
        [HttpPost]
        [AllowAnonymous]
        public JsonResult update(int id, BaseColumn model)
        {
            BoolString validation = model.BeforeEdit(db);
            if (validation.BoolValue)
                return Json(new { Message = validation.StringValue });
            db.Entry(model).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            validation = model.AfterEdit(db);
            if (validation.BoolValue)
                return Json(new { Message = validation.StringValue });
            return Json(new { id = model.id, MessageSucess = "That Column saved successfully." });
        }
        [HttpDelete]
        [AllowAnonymous]
        public JsonResult remove(int id = 0)
        {
            try
            {
                BaseColumn model = db.BaseColumns.Find(id);
                if (model != null)
                {
                    BoolString validation = model.BeforeDelete(db);
                    if (validation.BoolValue)
                        return Json(new { Message = validation.StringValue }, JsonRequestBehavior.AllowGet);
                    db.BaseColumns.Remove(model);
                    db.SaveChanges();
                    validation = model.AfterDelete(db);
                    if (validation.BoolValue)
                        return Json(new { Message = validation.StringValue }, JsonRequestBehavior.AllowGet);
                    return Json(new { id = model.id, MessageSucess = "That Column deleted successfully." }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { Message = "This record no longer exists" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Message = Helper.ModeralException(ex).Replace("@table", "Column") }, JsonRequestBehavior.AllowGet);
            }
        }
	}
}
