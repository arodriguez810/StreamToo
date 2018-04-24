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
    public class RDEventController : BaseController
    {
[IsView]
        public ActionResult Index(int id = 0, string from = "")
        {
             List<BaseDynamicColumnList> bDynamicColumn = db.BaseDynamicColumnLists.ToList();
             List<BaseDynamicList> bDynamicList = db.BaseDynamicLists.ToList();
             ViewBag.column = bDynamicColumn;
             ViewBag.dynamicList = bDynamicList;
             ViewBag.from = from;
             ViewBag.tableName = "RDEvent";
             return PartialView(new RDEvent());
        }
[IsView]
        public ActionResult Form(int id=0, string from = "")
        {
ViewBag.relations = db.VWISRElations.Where(d => d.PK_Table == "RDEvent").ToList();
            ViewBag.from = from;
            if (id == 0)
                return PartialView(new RDEvent(){ active=true, creationDate = DateTime.Now});
            else
            {
                RDEvent model = db.RDEvents.Find(id);
                return PartialView(model);
            }
        }
[HttpPost]
[ValidateInput(false)]
        public JsonResult Save(RDEvent model, FormCollection form)
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
                
                db.RDEvents.Add(model);
                db.SaveChanges();
                validation = model.AfterSave(db);
                if (validation.BoolValue)
                    return Json(new { Message = validation.StringValue });
                validation = model.AfterCreate(db);
                if (validation.BoolValue)
                    return Json(new { Message = validation.StringValue });
            }
                return Json(new { id = model.id, MessageSucess = "That Event saved successfully." });
            }
            catch (Exception ex)
            {
                return Json(new { Message = Helper.ModeralException(ex).Replace("@table", "Event") });
            }
            }
public JsonResult ToggleActive(int id = 0)
        {
            try
            {
	RDEvent model = db.RDEvents.Find(id);
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
                        return Json(new { id = model.id, MessageSucess = "That Event Inactive successfully." }, JsonRequestBehavior.AllowGet);
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
                        return Json(new { id = model.id, MessageSucess = "That Event Active successfully." }, JsonRequestBehavior.AllowGet);
                    }
                }
                return Json(new { Message = "This record no longer exists" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Message = Helper.ModeralException(ex).Replace("@table", "Event") }, JsonRequestBehavior.AllowGet);
            }
        }
public JsonResult Delete(int id = 0)
{
try
 {
	RDEvent model = db.RDEvents.Find(id);
if(model != null){                    BoolString validation = model.BeforeDelete(db);
                    if (validation.BoolValue)
                        return Json(new { Message = validation.StringValue }, JsonRequestBehavior.AllowGet);
	db.RDEvents.Remove(model);
	db.SaveChanges();
                    validation = model.AfterDelete(db);
                
                    if (validation.BoolValue)
                        return Json(new { Message = validation.StringValue }, JsonRequestBehavior.AllowGet);
                return Json(new { id = model.id, MessageSucess = "That Event deleted successfully." }, JsonRequestBehavior.AllowGet);
            }
return Json(new { Message = "This record no longer exists" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Message = Helper.ModeralException(ex).Replace("@table", "Event") }, JsonRequestBehavior.AllowGet);
            }
            }
public JsonResult data(FormCollection form, string from = "", string filter = "")
{
	JsonCsTableHelper table = new JsonCsTableHelper(db, form, Request.QueryString);
	table.TableName = "[RDEvent]";
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
table.ReplacedText.Add("active", ("Active")); 
table.ReplacedText.Add("creationDate", ("Creation Date")); 
table.ReplacedText.Add("name", ("Name")); 
table.ReplacedText.Add("description", ("Description")); 
table.ReplacedText.Add("channel_channel", ("Channel")); 
table.ReplacedText.Add("dateFrom", ("Date From")); 
table.ReplacedText.Add("dateTo", ("Date To")); 
table.ReplacedText.Add("timeFrom", ("Time From")); 
table.ReplacedText.Add("timeTo", ("Time To")); 
table.ReplacedText.Add("isAllDay", ("Is All Day")); 
	table.SearchCondition = " id like '%{0}%'  OR  active like '%{0}%'  OR  creationDate like '%{0}%'  OR  name like '%{0}%'  OR  description like '%{0}%'  OR  (select top 1 name from [RDChannel] where id=channel_channel) like '%{0}%'  OR  dateFrom like '%{0}%'  OR  dateTo like '%{0}%'  OR  timeFrom like '%{0}%'  OR  timeTo like '%{0}%'  OR  isAllDay like '%{0}%' ";
	table.VarName = "csRDEvent" + from.Split(':')[0];
	table.ReferenceText = "Event";
	table.Links.Add(new CsTableLink() { ButtonType = URLHelper.buttonType("RDEvent"), Href = URLHelper.getActionUrlRelation("RDEvent", "Form"), Title = "Edit", Css = "btn btn-warning btn-circle refreshTable", Icon = BaseUIIconText.fa.fa_fa_pencil, Extra = extraRelation });
table.Links.Add(new CsTableLink() { ButtonType = CsTableLinkType.functionButton, Href = URLHelper.getActionUrl("RDEvent", "ToggleActive"), Title = "Active/Inactive", Css = "btn btn-warning btn-circle refreshTable", Icon = BaseUIIconText.fa.fa_fa_asterisk });
	table.Links.Add(new CsTableLink() { ButtonType = CsTableLinkType.deleteButton, Href = URLHelper.getActionUrl("RDEvent", "Delete"), Title = "Delete", Css = "btn btn-danger btn-circle refreshTable", Icon = BaseUIIconText.fa.fa_fa_trash_o });
table.buildDataTable(filter);
	foreach (DataRow item in table.Data.Rows) 
{ 
	if (item["active"] != null) 
{ 
    item["active"] = (item["active"].ToString() == "1" || item["active"].ToString() == "True") ? ("Yes") : ("No"); 
} 
	if (item["channel_channel"] != null) 
{ 
 if (item["channel_channel"].ToString() != "")
 {
    int channel_channel = int.Parse(item["channel_channel"].ToString()); 
    item["channel_channel"] = db.RDChannels.FirstOrDefault(d => d.id == channel_channel).name; 
 } 
else
     item["channel_channel"] = "Not Set"; 
} 
	if (item["isAllDay"] != null) 
{ 
    item["isAllDay"] = (item["isAllDay"].ToString() == "1" || item["isAllDay"].ToString() == "True") ? ("Yes") : ("No"); 
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
	table.TableName = "[RDEvent]";
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
table.ReplacedText.Add("active", ("Active")); 
table.ReplacedText.Add("creationDate", ("Creation Date")); 
table.ReplacedText.Add("name", ("Name")); 
table.ReplacedText.Add("description", ("Description")); 
table.ReplacedText.Add("channel_channel", ("Channel_channel")); 
table.ReplacedText.Add("dateFrom", ("Date From")); 
table.ReplacedText.Add("dateTo", ("Date To")); 
table.ReplacedText.Add("timeFrom", ("Time From")); 
table.ReplacedText.Add("timeTo", ("Time To")); 
table.ReplacedText.Add("isAllDay", ("Is All Day")); 
	table.SearchCondition = " id like '%{0}%'  OR  active like '%{0}%'  OR  creationDate like '%{0}%'  OR  name like '%{0}%'  OR  description like '%{0}%'  OR  (select top 1 name from [RDChannel] where id=channel_channel) like '%{0}%'  OR  dateFrom like '%{0}%'  OR  dateTo like '%{0}%'  OR  timeFrom like '%{0}%'  OR  timeTo like '%{0}%'  OR  isAllDay like '%{0}%' ";
	table.VarName = "csRDEvent";
	table.ReferenceText = "Event";
	table.Links.Add(new CsTableLink() { ButtonType = CsTableLinkType.button, Href = URLHelper.getActionUrl("RDEvent", "Form"), Title = "Edit", Css = "btn btn-warning btn-circle refreshTable", Icon = BaseUIIconText.fa.fa_fa_pencil });
	table.Links.Add(new CsTableLink() { ButtonType = CsTableLinkType.deleteButton, Href = URLHelper.getActionUrl("RDEvent", "Delete"), Title = "Delete", Css = "btn btn-danger btn-circle refreshTable", Icon = BaseUIIconText.fa.fa_fa_trash_o });
if (Request.QueryString.AllKeys.Contains("filter"))
                table.buildDataTable(Request.QueryString["filter"]);
            else
                table.buildDataTable("");
	foreach (DataRow item in table.Data.Rows) 
{ 
	if (item["active"] != null) 
{ 
    item["active"] = (item["active"].ToString() == "1" || item["active"].ToString() == "True") ? ("Yes") : ("No"); 
} 
	if (item["channel_channel"] != null) 
{ 
 if (item["channel_channel"].ToString() != "")
 {
    int channel_channel = int.Parse(item["channel_channel"].ToString()); 
    item["channel_channel"] = db.RDChannels.FirstOrDefault(d => d.id == channel_channel).name; 
 } 
else
     item["channel_channel"] = "Not Set"; 
} 
	if (item["isAllDay"] != null) 
{ 
    item["isAllDay"] = (item["isAllDay"].ToString() == "1" || item["isAllDay"].ToString() == "True") ? ("Yes") : ("No"); 
} 
} 
	wb.Worksheets.Add(table.Data, "Event");
	using (var memoryStream = new MemoryStream())
	{
		wb.SaveAs(memoryStream);
		byte[] byteInfo = memoryStream.ToArray();
		return File(byteInfo, System.Net.Mime.MediaTypeNames.Application.Octet, String.Format("Event StreamToo {0}.xlsx", DateTime.Now.ToShortDateString()));
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
            DataTable data = Helper.getData($"SELECT [{string.Join("],[", sel)}] FROM [RDEvent] {whereCondition} ".Replace("[*]", "*"), db);
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
            RDEvent model = db.RDEvents.Find(id);
            if (model != null)
                return Json(model, JsonRequestBehavior.AllowGet);
            else
                return Json(new { Message = "This record no longer exists" }, JsonRequestBehavior.AllowGet);
        }
        [HttpPut]
        [AllowAnonymous]
        public JsonResult create(RDEvent model)
        {
            BoolString validation = model.BeforeSave(db);
            if (validation.BoolValue)
                return Json(new { Message = validation.StringValue });
            db.RDEvents.Add(model);
            db.SaveChanges();
            validation = model.AfterSave(db);
            if (validation.BoolValue)
                return Json(new { Message = validation.StringValue });
            return Json(new { id = model.id, MessageSucess = "That Event saved successfully." });
        }
        [HttpPost]
        [AllowAnonymous]
        public JsonResult update(int id, RDEvent model)
        {
            BoolString validation = model.BeforeEdit(db);
            if (validation.BoolValue)
                return Json(new { Message = validation.StringValue });
            db.Entry(model).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            validation = model.AfterEdit(db);
            if (validation.BoolValue)
                return Json(new { Message = validation.StringValue });
            return Json(new { id = model.id, MessageSucess = "That Event saved successfully." });
        }
        [HttpDelete]
        [AllowAnonymous]
        public JsonResult remove(int id = 0)
        {
            try
            {
                RDEvent model = db.RDEvents.Find(id);
                if (model != null)
                {
                    BoolString validation = model.BeforeDelete(db);
                    if (validation.BoolValue)
                        return Json(new { Message = validation.StringValue }, JsonRequestBehavior.AllowGet);
                    db.RDEvents.Remove(model);
                    db.SaveChanges();
                    validation = model.AfterDelete(db);
                    if (validation.BoolValue)
                        return Json(new { Message = validation.StringValue }, JsonRequestBehavior.AllowGet);
                    return Json(new { id = model.id, MessageSucess = "That Event deleted successfully." }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { Message = "This record no longer exists" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Message = Helper.ModeralException(ex).Replace("@table", "Event") }, JsonRequestBehavior.AllowGet);
            }
        }
	}
}
