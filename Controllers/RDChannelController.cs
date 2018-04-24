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
    public class RDChannelController : BaseController
    {
[IsView]
        public ActionResult Index(int id = 0, string from = "")
        {
             List<BaseDynamicColumnList> bDynamicColumn = db.BaseDynamicColumnLists.ToList();
             List<BaseDynamicList> bDynamicList = db.BaseDynamicLists.ToList();
             ViewBag.column = bDynamicColumn;
             ViewBag.dynamicList = bDynamicList;
             ViewBag.from = from;
             ViewBag.tableName = "RDChannel";
             return PartialView(new RDChannel());
        }
[IsView]
        public ActionResult Form(int id=0, string from = "")
        {
ViewBag.relations = db.VWISRElations.Where(d => d.PK_Table == "RDChannel").ToList();
            ViewBag.from = from;
            if (id == 0)
                return PartialView(new RDChannel(){ active=true, creationDate = DateTime.Now});
            else
            {
                RDChannel model = db.RDChannels.Find(id);
                return PartialView(model);
            }
        }
[HttpPost]
[ValidateInput(false)]
        public JsonResult Save(RDChannel model, FormCollection form)
        {
try
            {
            if (model.id != 0)
            {
                    BoolString validation = model.BeforeEdit(db);
                    if (validation.BoolValue)
                        return Json(new { Message = validation.StringValue });
                    db.Entry(model).State = System.Data.Entity.EntityState.Modified;
if (Request.Files.Count > 0)
   if (Request.Files["iconImage"].ContentLength == 0)
   db.Entry(model).Property(x => x.iconImage).IsModified = false;
if (Request.Files.Count > 0)
   if (Request.Files["logoImage"].ContentLength == 0)
   db.Entry(model).Property(x => x.logoImage).IsModified = false;
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
                
                db.RDChannels.Add(model);
                db.SaveChanges();
                validation = model.AfterSave(db);
                if (validation.BoolValue)
                    return Json(new { Message = validation.StringValue });
                validation = model.AfterCreate(db);
                if (validation.BoolValue)
                    return Json(new { Message = validation.StringValue });
            }
			   HttpPostedFileBase file = null;
            if (Request.Files.Count > 0)
            {
                file = Request.Files["iconImage"];
                if (file != null && file.ContentLength > 0)
                {
                    string ext = Path.GetExtension(file.FileName);
                    string filename = model.id.ToString() + ext;
                    string directory = Server.MapPath("~/files/RDChannel/iconImage/");
                    if (!Directory.Exists(directory))
                        Directory.CreateDirectory(directory);
                    var path = Path.Combine(directory, filename);
                    file.SaveAs(path);
                    model.iconImage = filename;
                }

                file = Request.Files["logoImage"];
                if (file != null && file.ContentLength > 0)
                {
                    string ext = Path.GetExtension(file.FileName);
                    string filename = model.id.ToString() + ext;
                    string directory = Server.MapPath("~/files/RDChannel/logoImage/");
                    if (!Directory.Exists(directory))
                        Directory.CreateDirectory(directory);
                    var path = Path.Combine(directory, filename);
                    file.SaveAs(path);
                    model.logoImage = filename;
                }

                }
                db.SaveChanges();
                return Json(new { id = model.id, MessageSucess = "That Channel saved successfully." });
            }
            catch (Exception ex)
            {
                return Json(new { Message = Helper.ModeralException(ex).Replace("@table", "Channel") });
            }
            }
public JsonResult ToggleActive(int id = 0)
        {
            try
            {
	RDChannel model = db.RDChannels.Find(id);
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
                        return Json(new { id = model.id, MessageSucess = "That Channel Inactive successfully." }, JsonRequestBehavior.AllowGet);
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
                        return Json(new { id = model.id, MessageSucess = "That Channel Active successfully." }, JsonRequestBehavior.AllowGet);
                    }
                }
                return Json(new { Message = "This record no longer exists" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Message = Helper.ModeralException(ex).Replace("@table", "Channel") }, JsonRequestBehavior.AllowGet);
            }
        }
public JsonResult Delete(int id = 0)
{
try
 {
	RDChannel model = db.RDChannels.Find(id);
if(model != null){                    BoolString validation = model.BeforeDelete(db);
                    if (validation.BoolValue)
                        return Json(new { Message = validation.StringValue }, JsonRequestBehavior.AllowGet);
	db.RDChannels.Remove(model);
	db.SaveChanges();
                    validation = model.AfterDelete(db);
                
                    if (validation.BoolValue)
                        return Json(new { Message = validation.StringValue }, JsonRequestBehavior.AllowGet);
                return Json(new { id = model.id, MessageSucess = "That Channel deleted successfully." }, JsonRequestBehavior.AllowGet);
            }
return Json(new { Message = "This record no longer exists" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Message = Helper.ModeralException(ex).Replace("@table", "Channel") }, JsonRequestBehavior.AllowGet);
            }
            }
public JsonResult data(FormCollection form, string from = "", string filter = "")
{
	JsonCsTableHelper table = new JsonCsTableHelper(db, form, Request.QueryString);
	table.TableName = "[RDChannel]";
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
table.ReplacedText.Add("displayName", ("Display Name")); 
table.ReplacedText.Add("description", ("Description")); 
table.ReplacedText.Add("url", ("Url")); 
table.ReplacedText.Add("orderNum", ("Order Num")); 
table.ReplacedText.Add("isTemporal", ("Is Temporal")); 
table.ReplacedText.Add("iconImage", ("Icon Image")); 
table.ReplacedText.Add("logoImage", ("Logo Image")); 
table.ReplacedText.Add("category_category", ("Category")); 
table.ReplacedText.Add("type_type", ("Type")); 
	table.SearchCondition = " id like '%{0}%'  OR  active like '%{0}%'  OR  creationDate like '%{0}%'  OR  name like '%{0}%'  OR  displayName like '%{0}%'  OR  description like '%{0}%'  OR  url like '%{0}%'  OR  orderNum like '%{0}%'  OR  isTemporal like '%{0}%'  OR  iconImage like '%{0}%'  OR  logoImage like '%{0}%'  OR  (select top 1 name from [RDCategory] where id=category_category) like '%{0}%'  OR  (select top 1 name from [RDType] where id=type_type) like '%{0}%' ";
	table.VarName = "csRDChannel" + from.Split(':')[0];
	table.ReferenceText = "Channel";
	table.Links.Add(new CsTableLink() { ButtonType = URLHelper.buttonType("RDChannel"), Href = URLHelper.getActionUrlRelation("RDChannel", "Form"), Title = "Edit", Css = "btn btn-warning btn-circle refreshTable", Icon = BaseUIIconText.fa.fa_fa_pencil, Extra = extraRelation });
table.Links.Add(new CsTableLink() { ButtonType = CsTableLinkType.functionButton, Href = URLHelper.getActionUrl("RDChannel", "ToggleActive"), Title = "Active/Inactive", Css = "btn btn-warning btn-circle refreshTable", Icon = BaseUIIconText.fa.fa_fa_asterisk });
	table.Links.Add(new CsTableLink() { ButtonType = CsTableLinkType.deleteButton, Href = URLHelper.getActionUrl("RDChannel", "Delete"), Title = "Delete", Css = "btn btn-danger btn-circle refreshTable", Icon = BaseUIIconText.fa.fa_fa_trash_o });
table.buildDataTable(filter);
	foreach (DataRow item in table.Data.Rows) 
{ 
	if (item["active"] != null) 
{ 
    item["active"] = (item["active"].ToString() == "1" || item["active"].ToString() == "True") ? ("Yes") : ("No"); 
} 
	if (item["isTemporal"] != null) 
{ 
    item["isTemporal"] = (item["isTemporal"].ToString() == "1" || item["isTemporal"].ToString() == "True") ? ("Yes") : ("No"); 
} 
	if (item["iconImage"] != null) 
{ 
    item["iconImage"] = "#file&&&" + "iconImage<>" + item["iconImage"].ToString() + "<>RDChannel";  
} 
	if (item["logoImage"] != null) 
{ 
    item["logoImage"] = "#file&&&" + "logoImage<>" + item["logoImage"].ToString() + "<>RDChannel";  
} 
	if (item["category_category"] != null) 
{ 
 if (item["category_category"].ToString() != "")
 {
    int category_category = int.Parse(item["category_category"].ToString()); 
    item["category_category"] = db.RDCategories.FirstOrDefault(d => d.id == category_category).name; 
 } 
else
     item["category_category"] = "Not Set"; 
} 
	if (item["type_type"] != null) 
{ 
 if (item["type_type"].ToString() != "")
 {
    int type_type = int.Parse(item["type_type"].ToString()); 
    item["type_type"] = db.RDTypes.FirstOrDefault(d => d.id == type_type).name; 
 } 
else
     item["type_type"] = "Not Set"; 
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
	table.TableName = "[RDChannel]";
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
table.ReplacedText.Add("displayName", ("Display Name")); 
table.ReplacedText.Add("description", ("Description")); 
table.ReplacedText.Add("url", ("Url")); 
table.ReplacedText.Add("orderNum", ("Order Num")); 
table.ReplacedText.Add("isTemporal", ("Is Temporal")); 
table.ReplacedText.Add("iconImage", ("Icon Image")); 
table.ReplacedText.Add("logoImage", ("Logo Image")); 
table.ReplacedText.Add("category_category", ("Category_category")); 
table.ReplacedText.Add("type_type", ("Type_type")); 
	table.SearchCondition = " id like '%{0}%'  OR  active like '%{0}%'  OR  creationDate like '%{0}%'  OR  name like '%{0}%'  OR  displayName like '%{0}%'  OR  description like '%{0}%'  OR  url like '%{0}%'  OR  orderNum like '%{0}%'  OR  isTemporal like '%{0}%'  OR  iconImage like '%{0}%'  OR  logoImage like '%{0}%'  OR  (select top 1 name from [RDCategory] where id=category_category) like '%{0}%'  OR  (select top 1 name from [RDType] where id=type_type) like '%{0}%' ";
	table.VarName = "csRDChannel";
	table.ReferenceText = "Channel";
	table.Links.Add(new CsTableLink() { ButtonType = CsTableLinkType.button, Href = URLHelper.getActionUrl("RDChannel", "Form"), Title = "Edit", Css = "btn btn-warning btn-circle refreshTable", Icon = BaseUIIconText.fa.fa_fa_pencil });
	table.Links.Add(new CsTableLink() { ButtonType = CsTableLinkType.deleteButton, Href = URLHelper.getActionUrl("RDChannel", "Delete"), Title = "Delete", Css = "btn btn-danger btn-circle refreshTable", Icon = BaseUIIconText.fa.fa_fa_trash_o });
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
	if (item["isTemporal"] != null) 
{ 
    item["isTemporal"] = (item["isTemporal"].ToString() == "1" || item["isTemporal"].ToString() == "True") ? ("Yes") : ("No"); 
} 
	if (item["iconImage"] != null) 
{ 
    item["iconImage"] = "#file&&&" + "iconImage<>" + item["iconImage"].ToString() + "<>RDChannel";  
} 
	if (item["logoImage"] != null) 
{ 
    item["logoImage"] = "#file&&&" + "logoImage<>" + item["logoImage"].ToString() + "<>RDChannel";  
} 
	if (item["category_category"] != null) 
{ 
 if (item["category_category"].ToString() != "")
 {
    int category_category = int.Parse(item["category_category"].ToString()); 
    item["category_category"] = db.RDCategories.FirstOrDefault(d => d.id == category_category).name; 
 } 
else
     item["category_category"] = "Not Set"; 
} 
	if (item["type_type"] != null) 
{ 
 if (item["type_type"].ToString() != "")
 {
    int type_type = int.Parse(item["type_type"].ToString()); 
    item["type_type"] = db.RDTypes.FirstOrDefault(d => d.id == type_type).name; 
 } 
else
     item["type_type"] = "Not Set"; 
} 
} 
	wb.Worksheets.Add(table.Data, "Channel");
	using (var memoryStream = new MemoryStream())
	{
		wb.SaveAs(memoryStream);
		byte[] byteInfo = memoryStream.ToArray();
		return File(byteInfo, System.Net.Mime.MediaTypeNames.Application.Octet, String.Format("Channel StreamToo {0}.xlsx", DateTime.Now.ToShortDateString()));
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
            DataTable data = Helper.getData($"SELECT [{string.Join("],[", sel)}] FROM [RDChannel] {whereCondition} ".Replace("[*]", "*"), db);
            List<string> ncolumns = new List<string>();
            foreach (DataColumn item in data.Columns)
                ncolumns.Add(item.ColumnName);
                ncolumns.Add("ClientChannels");
                ncolumns.Add("Events");
                ncolumns.Add("Programmings");
            List<object> rows = new List<object>();
            foreach (DataRow item in data.Rows)
            {
                List<object> adds = item.ItemArray.ToList();
adds.Add(Helper.Regular(db, item, "RDClientChannel", $" where [channel_channel] = '@id@'"));
adds.Add(Helper.Regular(db, item, "RDEvent", $" where [channel_channel] = '@id@'"));
adds.Add(Helper.Regular(db, item, "RDProgramming", $" where [channel_channel] = '@id@'"));
                rows.Add(adds.ToArray());
            }
            return Json(new { columns = ncolumns, rows = rows, count = data.Rows.Count }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        [AllowAnonymous]
        public JsonResult get(int id)
        {
            RDChannel model = db.RDChannels.Find(id);
            if (model != null)
                return Json(model, JsonRequestBehavior.AllowGet);
            else
                return Json(new { Message = "This record no longer exists" }, JsonRequestBehavior.AllowGet);
        }
        [HttpPut]
        [AllowAnonymous]
        public JsonResult create(RDChannel model)
        {
            BoolString validation = model.BeforeSave(db);
            if (validation.BoolValue)
                return Json(new { Message = validation.StringValue });
            db.RDChannels.Add(model);
            db.SaveChanges();
            validation = model.AfterSave(db);
            if (validation.BoolValue)
                return Json(new { Message = validation.StringValue });
            return Json(new { id = model.id, MessageSucess = "That Channel saved successfully." });
        }
        [HttpPost]
        [AllowAnonymous]
        public JsonResult update(int id, RDChannel model)
        {
            BoolString validation = model.BeforeEdit(db);
            if (validation.BoolValue)
                return Json(new { Message = validation.StringValue });
            db.Entry(model).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            validation = model.AfterEdit(db);
            if (validation.BoolValue)
                return Json(new { Message = validation.StringValue });
            return Json(new { id = model.id, MessageSucess = "That Channel saved successfully." });
        }
        [HttpDelete]
        [AllowAnonymous]
        public JsonResult remove(int id = 0)
        {
            try
            {
                RDChannel model = db.RDChannels.Find(id);
                if (model != null)
                {
                    BoolString validation = model.BeforeDelete(db);
                    if (validation.BoolValue)
                        return Json(new { Message = validation.StringValue }, JsonRequestBehavior.AllowGet);
                    db.RDChannels.Remove(model);
                    db.SaveChanges();
                    validation = model.AfterDelete(db);
                    if (validation.BoolValue)
                        return Json(new { Message = validation.StringValue }, JsonRequestBehavior.AllowGet);
                    return Json(new { id = model.id, MessageSucess = "That Channel deleted successfully." }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { Message = "This record no longer exists" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Message = Helper.ModeralException(ex).Replace("@table", "Channel") }, JsonRequestBehavior.AllowGet);
            }
        }
	}
}
