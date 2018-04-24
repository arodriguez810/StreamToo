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
    public class RDClientChannelController : BaseController
    {
        [IsView]
        public ActionResult Index(int id = 0, string from = "")
        {
            List<BaseDynamicColumnList> bDynamicColumn = db.BaseDynamicColumnLists.ToList();
            List<BaseDynamicList> bDynamicList = db.BaseDynamicLists.ToList();
            ViewBag.column = bDynamicColumn;
            ViewBag.dynamicList = bDynamicList;
            ViewBag.from = from;
            ViewBag.tableName = "RDClientChannel";
            return PartialView(new RDClientChannel());
        }
        [IsView]
        public ActionResult Form(string id = "", string from = "")
        {
            ViewBag.relations = db.VWISRElations.Where(d => d.PK_Table == "RDClientChannel").ToList();
            ViewBag.from = from;
            if (id == "")
                return PartialView(new RDClientChannel() { active = true, creationDate = DateTime.Now });
            else
            {
                RDClientChannel model = db.RDClientChannels.Find(Guid.Parse(id.Replace("─", "-")));
                return PartialView(model);
            }
        }
        [HttpPost]
        [ValidateInput(false)]
        public JsonResult Save(RDClientChannel model, FormCollection form)
        {
            try
            {
                model.id = form["id"].ToString() == "00000000-0000-0000-0000-000000000000" ? new Guid() : Guid.Parse(form["id"].ToString().Replace("─", "-"));
                if (model.id != new Guid())
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
                    model.id = Guid.NewGuid();
                    BoolString validation = model.BeforeSave(db);
                    if (validation.BoolValue)
                        return Json(new { Message = validation.StringValue });
                    validation = model.BeforeCreate(db);
                    if (validation.BoolValue)
                        return Json(new { Message = validation.StringValue });

                    db.RDClientChannels.Add(model);
                    db.SaveChanges();
                    validation = model.AfterSave(db);
                    if (validation.BoolValue)
                        return Json(new { Message = validation.StringValue });
                    validation = model.AfterCreate(db);
                    if (validation.BoolValue)
                        return Json(new { Message = validation.StringValue });
                }
                return Json(new { id = model.id, MessageSucess = "That Client Channel saved successfully." });
            }
            catch (Exception ex)
            {
                return Json(new { Message = Helper.ModeralException(ex).Replace("@table", "Client Channel") });
            }
        }
        public JsonResult ToggleActive(string id = "")
        {
            try
            {
                RDClientChannel model = db.RDClientChannels.Find(Guid.Parse(id.Replace("─", "-")));
                if (model != null)
                {
                    if (model.active.Value)
                    {
                        BoolString validation = model.BeforeInactive(db);
                        if (validation.BoolValue)
                            return Json(new { Message = validation.StringValue }, JsonRequestBehavior.AllowGet);
                        model.active = !model.active;
                        db.SaveChanges();
                        validation = model.AfterInactive(db);
                        if (validation.BoolValue)
                            return Json(new { Message = validation.StringValue }, JsonRequestBehavior.AllowGet);
                        return Json(new { id = model.id, MessageSucess = "That Client Channel Inactive successfully." }, JsonRequestBehavior.AllowGet);
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
                        return Json(new { id = model.id, MessageSucess = "That Client Channel Active successfully." }, JsonRequestBehavior.AllowGet);
                    }
                }
                return Json(new { Message = "This record no longer exists" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Message = Helper.ModeralException(ex).Replace("@table", "Client Channel") }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult Delete(string id = "")
        {
            try
            {
                RDClientChannel model = db.RDClientChannels.Find(Guid.Parse(id.Replace("─", "-")));
                if (model != null)
                {
                    BoolString validation = model.BeforeDelete(db);
                    if (validation.BoolValue)
                        return Json(new { Message = validation.StringValue }, JsonRequestBehavior.AllowGet);
                    db.RDClientChannels.Remove(model);
                    db.SaveChanges();
                    validation = model.AfterDelete(db);

                    if (validation.BoolValue)
                        return Json(new { Message = validation.StringValue }, JsonRequestBehavior.AllowGet);
                    return Json(new { id = model.id, MessageSucess = "That Client Channel deleted successfully." }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { Message = "This record no longer exists" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Message = Helper.ModeralException(ex).Replace("@table", "Client Channel") }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult data(FormCollection form, string from = "", string filter = "")
        {
            JsonCsTableHelper table = new JsonCsTableHelper(db, form, Request.QueryString);
            table.TableName = "[RDClientChannel]";
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
            table.ReplacedText.Add("channel_channel", ("Channel"));
            table.ReplacedText.Add("visitCount", ("Visit Count"));
            table.ReplacedText.Add("lastVisit", ("Last Visit"));
            table.ReplacedText.Add("client_client", ("Client"));
            table.ReplacedText.Add("isFavorite", ("Is Favorite"));
            table.ReplacedText.Add("active", ("Active"));
            table.ReplacedText.Add("creationDate", ("Creation Date"));
            table.SearchCondition = " id like '%{0}%'  OR  (select top 1 name from [RDChannel] where id=channel_channel) like '%{0}%'  OR  visitCount like '%{0}%'  OR  lastVisit like '%{0}%'  OR  (select top 1 email from [RDClient] where id=client_client) like '%{0}%'  OR  isFavorite like '%{0}%'  OR  active like '%{0}%'  OR  creationDate like '%{0}%' ";
            table.VarName = "csRDClientChannel" + from.Split(':')[0];
            table.ReferenceText = "ClientChannel";
            table.Links.Add(new CsTableLink() { ButtonType = URLHelper.buttonType("RDClientChannel"), Href = URLHelper.getActionUrlRelation("RDClientChannel", "Form"), Title = "Edit", Css = "btn btn-warning btn-circle refreshTable", Icon = BaseUIIconText.fa.fa_fa_pencil, Extra = extraRelation });
            table.Links.Add(new CsTableLink() { ButtonType = CsTableLinkType.functionButton, Href = URLHelper.getActionUrl("RDClientChannel", "ToggleActive"), Title = "Active/Inactive", Css = "btn btn-warning btn-circle refreshTable", Icon = BaseUIIconText.fa.fa_fa_asterisk });
            table.Links.Add(new CsTableLink() { ButtonType = CsTableLinkType.deleteButton, Href = URLHelper.getActionUrl("RDClientChannel", "Delete"), Title = "Delete", Css = "btn btn-danger btn-circle refreshTable", Icon = BaseUIIconText.fa.fa_fa_trash_o });
            table.buildDataTable(filter);
            foreach (DataRow item in table.Data.Rows)
            {
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
                if (item["client_client"] != null)
                {
                    if (item["client_client"].ToString() != "")
                    {
                        int client_client = int.Parse(item["client_client"].ToString());
                        item["client_client"] = db.RDClients.FirstOrDefault(d => d.id == client_client).email;
                    }
                    else
                        item["client_client"] = "Not Set";
                }
                if (item["isFavorite"] != null)
                {
                    item["isFavorite"] = (item["isFavorite"].ToString() == "1" || item["isFavorite"].ToString() == "True") ? ("Yes") : ("No");
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
            table.TableName = "[RDClientChannel]";
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
            table.ReplacedText.Add("channel_channel", ("Channel_channel"));
            table.ReplacedText.Add("visitCount", ("Visit Count"));
            table.ReplacedText.Add("lastVisit", ("Last Visit"));
            table.ReplacedText.Add("client_client", ("Client_client"));
            table.ReplacedText.Add("isFavorite", ("Is Favorite"));
            table.ReplacedText.Add("active", ("Active"));
            table.ReplacedText.Add("creationDate", ("Creation Date"));
            table.SearchCondition = " id like '%{0}%'  OR  (select top 1 name from [RDChannel] where id=channel_channel) like '%{0}%'  OR  visitCount like '%{0}%'  OR  lastVisit like '%{0}%'  OR  (select top 1 email from [RDClient] where id=client_client) like '%{0}%'  OR  isFavorite like '%{0}%'  OR  active like '%{0}%'  OR  creationDate like '%{0}%' ";
            table.VarName = "csRDClientChannel";
            table.ReferenceText = "ClientChannel";
            table.Links.Add(new CsTableLink() { ButtonType = CsTableLinkType.button, Href = URLHelper.getActionUrl("RDClientChannel", "Form"), Title = "Edit", Css = "btn btn-warning btn-circle refreshTable", Icon = BaseUIIconText.fa.fa_fa_pencil });
            table.Links.Add(new CsTableLink() { ButtonType = CsTableLinkType.deleteButton, Href = URLHelper.getActionUrl("RDClientChannel", "Delete"), Title = "Delete", Css = "btn btn-danger btn-circle refreshTable", Icon = BaseUIIconText.fa.fa_fa_trash_o });
            if (Request.QueryString.AllKeys.Contains("filter"))
                table.buildDataTable(Request.QueryString["filter"]);
            else
                table.buildDataTable("");
            foreach (DataRow item in table.Data.Rows)
            {
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
                if (item["client_client"] != null)
                {
                    if (item["client_client"].ToString() != "")
                    {
                        int client_client = int.Parse(item["client_client"].ToString());
                        item["client_client"] = db.RDClients.FirstOrDefault(d => d.id == client_client).email;
                    }
                    else
                        item["client_client"] = "Not Set";
                }
                if (item["isFavorite"] != null)
                {
                    item["isFavorite"] = (item["isFavorite"].ToString() == "1" || item["isFavorite"].ToString() == "True") ? ("Yes") : ("No");
                }
                if (item["active"] != null)
                {
                    item["active"] = (item["active"].ToString() == "1" || item["active"].ToString() == "True") ? ("Yes") : ("No");
                }
            }
            wb.Worksheets.Add(table.Data, "Client Channel");
            using (var memoryStream = new MemoryStream())
            {
                wb.SaveAs(memoryStream);
                byte[] byteInfo = memoryStream.ToArray();
                return File(byteInfo, System.Net.Mime.MediaTypeNames.Application.Octet, String.Format("Client Channel StreamToo {0}.xlsx", DateTime.Now.ToShortDateString()));
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
            DataTable data = Helper.getData($"SELECT [{string.Join("],[", sel)}] FROM [RDClientChannel] {whereCondition} ".Replace("[*]", "*"), db);
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
        public JsonResult get(string id)
        {
            RDClientChannel model = db.RDClientChannels.Find(id);
            if (model != null)
                return Json(model, JsonRequestBehavior.AllowGet);
            else
                return Json(new { Message = "This record no longer exists" }, JsonRequestBehavior.AllowGet);
        }
        [HttpPut]
        [AllowAnonymous]
        public JsonResult create(RDClientChannel model)
        {
            BoolString validation = model.BeforeSave(db);
            if (validation.BoolValue)
                return Json(new { Message = validation.StringValue });
            db.RDClientChannels.Add(model);
            db.SaveChanges();
            validation = model.AfterSave(db);
            if (validation.BoolValue)
                return Json(new { Message = validation.StringValue });
            return Json(new { id = model.id, MessageSucess = "That Client Channel saved successfully." });
        }
        [HttpPost]
        [AllowAnonymous]
        public JsonResult update(int id, RDClientChannel model)
        {
            BoolString validation = model.BeforeEdit(db);
            if (validation.BoolValue)
                return Json(new { Message = validation.StringValue });
            db.Entry(model).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            validation = model.AfterEdit(db);
            if (validation.BoolValue)
                return Json(new { Message = validation.StringValue });
            return Json(new { id = model.id, MessageSucess = "That Client Channel saved successfully." });
        }
        [HttpDelete]
        [AllowAnonymous]
        public JsonResult remove(int id = 0)
        {
            try
            {
                RDClientChannel model = db.RDClientChannels.Find(id);
                if (model != null)
                {
                    BoolString validation = model.BeforeDelete(db);
                    if (validation.BoolValue)
                        return Json(new { Message = validation.StringValue }, JsonRequestBehavior.AllowGet);
                    db.RDClientChannels.Remove(model);
                    db.SaveChanges();
                    validation = model.AfterDelete(db);
                    if (validation.BoolValue)
                        return Json(new { Message = validation.StringValue }, JsonRequestBehavior.AllowGet);
                    return Json(new { id = model.id, MessageSucess = "That Client Channel deleted successfully." }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { Message = "This record no longer exists" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Message = Helper.ModeralException(ex).Replace("@table", "Client Channel") }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
