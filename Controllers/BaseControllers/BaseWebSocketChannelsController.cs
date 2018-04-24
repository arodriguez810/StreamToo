using Admin.BaseClass;
using Admin.BaseClass.UI;
using Admin.CustomCode;
using Admin.Models;
using ClosedXML.Excel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace Admin.Controllers
{
    public class BaseWebSocketChannelsController : BaseController
    {
        [IsView]
        public ActionResult Index(int id = 0, string from = "")
        {
            List<BaseDynamicColumnList> bDynamicColumn = db.BaseDynamicColumnLists.ToList();
            List<BaseDynamicList> bDynamicList = db.BaseDynamicLists.ToList();
            ViewBag.column = bDynamicColumn;
            ViewBag.dynamicList = bDynamicList;
            ViewBag.from = from;
            ViewBag.tableName = "BaseWebSocketChannel";
            return PartialView(new BaseWebSocketChannel());
        }

        [IsView]
        public ActionResult Form(int id = 0, string from = "")
        {
            ViewBag.relations = db.VWISRElations.Where(d => d.PK_Table == "BaseWebSocketChannels").ToList();
            ViewBag.from = from;
            if (id == 0)
                return PartialView(new BaseWebSocketChannel());
            else
            {
                BaseWebSocketChannel model = db.BaseWebSocketChannels.Find(id);
                return PartialView(model);
            }
        }

        [IsView]
        public ActionResult FormEmit(int id = 0)
        {
            if (id == 0)
                return PartialView(new BaseWebSocketChannel());
            else
            {
                BaseWebSocketChannel model = db.BaseWebSocketChannels.Find(id);
                return PartialView(model);
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public JsonResult Save(BaseWebSocketChannel model, FormCollection form)
        {
            try
            {
                if (string.IsNullOrEmpty(model.ParametersName))
                    model.ParametersName = string.Empty;

                model.ParametersName = model.ParametersName.Trim();
                model.Name = Regex.Replace(model.Name, @"\s+", "-").Trim();

                if (model.Id != 0)
                {
                    db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                }
                else
                {
                    db.BaseWebSocketChannels.Add(model);
                }

                db.SaveChanges();
                return Json(new { id = model.Id, MessageSucess = "That Web Socket Channels saved successfully." });
            }
            catch (Exception ex)
            {
                return Json(new { Message = Helper.ModeralException(ex).Replace("@table", "Web Socket Channels") });
            }
        }

        public JsonResult Delete(int id = 0)
        {
            try
            {
                BaseWebSocketChannel model = db.BaseWebSocketChannels.Find(id);
                if (model != null)
                {
                    db.BaseWebSocketChannels.Remove(model);
                    db.SaveChanges();

                    return Json(new { id = model.Id, MessageSucess = "That Web Socket Channels deleted successfully." }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { Message = "This record no longer exists" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Message = Helper.ModeralException(ex).Replace("@table", "Web Socket Channels") }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult data(FormCollection form, string from = "", string filter = "")
        {
            JsonCsTableHelper table = new JsonCsTableHelper(db, form, Request.QueryString);
            table.TableName = "[BaseWebSocketChannels]";
            table.PrimaryKey = "Id";
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
            table.ReplacedText.Add("Name", ("Name"));
            table.ReplacedText.Add("IsGlobal", ("Is Global"));
            table.ReplacedText.Add("ServerCode", ("Server Code"));
            table.ReplacedText.Add("ParametersName", ("Parameters Name"));
            table.ReplacedText.Add("StartDate", ("Start Date"));
            table.ReplacedText.Add("EndDate", ("End Date"));
            table.ReplacedText.Add("StartTime", ("Start Time"));
            table.ReplacedText.Add("EndTime", ("End Time"));
            table.ReplacedText.Add("DataEmitExample", ("Data Emit Example"));
            table.ReplacedText.Add("ClientCode", ("Client Code"));
            table.ReplacedText.Add("NoDateLimit", ("No Date Limit"));
            table.ReplacedText.Add("NoTimeLimit", ("No Time Limit"));
            table.SearchCondition = " Id like '%{0}%'  OR  Name like '%{0}%'  OR  IsGlobal like '%{0}%'  OR  ServerCode like '%{0}%'  OR  ParametersName like '%{0}%'  OR  StartDate like '%{0}%'  OR  EndDate like '%{0}%'  OR  StartTime like '%{0}%'  OR  EndTime like '%{0}%'  OR  DataEmitExample like '%{0}%'  OR  ClientCode like '%{0}%' OR  NoDateLimit like '%{0}%' OR  NoTimeLimit like '%{0}%' ";
            table.VarName = "BaseWebSocketChannels" + from.Split(':')[0];
            table.ReferenceText = "BaseWebSocketChannels";
            table.Links.Add(new CsTableLink() { ButtonType = URLHelper.buttonType("BaseWebSocketChannels"), Href = URLHelper.getActionUrlRelation("BaseWebSocketChannels", "Form"), Title = "Edit", Css = "btn btn-warning btn-circle refreshTable", Icon = BaseUIIconText.fa.fa_fa_pencil, Extra = extraRelation });
            table.Links.Add(new CsTableLink() { ButtonType = CsTableLinkType.button, Href = URLHelper.getActionUrl("BaseWebSocketChannels", "FormEmit"), Title = "Make Test Emit", Css = "btn btn-warning btn-circle", Icon = BaseUIIconText.fa.fa_fa_envelope });
            table.Links.Add(new CsTableLink() { ButtonType = CsTableLinkType.deleteButton, Href = URLHelper.getActionUrl("BaseWebSocketChannels", "Delete"), Title = "Delete", Css = "btn btn-danger btn-circle refreshTable", Icon = BaseUIIconText.fa.fa_fa_trash_o });
            table.buildDataTable(filter);
            foreach (DataRow item in table.Data.Rows)
            {
                if (item["IsGlobal"] != null)
                {
                    item["IsGlobal"] = (item["IsGlobal"].ToString() == "1" || item["IsGlobal"].ToString() == "True") ? ("Yes") : ("No");
                }

                if (item["NoDateLimit"] != null)
                {
                    item["NoDateLimit"] = (item["NoDateLimit"].ToString() == "1" || item["NoDateLimit"].ToString() == "True") ? ("Yes") : ("No");
                }

                if (item["NoTimeLimit"] != null)
                {
                    item["NoTimeLimit"] = (item["NoTimeLimit"].ToString() == "1" || item["NoTimeLimit"].ToString() == "True") ? ("Yes") : ("No");
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
            table.TableName = "[BaseWebSocketChannels]";
            table.PrimaryKey = "Id";
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
            table.ReplacedText.Add("Name", ("Name"));
            table.ReplacedText.Add("IsGlobal", ("Is Global"));
            table.ReplacedText.Add("ServerCode", ("Server Code"));
            table.ReplacedText.Add("ParametersName", ("Parameters Name"));
            table.ReplacedText.Add("StartDate", ("Start Date"));
            table.ReplacedText.Add("EndDate", ("End Date"));
            table.ReplacedText.Add("StartTime", ("Start Time"));
            table.ReplacedText.Add("EndTime", ("End Time"));
            table.ReplacedText.Add("DataEmitExample", ("Data Emit Example"));
            table.ReplacedText.Add("ClientCode", ("Client Code"));
            table.ReplacedText.Add("NoDateLimit", ("No Date Limit"));
            table.ReplacedText.Add("NoTimeLimit", ("No Time Limit"));
            table.SearchCondition = " Id like '%{0}%'  OR  Name like '%{0}%'  OR  IsGlobal like '%{0}%'  OR  ServerCode like '%{0}%'  OR  ParametersName like '%{0}%'  OR  StartDate like '%{0}%'  OR  EndDate like '%{0}%'  OR  StartTime like '%{0}%'  OR  EndTime like '%{0}%'  OR  DataEmitExample like '%{0}%'  OR  ClientCode like '%{0}%' OR  NoDateLimit like '%{0}%' OR  NoTimeLimit like '%{0}%' ";
            table.VarName = "csBaseWebSocketChannels";
            table.ReferenceText = "WebSocketChannels";
            table.Links.Add(new CsTableLink() { ButtonType = CsTableLinkType.button, Href = URLHelper.getActionUrl("BaseWebSocketChannels", "Form"), Title = "Edit", Css = "btn btn-warning btn-circle refreshTable", Icon = BaseUIIconText.fa.fa_fa_pencil });
            table.Links.Add(new CsTableLink() { ButtonType = CsTableLinkType.deleteButton, Href = URLHelper.getActionUrl("BaseWebSocketChannels", "Delete"), Title = "Delete", Css = "btn btn-danger btn-circle refreshTable", Icon = BaseUIIconText.fa.fa_fa_trash_o });
            if (Request.QueryString.AllKeys.Contains("filter"))
                table.buildDataTable(Request.QueryString["filter"]);
            else
                table.buildDataTable("");
            foreach (DataRow item in table.Data.Rows)
            {
                if (item["IsGlobal"] != null)
                {
                    item["IsGlobal"] = (item["IsGlobal"].ToString() == "1" || item["IsGlobal"].ToString() == "True") ? ("Yes") : ("No");
                }

                if (item["NoDateLimit"] != null)
                {
                    item["NoDateLimit"] = (item["NoDateLimit"].ToString() == "1" || item["NoDateLimit"].ToString() == "True") ? ("Yes") : ("No");
                }

                if (item["NoTimeLimit"] != null)
                {
                    item["NoTimeLimit"] = (item["NoTimeLimit"].ToString() == "1" || item["NoTimeLimit"].ToString() == "True") ? ("Yes") : ("No");
                }
            }
            wb.Worksheets.Add(table.Data, "Web Socket Channels");
            using (var memoryStream = new MemoryStream())
            {
                wb.SaveAs(memoryStream);
                byte[] byteInfo = memoryStream.ToArray();
                return File(byteInfo, System.Net.Mime.MediaTypeNames.Application.Octet, String.Format("Web Socket Channels SMSChat {0}.xlsx", DateTime.Now.ToShortDateString()));
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
            DataTable data = Helper.getData($"SELECT [{string.Join("],[", sel)}] FROM [BaseWebSocketChannels] {whereCondition} ".Replace("[*]", "*"), db);
            List<string> ncolumns = new List<string>();
            foreach (DataColumn item in data.Columns)
                ncolumns.Add(item.ColumnName);
            ncolumns.Add("WebSocketChannelBlackListProfiles");
            ncolumns.Add("WebSocketChannelBlackListUsers");
            List<object> rows = new List<object>();
            foreach (DataRow item in data.Rows)
            {
                List<object> adds = item.ItemArray.ToList();
                adds.Add(Helper.Regular(db, item, "BaseWebSocketChannelBlackListProfile", $" where [WebSocketChannelId] = '@id@'"));
                adds.Add(Helper.Regular(db, item, "BaseWebSocketChannelBlackListUser", $" where [WebSocketChannelId] = '@id@'"));
                rows.Add(adds.ToArray());
            }
            return Json(new { columns = ncolumns, rows = rows, count = data.Rows.Count }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [AllowAnonymous]
        public JsonResult get(int id)
        {
            BaseWebSocketChannel model = db.BaseWebSocketChannels.Find(id);
            if (model != null)
                return Json(model, JsonRequestBehavior.AllowGet);
            else
                return Json(new { Message = "This record no longer exists" }, JsonRequestBehavior.AllowGet);
        }

        [HttpPut]
        [AllowAnonymous]
        public JsonResult create(BaseWebSocketChannel model)
        {
            if (string.IsNullOrEmpty(model.ParametersName))
                model.ParametersName = string.Empty;

            model.ParametersName = model.ParametersName.Trim();
            model.Name = Regex.Replace(model.Name, @"\s+", "-").Trim();

            db.BaseWebSocketChannels.Add(model);
            db.SaveChanges();

            return Json(new { id = model.Id, MessageSucess = "That Web Socket Channels saved successfully." });
        }

        [HttpPost]
        [AllowAnonymous]
        public JsonResult update(int id, BaseWebSocketChannel model)
        {
            if (string.IsNullOrEmpty(model.ParametersName))
                model.ParametersName = string.Empty;

            model.ParametersName = model.ParametersName.Trim();
            model.Name = Regex.Replace(model.Name, @"\s+", "-").Trim();

            db.Entry(model).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            return Json(new { id = model.Id, MessageSucess = "That Web Socket Channels saved successfully." });
        }

        [HttpDelete]
        [AllowAnonymous]
        public JsonResult remove(int id = 0)
        {
            try
            {
                BaseWebSocketChannel model = db.BaseWebSocketChannels.Find(id);
                if (model != null)
                {
                    db.BaseWebSocketChannels.Remove(model);
                    db.SaveChanges();

                    return Json(new { id = model.Id, MessageSucess = "That Web Socket Channels deleted successfully." }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { Message = "This record no longer exists" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Message = Helper.ModeralException(ex).Replace("@table", "Web Socket Channels") }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
