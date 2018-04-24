using Admin.BaseClass;
using Admin.BaseClass.UI;
using Admin.CustomCode;
using Admin.Models;
using Admin.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web.Mvc;
namespace Admin.Controllers
{
    public class BaseQueryGraphController : BaseController
    {
        [IsView]
        public ActionResult Index()
        {
            return PartialView(new BaseQueryGraph());
        }

        [IsView]
        public ActionResult Form(int id = 0)
        {
            if (id == 0)
                return PartialView(new BaseQueryGraph());
            else
            {
                BaseQueryGraph model = db.BaseQueryGraphs.Find(id);
                return PartialView(model);
            }
        }

        [HttpPost]
        public JsonResult Save(BaseQueryGraph model, FormCollection form)
        {
            if (model.id != 0)
            {
                if (ModelState.IsValid)
                {
                    db.Entry(model).State = EntityState.Modified;
                    if (form["querys"] != null)
                    {
                        model.query = form["querys"];
                    }
                    db.SaveChanges();
                }
            }
            else
            {
                if (form["querys"] != null)
                {
                    model.query = form["querys"];
                }
                db.BaseQueryGraphs.Add(model);
                db.SaveChanges();
            }
            return Json(model.id);
        }
        public JsonResult Delete(int id = 0)
        {
            BaseQueryGraph model = db.BaseQueryGraphs.Find(id);
            if (model != null)
            {
                var graph = db.BaseGraphs.FirstOrDefault(x => x.queryGraphID == model.id);
                if (graph != null)
                {
                    Helper.executeNonQUery("delete from GraphsTypes where GraphID = " + graph.id, db);
                    Helper.executeNonQUery("delete from Graphs where queryGraphID =" + model.id, db);
                }
                Helper.executeNonQUery("delete from BaseQueryGraph where id =" + model.id, db);
                //db.QueryGraphs.Remove(model);
                //db.SaveChanges();
            } return Json("ok", JsonRequestBehavior.AllowGet);
        }
        public JsonResult data(FormCollection form, string filter = "")
        {
            JsonCsTableHelper table = new JsonCsTableHelper(db, form, Request.QueryString);
            table.TableName = "[BaseQueryGraph]";
            table.PrimaryKey = "id";
            table.SearchCondition = " id like '%{0}%' OR name like '%{0}%' OR query like '%{0}%' ";
            table.VarName = "csQueryGraph";
            table.ReferenceText = "QueryGraph";
            table.Links.Add(new CsTableLink() { ButtonType = CsTableLinkType.button, Href = URLHelper.getActionUrl("BaseQueryGraph", "Form"), Title = "Edit", Css = "btn btn-warning btn-circle refreshTable", Icon = BaseUIIconText.fa.fa_fa_pencil });
            table.Links.Add(new CsTableLink() { ButtonType = CsTableLinkType.deleteButton, Href = URLHelper.getActionUrl("BaseQueryGraph", "Delete"), Title = "Delete", Css = "btn btn-danger btn-circle refreshTable", Icon = BaseUIIconText.fa.fa_fa_trash_o });
            table.buildDataTable(filter);
            table.buildDefaultJson();
            table.buildLinks();
            return Json(table.Json, JsonRequestBehavior.AllowGet);
        }
        public JsonResult TableColumns(string tableName, int idTab = 0)
        {
            StringBuilder html = new StringBuilder();

            List<optionsSelect> l = new List<optionsSelect>();
            DataTable dt = Helper.getData(
                string.Format("select C.name as 'Column' from sys.columns C " +
                                                        " where object_id= (select object_id from sys.tables where name='{0}') " +
                                                        " and C.name != 'sysname'", tableName.Replace("[", "").Replace("]", "")),
                                                        db); //--and C.name = '{1}'
            if (dt.Rows.Count > 0)
            {
                string cols = "";
                foreach (DataRow dr in dt.Rows)
                {
                    cols = dr["Column"].ToString();
                    bool sel = columnSelected(cols, idTab, tableName);
                    l.Add(new optionsSelect() { name = cols, selected = "" + sel.ToString().ToLower() + "" });
                }
            }
            return Json(l);
        }
        public bool columnSelected(string column, int tabid, string tablename = "")
        {
            var repo = db.BaseQueryGraphs.Find(tabid);
            bool ex = false;
            if (repo != null)
            {
                if (!string.IsNullOrEmpty(repo.query))
                {
                    string table = getTablenames(repo.query);
                    if (table == tablename)
                    {
                        ex = repo.query.Contains(column);
                    }
                }
            }
            return ex;
        }
        public class optionsSelect
        {
            public string name { get; set; }
            public string selected { get; set; }
        }
        public string getTablenames(string QueryString)
        {
            string connString = ConfigurationManager.ConnectionStrings["Context"].ConnectionString;
            SqlConnection con = new SqlConnection(connString);
            con.Open();
            string table = "";
            DataTable dt = con.GetSchema("Tables");

            foreach (DataRow dr in dt.Rows)
            {
                if (QueryString.ToLower().Contains("[" + dr[2].ToString().ToLower() + "]"))
                //if (QueryString.ToLower().IndexOf(dr[2].ToString().ToLower())>-1)
                {
                    table = dr[2].ToString();
                    break;
                }
                //else if (QueryString.ToLower().Contains(dr[2].ToString().ToLower()))
                //{
                //    table = dr[2].ToString();
                //    break;
                //}
            }
            con.Close();

            return table;
        }
        public string createView(string query, string name)
        {
            string view = "";
            string namepattern = "VWPREVIEW_DynamicView_" + name.Replace(" ", "_");
            view = string.Format("CREATE VIEW [{0}] as {1}", namepattern, query);

            DataTable exis = Helper.getData(string.Format("select count(*) as cant from INFORMATION_SCHEMA.VIEWS where " +
                                            "table_name = '{0}'", namepattern), db);

            if (exis.Rows.Count > 0)
            {
                if (exis.Rows[0]["cant"].ToString() == "0")
                {
                    try
                    {
                        Helper.executeNonQUery(view, db);
                    }
                    catch (Exception)
                    {
                        //namepattern = query;
                    }
                }
                else
                {

                    view = string.Format("ALTER VIEW [{0}] as {1}", namepattern, query);
                    try
                    {
                        Helper.executeNonQUery(view, db);
                    }
                    catch (Exception)
                    {
                        //namepattern = query;
                    }

                }
            }

            return namepattern;
        }
        public JsonResult previewData(FormCollection form, string query, string namequery)
        {
            //var re = db.Reports.Find(queryid);
            JsonCsTableHelper table = new JsonCsTableHelper(db, form, Request.QueryString);
            JsonCsTable json = new JsonCsTable();

            if (!string.IsNullOrEmpty(query))
            {
                table.TableName = "[Report]";
                table.VarName = "csReport";
                table.ReferenceText = "Report";
                table.Links.Add(new CsTableLink() { ButtonType = CsTableLinkType.button, Href = URLHelper.getActionUrl("Report", "Form"), Title = "Edit", Css = "btn btn-warning btn-circle refreshTable", Icon = BaseUIIconText.fa.fa_fa_pencil });

                string searchCondition = "";
                string primaryKey = "";
                string tablename = "";
                string tablenameOriginal = "";
                string where = "";
                string consulta = "";
                string consultaOriginal = "";
                string consultaColumns = "";
                DataTable data = new DataTable();

                string[] filterAllowed = { };

                consulta = createView(query, namequery);
                consultaOriginal = "SELECT * FROM " + consulta;
                tablename = consulta;
                tablenameOriginal = tablename;
                DataTable cols = Helper.getData("SELECT * FROM " + consulta, db);

                foreach (DataColumn item in cols.Columns)
                {
                    primaryKey = item.ColumnName;
                    break;
                }

                foreach (DataColumn item in cols.Columns)
                {
                    searchCondition += " " + item.ColumnName + " like '%{0}%' OR";
                    consultaColumns += "[" + item.ColumnName + "],";
                }
                consultaColumns += (consultaColumns != "") ? "*" : "";
                consultaColumns = consultaColumns.Replace(",*", "");

                int current = 0;
                if (form["offset"] != null) current = int.Parse(form["offset"]);
                int limit = 0;
                if (form["limit"] != null) limit = int.Parse(form["limit"]);
                string order = form["order"];
                string search = form["cstable_search"];

                string conditions = "", orders = "";
                if (!string.IsNullOrEmpty(search) && !string.IsNullOrEmpty(searchCondition))
                    conditions = string.Format("Where " + searchCondition, search);
                if (!string.IsNullOrEmpty(order))
                    orders = string.Format("{0}", order);
                else
                    orders = string.Format("{0} ASC", primaryKey);


                DataTable columns = Helper.getData(string.Format("select * from VWBaseNiiAll where tableName='{0}' and type!='sysname' order by column_id", tablename.Replace("[", "").Replace("]", "")), db);
                foreach (DataRow item in columns.Rows)
                {
                    Admin.Controllers.BaseDeveloperController.dbProperties properties = new
                        Admin.Controllers.BaseDeveloperController.dbProperties(item);

                    if (form.AllKeys.Contains(properties.Name) || form.AllKeys.Contains("filter_" + properties.Name))
                    {
                        string finalKey = "";
                        if (!string.IsNullOrEmpty(form[properties.Name]))
                        {
                            finalKey = properties.Name;
                        }
                        else if (!string.IsNullOrEmpty(form["filter_" + properties.Name]))
                        {
                            finalKey = "filter_" + properties.Name;
                        }

                        if (!string.IsNullOrEmpty(finalKey))
                        {
                            if (filterAllowed.Contains(finalKey.Replace("filter_", "")) || filterAllowed.Length == 0)
                            {
                                if (!string.IsNullOrEmpty(form[finalKey].ToString()))
                                {
                                    string finalValue = form[finalKey];
                                    if (properties.Type == "bit")
                                    {
                                        finalValue = finalValue.Replace("true", "1").Replace("false", "0");
                                    }

                                    conditions += !conditions.Contains("Where") ? "Where " : "";
                                    conditions += string.Format(" {0} like '%{1}%' OR", properties.Name, finalValue);
                                }
                            }
                        }
                    }
                }

                conditions += "*$*";
                conditions = conditions.Replace("OR*$*", "");
                conditions = conditions.Replace("*$*", "");

                if (string.IsNullOrEmpty(where))
                {
                    string c = "";
                    if (!string.IsNullOrEmpty(consultaColumns))
                    {
                        c = string.Format(" SELECT {0} FROM ", consultaColumns);
                    }
                    else
                    {
                        tablename = "SELECT * FROM " + consulta;
                    }
                    //consulta = string.Format("SELECT * FROM ({3} {4}) AS DATA ORDER BY {0} OFFSET {1} ROWS FETCH NEXT {2} ROWS ONLY", primaryKey, current, limit, consulta, conditions);
                    consulta = string.Format("SELECT * FROM ( {5} {3} {4}) AS DATA ORDER BY {0} OFFSET {1} ROWS FETCH NEXT {2} ROWS ONLY", (primaryKey == "") ? "1" : primaryKey, current, limit, tablename, conditions, c);
                    data = Helper.getData(consulta, db);
                }
                else
                {
                    string c = "";
                    if (!string.IsNullOrEmpty(consultaColumns))
                    {
                        c = string.Format(" SELECT {0} FROM ", consultaColumns);
                    }
                    else
                    {
                        tablename = "SELECT * FROM " + consulta;
                    }
                    //consulta = string.Format("SELECT * FROM ({3} {4}) AS DATA WHERE {5} ORDER BY {0} OFFSET {1} ROWS FETCH NEXT {2} ROWS ONLY", primaryKey, current, limit, consulta, conditions, where);
                    consulta = string.Format("SELECT * FROM ({6} {3} {4}) AS DATA WHERE {5} ORDER BY {0} OFFSET {1} ROWS FETCH NEXT {2} ROWS ONLY", (primaryKey == "") ? "1" : primaryKey, current, limit, tablename, conditions, where, c);
                    data = Helper.getData(consulta, db);
                }
                DataTable counts = Helper.getData(string.Format("SELECT count(*) FROM {1} {0}", conditions, tablenameOriginal), db);
                json.count = counts.Rows[0][0].ToString();

                DataTable columns2 = Helper.getData(string.Format("select * from VWBaseNiiAll where tableName='{0}' and type!='sysname' order by column_id", tablenameOriginal.Replace("[", "").Replace("]", "")), db);
                foreach (DataRow item in columns2.Rows)
                {
                    Admin.Controllers.BaseDeveloperController.dbProperties properties = new
                        Admin.Controllers.BaseDeveloperController.dbProperties(item);

                    if (consultaOriginal.ToLower().Contains(properties.Name.ToLower()) || consultaOriginal.Contains("*"))
                    {
                        json.columns.Add(new Column() { t = Helper.firstUpperCaseAndTs(properties.Name), p = "", n = properties.Name });
                    }
                }
                foreach (DataRow row in data.Rows)
                {
                    Row newRow = new Row();
                    string key = "";
                    for (int i = 0; i < row.ItemArray.Length; i++)
                    {
                        object item = row.ItemArray[i];

                        Admin.Controllers.BaseDeveloperController.dbProperties properties = new
                        Admin.Controllers.BaseDeveloperController.dbProperties(columns2.Rows[i]);

                        if (properties.Name == primaryKey)
                        {
                            key = item.ToString();
                        }
                        newRow.d.Add(new D() { d = (item == null ? "" : item.ToString()), p = "" });
                    }
                    newRow.p = "";
                    json.rows.Add(newRow);
                }
            }
            table.Json = json;
            return Json(table.Json, JsonRequestBehavior.AllowGet);
        }
    }
}
