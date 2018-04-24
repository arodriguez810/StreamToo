using Admin.BaseModels.ViewModels;
using Admin.Models;
using Admin.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Admin.CustomCode
{
    public class JsonCsTableHelper
    {
        string tableName,
            searchCondition,
            primaryKey,
            varName = "csTable",
            referenceText = "registro",
            additionalWhere,
            finalQuery,
            countQuery,
            exportQuery;

        string[] filterAllowed;

        public string[] FilterAllowed
        {
            get { return filterAllowed; }
            set { filterAllowed = value; }
        }

        bool replaceUnderScore = false;

        public bool ReplaceUnderScore
        {
            get { return replaceUnderScore; }
            set { replaceUnderScore = value; }
        }

        Dictionary<string, string> replacedText = new Dictionary<string, string>();

        public Dictionary<string, string> ReplacedText
        {
            get { return replacedText; }
            set { replacedText = value; }
        }

        public string CountQuery
        {
            get { return countQuery; }
            set { countQuery = value; }
        }

        public JsonCsTableHelper(Context dbp, FormCollection pform, NameValueCollection qform)
        {
            form = pform;
            form.Add(qform);
            links = new List<CsTableLink>();
            json = new JsonCsTable();
            db = dbp;
        }

        public string FinalQuery
        {
            get { return finalQuery; }
            set { finalQuery = value; }
        }

        public string ExportQuery
        {
            get { return exportQuery; }
            set { exportQuery = value; }
        }

        public string AdditionalWhere
        {
            get { return additionalWhere; }
            set { additionalWhere = value; }
        }
        public string[] hideColumns = new string[] { "creationDate", "createdBy", "creadoFecha", "creadoUsuario", "modificadoFecha", "modificadoUsuario", "canceladoFecha", "canceladoUsuario", "deleted" };
        private string[] noFormatColumn = new string[] { };
        public string ReferenceText
        {
            get { return referenceText; }
            set { referenceText = value; }
        }

        public string VarName
        {
            get { return varName; }
            set { varName = value; }
        }

        private bool isButton;
        public bool IsButton
        {
            get { return isButton; }
            set { isButton = value; }
        }
        public string PrimaryKey
        {
            get { return primaryKey; }
            set { primaryKey = value; }
        }
        public string SearchCondition
        {
            get { return searchCondition; }
            set { searchCondition = value; }
        }
        public string TableName
        {
            get { return tableName; }
            set { tableName = value; }
        }

        string tableQuery;
        public string TableQuery
        {
            get { return tableQuery; }
            set { tableQuery = value; }
        }
        List<CsTableLink> links;
        public List<CsTableLink> Links
        {
            get { return links; }
            set { links = value; }
        }
        DataTable data;
        public DataTable Data
        {
            get { return data; }
            set { data = value; }
        }
        Context db;
        FormCollection form;
        public FormCollection Form
        {
            get { return form; }
            set { form = value; }
        }
        int count;
        public int Count
        {
            get { return count; }
            set { count = value; }
        }
        JsonCsTable json;

        public JsonCsTable Json
        {
            get { return json; }
            set { json = value; }
        }

        public string[] NoFormatColumn
        {
            get
            {
                return noFormatColumn;
            }

            set
            {
                noFormatColumn = value;
            }
        }

        public void buildDataTable(string[] hideColumns = null, string additionalFilters = "", params string[] filterAllowed)
        {
            if (hideColumns != null)
            {
                this.hideColumns = hideColumns;
            }
            int current = 0;
            if (form["offset"] != null) current = int.Parse(form["offset"]);
            int limit = 0;
            if (form["limit"] != null) limit = int.Parse(form["limit"]);
            string order = form["orderName"];
            string orderSort = form["orderSort"];
            string search = form["cstable_search"];
            string conditions = "", orders = "";
            if (!string.IsNullOrEmpty(search))
                conditions = string.Format("Where " + searchCondition, search);
            if (!string.IsNullOrEmpty(order))
                orders = string.Format("[{0}] {1}", order, orderSort);
            else
                orders = string.Format("[{0}] DESC", primaryKey);
            conditions = conditions + additionalFilters;
            if (string.IsNullOrEmpty(this.additionalWhere))
            {
                string tableOrQuery = (!string.IsNullOrEmpty(tableQuery) ? tableQuery : "SELECT * FROM " + tableName);
                exportQuery = string.Format("{0} {1} ORDER BY {2}", tableOrQuery, conditions, orders);
                finalQuery = string.Format("{4} {1} ORDER BY {2} OFFSET {3} ROWS FETCH NEXT {0} ROWS ONLY", limit, conditions, orders, current, tableOrQuery);
                if (!string.IsNullOrEmpty(tableQuery))
                    countQuery = string.Format("SELECT count(*) as pp FROM ({4} {1}) as Data", limit, conditions, orders, current, tableQuery);
                else
                    countQuery = string.Format("SELECT count(*) as pp FROM {4} {1}", limit, conditions, orders, current, tableName);
            }
            else
            {
                string tableOrQuery = (!string.IsNullOrEmpty(tableQuery) ? tableQuery : "SELECT * FROM " + tableName);
                exportQuery = string.Format("SELECT * FROM ({0} {1}) AS DATA WHERE {2} ORDER BY {3}", tableOrQuery, conditions, additionalWhere, orders);
                finalQuery = string.Format("SELECT * FROM ({4} {1}) AS DATA WHERE {5} ORDER BY {2} OFFSET {3} ROWS FETCH NEXT {0} ROWS ONLY", limit, conditions, orders, current, tableOrQuery, additionalWhere);
                countQuery = string.Format("SELECT count(*) as pp FROM ({4} {1}) AS DATA WHERE {5}", limit, conditions, orders, current, tableOrQuery, additionalWhere);
            }
            json.query = finalQuery;
            data = Helper.getData(finalQuery, db);
            DataTable counts = Helper.getData(countQuery, db);
            if (counts.Rows.Count > 0)
            {
                if (!string.IsNullOrEmpty(counts.Rows[0]["pp"].ToString()))
                {
                    json.count = counts.Rows[0][0].ToString();
                }
            }
        }

        public void buildDataTable(string filters = "", string firstOrder = "DESC")
        {
            if (filters == "where" || filters == "where ")
                filters = "";
            string current = " OFFSET 0 ROWS ";
            if (form["offset"] != null) current = " OFFSET " + form["offset"] + " ROWS ";
            string limit = "";
            if (form["limit"] == "0") form["limit"] = "1";
            if (form["limit"] != null) limit = " FETCH NEXT " + form["limit"] + " ROWS ONLY";
            string order = form["orderName"];
            string orderSort = form["orderSort"];
            string search = form["cstable_search"];
            if (search == ",") search = "";
            string conditions = "", orders = "";
            if (!string.IsNullOrEmpty(search))
                conditions = string.Format("Where " + searchCondition, search);
            if (!string.IsNullOrEmpty(order))
                orders = string.Format("ORDER BY [{0}] {1}", order, orderSort);
            else
                orders = string.Format("ORDER BY [{0}] {1}", primaryKey, firstOrder);
            if (string.IsNullOrEmpty(this.additionalWhere))
            {
                string tableOrQuery = (!string.IsNullOrEmpty(tableQuery) ? tableQuery : "SELECT * FROM " + tableName);
                exportQuery = string.Format("{0} {1} {2}", tableOrQuery, conditions, orders);
                finalQuery = string.Format("{4} {1} {2} {3} {0}", limit, conditions, orders, current, tableOrQuery);
                if (!string.IsNullOrEmpty(tableQuery))
                    countQuery = string.Format("SELECT count(*) as pp FROM ({4} {1}) as Data", limit, conditions, orders, current, tableQuery);
                else
                    countQuery = string.Format("SELECT count(*) as pp FROM {4} {1}", limit, conditions, orders, current, tableName);
            }
            else
            {
                string tableOrQuery = (!string.IsNullOrEmpty(tableQuery) ? tableQuery : "SELECT * FROM " + tableName);
                exportQuery = string.Format("SELECT  * FROM ({0} {1}) AS DATA WHERE {2} {3}", tableOrQuery, conditions, additionalWhere, orders);
                finalQuery = string.Format("SELECT * FROM ({4} {1}) AS DATA WHERE {5} {2} {3} {0}", limit, conditions, orders, current, tableOrQuery, additionalWhere);
                countQuery = string.Format("SELECT count(*) as pp FROM ({4} {1}) AS DATA WHERE {5}", limit, conditions, orders, current, tableOrQuery, additionalWhere);
            }
            json.query = finalQuery;
            DataTable counts = new DataTable();
            if (!string.IsNullOrEmpty(filters))
            {
                data = Helper.getData($"select * from ({finalQuery.Split(new string[] { "ORDER BY" }, StringSplitOptions.RemoveEmptyEntries)[0]}) as ALLDATA {filters} {orders} {current} {limit}", db);
                counts = Helper.getData($"select count(*) as pp  from ({finalQuery.Split(new string[] { "ORDER BY" }, StringSplitOptions.RemoveEmptyEntries)[0]}) as ALLDATA {filters}", db);
            }
            else
            {
                data = Helper.getData(finalQuery, db);
                counts = Helper.getData(countQuery, db);
            }
            if (counts.Rows.Count > 0)
                if (!string.IsNullOrEmpty(counts.Rows[0]["pp"].ToString()))
                {
                    json.count = counts.Rows[0][0].ToString();
                }
        }

        public void buildDataTableCustomSchema(string filters = "", string firstOrder = "DESC", string schema = "dbo")
        {
            if (string.IsNullOrEmpty(schema) || string.IsNullOrWhiteSpace(schema))
            {
                schema = "dbo";
            }

            if (filters == "where" || filters == "where ")
                filters = "";
            string current = " OFFSET 0 ROWS ";
            if (form["offset"] != null) current = " OFFSET " + form["offset"] + " ROWS ";
            string limit = "";
            if (form["limit"] == "0") form["limit"] = "1";
            if (form["limit"] != null) limit = " FETCH NEXT " + form["limit"] + " ROWS ONLY";
            string order = form["orderName"];
            string orderSort = form["orderSort"];
            string search = form["cstable_search"];
            if (search == ",") search = "";
            string conditions = "", orders = "";
            if (!string.IsNullOrEmpty(search))
                conditions = string.Format("Where " + searchCondition, search);
            if (!string.IsNullOrEmpty(order))
                orders = string.Format("ORDER BY [{0}] {1}", order, orderSort);
            else
                orders = string.Format("ORDER BY [{0}] {1}", primaryKey, firstOrder);
            if (string.IsNullOrEmpty(this.additionalWhere))
            {
                string tableOrQuery = (!string.IsNullOrEmpty(tableQuery) ? tableQuery : $"SELECT * FROM {schema}.{tableName}");
                exportQuery = string.Format("{0} {1} {2}", tableOrQuery, conditions, orders);
                finalQuery = string.Format("{4} {1} {2} {3} {0}", limit, conditions, orders, current, tableOrQuery);
                if (!string.IsNullOrEmpty(tableQuery))
                    countQuery = string.Format("SELECT count(*) as pp FROM ({4} {1}) as Data", limit, conditions, orders, current, tableQuery);
                else
                    countQuery = string.Format("SELECT count(*) as pp FROM {4}.{5} {1}", limit, conditions, orders, current, schema, tableName);
            }
            else
            {
                string tableOrQuery = (!string.IsNullOrEmpty(tableQuery) ? tableQuery : $"SELECT * FROM {schema}.{tableName}");
                exportQuery = string.Format("SELECT  * FROM ({0} {1}) AS DATA WHERE {2} {3}", tableOrQuery, conditions, additionalWhere, orders);
                finalQuery = string.Format("SELECT * FROM ({4} {1}) AS DATA WHERE {5} {2} {3} {0}", limit, conditions, orders, current, tableOrQuery, additionalWhere);
                countQuery = string.Format("SELECT count(*) as pp FROM ({4} {1}) AS DATA WHERE {5}", limit, conditions, orders, current, tableOrQuery, additionalWhere);
            }
            json.query = finalQuery;
            DataTable counts = new DataTable();
            if (!string.IsNullOrEmpty(filters))
            {
                data = Helper.getData($"select * from ({finalQuery.Split(new string[] { "ORDER BY" }, StringSplitOptions.RemoveEmptyEntries)[0]}) as ALLDATA {filters} {orders} {current} {limit}", db);
                counts = Helper.getData($"select count(*) as pp  from ({finalQuery.Split(new string[] { "ORDER BY" }, StringSplitOptions.RemoveEmptyEntries)[0]}) as ALLDATA {filters}", db);
            }
            else
            {
                data = Helper.getData(finalQuery, db);
                counts = Helper.getData(countQuery, db);
            }
            if (counts.Rows.Count > 0)
            {
                if (!string.IsNullOrEmpty(counts.Rows[0]["pp"].ToString()))
                {
                    json.count = counts.Rows[0][0].ToString();
                }
            }
        }

        public void buildDataTableNoOrder(string orders, string[] hideColumns = null, string additionalFilters = "", params string[] filterAllowed)
        {
            if (hideColumns != null)
            {
                this.hideColumns = hideColumns;
            }
            int current = 0;
            if (form["offset"] != null) current = int.Parse(form["offset"]);
            int limit = 0;
            if (form["limit"] != null) limit = int.Parse(form["limit"]);
            //string order = form["orderName"];
            //string orderSort = form["orderSort"];
            string search = form["cstable_search"];

            string conditions = "";
            if (!string.IsNullOrEmpty(search))
                conditions = string.Format("Where " + searchCondition, search);
            //if (!string.IsNullOrEmpty(order))
            //    orders = string.Format("[{0}] {1}", order, orderSort);
            //else
            //    orders = string.Format("[{0}] DESC", primaryKey);

            //DataTable forField = Helper.getData(string.Format("SELECT top 1 * FROM {0}", tableName), db);
            DataTable columns = Helper.getData(string.Format("select * from VWBaseNiiAll where tableName='{0}' and type!='sysname' order by column_id", tableName.Replace("[", "").Replace("]", "")), db);
            foreach (DataRow item in columns.Rows)
            {
                Controllers.BaseDeveloperController.dbProperties properties = new
                    Controllers.BaseDeveloperController.dbProperties(item);
                if (properties.Name.ToLower().EndsWith("nd"))
                {
                    string[] hides = this.hideColumns;
                    List<string> tohide = hides.ToList();
                    tohide.Add(properties.Name);
                    this.hideColumns = tohide.ToArray();
                }
                string originalName = properties.Name;
                if (this.replaceUnderScore)
                {
                    properties.Name = properties.Name.Replace(" ", "_");
                }

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
                                conditions += string.Format(" [{0}] like '%{1}%' OR", originalName, finalValue);
                            }
                        }
                    }
                }
            }

            conditions += "*$*";
            conditions = conditions.Replace("OR*$*", "");
            conditions = conditions.Replace("*$*", "");
            conditions = conditions + additionalFilters;

            if (string.IsNullOrEmpty(this.additionalWhere))
            {
                data = Helper.getData(string.Format("SELECT * FROM {4} {1} ORDER BY {2} OFFSET {3} ROWS FETCH NEXT {0} ROWS ONLY", limit, conditions, orders, current, tableName), db);
                exportQuery = string.Format("SELECT * FROM {0} {1} ORDER BY {2}", tableName, conditions, orders);
                finalQuery = string.Format("SELECT * FROM {4} {1} ORDER BY {2} OFFSET {3} ROWS FETCH NEXT {0} ROWS ONLY", limit, conditions, orders, current, tableName);
                json.query = finalQuery;
                countQuery = string.Format("SELECT count(*) as pp FROM {4} {1}", limit, conditions, orders, current, tableName);
                DataTable counts = Helper.getData(string.Format("SELECT count(*) as pp FROM {4} {1}", limit, conditions, orders, current, tableName), db);
                //json.count = counts.Rows[0][0].ToString();
                if (counts.Rows.Count > 0)
                {
                    if (!string.IsNullOrEmpty(counts.Rows[0]["pp"].ToString()))
                    {
                        json.count = counts.Rows[0][0].ToString();
                    }
                }
            }
            else
            {
                data = Helper.getData(string.Format("SELECT * FROM (SELECT * FROM {4} {1}) AS DATA WHERE {5} ORDER BY {2} OFFSET {3} ROWS FETCH NEXT {0} ROWS ONLY", limit, conditions, orders, current, tableName, additionalWhere), db);
                exportQuery = string.Format("SELECT * FROM (SELECT * FROM {0} {1}) AS DATA WHERE {2} ORDER BY {3}", tableName, conditions, additionalWhere, orders);
                finalQuery = string.Format("SELECT * FROM (SELECT * FROM {4} {1}) AS DATA WHERE {5} ORDER BY {2} OFFSET {3} ROWS FETCH NEXT {0} ROWS ONLY", limit, conditions, orders, current, tableName, additionalWhere);
                json.query = finalQuery;
                countQuery = string.Format("SELECT count(*) as pp FROM {4} {1}", limit, conditions, orders, current, tableName);
                DataTable counts = Helper.getData(string.Format("SELECT count(*) as pp FROM (SELECT * FROM {4} {1}) AS DATA WHERE {5}", limit, conditions, orders, current, tableName, additionalWhere), db);
                if (counts.Rows.Count > 0)
                {
                    if (!string.IsNullOrEmpty(counts.Rows[0]["pp"].ToString()))
                    {
                        json.count = counts.Rows[0][0].ToString();
                    }
                }
            }

        }

        public void buildDataTableUser(string[] hideColumns = null, string additionalFilters = "", params string[] filterAllowed)
        {
            if (hideColumns != null)
            {
                this.hideColumns = hideColumns;
            }
            int current = 0;
            if (form["offset"] != null) current = int.Parse(form["offset"]);
            int limit = 0;
            if (form["limit"] != null) limit = int.Parse(form["limit"]);
            string order = form["orderName"];
            string orderSort = form["orderSort"];
            string search = form["cstable_search"];

            string conditions = "", orders = "";
            if (!string.IsNullOrEmpty(search))
                conditions = string.Format("Where " + searchCondition, search);
            if (!string.IsNullOrEmpty(order))
                orders = string.Format("[{0}] {1}", order, orderSort);
            else
                orders = string.Format("[{0}] ASC", primaryKey);

            //DataTable forField = Helper.getData(string.Format("SELECT top 1 * FROM {0}", tableName), db);
            DataTable columns = Helper.getData(string.Format("select * from VWBaseNiiAll where tableName='{0}' and type!='sysname' order by column_id", tableName.Replace("[", "").Replace("]", "")), db);
            foreach (DataRow item in columns.Rows)
            {
                Controllers.BaseDeveloperController.dbProperties properties = new
                    Controllers.BaseDeveloperController.dbProperties(item);
                string originalName = properties.Name;
                if (this.replaceUnderScore)
                {
                    properties.Name = properties.Name.Replace(" ", "_");
                }

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

                                if (originalName == "Última IP")
                                {
                                    conditions += string.Format(" [{0}] like '{1}' AND", originalName, finalValue);
                                }
                                else if (originalName == "Identificacion")
                                {
                                    conditions += string.Format(" [{0}] like '{1}' AND", originalName, finalValue.Replace("-", ""));
                                }
                                else if (originalName != "Última IP" || originalName != "Identificacion")
                                {
                                    conditions += string.Format(" [{0}] like '%{1}%' AND", originalName, finalValue);
                                }

                            }
                            else
                            {
                                conditions = conditions.Replace("AND", "");
                            }
                        }
                    }
                }
            }

            conditions += "*$*";
            conditions = conditions.Replace("AND*$*", "");
            conditions = conditions.Replace("*$*", "");
            conditions = conditions + additionalFilters;

            if (string.IsNullOrEmpty(this.additionalWhere))
            {
                data = Helper.getData(string.Format("SELECT * FROM {4} {1} ORDER BY {2} OFFSET {3} ROWS FETCH NEXT {0} ROWS ONLY", limit, conditions, orders, current, tableName), db);
                exportQuery = string.Format("SELECT * FROM {0} {1} ORDER BY {2}", tableName, conditions, orders);
                finalQuery = string.Format("SELECT * FROM {4} {1} ORDER BY {2} OFFSET {3} ROWS FETCH NEXT {0} ROWS ONLY", limit, conditions, orders, current, tableName);
                json.query = finalQuery;
                countQuery = string.Format("SELECT count(*) as pp FROM {4} {1}", limit, conditions, orders, current, tableName);
                DataTable counts = Helper.getData(string.Format("SELECT count(*) as pp FROM {4} {1}", limit, conditions, orders, current, tableName), db);
                //json.count = counts.Rows[0][0].ToString();
                if (counts.Rows.Count > 0)
                {
                    if (!string.IsNullOrEmpty(counts.Rows[0]["pp"].ToString()))
                    {
                        json.count = counts.Rows[0][0].ToString();
                    }
                }
            }
            else
            {
                data = Helper.getData(string.Format("SELECT * FROM (SELECT * FROM {4} {1}) AS DATA WHERE {5} ORDER BY {2} OFFSET {3} ROWS FETCH NEXT {0} ROWS ONLY", limit, conditions, orders, current, tableName, additionalWhere), db);
                exportQuery = string.Format("SELECT * FROM (SELECT * FROM {0} {1}) AS DATA WHERE {2} ORDER BY {3}", tableName, conditions, additionalWhere, orders);
                json.query = finalQuery;
                finalQuery = string.Format("SELECT * FROM (SELECT * FROM {4} {1}) AS DATA WHERE {5} ORDER BY {2} OFFSET {3} ROWS FETCH NEXT {0} ROWS ONLY", limit, conditions, orders, current, tableName, additionalWhere);
                countQuery = string.Format("SELECT count(*) as pp FROM {4} {1}", limit, conditions, orders, current, tableName);
                DataTable counts = Helper.getData(string.Format("SELECT count(*) as pp FROM (SELECT * FROM {4} {1}) AS DATA WHERE {5}", limit, conditions, orders, current, tableName, additionalWhere), db);
                if (counts.Rows.Count > 0)
                {
                    if (!string.IsNullOrEmpty(counts.Rows[0]["pp"].ToString()))
                    {
                        json.count = counts.Rows[0][0].ToString();
                    }
                }
            }

        }

        public void buildDefaultJson()
        {
            BaseConfiguration config = new BaseConfiguration();
            DataTable columns = Helper.getData(string.Format("select * from VWBaseNiiAll where tableName='{0}' and type!='sysname' order by column_id", tableName.Replace("[", "").Replace("]", "")), db);
            string filteredBy = "";

            int current = 0;
            if (form["offset"] != null) current = int.Parse(form["offset"]);
            int limit = 0;
            if (form["limit"] == "0") form["limit"] = "1";
            if (form["limit"] != null) limit = int.Parse(form["limit"]);
            string order = form["orderName"];
            string orderSort = form["orderSort"];
            string search = form["cstable_search"];

            string conditions = "", orders = "";
            if (!string.IsNullOrEmpty(search))
                conditions = string.Format("Where " + searchCondition, search);
            if (!string.IsNullOrEmpty(order))
                orders = string.Format("[{0}] {1}", order, orderSort);
            else
                orders = string.Format("[{0}] DESC", primaryKey);


            foreach (DataRow item in columns.Rows)
            {
                Controllers.BaseDeveloperController.dbProperties properties = new Controllers.BaseDeveloperController.dbProperties(item);
                if (!hideColumns.Contains(properties.Name))
                    if (replacedText.ContainsKey(properties.Name))
                        json.columns.Add(new Column() { t = Helper.firstUpperCaseAndTs(replacedText[properties.Name]), p = "", n = properties.Name });
                    else
                        json.columns.Add(new Column() { t = Helper.firstUpperCaseAndTs(properties.Name), p = "", n = properties.Name });
            }


            foreach (DataRow row in data.Rows)
            {
                Row newRow = new Row();
                string key = "";
                int span = 0;
                for (int i = 0; i < row.ItemArray.Length; i++)
                {
                    object item = row.ItemArray[i];
                    Controllers.BaseDeveloperController.dbProperties properties = new Controllers.BaseDeveloperController.dbProperties(columns.Rows[i]);

                    if (!hideColumns.Contains(properties.Name))
                    {
                        span++;

                        if (properties.Name == primaryKey)
                        {
                            key = item.ToString().Replace("-", "─").Replace("data─", "data-");
                        }
                        if (properties.Type == "date")
                        {

                            string finalValue = DateTime.Parse(item.ToString()).ToString(config.dateOutFormat);
                            newRow.d.Add(new D() { d = finalValue.Replace(" ", " "), p = "style=\"text-align: left;\"", column = properties.Name });
                            continue;
                        }
                        else
                            if (properties.Type == "datetime")
                        {

                            string finalValue = "Not Set";
                            if (item.ToString().Contains("@noparse@"))
                            {
                                finalValue = item.ToString().Replace("@noparse@", "");
                            }
                            else
                            {
                                try
                                {
                                    finalValue = DateTime.Parse(item.ToString()).ToString("yyyy/MM/dd HH:mm");
                                }
                                catch { }
                            }
                            newRow.d.Add(new D() { d = finalValue.Replace(" ", " "), p = "style=\"text-align: left;\"", column = properties.Name });

                            continue;
                        }
                        else
                                if (properties.Type == "money")
                        {
                            if (properties.Scale > 0)
                            {
                                newRow.d.Add(new D() { d = item.ToString(), p = "style=\"text-align: right;\"", column = properties.Name });
                                continue;
                            }
                        }
                        else
                        if (properties.Type == "numeric")
                        {
                            if (properties.Scale > 0)
                            {
                                decimal monto = decimal.Parse(item.ToString());
                                string finalValue = monto.ToString("N", System.Globalization.CultureInfo.GetCultureInfo("en-US"));
                                newRow.d.Add(new D() { d = finalValue, p = "style=\"text-align: right;\"", column = properties.Name });
                                continue;
                            }
                        }
                        newRow.d.Add(new D() { d = (item == null ? "Not Set" : item.ToString().Replace(" ", " ").Replace("-", "─").Replace("data─", "data-")), p = "", column = properties.Name });

                    }
                }
                newRow.p = "";
                json.rows.Add(newRow);

            }
            if (this.data.Rows.Count == 0)
            {
                Row newRow = new Row();
                newRow.d.Add(new D() { d = ("This list contains no items to display"), p = string.Format("colspan='{0}' class='text-center'", columns.Rows.Count - (hideColumns.Count() - 2)) });
                json.rows.Add(newRow);
            }
            else
            {
                Row rowHead = new Row() { p = "" };
                rowHead.d.Add(new D()
                {
                    p = "class='lastRow' colspan=" + json.columns.Count() + 1,
                    d =
                        string.Format("{0} <b>{4}</b> {1} <b>{5}</b> {2} <b>{6}</b> {3}, {7} {8}",
                        ("Showing"), ("to"), ("of"), ("entries"), current + 1,
                        ((current + limit) > int.Parse(json.count) ? int.Parse(json.count) : (current + limit)), json.count,
                        ("sorted by"), orders.Replace("[", "'").Replace("]", "'").Replace("ASC", ("ascendant")).Replace("DESC", ("descending"))) +
                        (filteredBy != "" ?
                        (", filtered by: ") + filteredBy : "")
                });
                json.rows.Add(rowHead);
            }
        }

        public void buildLinks()
        {
            if (data.Rows.Count > 0)
            {
                int keyIndex = 0;
                foreach (var item in json.columns)
                {
                    if (item.n.ToLower() == primaryKey.ToLower())
                    {
                        break;
                    }
                    keyIndex++;
                }
                json.columns.Add(new Column() { t = ("Options"), p = "", n = "" });
                foreach (var item in json.rows)
                {
                    if (item.d.Count > 1)
                        if (!item.d[keyIndex].p.Contains("lastRow"))
                        {
                            if (item.d[keyIndex].d != "")
                            {
                                D forLink = new D();
                                foreach (CsTableLink link in links)
                                {
                                    forLink.d += link.getHtml(item.d[keyIndex].d, referenceText, varName, item, json.columns);
                                    forLink.p = "style='display: flex'";
                                }
                                item.d.Add(forLink);
                            }
                            else
                            {
                                item.d.Add(new D() { d = "" });
                            }
                        }
                }
            }
            else
            {
                int keyIndex = 0;
                foreach (var item in json.columns)
                {
                    if (item.n.ToLower() == primaryKey.ToLower())
                    {
                        break;
                    }
                    keyIndex++;
                }
                json.columns.Add(new Column() { t = ("Options"), p = "", n = "" });
                foreach (var item in json.rows)
                {
                    if (!item.d[keyIndex].p.Contains("lastRow"))
                    {
                        if (item.d[keyIndex].d != "")
                        {
                            D forLink = new D();
                            foreach (CsTableLink link in links)
                            {
                                forLink.d += "";
                                forLink.p = "";
                            }
                            item.d.Add(forLink);
                        }
                        else
                        {
                            item.d.Add(new D() { d = "" });
                        }
                    }
                }
            }
        }
    }

    public enum CsTableLinkType
    {
        button = 0,
        link = 1,
        deleteButton = 2,
        linkExternal = 3,
        linkOnclickCallJS = 4,
        linkWithID = 5,
        functionButton = 6,
        statusButton = 7,
    }

    public class CsTableLink
    {
        string text, css, href, icon, title, fullText, onclick, id, extra, target;
        string noAction, yesAction;
        List<string> hideWhere;

        public List<string> HideWhere
        {
            get { return hideWhere; }
            set { hideWhere = value; }
        }

        public CsTableLink()
        {
            hideWhere = new List<string>();
        }

        public string YesAction
        {
            get { return yesAction; }
            set { yesAction = value; }
        }

        public string NoAction
        {
            get { return noAction; }
            set { noAction = value; }
        }

        public string Extra
        {
            get { return Extra1; }
            set { Extra1 = value; }
        }

        public string ID
        {
            get { return Id; }
            set { Id = value; }
        }

        public string Onclick
        {
            get { return Onclick1; }
            set { Onclick1 = value; }
        }

        public string FullText
        {
            get { return FullText1; }
            set { FullText1 = value; }
        }
        CsTableLinkType buttonType;

        public CsTableLinkType ButtonType
        {
            get { return buttonType; }
            set { buttonType = value; }
        }

        public string Title
        {
            get { return Title1; }
            set { Title1 = value; }
        }

        public string Icon
        {
            get { return Icon1; }
            set { Icon1 = value; }
        }

        public string Href
        {
            get { return Href1; }
            set { Href1 = value; }
        }

        public string Css
        {
            get { return Css1; }
            set { Css1 = value; }
        }

        public string Text
        {
            get { return Text1; }
            set { Text1 = value; }
        }

        public string Text1
        {
            get
            {
                return text;
            }

            set
            {
                text = value;
            }
        }

        public string Css1
        {
            get
            {
                return css;
            }

            set
            {
                css = value;
            }
        }

        public string Href1
        {
            get
            {
                return href;
            }

            set
            {
                href = value;
            }
        }

        public string Icon1
        {
            get
            {
                return icon;
            }

            set
            {
                icon = value;
            }
        }

        public string Title1
        {
            get
            {
                return title;
            }

            set
            {
                title = value;
            }
        }

        public string FullText1
        {
            get
            {
                return fullText;
            }

            set
            {
                fullText = value;
            }
        }

        public string Onclick1
        {
            get
            {
                return onclick;
            }

            set
            {
                onclick = value;
            }
        }

        public string Id
        {
            get
            {
                return id;
            }

            set
            {
                id = value;
            }
        }

        public string Extra1
        {
            get
            {
                return extra;
            }

            set
            {
                extra = value;
            }
        }

        public string Target
        {
            get
            {
                return target;
            }

            set
            {
                target = value;
            }
        }

        public string getHtml(string key, string recordText, string varName, DataRow row)
        {
            if (hideWhere.Count > 0)
            {
                foreach (var item in hideWhere)
                {
                    string[] splited = item.Split('=');
                    string columna = splited[0];
                    string value = splited[0];
                    if (row[columna].ToString() != value)
                    {
                        return "";
                    }
                }
            }

            switch (buttonType)
            {

                case CsTableLinkType.button:
                    {
                        return string.Format("<input id=\"refi\" type=\"hidden\" value=\"" + recordText + "\" />\n <button data-loading-text=\"<i class='fa fa-refresh fa-spin'></i>\" title=\"{3}\" class=\"{0} baseDynamicModal\" data-action=\"{1}\"><i class=\"{2}\"></i>{4}</button>", Css1, Href1 + "/" + key + Extra1, Icon1, (Title1), Text1);
                    }
                case CsTableLinkType.link:
                    {
                        if (Href1 != "#")
                        {
                            return string.Format("<a class='{2}' title=\"{5}\" href=\"{0}/{3}{6}\"><i class=\"{4}\"></i>{1}</a>", Href, (Text), Css, key, Icon, Title1, Extra1);
                        }
                        else
                        {
                            return string.Format("<a class='{2}' title=\"{5}\" href=\"#\"><i class=\"{4}\"></i>{1}</a>", Href, (Text), Css, key, Icon, Title1, Extra1);
                        }

                    }
                case CsTableLinkType.deleteButton:
                    {
                        StringBuilder html = new StringBuilder();
                        html.AppendFormat("<button ");
                        //html.AppendFormat("data-message=\"{1} {0}?\" ", recordText, ("Are you sure to delete this"));
                        html.AppendFormat("data-message=\"{0}\" ", string.IsNullOrEmpty(FullText1) ? ("Are you sure to delete this " + recordText + "?") : FullText1);
                        //html.AppendFormat("data-errormessage=\"{1} {0}\"", recordText, ("Could not delete this"));
                        html.AppendFormat("data-errormessage=\"{0}\"", ("Could not delete this " + recordText));
                        //html.AppendFormat("data-title=\"{1} {0}\"", recordText, ("Delete"));
                        html.AppendFormat("data-title=\"{0}\"", "" + ("Delete " + recordText));
                        html.AppendFormat("data-buttons=\"[{0}][{1}]\"", ("No"), ("Yes"));
                        html.AppendFormat("data-btnactions=\"{2}=>%{0}.data('CStable').update();{3}&{1}=>{4}\"", varName, ("No"), ("Yes"), yesAction, noAction);
                        html.AppendFormat("class=\"{1} delete\" data-action=\"{0}\" title=\"{2}\">", Href1 + "/" + key, Css1, ("Delete"));
                        html.AppendFormat("	<i class=\"fa fa-trash-o\"></i>");
                        html.AppendFormat("</button>");
                        return html.ToString();
                    }
                case CsTableLinkType.statusButton:
                    {
                        StringBuilder html = new StringBuilder();
                        html.AppendFormat("<button ");
                        //html.AppendFormat("data-message=\"{1} {0}?\" ", recordText, ("Are you sure to delete this"));
                        html.AppendFormat("data-message=\"{0}\" ", string.IsNullOrEmpty(FullText1) ? ("Are you sure to @status@ this " + recordText + "?") : FullText1);
                        //html.AppendFormat("data-errormessage=\"{1} {0}\"", recordText, ("Could not delete this"));
                        html.AppendFormat("data-errormessage=\"{0}\"", ("Could not @status@ this " + recordText));
                        //html.AppendFormat("data-title=\"{1} {0}\"", recordText, ("Delete"));
                        html.AppendFormat("data-title=\"{0}\"", "" + ("@Status@ " + recordText));
                        html.AppendFormat("data-buttons=\"[{0}][{1}]\"", ("No"), ("Yes"));
                        html.AppendFormat("data-btnactions=\"{2}=>%{0}.data('CStable').update();{3}&{1}=>{4}\"", varName, ("No"), ("Yes"), yesAction, noAction);
                        html.AppendFormat("class=\"{1} delete\" data-action=\"{0}\" title=\"{2}\">", Href1 + "/" + key, Css1, ("Delete"));
                        html.AppendFormat("	<i class=\"{0}\"></i>", Icon);
                        html.AppendFormat("</button>");
                        return html.ToString();
                    }

                case CsTableLinkType.functionButton:
                    {
                        StringBuilder html = new StringBuilder();
                        html.AppendFormat("<button ");
                        //html.AppendFormat("data-message=\"{1} {0}?\" ", recordText, ("Are you sure to delete this"));
                        html.AppendFormat("data-message=\"{0}\" ", FullText1);
                        //html.AppendFormat("data-errormessage=\"{1} {0}\"", recordText, ("Could not delete this"));
                        html.AppendFormat("data-errormessage=\"{0}\"", recordText);
                        //html.AppendFormat("data-title=\"{1} {0}\"", recordText, ("Delete"));
                        html.AppendFormat("data-title=\"{0}\"", "" + Title1);
                        html.AppendFormat("data-buttons=\"[{0}][{1}]\"", ("No"), ("Yes"));
                        html.AppendFormat("data-btnactions=\"{2}=>%{0}.data('CStable').update();&{1}=>\"", varName, ("No"), ("Yes"));
                        html.AppendFormat("class=\"{1} delete\" data-action=\"{0}\" title=\"{2}\">", Href1 + "/" + key, Css1, Title1);
                        html.AppendFormat("	<i class=\"{0}\"></i>", Icon);
                        html.AppendFormat("</button>");
                        return html.ToString();
                    }
                case CsTableLinkType.linkExternal:
                    {
                        return string.Format("<a class='{2}' title=\"{5}\" target=\"_blank\" href=\"{0}/{3}\"><i class=\"{4}\"></i>{1}</a>", Href, (Text), Css, key, Icon, Title1);
                    }
                case CsTableLinkType.linkOnclickCallJS:
                    {
                        return string.Format("<a class='{2}' title=\"{5}\" onclick=\"{6}\" \"><i class=\"{4}\"></i>{1}</a>", Href, (Text), Css, key, Icon, Title1, Onclick1);
                    }
                case CsTableLinkType.linkWithID:
                    {
                        return string.Format("<button class='{2}' title=\"{5}\" onclick=\"{6}\" id=\"{7}\" \"><i class=\"{4}\"></i>{1}</button>", Href, (Text), Css, key, Icon, Title1, Onclick1, Id);
                    }
                default:
                    {
                        return string.Format("<a class='{2}' title=\"{5}\" href=\"{0}/{3}\"><i class=\"{4}\"></i>{1}</a>", Href, (Text), Css, key, Icon, Title1);
                    }
            }
        }

        public string getHtml(string key, string recordText, string varName, Row row, List<Column> columns)
        {
            if (hideWhere.Count > 0)
            {
                foreach (var item in hideWhere)
                {
                    string[] splited = item.Split(new string[] { "=", ">", "<", "<=", ">=", "!=", "(Contains)", "(NoContains)" }, StringSplitOptions.RemoveEmptyEntries);
                    string columna = splited[0];
                    int? index = null;
                    int count = 0;
                    foreach (Column column in columns)
                    {
                        if (column.n.ToLower() == columna.ToLower())
                        {
                            index = count;
                            break;
                        }
                        count++;
                    }
                    string value = splited[1].Replace(" ", " ");
                    if (index.HasValue)
                    {

                        if (item.Contains("!="))
                        {
                            if (row.d[index.Value].d == value)
                            {
                                return "";
                            }
                        }
                        else
                            if (item.Contains("(Contains)"))
                        {
                            if (!row.d[index.Value].d.Contains(value))
                            {
                                return "";
                            }
                        }
                        else
                                if (item.Contains("(NoContains)"))
                        {
                            if (row.d[index.Value].d.Contains(value))
                            {
                                return "";
                            }
                        }
                        else
                                    if (item.Contains("="))
                        {
                            if (row.d[index.Value].d != value)
                            {
                                return "";
                            }
                        }


                    }
                }
            }

            switch (buttonType)
            {

                case CsTableLinkType.button:
                    {
                        return string.Format("<input id=\"refi\" type=\"hidden\" value=\"" + recordText + "\" />\n <button data-loading-text=\"<i class='fa fa-refresh fa-spin'></i>\" title=\"{3}\" class=\"{0} baseDynamicModal\" data-action=\"{1}\"><i class=\"{2}\"></i>{4}</button>", Css1, Href1 + "/" + key + Extra1, Icon1, (Title1), Text1);
                    }
                case CsTableLinkType.link:
                    {
                        Css1 = Css1.Replace("btn-warning", "btn-info");
                        if (Href1 != "#")
                        {
                            return string.Format("<a " + target + " class='{2}' title=\"{5}\" href=\"{0}/{3}{6}\"><i class=\"{4}\"></i>{1}</a>", Href, (Text), Css, key, Icon, Title1, Extra1);
                        }
                        else
                        {
                            return string.Format("<a " + target + " class='{2}' title=\"{5}\" href=\"javascript:void(0)\"><i class=\"{4}\"></i>{1}</a>", Href, (Text), Css, key, Icon, Title1, Extra1);
                        }
                    }
                case CsTableLinkType.deleteButton:
                    {
                        StringBuilder html = new StringBuilder();
                        html.AppendFormat("<button ");
                        //html.AppendFormat("data-message=\"{1} {0}?\" ", recordText, ("Are you sure to delete this"));
                        html.AppendFormat("data-message=\"{0}\" ", string.IsNullOrEmpty(FullText1) ? ("Are you sure to delete this " + recordText + "?") : FullText1);
                        //html.AppendFormat("data-errormessage=\"{1} {0}\"", recordText, ("Could not delete this"));
                        html.AppendFormat("data-errormessage=\"{0}\"", ("Could not delete this " + recordText));
                        //html.AppendFormat("data-title=\"{1} {0}\"", recordText, ("Delete"));
                        html.AppendFormat("data-title=\"{0}\"", string.IsNullOrEmpty(title) ? ("" + ("Delete " + recordText)) : title + " " + recordText);
                        html.AppendFormat("data-buttons=\"[{0}][{1}]\"", ("No"), ("Yes"));
                        html.AppendFormat("data-btnactions=\"{2}=>%{0}.data('CStable').update();{3}&{1}=>{4}\"", varName, ("No"), ("Yes"), yesAction, noAction);
                        html.AppendFormat("class=\"{1} delete\" data-action=\"{0}\" title=\"{2}\">", Href1 + "/" + key, Css1, ("Delete"));
                        html.AppendFormat("	<i class=\"{0}\"></i>", Icon);
                        html.AppendFormat("</button>");
                        return html.ToString();
                    }

                case CsTableLinkType.functionButton:
                    {
                        bool isActive = false;
                        bool activeString = true;
                        if (row.d.Count(d => d.column == "active") > 0)
                        {
                            if (row.d.Count(d => d.d == "No" && d.column == "active") > 0)
                                activeString = false;
                            isActive = true;
                        }
                        StringBuilder html = new StringBuilder();
                        html.AppendFormat("<button ");
                        //html.AppendFormat("data-message=\"{1} {0}?\" ", recordText, ("Are you sure to delete this"));
                        if (!isActive)
                        {
                            html.AppendFormat("data-message=\"{0}\" ", FullText1);
                            html.AppendFormat("data-errormessage=\"{0}\"", recordText);
                            html.AppendFormat("data-title=\"{0}\"", "" + Title1);
                            html.AppendFormat("data-buttons=\"[{0}][{1}]\"", ("No"), ("Yes"));
                            html.AppendFormat("data-btnactions=\"{2}=>%{0}.data('CStable').update();&{1}=>\"", varName, ("No"), ("Yes"));
                            html.AppendFormat("class=\"{1} delete\" data-action=\"{0}\" title=\"{2}\">", Href1 + "/" + key, Css1, Title1);
                            html.AppendFormat("	<i class=\"{0}\"></i>", Icon);
                        }
                        else
                        {
                            html.AppendFormat("data-message=\"{0}\" ", ($"Are you sure to {(activeString ? "Inactive" : "Active")} this " + recordText + "?"));
                            html.AppendFormat("data-errormessage=\"{0}\"", recordText);
                            html.AppendFormat("data-title=\"{0}\"", "" + Title1);
                            html.AppendFormat("data-buttons=\"[{0}][{1}]\"", ("No"), ("Yes"));
                            html.AppendFormat("data-btnactions=\"{2}=>%{0}.data('CStable').update();&{1}=>\"", varName, ("No"), ("Yes"));
                            html.AppendFormat("class=\"{1} delete\" data-action=\"{0}\" title=\"{2}\">", Href1 + "/" + key, Css1, activeString ? "Turn Inactive" : "Turn Active");
                            html.AppendFormat("	<i class=\"{0}\"></i>", activeString ? "glyphicon glyphicon-check" : "glyphicon glyphicon-unchecked");
                        }

                        html.AppendFormat("</button>");
                        return html.ToString();
                    }
                case CsTableLinkType.linkExternal:
                    {
                        return string.Format("<a class='{2}' title=\"{5}\" target=\"_blank\" href=\"{0}/{3}\"><i class=\"{4}\"></i>{1}</a>", Href, (Text), Css, key, Icon, Title1);
                    }
                case CsTableLinkType.linkOnclickCallJS:
                    {
                        return string.Format("<a class='{2}' title=\"{5}\" onclick=\"{6}\" \"><i class=\"{4}\"></i>{1}</a>", Href, (Text), Css, key, Icon, Title1, Onclick1);
                    }
                case CsTableLinkType.linkWithID:
                    {
                        return string.Format("<button class='{2}' title=\"{5}\" onclick=\"{6}\" id=\"{7}\" \"><i class=\"{4}\"></i>{1}</button>", Href, (Text), Css, key, Icon, Title1, Onclick1, Id);
                    }
                default:
                    {
                        return string.Format("<a class='{2}' title=\"{5}\" href=\"{0}/{3}\"><i class=\"{4}\"></i>{1}</a>", Href, (Text), Css, key, Icon, Title1);
                    }
            }
        }
    }
}