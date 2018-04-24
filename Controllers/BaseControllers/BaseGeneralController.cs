using Admin.CustomCode;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Admin.Controllers
{

    public class BaseGeneralController : BaseController
    {
        public class UpdateDropDownModel
        {
            string where, table, text, value, optionLabel, toggleOff, toggleON, @checked, name, type, groupby;
            bool toggle;

            public string Groupby
            {
                get { return groupby; }
                set { groupby = value; }
            }

            public bool Toggle
            {
                get { return toggle; }
                set { toggle = value; }
            }

            public string Type
            {
                get { return type; }
                set { type = value; }
            }

            public string Name
            {
                get { return name; }
                set { name = value; }
            }

            public string Checked
            {
                get { return @checked; }
                set { @checked = value; }
            }

            public string ToggleON
            {
                get { return toggleON; }
                set { toggleON = value; }
            }

            public string ToggleOff
            {
                get { return toggleOff; }
                set { toggleOff = value; }
            }
            int col, row;

            public int Row
            {
                get { return row; }
                set { row = value; }
            }

            public int Col
            {
                get { return col; }
                set { col = value; }
            }

            public string OptionLabel
            {
                get { return optionLabel; }
                set { optionLabel = value; }
            }
            string[] selected;

            public string[] Selected
            {
                get { return selected; }
                set { selected = value; }
            }

            public string Value
            {
                get { return this.value; }
                set { this.value = value; }
            }

            public string Text
            {
                get { return text; }
                set { text = value; }
            }

            public string Table
            {
                get { return table; }
                set { table = value; }
            }

            public string Where
            {
                get { return where; }
                set { where = value; }
            }
        }

        public class DropDownListJson
        {
            string value, text, group;

            public string Group
            {
                get { return group; }
                set { group = value; }
            }

            public string Text
            {
                get { return text; }
                set { text = value; }
            }

            public string Value
            {
                get { return this.value; }
                set { this.value = value; }
            }
            bool selected;

            public bool Selected
            {
                get { return selected; }
                set { selected = value; }
            }
        }

        public JsonResult UpdateDropDown(UpdateDropDownModel model, FormCollection form)
        {

            DataTable @default = Helper.getData(string.Format("select  top 1 COLUMN_NAME,DATA_TYPE from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME='{0}' and DATA_TYPE='nvarchar'", model.Table), db); 
            if (@default.Rows.Count == 0)
            {
                @default = Helper.getData(string.Format("select COLUMN_NAME,DATA_TYPE from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME='{0}' and COLUMN_NAME='name'", model.Table), db);
            }
            string orderRichBy = "";
            if (@default.Rows.Count > 0)
                orderRichBy = " order by " + @default.Rows[0][0].ToString();

            DataTable data = Helper.getData(string.Format("SELECT {0} as value, {1} as text {4} from {2} {3} {6} {5}", model.Value, model.Text, model.Table, model.Where
                ,
                !string.IsNullOrEmpty(model.Groupby) ? " ," + model.Groupby + " as groupby " : "",
                !string.IsNullOrEmpty(model.Groupby) ? " order by " + model.Groupby : "",
                string.IsNullOrEmpty(model.Groupby) ? orderRichBy : ""), db);
            List<DropDownListJson> json = new List<DropDownListJson>();
            if (!string.IsNullOrEmpty(model.OptionLabel))
                json.Add(new DropDownListJson() { Value = "0", Text = (model.OptionLabel) });

            foreach (DataRow item in data.Rows)
            {
                bool selected = false;
                if (model.Selected != null)
                    selected = model.Selected.Contains(item["value"].ToString());
                if (!string.IsNullOrEmpty(model.Groupby))
                    json.Add(new DropDownListJson() { Value = item["value"].ToString(), Text = item["text"].ToString(), Group = item["groupby"].ToString(), Selected = selected });
                else
                    json.Add(new DropDownListJson() { Value = item["value"].ToString(), Text = item["text"].ToString(), Selected = selected });
            }
            return Json(json, JsonRequestBehavior.AllowGet);
        }

        public string UpdateRadioButton(UpdateDropDownModel model, FormCollection form)
        {
            DataTable data = Helper.getData(string.Format("SELECT {0} as value, {1} as text from {2} {3}", model.Value, model.Text, model.Table, model.Where), db);
            int col = 12 / (model.Col > 4 ? 4 : model.Col);
            StringBuilder html = new StringBuilder();
            int index = 1;
            bool wasClose = true;
            foreach (DataRow item in data.Rows)
            {
                if (index == 1 && wasClose)
                {
                    html.AppendFormat("	<div class=\"col col-{0}\">", col);
                    wasClose = false;
                }
                bool toggle = (!string.IsNullOrEmpty(model.ToggleOff) && !string.IsNullOrEmpty(model.ToggleON));
                html.AppendFormat("<label class=\"{0}\">", toggle ? "toggle" : model.Type);
                bool selected = model.Selected.Contains(item["value"].ToString());
                html.AppendFormat("<input type=\"{3}\" {0} name=\"{1}\" value=\"{2}\" >", selected ? "checked=\"checked\"" : "", model.Name, item["value"].ToString(), model.Type);
                if (!string.IsNullOrEmpty(model.ToggleOff) && !string.IsNullOrEmpty(model.ToggleON))
                    html.AppendFormat("<i data-swchoff-text=\"{0}\" data-swchon-text=\"{1}\"></i>", (model.ToggleOff), (model.ToggleON));
                else
                    html.AppendFormat("<i></i>");
                html.AppendFormat("{0}</label>", item["text"]);
                if (index % model.Row == 0)
                {
                    html.AppendFormat("</div>");
                    wasClose = true;
                }
            }
            return html.ToString();
        }


        [ValidateInput(false)]
        public JsonResult Badge(string query)
        {
            Dictionary<string, object> keywords = new Dictionary<string, object>();
            keywords.Add("@userID", Helper.GetUser(db).ID);

            string finalQuery = query;
            foreach (KeyValuePair<string, object> item in keywords)
            {
                finalQuery = finalQuery.Replace(item.Key, item.Value.ToString());
            }

            DataTable result = Helper.getData(finalQuery, db);
            if (result.Rows.Count > 0)
            {
                if (result.Rows[0].ItemArray.Count() > 0)
                {
                    return Json(result.Rows[0][0].ToString());
                }
            }
            return Json(0);
        }

        [ValidateInput(false)]
        public JsonResult ValidateQuery(FormCollection form)
        {
            if (string.IsNullOrEmpty(form["directQuery"]))
            {
                //if (string.IsNullOrEmpty(form["description"]))
                //    return Json("This field is required.");
            }

            if (form.AllKeys.Contains("querys"))
            {
                if (string.IsNullOrEmpty(form["querys"]))
                {
                    return Json("This field is required.");
                }
            }

            if (form.AllKeys.Contains("description"))
            {
                if (string.IsNullOrEmpty(form["description"]))
                {
                    return Json("This field is required.");
                }
            }

            try
            {
                if (!string.IsNullOrEmpty(form["description"]))
                {
                    Helper.executeNonQUery(form["description"], db);
                }
                else if (!string.IsNullOrEmpty(form["directQuery"]))
                {
                    Helper.executeNonQUery(form["directQuery"], db);
                }
                else if (!string.IsNullOrEmpty(form["querys"]))
                {
                    Helper.executeNonQUery(form["querys"], db);
                }
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }

            return Json(true);
        }

        [ValidateInput(false)]
        public JsonResult ValidateDataType(FormCollection form)
        {
            if (string.IsNullOrEmpty(form["sqlType"]))
                return Json("This field is required.");

            try
            {
                Helper.executeNonQUery(string.Format("Declare @field {0}", form["sqlType"]), db);
            }
            catch
            {
                return Json(("Invalid data type"));
            }

            return Json(true);
        }

        [ValidateInput(false)]
        public JsonResult ValidateTableName(FormCollection form)
        {
            if (string.IsNullOrEmpty(form["name"]))
                return Json("This field is required.");


            string name = form["name"].ToString();
            string comand = "";
            string prefijo = "Dynamic_";
            comand = string.Format("CREATE TABLE [{0}{1}] ( " +
                                  "id int NOT NULL IDENTITY(1,1) PRIMARY KEY)",
                                  prefijo, name);
            try
            {
                //if (existTable(prefijo.Trim()+name.Trim())) { return Json(("This table already exists")); }

                Helper.executeNonQUery(comand, db);
                Helper.executeNonQUery(string.Format("DROP TABLE [{0}{1}]", prefijo, name), db);
            }
            catch (Exception ex)
            {
                return Json((ex.Message));
            }


            return Json(true);
        }

        [ValidateInput(false)]
        public JsonResult ValidateStoredProcedure(FormCollection form)
        {
            string Querycode = form["Querycode"];
            string procedurename = form["procedureName"];
            string parameter = form["Parameters"];
            string edicion = form["edicion"];

            StringBuilder queryFinal = new StringBuilder();
            if (string.IsNullOrEmpty(Querycode) || string.IsNullOrEmpty(procedurename) || string.IsNullOrEmpty(parameter))
                return Json("This field is required.");

            try
            {
                if (edicion == "No")
                {
                    queryFinal.AppendFormat("CREATE PROCEDURE dbo.{0}", procedurename);
                    queryFinal.AppendFormat(" ({0}) ", parameter);
                    queryFinal.Append("AS BEGIN ");
                    queryFinal.Append(Querycode);
                    queryFinal.Append(" END");
                }
                else if (edicion == "Si")
                {
                    procedurename = "TryValid";
                    queryFinal.AppendFormat("CREATE PROCEDURE dbo.{0}", procedurename);
                    queryFinal.AppendFormat(" ({0}) ", parameter);
                    queryFinal.Append("AS BEGIN ");
                    queryFinal.Append(Querycode);
                    queryFinal.Append(" END");
                }

                Helper.executeNonQUery(queryFinal.ToString(), db);
            }
            catch (Exception ex)
            {
                try
                {
                    string drop = string.Format("Drop PROCEDURE {0}", procedurename);
                    Helper.executeNonQUery(drop, db);
                }
                catch (Exception) { }

                return Json(ex.Message);
            }

            try
            {
                string drop2 = string.Format("Drop PROCEDURE {0}", procedurename);
                Helper.executeNonQUery(drop2, db);
            }
            catch (Exception) { }

            return Json(true);
        }

        [ValidateInput(false)]
        public JsonResult ValidateParameterSP(FormCollection form)
        {
            string param = form["Parameters"];

            if (string.IsNullOrEmpty(param))
                return Json("This field is required.");

            try
            {
                string execute = "DECLARE " + param;

                Helper.executeNonQUery(execute, db);
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }

            return Json(true);
        }

        [ValidateInput(false)]
        public JsonResult NotRepeatFlow(FormCollection form)
        {
            string flow = form["flowOrder"];
            string docid = form["documentID"];
            string flowOrder_old = form["flowOrder_old"];

            if (string.IsNullOrEmpty(flow))
            {
                return Json("This field is required.");
            }

            if (flowOrder_old != flow)
            {
                DataTable dt = Helper.getData("Select count(flowOrder) from dbo.DocumentConceptStatusProcedure where flowOrder = " + flow +
                    " AND documentID = " + docid, db);

                if (dt.Rows.Count > 0)
                {
                    if (int.Parse(dt.Rows[0][0].ToString()) > 0)
                    {
                        return Json("Este Flujo ya existe.");
                    }

                }
            }

            return Json(true);
        }


        [ValidateInput(false)]
        public JsonResult ValidateNumericTypeCode(FormCollection form)
        {
            string val = form["codeString"];
            if (string.IsNullOrEmpty(val))
            {
                return Json("This field is required.");
            }

            if (!val.StartsWith("@"))
            {
                return Json(("Variables must start with @."));
            }

            return Json(true);
        }

        [ValidateInput(false)]
        public JsonResult ValidateStructListOf(FormCollection form)
        {
            string val = form["listOf"];
            if (string.IsNullOrEmpty(val))
            {
                return Json("This field is required.");
            }

            string[] splitVal = val.Split(';');
            string estructuraDefault = "Tabla;Campo Valor;Campo Texto";
            string estructuraWithWhere = "table;id;name;Where";
            string estructuraWithGroup = "table;id;name;Where;groupby";
            string[] splitDefault = estructuraDefault.Split(';');
            string[] splitWithWhere = estructuraWithWhere.Split(';');
            string[] splitWithGroup = estructuraWithGroup.Split(';');

            if (splitDefault.Length == splitVal.Length)
            {
                return Json(true);
            }
            else if (splitWithWhere.Length == splitVal.Length)
            {
                return Json(true);
            }
            else if (splitWithGroup.Length == splitVal.Length)
            {
                return Json(true);
            }
            else
            {
                return Json(("Written structure is incorrect, please write something like this: Table;Field Value;Text Field"));
            }
        }

        public string DataTypesFromSql()
        {
            StringBuilder html = new StringBuilder();

            DataTable dt = Helper.getData("SELECT name FROM sys.types ORDER BY name", db);
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    string nameDatatyope = dr["name"].ToString();
                    string length = "";
                    if (nameDatatyope.Contains("nvarchar") || nameDatatyope.Contains("varchar") || nameDatatyope.Contains("varbinary"))
                    {
                        length = "(50)";
                    }
                    else if (nameDatatyope.Contains("nchar") || nameDatatyope.Contains("char"))
                    {
                        length = "(10)";
                    }
                    else if (nameDatatyope.Contains("decimal") || nameDatatyope.Contains("numeric"))
                    {
                        length = "(18,0)";
                    }
                    html.AppendFormat("<option value=\"{0}{1}\"></option>", nameDatatyope, length);
                }
            }



            return html.ToString();
        }

        [ValidateInput(false)]
        [AllowAnonymous]
        public JsonResult ExistIdentification(FormCollection ident)
        {
            string e = ident["documentID"].Replace("-", "");

            if (string.IsNullOrEmpty(e))
            {
                return Json(("This field is required."));
            }

            int us = 0;
            us = db.BaseUsers.Where(x => x.userStatusID == 1).Count();

            if (us <= 0)
            {
                return Json(("This document ID does not exist."));
            }

            return Json(true);
        }


        [ValidateInput(false)]
        [AllowAnonymous]
        public JsonResult ExistEmail(FormCollection ident)
        {
            string e = ident["username"].Trim();
            int notmyID = 0;
            if (ident["ID"] != null)
                notmyID = int.Parse(ident["ID"]);

            if (string.IsNullOrEmpty(e))
            {
                return Json(("This field is required."));
            }

            int us = 0;
            us = db.BaseUsers.Where(x => x.username == e && x.userStatusID == 1 && x.ID != notmyID).Count();

            if (us > 0)
            {
                return Json("Este Correo Electrónico ya existe.");
            }

            return Json(true);
        }

        [ValidateInput(false)]
        [AllowAnonymous]
        public JsonResult SameValuePassword(FormCollection form)
        {
            string Campo = form["password"].Trim();
            string CampoComparar = form["confirmpassword"].Trim();

            if (string.IsNullOrEmpty(CampoComparar))
            {
                return Json(("This field is required."));
            }

            if (CampoComparar != Campo)
            {
                return Json("Las Contraseñas deben ser iguales.");
            }

            return Json(true);
        }


    }
}
