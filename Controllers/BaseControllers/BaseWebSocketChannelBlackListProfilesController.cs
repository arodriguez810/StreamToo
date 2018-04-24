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
using System.Web.Mvc;

namespace Admin.Controllers
{
    public class BaseWebSocketChannelBlackListProfilesController : BaseController
    {
        [IsView]
        public ActionResult Index(int id = 0, string from = "")
        {
            List<BaseDynamicColumnList> bDynamicColumn = db.BaseDynamicColumnLists.ToList();
            List<BaseDynamicList> bDynamicList = db.BaseDynamicLists.ToList();
            ViewBag.column = bDynamicColumn;
            ViewBag.dynamicList = bDynamicList;
            ViewBag.from = from;
            ViewBag.tableName = "BaseWebSocketChannelBlackListProfiles";
            return PartialView(new BaseWebSocketChannelBlackListProfile());
        }

        [IsView]
        public ActionResult Form(int id = 0, string from = "")
        {
            ViewBag.relations = db.VWISRElations.Where(d => d.PK_Table == "BaseWebSocketChannelBlackListProfiles").ToList();
            ViewBag.from = from;
            if (id == 0)
                return PartialView(new BaseWebSocketChannelBlackListProfile());
            else
            {
                BaseWebSocketChannelBlackListProfile model = db.BaseWebSocketChannelBlackListProfiles.Find(id);
                return PartialView(model);
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public JsonResult Save(BaseWebSocketChannelBlackListProfile model, FormCollection form)
        {
            try
            {
                if (model.Id != 0)
                {
                    db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
                else
                {
                    db.BaseWebSocketChannelBlackListProfiles.Add(model);
                    db.SaveChanges();
                }
                return Json(new { id = model.Id, MessageSucess = "That Web Socket Channel Black List Profiles saved successfully." });
            }
            catch (Exception ex)
            {
                return Json(new { Message = Helper.ModeralException(ex).Replace("@table", "Web Socket Channel Black List Profiles") });
            }
        }

        public JsonResult Delete(int id = 0)
        {
            try
            {
                BaseWebSocketChannelBlackListProfile model = db.BaseWebSocketChannelBlackListProfiles.Find(id);
                if (model != null)
                {
                    db.BaseWebSocketChannelBlackListProfiles.Remove(model);
                    db.SaveChanges();

                    return Json(new { id = model.Id, MessageSucess = "That Web Socket Channel Black List Profiles deleted successfully." }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { Message = "This record no longer exists" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Message = Helper.ModeralException(ex).Replace("@table", "Web Socket Channel Black List Profiles") }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult data(FormCollection form, string from = "", string filter = "")
        {
            JsonCsTableHelper table = new JsonCsTableHelper(db, form, Request.QueryString);
            table.TableName = "[BaseWebSocketChannelBlackListProfiles]";
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
            table.ReplacedText.Add("ProfileId", ("Profile Id"));
            table.ReplacedText.Add("WebSocketChannelId", ("Web Socket Channel Id"));
            table.SearchCondition = " Id like '%{0}%'  OR  (select top 1 name from [BaseProfile] where id=ProfileId) like '%{0}%'  OR  (select top 1 Name from [BaseWebSocketChannels] where id=WebSocketChannelId) like '%{0}%' ";
            table.VarName = "csBaseWebSocketChannelBlackListProfiles" + from.Split(':')[0];
            table.ReferenceText = "WebSocketChannelBlackListProfiles";
            table.Links.Add(new CsTableLink() { ButtonType = URLHelper.buttonType("BaseWebSocketChannelBlackListProfiles"), Href = URLHelper.getActionUrlRelation("BaseWebSocketChannelBlackListProfiles", "Form"), Title = "Edit", Css = "btn btn-warning btn-circle refreshTable", Icon = BaseUIIconText.fa.fa_fa_pencil, Extra = extraRelation });
            table.Links.Add(new CsTableLink() { ButtonType = CsTableLinkType.deleteButton, Href = URLHelper.getActionUrl("BaseWebSocketChannelBlackListProfiles", "Delete"), Title = "Delete", Css = "btn btn-danger btn-circle refreshTable", Icon = BaseUIIconText.fa.fa_fa_trash_o });
            table.buildDataTable(filter);
            foreach (DataRow item in table.Data.Rows)
            {
                if (item["ProfileId"] != null)
                {
                    if (item["ProfileId"].ToString() != "")
                    {
                        int ProfileId = int.Parse(item["ProfileId"].ToString());
                        item["ProfileId"] = db.BaseProfiles.FirstOrDefault(d => d.id == ProfileId).name;
                    }
                    else
                        item["ProfileId"] = "Not Set";
                }
                if (item["WebSocketChannelId"] != null)
                {
                    if (item["WebSocketChannelId"].ToString() != "")
                    {
                        int WebSocketChannelId = int.Parse(item["WebSocketChannelId"].ToString());
                        item["WebSocketChannelId"] = db.BaseWebSocketChannels.FirstOrDefault(d => d.Id == WebSocketChannelId).Name;
                    }
                    else
                        item["WebSocketChannelId"] = "Not Set";
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
            table.TableName = "[BaseWebSocketChannelBlackListProfiles]";
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
            table.ReplacedText.Add("ProfileId", ("Profile Id"));
            table.ReplacedText.Add("WebSocketChannelId", ("Web Socket Channel Id"));
            table.SearchCondition = " Id like '%{0}%'  OR  (select top 1 name from [BaseProfile] where id=ProfileId) like '%{0}%'  OR  (select top 1 Name from [BaseWebSocketChannels] where id=WebSocketChannelId) like '%{0}%' ";
            table.VarName = "csBaseWebSocketChannelBlackListProfiles";
            table.ReferenceText = "WebSocketChannelBlackListProfiles";
            table.Links.Add(new CsTableLink() { ButtonType = CsTableLinkType.button, Href = URLHelper.getActionUrl("BaseWebSocketChannelBlackListProfiles", "Form"), Title = "Edit", Css = "btn btn-warning btn-circle refreshTable", Icon = BaseUIIconText.fa.fa_fa_pencil });
            table.Links.Add(new CsTableLink() { ButtonType = CsTableLinkType.deleteButton, Href = URLHelper.getActionUrl("BaseWebSocketChannelBlackListProfiles", "Delete"), Title = "Delete", Css = "btn btn-danger btn-circle refreshTable", Icon = BaseUIIconText.fa.fa_fa_trash_o });
            if (Request.QueryString.AllKeys.Contains("filter"))
                table.buildDataTable(Request.QueryString["filter"]);
            else
                table.buildDataTable("");
            foreach (DataRow item in table.Data.Rows)
            {
                if (item["UserId"] != null)
                {
                    if (item["UserId"].ToString() != "")
                    {
                        int UserId = int.Parse(item["UserId"].ToString());
                        item["UserId"] = db.BaseUsers.FirstOrDefault(d => d.ID == UserId).username;
                    }
                    else
                        item["UserId"] = "Not Set";
                }
                if (item["WebSocketChannelId"] != null)
                {
                    if (item["WebSocketChannelId"].ToString() != "")
                    {
                        int WebSocketChannelId = int.Parse(item["WebSocketChannelId"].ToString());
                        item["WebSocketChannelId"] = db.BaseWebSocketChannels.FirstOrDefault(d => d.Id == WebSocketChannelId).Name;
                    }
                    else
                        item["WebSocketChannelId"] = "Not Set";
                }
            }
            wb.Worksheets.Add(table.Data, "Web Socket Channel Black List Profiles");
            using (var memoryStream = new MemoryStream())
            {
                wb.SaveAs(memoryStream);
                byte[] byteInfo = memoryStream.ToArray();
                return File(byteInfo, System.Net.Mime.MediaTypeNames.Application.Octet, String.Format("Web Socket Channel Black List Profiles SMSChat {0}.xlsx", DateTime.Now.ToShortDateString()));
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
            DataTable data = Helper.getData($"SELECT [{string.Join("],[", sel)}] FROM [BaseWebSocketChannelBlackListProfiles] {whereCondition} ".Replace("[*]", "*"), db);
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
            BaseWebSocketChannelBlackListProfile model = db.BaseWebSocketChannelBlackListProfiles.Find(id);
            if (model != null)
                return Json(model, JsonRequestBehavior.AllowGet);
            else
                return Json(new { Message = "This record no longer exists" }, JsonRequestBehavior.AllowGet);
        }

        [HttpPut]
        [AllowAnonymous]
        public JsonResult create(BaseWebSocketChannelBlackListProfile model)
        {
            db.BaseWebSocketChannelBlackListProfiles.Add(model);
            db.SaveChanges();

            return Json(new { id = model.Id, MessageSucess = "That Web Socket Channel Black List Profiles saved successfully." });
        }

        [HttpPost]
        [AllowAnonymous]
        public JsonResult update(int id, BaseWebSocketChannelBlackListProfile model)
        {
            db.Entry(model).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            return Json(new { id = model.Id, MessageSucess = "That Web Socket Channel Black List Profiles saved successfully." });
        }

        [HttpDelete]
        [AllowAnonymous]
        public JsonResult remove(int id = 0)
        {
            try
            {
                BaseWebSocketChannelBlackListProfile model = db.BaseWebSocketChannelBlackListProfiles.Find(id);
                if (model != null)
                {
                    db.BaseWebSocketChannelBlackListProfiles.Remove(model);
                    db.SaveChanges();

                    return Json(new { id = model.Id, MessageSucess = "That Web Socket Channel Black List Profiles deleted successfully." }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { Message = "This record no longer exists" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Message = Helper.ModeralException(ex).Replace("@table", "Web Socket Channel Black List Profiles") }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
