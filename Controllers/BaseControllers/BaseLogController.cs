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
    public class BaseLogController : BaseController
    {
        [IsView]
        public ActionResult Index(int id = 0, string from = "")
        {
            List<BaseDynamicColumnList> bDynamicColumn = db.BaseDynamicColumnLists.ToList();
            List<BaseDynamicList> bDynamicList = db.BaseDynamicLists.ToList();
            ViewBag.column = bDynamicColumn;
            ViewBag.dynamicList = bDynamicList;
            ViewBag.from = from;
            ViewBag.tableName = "BaseLog";
            return PartialView(new BaseLog());
        }
        public JsonResult data(FormCollection form, string from = "", string filter = "")
        {
            JsonCsTableHelper table = new JsonCsTableHelper(db, form, Request.QueryString);
            table.TableName = "[VWBaseLog]";
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
            table.ReplacedText.Add("created", ("Created"));
            table.ReplacedText.Add("basedynamicList_entity", ("Entity"));
            table.ReplacedText.Add("entityId", ("Entity Id"));
            table.ReplacedText.Add("entityIdU", ("Entity Id U"));
            table.ReplacedText.Add("entityIdS", ("Entity Id S"));
            table.ReplacedText.Add("date", ("Date"));
            table.ReplacedText.Add("baseUser_user", ("User"));
            table.ReplacedText.Add("baseLogAction_action", ("Action"));
            table.ReplacedText.Add("version", ("Version"));
            table.SearchCondition = " [Execute at] like '%{0}%'  OR  (select top 1 name from [BaseDynamicList] where id=basedynamicList_entity) like '%{0}%'  OR  entityId like '%{0}%'  OR  entityIdU like '%{0}%'  OR  entityIdS like '%{0}%'  OR  date like '%{0}%'  OR  (select top 1 username from [BaseUser] where ID=baseUser_user) like '%{0}%'  OR  (select top 1 name from [BaseLogAction] where id=baseLogAction_action) like '%{0}%'  OR  version like '%{0}%' ";
            table.VarName = "csBaseLog" + from.Split(':')[0];
            table.ReferenceText = "BaseLog";
            table.buildDataTable(filter);
            foreach (DataRow item in table.Data.Rows)
            {
                if (item["basedynamicList_entity"] != null)
                {
                    if (item["basedynamicList_entity"].ToString() != "")
                    {
                        int basedynamicList_entity = int.Parse(item["basedynamicList_entity"].ToString());
                        item["basedynamicList_entity"] = db.BaseDynamicLists.FirstOrDefault(d => d.id == basedynamicList_entity).name;
                    }
                    else
                        item["basedynamicList_entity"] = "Not Set";
                }
                if (item["baseUser_user"] != null)
                {
                    if (item["baseUser_user"].ToString() != "")
                    {
                        int baseUser_user = int.Parse(item["baseUser_user"].ToString());
                        item["baseUser_user"] = db.BaseUsers.FirstOrDefault(d => d.ID == baseUser_user).username;
                    }
                    else
                        item["baseUser_user"] = "Not Set";
                }
                if (item["baseLogAction_action"] != null)
                {
                    if (item["baseLogAction_action"].ToString() != "")
                    {
                        int baseLogAction_action = int.Parse(item["baseLogAction_action"].ToString());
                        item["baseLogAction_action"] = db.BaseLogActions.FirstOrDefault(d => d.id == baseLogAction_action).name;
                    }
                    else
                        item["baseLogAction_action"] = "Not Set";
                }
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
            table.TableName = "[BaseLog]";
            table.PrimaryKey = "created";
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
            table.ReplacedText.Add("created", ("Created"));
            table.ReplacedText.Add("basedynamicList_entity", ("Basedynamic List_entity"));
            table.ReplacedText.Add("entityId", ("Entity Id"));
            table.ReplacedText.Add("entityIdU", ("Entity Id U"));
            table.ReplacedText.Add("entityIdS", ("Entity Id S"));
            table.ReplacedText.Add("date", ("Date"));
            table.ReplacedText.Add("baseUser_user", ("Base User_user"));
            table.ReplacedText.Add("baseLogAction_action", ("Base Log Action_action"));
            table.ReplacedText.Add("version", ("Version"));
            table.SearchCondition = " created like '%{0}%'  OR  (select top 1 name from [BaseDynamicList] where id=basedynamicList_entity) like '%{0}%'  OR  entityId like '%{0}%'  OR  entityIdU like '%{0}%'  OR  entityIdS like '%{0}%'  OR  date like '%{0}%'  OR  (select top 1 username from [BaseUser] where ID=baseUser_user) like '%{0}%'  OR  (select top 1 name from [BaseLogAction] where id=baseLogAction_action) like '%{0}%'  OR  version like '%{0}%' ";
            table.VarName = "csBaseLog";
            table.ReferenceText = "BaseLog";
            if (Request.QueryString.AllKeys.Contains("filter"))
                table.buildDataTable(Request.QueryString["filter"]);
            else
                table.buildDataTable("");
            foreach (DataRow item in table.Data.Rows)
            {
                if (item["basedynamicList_entity"] != null)
                {
                    if (item["basedynamicList_entity"].ToString() != "")
                    {
                        int basedynamicList_entity = int.Parse(item["basedynamicList_entity"].ToString());
                        item["basedynamicList_entity"] = db.BaseDynamicLists.FirstOrDefault(d => d.id == basedynamicList_entity).name;
                    }
                    else
                        item["basedynamicList_entity"] = "Not Set";
                }
                if (item["baseUser_user"] != null)
                {
                    if (item["baseUser_user"].ToString() != "")
                    {
                        int baseUser_user = int.Parse(item["baseUser_user"].ToString());
                        item["baseUser_user"] = db.BaseUsers.FirstOrDefault(d => d.ID == baseUser_user).username;
                    }
                    else
                        item["baseUser_user"] = "Not Set";
                }
                if (item["baseLogAction_action"] != null)
                {
                    if (item["baseLogAction_action"].ToString() != "")
                    {
                        int baseLogAction_action = int.Parse(item["baseLogAction_action"].ToString());
                        item["baseLogAction_action"] = db.BaseLogActions.FirstOrDefault(d => d.id == baseLogAction_action).name;
                    }
                    else
                        item["baseLogAction_action"] = "Not Set";
                }
            }
            wb.Worksheets.Add(table.Data, "Log");
            using (var memoryStream = new MemoryStream())
            {
                wb.SaveAs(memoryStream);
                byte[] byteInfo = memoryStream.ToArray();
                return File(byteInfo, System.Net.Mime.MediaTypeNames.Application.Octet, String.Format("Log BUZZSPY {0}.xlsx", DateTime.Now.ToShortDateString()));
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
            DataTable data = Helper.getData($"SELECT [{string.Join("],[", sel)}] FROM [BaseLog] {whereCondition} ".Replace("[*]", "*"), db);
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
            BaseLog model = db.BaseLogs.Find(id);
            if (model != null)
                return Json(model, JsonRequestBehavior.AllowGet);
            else
                return Json(new { Message = "This record no longer exists" }, JsonRequestBehavior.AllowGet);
        }
        [HttpPut]
        [AllowAnonymous]
        public JsonResult create(BaseLog model)
        {
            BoolString validation = model.BeforeSave(db);
            if (validation.BoolValue)
                return Json(new { Message = validation.StringValue });
            db.BaseLogs.Add(model);
            db.SaveChanges();
            validation = model.AfterSave(db);
            if (validation.BoolValue)
                return Json(new { Message = validation.StringValue });
            return Json(new { id = model.created, MessageSucess = "That Log saved successfully." });
        }
        [HttpPost]
        [AllowAnonymous]
        public JsonResult update(int id, BaseLog model)
        {
            BoolString validation = model.BeforeEdit(db);
            if (validation.BoolValue)
                return Json(new { Message = validation.StringValue });
            db.Entry(model).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            validation = model.AfterEdit(db);
            if (validation.BoolValue)
                return Json(new { Message = validation.StringValue });
            return Json(new { id = model.created, MessageSucess = "That Log saved successfully." });
        }
        [HttpDelete]
        [AllowAnonymous]
        public JsonResult remove(int id = 0)
        {
            try
            {
                BaseLog model = db.BaseLogs.Find(id);
                if (model != null)
                {
                    BoolString validation = model.BeforeDelete(db);
                    if (validation.BoolValue)
                        return Json(new { Message = validation.StringValue }, JsonRequestBehavior.AllowGet);
                    db.BaseLogs.Remove(model);
                    db.SaveChanges();
                    validation = model.AfterDelete(db);
                    if (validation.BoolValue)
                        return Json(new { Message = validation.StringValue }, JsonRequestBehavior.AllowGet);
                    return Json(new { id = model.created, MessageSucess = "That Log deleted successfully." }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { Message = "This record no longer exists" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Message = Helper.ModeralException(ex).Replace("@table", "Log") }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
