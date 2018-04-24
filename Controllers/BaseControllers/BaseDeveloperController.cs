using Admin.CustomCode;
using Admin.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Admin.BaseModels.ViewModels;
using System.Data.Entity.Design.PluralizationServices;
using System.Globalization;

namespace Admin.Controllers
{

    public class BaseDeveloperController : BaseController
    {
        public class dbProperties
        {
            public dbProperties(DataRow item)
            {
                column_id = int.Parse(item["column_id"].ToString());
                type = item["type"].ToString();
                max_length = int.Parse(item["max_length"].ToString());
                precision = int.Parse(item["precision"].ToString());
                scale = int.Parse(item["scale"].ToString());
                name = item["name"].ToString();
                tableName = item["tableName"].ToString();
                fk = item["fk"].ToString();
                is_nullable = item["is_nullable"].ToString() == "True" ? true : false;
                is_identity = item["is_identity"].ToString() == "True" ? true : false;
                is_computed = item["is_computed"].ToString() == "True" ? true : false;
                alterText = item["alterText"].ToString();
                comment = item["comment"].ToString();
                if (type == "nvarchar")
                {
                    max_length = max_length / 2;
                }

            }
            int column_id, max_length, precision, scale;
            public int Scale
            {
                get { return scale; }
                set { scale = value; }
            }

            public int Precision
            {
                get { return precision; }
                set { precision = value; }
            }

            public int Max_length
            {
                get { return max_length; }
                set { max_length = value; }
            }

            public int Column_id
            {
                get { return column_id; }
                set { column_id = value; }
            }

            string name, tableName, type, fk, alterText;

            public string AlterText
            {
                get { return alterText; }
                set { alterText = value; }
            }

            public string Fk
            {
                get { return fk; }
                set { fk = value; }
            }

            public string Type
            {
                get { return type; }
                set { type = value; }
            }

            public string TableName
            {
                get { return tableName; }
                set { tableName = value; }
            }

            public string Name
            {
                get { return name; }
                set { name = value; }
            }

            bool is_nullable, is_identity, is_computed;

            public bool Is_computed
            {
                get { return is_computed; }
                set { is_computed = value; }
            }

            public bool Is_identity
            {
                get { return is_identity; }
                set { is_identity = value; }
            }

            public bool Is_nullable
            {
                get { return is_nullable; }
                set { is_nullable = value; }
            }

            public string comment { get; set; }
        }


        public void GetFields(string tableName, DataTable columns, int formSplit, StringBuilder form, bool report = false, string prefix = "", string[] hideplus = null)
        {
            form.AppendFormat("<fieldset>" + Environment.NewLine);

            if (!report)
                if (!string.IsNullOrEmpty(tableName))
                {
                    foreach (DataRow item in columns.Rows)
                    {
                        dbProperties properties = new dbProperties(item);
                        if (properties.Name.ToLower() == "id")
                        {
                            form.AppendFormat("@Html.HiddenFor(d => d." + properties.Name + ")" + Environment.NewLine);
                        }
                        if (properties.Name.ToLower() == "active")
                        {
                            form.AppendFormat("@Html.HiddenFor(d => d." + properties.Name + ")" + Environment.NewLine);
                        }
                        else
                            if (properties.Name.ToLower() == "creationdate")
                        {
                            form.AppendFormat("@Html.HiddenFor(d => d." + properties.Name + ")" + Environment.NewLine);
                        }
                        else if (hideplus != null)
                        {
                            if (hideplus.Contains(properties.Name))
                            {
                                form.AppendFormat("@Html.HiddenFor(d => d." + properties.Name + ")" + Environment.NewLine);
                            }
                        }
                    }
                }

            if (!string.IsNullOrEmpty(tableName))
            {
                int fieldsCount = 0;
                foreach (DataRow item in columns.Rows)
                {
                    dbProperties properties = new dbProperties(item);
                    if (prefix != "")
                    {
                        if (properties.Type == "text" && properties.Name.ToLower().Contains("html"))
                            continue;
                        if (properties.Name.ToLower().Contains("file") || properties.Name.ToLower().Contains("image"))
                            continue;
                    }
                    if (properties.Name.ToLower() == "active")
                    {
                        continue;
                    }
                    else
                    if (properties.Name.ToLower() == "id")
                    {
                        continue;
                    }
                    else
                        if (properties.Name.ToLower() == "creationdate")
                    {
                        continue;
                    }
                    else if (properties.Name.ToLower().EndsWith("nd"))
                    {
                        form.AppendFormat("@Html.HiddenFor(d => d." + properties.Name + ")" + Environment.NewLine);
                        continue;
                    }
                    else if (hideplus != null)
                    {
                        if (hideplus.Contains(properties.Name))
                        {
                            continue;
                        }
                    }
                    if (fieldsCount % formSplit == 0)
                    {
                        if (fieldsCount != 0)
                        {
                            form.AppendFormat("</div>" + Environment.NewLine);
                        }
                        form.AppendFormat("<div class=\"row\">" + Environment.NewLine);
                    }
                    fieldsCount++;
                    string validations = "";
                    if (!properties.Is_nullable)
                        validations += "required;";

                    if (!string.IsNullOrEmpty(properties.AlterText))
                    {
                        string[] fkeys = properties.Fk.Split(';');
                        form.AppendFormat("@{{var selected{0} = new List<object>(); if(Model." + properties.Name + "!=null) {{ selected{0}.Add(Model." + properties.Name + "); }} }} " + Environment.NewLine, prefix + properties.Name);
                        form.AppendFormat("@Html.BaseDropDownList(\"{0}\", \"{1}\", \"{2}\", \"{3}\", \"\", " + (12 / formSplit) + ", \"{5}\", \"Select {5}\", \"\", Admin.BaseClass.UI.BaseUIIcon.fa.fa_fa_arrow_circle_down, selected{0}, false, \"Refresh {1}\", \"\", \"{4}\", \"\", null) " + Environment.NewLine,
                            prefix + properties.Name, fkeys[1], fkeys[3], properties.AlterText, validations.Trim(';'),
                            Helper.Capitalize(properties.Name.Split('_')[1]).Replace("I D", "").Replace("Id", ""));
                        continue;
                    }

                    switch (properties.Type)
                    {
                        case "nvarchar":
                        case "varchar":
                        case "nchar":
                        case "char":
                        case "text":
                            {
                                if (properties.Name.ToLower().Contains("email"))
                                    validations += "email;";

                                string mask = "";
                                string ckeditor = "";
                                if (prefix == "")
                                {
                                    if (properties.Name.ToLower().Contains("phone"))
                                        mask = " , data_mask = \"(999)-999-9999\" ";

                                    if (properties.Name.ToLower().Contains("card"))
                                        mask = " , data_mask = \"9999-9999-9999-9999\" ";

                                    if (properties.Type == "text" && properties.Name.ToLower().Contains("html"))
                                    {
                                        ckeditor = ", @class = \"ckeditor\" ";
                                    }
                                }


                                if (properties.Name.ToLower().Contains("tags"))
                                {
                                    form.AppendFormat("@Html.BaseTagsInput(\"{0}\", Model.tags, \"{2}\", " + (12 / formSplit) + ", \"\", \"{1}\")" + Environment.NewLine, properties.Name, validations.Trim(';'), Helper.Capitalize(properties.Name).Replace("ID", ""));
                                }
                                else
                                    if (properties.Name.ToLower().Contains("color"))
                                {
                                    form.AppendFormat("@Html.BaseColorPicker(\"{0}\", Model." + properties.Name + ", \"{1}\", " + (12 / formSplit) + ", false, \"{1}\", \"{2}\")" + Environment.NewLine,
                                        prefix + properties.Name, Helper.Capitalize(properties.Name).Replace("ID", ""), validations.Trim(';'));
                                }
                                else if (properties.Name.ToLower().Contains("file") || properties.Name.ToLower().Contains("image"))
                                {
                                    form.AppendFormat("@Html.BaseFile(\"{0}\", \"{1}\", " + (12 / formSplit) + ", Admin.BaseClass.UI.BaseUIIcon.fa.fa_fa_file, \"{2}\", \"\",\"\", Model.{0}, @HttpContext.Current.Request.RequestContext.RouteData.Values[\"controller\"].ToString())" + Environment.NewLine,
                                        prefix + properties.Name, Helper.Capitalize(properties.Name).Replace("ID", ""), validations.Trim(';'));
                                }
                                else
                                {
                                    if (properties.Max_length != -1 && properties.Max_length != 0)
                                        if (properties.Type != "text")
                                            validations += "maxlength:" + properties.Max_length + ";";


                                    form.AppendFormat("@Html.BaseTextBox(\"{0}\", Model." + properties.Name + ",\"{1}\", new {{ icon1 = new BaseTextBoxIcon() {{ Icon = Admin.BaseClass.UI.BaseUIIcon.fa.fa_fa_text_width, Position = BaseTextBoxIconPosition.icon_append }} }}, " + (ckeditor != "" ? 12 : (12 / formSplit)) + ", new {{ placeholder = (\"{1}\") " + mask + ckeditor + " }}, \"{2}\", \"\", {3}, new MyHtmlHelpers.BaseToolTip(\"" + (properties.comment != null ? properties.comment : "") + "\"))" + Environment.NewLine,
                                        prefix + properties.Name, Helper.Capitalize(properties.Name).Replace("ID", ""),
                                        validations.Trim(';'), ((properties.Max_length > 100 || properties.Type == "text") && prefix == "" ? "true" : "false"));
                                }
                                break;
                            }
                        case "money":
                            {
                                string precision = string.Format("maxlength:{0};", properties.Precision + 1);
                                string scale = string.Format("scale:{0};", properties.Scale);

                                validations += precision + scale;
                                validations += "number;";

                                form.AppendFormat("@Html.BaseSpiner(\"{0}\", Model." + properties.Name + ", \"{1}\", " + (12 / formSplit) + ", MyHtmlHelpers.spinnerDirection.left, \"\", \"{2}\", 0,  decimal.MaxValue, 0.10, 1, \"C\")" + Environment.NewLine,
                                    prefix + properties.Name, Helper.Capitalize(properties.Name).Replace("ID", ""), validations.Trim(';'));
                                break;
                            }
                        case "float":
                            {
                                string precision = string.Format("maxlength:30;", properties.Precision + 1);
                                string scale = string.Format("scale:15;", properties.Scale);

                                validations += precision + scale;
                                validations += "number;";

                                form.AppendFormat("@Html.BaseSpiner(\"{0}\", Model." + properties.Name + ", \"{1}\", " + (12 / formSplit) + ", MyHtmlHelpers.spinnerDirection.left, \"\", \"{2}\", 0,  decimal.MaxValue, 0.10, 1, \"N\")" + Environment.NewLine,
                                    prefix + properties.Name, Helper.Capitalize(properties.Name).Replace("ID", ""), validations.Trim(';'));
                                break;
                            }
                        case "decimal":
                        case "numeric":
                            {
                                string precision = string.Format("maxlength:{0};", properties.Precision + 1);
                                string scale = string.Format("scale:{0};", properties.Scale);

                                validations += precision + scale;
                                validations += "number;";

                                form.AppendFormat("@Html.BaseSpiner(\"{0}\", Model." + properties.Name + ", \"{1}\", " + (12 / formSplit) + ", MyHtmlHelpers.spinnerDirection.left, \"\", \"{2}\", 0,  decimal.MaxValue, 0.10, 1, \"C\")" + Environment.NewLine,
                                    prefix + properties.Name, Helper.Capitalize(properties.Name).Replace("ID", ""), validations.Trim(';'));
                                break;
                            }
                        case "int":
                        case "bigint":
                        case "smallint":
                        case "tinyint":
                            {
                                string max = "";
                                if (properties.Type == "int") max = "2147483647";
                                if (properties.Type == "bigint") max = "9223372036854775807";
                                if (properties.Type == "smallint") max = "255";
                                if (properties.Type == "tinyint") max = "32767";
                                validations += "number;";

                                form.AppendFormat("@Html.BaseSpiner(\"{0}\", Model." + properties.Name + ", \"{1}\", " + (12 / formSplit) + ", MyHtmlHelpers.spinnerDirection.both, \"\", \"{2}\", 0, {3}, 1, 1, \"n\")" + Environment.NewLine,
                                    prefix + properties.Name, Helper.Capitalize(properties.Name).Replace("ID", ""), validations.Trim(';'), max);
                                break;
                            }
                        case "datetime":
                            {
                                validations += "date;";
                                form.AppendFormat("@Html.BaseDateTimePick(\"{0}\", Model." + properties.Name + ", \"{1}\", " + (12 / formSplit) + ", \"{1}\", \"{2}\", \"\", \"\")" + Environment.NewLine,
                                    prefix + properties.Name, Helper.Capitalize(properties.Name).Replace("ID", ""), validations.Trim(';'));
                                break;
                            }
                        case "date":
                            {
                                validations += "date;";
                                form.AppendFormat("@Html.BaseDateTimePick(\"{0}\", Model." + properties.Name + ", \"{1}\", " + (12 / formSplit) + ", \"{1}\", \"{2}\", \"\", \"\", false)" + Environment.NewLine,
                                    prefix + properties.Name, Helper.Capitalize(properties.Name).Replace("ID", ""), validations.Trim(';'));
                                break;
                            }
                        case "time":
                            {
                                form.AppendFormat("@Html.BaseTimePicker(\"{0}\", Model." + properties.Name + ", \"{1}\", " + (12 / formSplit) + ", \"{1}\", \"{2}\", new MyHtmlHelpers.BaseToolTip(\"{1}\"))" + Environment.NewLine,
                                    prefix + properties.Name, Helper.Capitalize(properties.Name).Replace("ID", ""), validations.Trim(';'));
                                break;
                            }
                        case "bit":
                            {
                                form.AppendFormat("@Html.BaseCheckBox(\"{0}\", Model." + properties.Name + ", \"\", \"{1}\", " + (12 / formSplit) + ", \"Yes\", \"No\", \"\")" + Environment.NewLine,
                                    prefix + properties.Name, Helper.Capitalize(properties.Name).Replace("ID", ""), validations.Trim(';'));
                                break;
                            }
                    }
                }
            }
            form.AppendFormat("</div>" + Environment.NewLine);
            form.AppendFormat("</fieldset>" + Environment.NewLine);
        }

        public void GetJsonFields(DataTable columns, StringBuilder form)
        {
            foreach (DataRow item in columns.Rows)
            {
                dbProperties properties = new dbProperties(item);
                form.AppendFormat("            '#{0}': {{" + Environment.NewLine, properties.Name);
                form.AppendFormat("                isControl: false" + Environment.NewLine);
                form.AppendFormat("            }}," + Environment.NewLine);
            }
        }

        public void GetSearchConditions(DataTable columns, StringBuilder form)
        {
            string theLast = columns.Columns[columns.Columns.Count - 1].ColumnName;
            List<string> conditions = new List<string>();
            foreach (DataRow item in columns.Rows)
            {
                dbProperties properties = new dbProperties(item);
                if (!string.IsNullOrEmpty(properties.AlterText))
                {
                    string[] fkeys = properties.Fk.Split(';');
                    conditions.Add($" (select top 1 {properties.AlterText} from [{fkeys[1]}] where {fkeys[3]}={properties.Name}) like '%{{0}}%' ");
                }
                else
                    conditions.Add($" {properties.Name} like '%{{0}}%' ");
            }
            form.Append(string.Join(" OR ", conditions));
        }

        [IsView]
        public ActionResult NII()
        {
            return PartialView();
        }

        [IsView]
        public ActionResult NIIReport()
        {
            return PartialView();
        }

        [IsView]
        public ActionResult Styles()
        {
            BaseConfiguration config = new BaseConfiguration();

            Dictionary<string, string> styles = new Dictionary<string, string>();
            Dictionary<string, string> colors = new Dictionary<string, string>();

            foreach (var item in config.styles.Split(','))
                styles.Add(item, Helper.Capitalize(item));

            foreach (var item in config.colors.Split(','))
                colors.Add(item, Helper.Capitalize(item));

            ViewBag.styles = styles;
            ViewBag.colors = colors;
            return PartialView();
        }

        [HttpPost]
        [ValidateInput(false)]
        public JsonResult ChangeStyle(string theme, string primaryColor, string secundaryColor)
        {
            BaseConfiguration config = new BaseConfiguration();
            config.css = theme;
            config.primaryColor = primaryColor;
            config.secundaryColor = secundaryColor;
            return Json(new { message = "Refresh page for see the changes" });
        }

        public string IP(string content, string returns)
        {
            return content != "" ? returns : "";
        }

        [ValidateInput(false)]
        public JsonResult getChilds(string tableName = "")
        {
            DataTable relations = Helper.getData(string.Format("select child from VWBaseRelations where parent ='{0}' ", tableName), db);
            List<string> childs = new List<string>();
            foreach (DataRow item in relations.Rows)
                childs.Add(item[0].ToString());
            return Json(childs, JsonRequestBehavior.AllowGet);
        }

        [ValidateInput(false)]
        public JsonResult getParent(string tableName = "")
        {
            DataTable relations = Helper.getData(string.Format("select fk from VWBaseRelations where child ='{0}' ", tableName), db);
            return Json(relations.Rows[0][0].ToString(), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateInput(false)]
        public JsonResult GenerateForm(List<string> tableNames, bool buildColumns = true, string parentMenu = "", bool report = false, bool @base = false,
            bool isForm = true,
            bool isIndex = true,
            bool isController = true,
            bool isModelExtra = true,
            string auditoria = "")
        {
            PluralizationService pluralize = PluralizationService.CreateService(culture: CultureInfo.GetCultureInfo("EN"));


            //string[] hiddenplus = { "creadoUsuario", "modificadoUsuario", "canceladoUsuario", "creadoFecha", "modificadoFecha", "canceladoFecha", "deleted" };

            //string newLine = Environment.NewLine;
            //string onCreate = $"model.creadoUsuario=db.BaseUsers.Find(WebSecurity.CurrentUserId).FullName;{newLine}model.creadoFecha=DateTime.Now;";
            //string onEdit = $"model.modificadoUsuario=db.BaseUsers.Find(WebSecurity.CurrentUserId).FullName;{newLine}model.modificadoFecha=DateTime.Now;";
            //string onDelete = $"model.canceladoUsuario=db.BaseUsers.Find(WebSecurity.CurrentUserId).FullName;{newLine}model.canceladoFecha=DateTime.Now;";

            string[] hiddenplus = { "creadoUsuario", "modificadoUsuario", "canceladoUsuario", "creadoFecha", "modificadoFecha", "canceladoFecha", "deleted" };

            string newLine = Environment.NewLine;
            string onCreate = $"";
            string onEdit = $"";
            string onDelete = $"";


            int prefix = 2;
            if (@base)
                prefix = 4;
            if (report)
                prefix = 0;
            StringBuilder messages = new StringBuilder();
            foreach (string tableNameItem in tableNames)
            {
                string tableName = tableNameItem.Replace("*", "");
                DataTable relations = Helper.getData($"select * from VWBaseRelations where parent='{tableName}'", db);
                string primaryKey = "id";
                DataTable getp = Helper.getData($"SELECT Col.Column_Name from INFORMATION_SCHEMA.TABLE_CONSTRAINTS Tab, INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE Col  WHERE  Col.Constraint_Name = Tab.Constraint_Name AND Col.Table_Name = Tab.Table_Name AND Constraint_Type = 'PRIMARY KEY' AND Col.Table_Name = '{tableName}'", db);
                if (getp.Rows.Count > 0)
                {
                    primaryKey = getp.Rows[0][0].ToString();
                }
                bool isGuid = Helper.getData($"select * from INFORMATION_SCHEMA.COLUMNS where DATA_TYPE='uniqueidentifier' and COLUMN_NAME='{primaryKey}' and TABLE_NAME = '{tableName}'", db).Rows.Count > 0;
                bool isChar = Helper.getData($"select * from INFORMATION_SCHEMA.COLUMNS where DATA_TYPE like '%char%' and COLUMN_NAME='{primaryKey}' and TABLE_NAME = '{tableName}'", db).Rows.Count > 0;
                string withoutPrefix = tableName.Substring(prefix, tableName.Length - prefix);
                try
                {
                    DataTable columns = null;
                    if (!string.IsNullOrEmpty(tableName))
                    {
                        columns = Helper.getData(string.Format("select * from VWBaseNiiAll where tableName='{0}' and type!='sysname' order by column_id", tableName), db);
                    }

                    int formSplit = 1;
                    if (columns.Rows.Count <= 4)
                        formSplit = 1;
                    else if (columns.Rows.Count <= 12)
                        formSplit = 2;
                    else if (columns.Rows.Count <= 24)
                        formSplit = 3;
                    else formSplit = 4;

                    //Controller
                    StringBuilder form = new StringBuilder();
                    form.AppendFormat("using Admin.BaseClass;" + Environment.NewLine);
                    form.AppendFormat("using Admin.BaseClass.UI;" + Environment.NewLine);
                    form.AppendFormat("using Admin.CustomCode;" + Environment.NewLine);
                    form.AppendFormat("using Admin.Models;" + Environment.NewLine);
                    form.AppendFormat("using Newtonsoft.Json;" + Environment.NewLine);
                    form.AppendFormat("using ClosedXML.Excel;" + Environment.NewLine);
                    form.AppendFormat("using System;" + Environment.NewLine);
                    form.AppendFormat("using System.Collections.Generic;" + Environment.NewLine);
                    form.AppendFormat("using System.Data;" + Environment.NewLine);
                    form.AppendFormat("using System.IO;" + Environment.NewLine);
                    form.AppendFormat("using System.Linq;" + Environment.NewLine);
                    form.AppendFormat("using System.Web;" + Environment.NewLine);
                    form.AppendFormat("using System.Web.Mvc;" + Environment.NewLine);
                    form.AppendFormat("using WebMatrix.WebData;" + Environment.NewLine);
                    form.AppendFormat("" + Environment.NewLine);
                    form.AppendFormat("" + Environment.NewLine);
                    form.AppendFormat("namespace Admin.Controllers" + Environment.NewLine);
                    form.AppendFormat("{{" + Environment.NewLine);
                    string controller = string.Format("{0}Controller", tableName);
                    form.AppendFormat("    public class {0} : BaseController" + Environment.NewLine, controller);
                    form.AppendFormat("    {{" + Environment.NewLine);
                    form.AppendFormat("[IsView]" + Environment.NewLine);
                    form.AppendFormat("        public ActionResult Index({0})" + Environment.NewLine, "int id = 0, string from = \"\"");
                    form.AppendFormat("        {{" + Environment.NewLine);
                    form.AppendFormat("             List<BaseDynamicColumnList> bDynamicColumn = db.BaseDynamicColumnLists.ToList();" + Environment.NewLine);
                    form.AppendFormat("             List<BaseDynamicList> bDynamicList = db.BaseDynamicLists.ToList();" + Environment.NewLine);
                    form.AppendFormat("             ViewBag.column = bDynamicColumn;" + Environment.NewLine);
                    form.AppendFormat("             ViewBag.dynamicList = bDynamicList;" + Environment.NewLine);
                    form.AppendFormat("             ViewBag.from = from;" + Environment.NewLine);
                    form.AppendFormat("             ViewBag.tableName = \"{0}\";" + Environment.NewLine, tableName);
                    form.AppendFormat("             return PartialView(new {0}());" + Environment.NewLine, tableName);
                    form.AppendFormat("        }}" + Environment.NewLine);

                    string plural = pluralize.Pluralize(tableName);
                    if (!report)
                    {
                        form.AppendFormat("[IsView]" + Environment.NewLine);
                        if (!isGuid && !isChar)
                            form.AppendFormat("        public ActionResult Form(int id=0, string from = \"\")" + Environment.NewLine);
                        else
                            form.AppendFormat("        public ActionResult Form(string id=\"\", string from = \"\")" + Environment.NewLine);
                        form.AppendFormat("        {{" + Environment.NewLine);
                        form.Append($"ViewBag.relations = db.VWISRElations.Where(d => d.PK_Table == \"{tableName}\").ToList();" + Environment.NewLine);
                        form.AppendFormat("            ViewBag.from = from;" + Environment.NewLine);

                        if (!isGuid && !isChar)
                            form.AppendFormat("            if (id == 0)" + Environment.NewLine);
                        else
                            form.AppendFormat("            if (id == \"\")" + Environment.NewLine);

                        int hasCreationDate = Helper.getData(string.Format("select * from VWBaseNiiAll where tableName='{0}' and type!='sysname' and name='creationDate' order by column_id", tableName), db).Rows.Count;
                        if (hasCreationDate > 0)
                            form.AppendFormat("                return PartialView(new {0}(){{ active=true, creationDate = DateTime.Now}});" + Environment.NewLine, tableName);
                        else
                            form.AppendFormat("                return PartialView(new {0}(){{ active=true}});" + Environment.NewLine, tableName);
                        form.AppendFormat("            else" + Environment.NewLine);
                        form.AppendFormat("            {{" + Environment.NewLine);
                        if (isGuid)
                            form.AppendFormat("{1} model = db.{0}.Find(Guid.Parse(id.Replace(\"─\",\"-\")));" + Environment.NewLine, plural, tableName);
                        else if (isChar)
                            form.AppendFormat("                {1} model = db.{0}.Find(id);" + Environment.NewLine, plural, tableName);
                        else
                            form.AppendFormat("                {1} model = db.{0}.Find(id);" + Environment.NewLine, plural, tableName);



                        form.AppendFormat("                return PartialView(model);" + Environment.NewLine);
                        form.AppendFormat("            }}" + Environment.NewLine);
                        form.AppendFormat("        }}" + Environment.NewLine);


                        form.AppendFormat("[HttpPost]" + Environment.NewLine);
                        form.AppendFormat("[ValidateInput(false)]" + Environment.NewLine);
                        form.AppendFormat("        public JsonResult Save({0} model, FormCollection form)" + Environment.NewLine, tableName);
                        form.AppendFormat("        {{" + Environment.NewLine);
                        form.AppendFormat("try" + Environment.NewLine);
                        form.AppendFormat("            {{" + Environment.NewLine);


                        if (isGuid)
                        {
                            form.Append($"model.{primaryKey} = form[\"{primaryKey}\"].ToString() == \"00000000-0000-0000-0000-000000000000\" ? new Guid() : Guid.Parse(form[\"{primaryKey}\"].ToString().Replace(\"─\", \"-\"));" + Environment.NewLine);
                            form.AppendFormat("            if (model.{0} != new Guid())" + Environment.NewLine, primaryKey);
                        }
                        else if (isChar)
                            form.AppendFormat("            if (model.{0} != \"\")" + Environment.NewLine, primaryKey);
                        else
                            form.AppendFormat("            if (model.{0} != 0)" + Environment.NewLine, primaryKey);

                        form.AppendFormat("            {{" + Environment.NewLine);
                        form.Append($"                    BoolString validation = model.BeforeEdit(db);" + Environment.NewLine);
                        form.Append($"                    if (validation.BoolValue)" + Environment.NewLine);
                        form.Append($"                        return Json(new {{ Message = validation.StringValue }});" + Environment.NewLine);
                        form.AppendFormat("                    db.Entry(model).State = System.Data.Entity.EntityState.Modified;" + Environment.NewLine);

                        bool goinFile = false;

                        foreach (DataRow item in columns.Rows)
                        {
                            dbProperties properties = new dbProperties(item);
                            if (properties.Name.ToLower().Contains("image") || properties.Name.ToLower().Contains("file"))
                            {
                                goinFile = true;
                                break;
                            }
                        }

                        if (goinFile)
                        {
                            foreach (DataRow item in columns.Rows)
                            {
                                dbProperties properties = new dbProperties(item);
                                if (properties.Name.ToLower().Contains("image") || properties.Name.ToLower().Contains("file"))
                                {
                                    form.Append($"if (Request.Files.Count > 0)" + Environment.NewLine);
                                    form.Append($"   if (Request.Files[\"{properties.Name}\"].ContentLength == 0)" + Environment.NewLine);
                                    form.Append($"   db.Entry(model).Property(x => x.{properties.Name}).IsModified = false;" + Environment.NewLine);
                                }
                            }
                        }


                        form.Append($"                    validation = model.BeforeSave(db);" + Environment.NewLine);
                        form.Append($"                    if (validation.BoolValue)" + Environment.NewLine);
                        form.Append($"                        return Json(new {{ Message = validation.StringValue }});" + Environment.NewLine);

                        form.Append($"                    validation = model.BeforeEdit(db);" + Environment.NewLine);
                        form.Append($"                {onEdit}" + Environment.NewLine);
                        form.Append($"                    if (validation.BoolValue)" + Environment.NewLine);
                        form.Append($"                        return Json(new {{ Message = validation.StringValue }});" + Environment.NewLine);

                        form.AppendFormat("                    db.SaveChanges();" + Environment.NewLine);

                        form.Append($"                    validation = model.AfterSave(db);" + Environment.NewLine);
                        form.Append($"                {onEdit}" + Environment.NewLine);
                        form.Append($"                    if (validation.BoolValue)" + Environment.NewLine);
                        form.Append($"                        return Json(new {{ Message = validation.StringValue }});" + Environment.NewLine);

                        form.Append($"                    validation = model.AfterEdit(db);" + Environment.NewLine);
                        form.Append($"                {onEdit}" + Environment.NewLine);
                        form.Append($"                    if (validation.BoolValue)" + Environment.NewLine);
                        form.Append($"                        return Json(new {{ Message = validation.StringValue }});" + Environment.NewLine);
                        form.AppendFormat("            }}" + Environment.NewLine);
                        form.AppendFormat("            else" + Environment.NewLine);
                        form.AppendFormat("            {{" + Environment.NewLine);
                        if (isGuid)
                            form.Append($"                      model.id = Guid.NewGuid();" + Environment.NewLine);
                        form.AppendFormat("                 BoolString validation = model.BeforeSave(db);" + Environment.NewLine);
                        form.AppendFormat("                 if (validation.BoolValue)" + Environment.NewLine);
                        form.AppendFormat("                    return Json(new {{ Message = validation.StringValue }});" + Environment.NewLine);

                        form.AppendFormat("                 validation = model.BeforeCreate(db);" + Environment.NewLine);
                        form.AppendFormat("                 if (validation.BoolValue)" + Environment.NewLine);
                        form.AppendFormat("                    return Json(new {{ Message = validation.StringValue }});" + Environment.NewLine);
                        form.Append($"                {onCreate}" + Environment.NewLine);
                        form.AppendFormat("                db.{0}.Add(model);" + Environment.NewLine, plural);
                        form.AppendFormat("                db.SaveChanges();" + Environment.NewLine);
                        form.Append($"                validation = model.AfterSave(db);" + Environment.NewLine);
                        form.Append($"                if (validation.BoolValue)" + Environment.NewLine);
                        form.Append($"                    return Json(new {{ Message = validation.StringValue }});" + Environment.NewLine);
                        form.Append($"                validation = model.AfterCreate(db);" + Environment.NewLine);
                        form.Append($"                if (validation.BoolValue)" + Environment.NewLine);
                        form.Append($"                    return Json(new {{ Message = validation.StringValue }});" + Environment.NewLine);
                        form.AppendFormat("            }}" + Environment.NewLine);



                        if (goinFile)
                        {
                            form.AppendFormat("			   HttpPostedFileBase file = null;" + Environment.NewLine);
                            form.AppendFormat("            if (Request.Files.Count > 0)" + Environment.NewLine);
                            form.AppendFormat("            {{" + Environment.NewLine);
                            foreach (DataRow item in columns.Rows)
                            {
                                dbProperties properties = new dbProperties(item);
                                if (properties.Name.ToLower().Contains("image") || properties.Name.ToLower().Contains("file"))
                                {
                                    form.AppendFormat("                file = Request.Files[\"{0}\"];" + Environment.NewLine, properties.Name);
                                    form.AppendFormat("                if (file != null && file.ContentLength > 0)" + Environment.NewLine);
                                    form.AppendFormat("                {{" + Environment.NewLine);
                                    form.AppendFormat("                    string ext = Path.GetExtension(file.FileName);" + Environment.NewLine);
                                    form.AppendFormat("                    string filename = model.{0}.ToString() + ext;" + Environment.NewLine, primaryKey);
                                    form.AppendFormat("                    string directory = Server.MapPath(\"~/files/{0}/{1}/\");" + Environment.NewLine, tableName, properties.Name);
                                    form.AppendFormat("                    if (!Directory.Exists(directory))" + Environment.NewLine);
                                    form.AppendFormat("                        Directory.CreateDirectory(directory);" + Environment.NewLine);
                                    form.AppendFormat("                    var path = Path.Combine(directory, filename);" + Environment.NewLine);
                                    form.AppendFormat("                    file.SaveAs(path);" + Environment.NewLine);
                                    form.AppendFormat("                    model.{0} = filename;" + Environment.NewLine, properties.Name);
                                    form.AppendFormat("                }}" + Environment.NewLine);
                                    form.AppendFormat("" + Environment.NewLine);
                                }
                            }
                            form.AppendFormat("                }}" + Environment.NewLine);
                            form.AppendFormat("                db.SaveChanges();" + Environment.NewLine);
                        }
                        form.AppendFormat("                return Json(new {{ id = model.{1}, MessageSucess = \"That {0} saved successfully.\" }});" + Environment.NewLine, Helper.Capitalize(tableName, 2), primaryKey);
                        form.AppendFormat("            }}" + Environment.NewLine);
                        form.AppendFormat("            catch (Exception ex)" + Environment.NewLine);
                        form.AppendFormat("            {{" + Environment.NewLine);
                        form.AppendFormat("                return Json(new {{ Message = Helper.ModeralException(ex).Replace(\"@table\", \"{0}\") }});" + Environment.NewLine, Helper.Capitalize(tableName, 2));
                        form.AppendFormat("            }}" + Environment.NewLine);
                        form.AppendFormat("            }}" + Environment.NewLine);


                        if (isGuid)
                            form.AppendFormat("public JsonResult ToggleActive(string id = \"\")" + Environment.NewLine);
                        else if (isChar)
                            form.AppendFormat("public JsonResult ToggleActive(string id = \"\")" + Environment.NewLine);
                        else
                            form.AppendFormat("public JsonResult ToggleActive(int id = 0)" + Environment.NewLine);
                        form.Append($"        {{" + Environment.NewLine);
                        form.Append($"            try" + Environment.NewLine);
                        form.Append($"            {{" + Environment.NewLine);
                        if (!isGuid)
                            form.AppendFormat("	{0} model = db.{1}.Find(id);" + Environment.NewLine, tableName, plural);
                        else
                            form.AppendFormat("	{0} model = db.{1}.Find(Guid.Parse(id.Replace(\"─\", \"-\")));" + Environment.NewLine, tableName, plural);


                        form.Append($"                if (model != null)" + Environment.NewLine);
                        form.Append($"                {{" + Environment.NewLine);
                        form.Append($"                    if (model.active)" + Environment.NewLine);
                        form.Append($"                    {{" + Environment.NewLine);
                        form.Append($"                        BoolString validation = model.BeforeInactive(db);" + Environment.NewLine);
                        form.Append($"                        if (validation.BoolValue)" + Environment.NewLine);
                        form.Append($"                            return Json(new {{ Message = validation.StringValue }}, JsonRequestBehavior.AllowGet);" + Environment.NewLine);
                        form.Append($"                          model.active = !model.active;" + Environment.NewLine);
                        form.Append($"                        db.SaveChanges();" + Environment.NewLine);
                        form.Append($"                        validation = model.AfterInactive(db);" + Environment.NewLine);
                        form.Append($"                        if (validation.BoolValue)" + Environment.NewLine);
                        form.Append($"                            return Json(new {{ Message = validation.StringValue }}, JsonRequestBehavior.AllowGet);" + Environment.NewLine);
                        form.Append($"                        return Json(new {{ id = model.{primaryKey}, MessageSucess = \"That {Helper.Capitalize(tableName, 2)} Inactive successfully.\" }}, JsonRequestBehavior.AllowGet);" + Environment.NewLine);
                        form.Append($"                    }}" + Environment.NewLine);
                        form.Append($"                    else" + Environment.NewLine);
                        form.Append($"                    {{" + Environment.NewLine);
                        form.Append($"                        BoolString validation = model.BeforeActive(db);" + Environment.NewLine);
                        form.Append($"                        if (validation.BoolValue)" + Environment.NewLine);
                        form.Append($"                            return Json(new {{ Message = validation.StringValue }}, JsonRequestBehavior.AllowGet);" + Environment.NewLine);
                        form.Append($"                model.active = !model.active;" + Environment.NewLine);
                        form.Append($"                        db.SaveChanges();" + Environment.NewLine);
                        form.Append($"                        validation = model.AfterActive(db);" + Environment.NewLine);
                        form.Append($"                        if (validation.BoolValue)" + Environment.NewLine);
                        form.Append($"                            return Json(new {{ Message = validation.StringValue }}, JsonRequestBehavior.AllowGet);" + Environment.NewLine);
                        form.Append($"                        return Json(new {{ id = model.id, MessageSucess = \"That {Helper.Capitalize(tableName, 2)} Active successfully.\" }}, JsonRequestBehavior.AllowGet);" + Environment.NewLine);
                        form.Append($"                    }}" + Environment.NewLine);
                        form.Append($"                }}" + Environment.NewLine);
                        form.Append($"                return Json(new {{ Message = \"This record no longer exists\" }}, JsonRequestBehavior.AllowGet);" + Environment.NewLine);
                        form.Append($"            }}" + Environment.NewLine);
                        form.Append($"            catch (Exception ex)" + Environment.NewLine);
                        form.Append($"            {{" + Environment.NewLine);
                        form.Append($"                return Json(new {{ Message = Helper.ModeralException(ex).Replace(\"@table\", \"{Helper.Capitalize(tableName, 2)}\") }}, JsonRequestBehavior.AllowGet);" + Environment.NewLine);
                        form.Append($"            }}" + Environment.NewLine);
                        form.Append($"        }}" + Environment.NewLine);



                        if (isGuid)
                            form.AppendFormat("public JsonResult Delete(string id = \"\")" + Environment.NewLine);
                        else if (isChar)
                            form.AppendFormat("public JsonResult Delete(string id = \"\")" + Environment.NewLine);
                        else
                            form.AppendFormat("public JsonResult Delete(int id = 0)" + Environment.NewLine);
                        form.AppendFormat("{{" + Environment.NewLine);
                        form.AppendFormat("try" + Environment.NewLine);
                        form.AppendFormat(" {{" + Environment.NewLine);
                        if (!isGuid)
                            form.AppendFormat("	{0} model = db.{1}.Find(id);" + Environment.NewLine, tableName, plural);
                        else
                            form.AppendFormat("	{0} model = db.{1}.Find(Guid.Parse(id.Replace(\"─\", \"-\")));" + Environment.NewLine, tableName, plural);

                        form.AppendFormat("if(model != null){{");
                        form.Append($"                    BoolString validation = model.BeforeDelete(db);" + Environment.NewLine);
                        form.Append($"                    if (validation.BoolValue)" + Environment.NewLine);
                        form.Append($"                        return Json(new {{ Message = validation.StringValue }}, JsonRequestBehavior.AllowGet);" + Environment.NewLine);
                        form.AppendFormat("	db.{0}.Remove(model);" + Environment.NewLine, plural);
                        form.AppendFormat("	db.SaveChanges();" + Environment.NewLine);
                        form.Append($"                    validation = model.AfterDelete(db);" + Environment.NewLine);
                        form.Append($"                {onDelete}" + Environment.NewLine);
                        form.Append($"                    if (validation.BoolValue)" + Environment.NewLine);
                        form.Append($"                        return Json(new {{ Message = validation.StringValue }}, JsonRequestBehavior.AllowGet);" + Environment.NewLine);
                        form.AppendFormat("                return Json(new {{ id = model.{1}, MessageSucess = \"That {0} deleted successfully.\" }}, JsonRequestBehavior.AllowGet);" + Environment.NewLine, Helper.Capitalize(tableName, 2), primaryKey);
                        form.AppendFormat("            }}" + Environment.NewLine);
                        form.AppendFormat("return Json(new {{ Message = \"This record no longer exists\" }}, JsonRequestBehavior.AllowGet);" + Environment.NewLine);
                        form.AppendFormat("            }}" + Environment.NewLine);
                        form.AppendFormat("            catch (Exception ex)" + Environment.NewLine);
                        form.AppendFormat("            {{" + Environment.NewLine);
                        form.AppendFormat("                return Json(new {{ Message = Helper.ModeralException(ex).Replace(\"@table\", \"{0}\") }}, JsonRequestBehavior.AllowGet);" + Environment.NewLine, Helper.Capitalize(tableName, 2));
                        form.AppendFormat("            }}" + Environment.NewLine);
                        form.AppendFormat("            }}" + Environment.NewLine);
                    }
                    form.AppendFormat("public JsonResult data(FormCollection form, string from = \"\", string filter = \"\")" + Environment.NewLine);
                    form.AppendFormat("{{" + Environment.NewLine);
                    form.AppendFormat("	JsonCsTableHelper table = new JsonCsTableHelper(db, form, Request.QueryString);" + Environment.NewLine);
                    form.AppendFormat("	table.TableName = \"[{0}]\";" + Environment.NewLine, tableName);
                    if (!report)
                    {
                        form.AppendFormat("	table.PrimaryKey = \"{0}\";" + Environment.NewLine, primaryKey);
                    }
                    else
                        form.AppendFormat("	table.PrimaryKey = \"" + columns.Rows[0]["name"] + "\";" + Environment.NewLine);
                    form.Append($"            string[] hides = table.hideColumns;" + Environment.NewLine);
                    form.Append($"            List<string> tohide = hides.ToList();" + Environment.NewLine);
                    form.Append($"            //tohide.Add(\"column\");" + Environment.NewLine);
                    form.Append($"            //tohide.Remove(\"creationDate\");" + Environment.NewLine);
                    form.Append($"            string extraRelation = \"\";" + Environment.NewLine);
                    form.Append($"            if (!string.IsNullOrEmpty(from))" + Environment.NewLine);
                    form.Append($"            {{" + Environment.NewLine);
                    form.Append($"                string[] relation = from.Split(':');" + Environment.NewLine);
                    form.Append($"                table.AdditionalWhere = $\"{{relation[0]}}='{{relation[1]}}'\";" + Environment.NewLine);
                    form.Append($"                extraRelation = $\"?from={{from}}\";" + Environment.NewLine);
                    form.Append($"                tohide.Add(relation[0]);" + Environment.NewLine);
                    form.Append($"            }}" + Environment.NewLine);
                    form.Append($"            table.hideColumns = tohide.ToArray();" + Environment.NewLine);
                    foreach (DataRow item in columns.Rows)
                    {
                        dbProperties properties = new dbProperties(item);
                        string camelSepate = Helper.Capitalize(properties.Name.Replace("ID", ""));

                        if (string.IsNullOrEmpty(properties.AlterText))
                            form.AppendFormat("table.ReplacedText.Add(\"{0}\", (\"{1}\")); " + Environment.NewLine, properties.Name, camelSepate.Replace(" ", " "));
                        else
                        {
                            string[] fkeys = properties.Fk.Split(';');
                            form.AppendFormat("table.ReplacedText.Add(\"{0}\", (\"{1}\")); " + Environment.NewLine, properties.Name, Helper.Capitalize(properties.Name.Split('_')[1]).Replace(" ", " "), fkeys[1].Replace("Base", ""));
                        }
                    }
                    form.AppendFormat("	table.SearchCondition = \"");
                    GetSearchConditions(columns, form);
                    form.AppendFormat("\";" + Environment.NewLine);
                    form.AppendFormat("	table.VarName = \"cs{0}\" + from.Split(':')[0];" + Environment.NewLine, tableName);
                    form.AppendFormat("	table.ReferenceText = \"{0}\";" + Environment.NewLine, withoutPrefix);
                    if (!report)
                    {
                        form.AppendFormat("	table.Links.Add(new CsTableLink() {{ ButtonType = URLHelper.buttonType(\"{0}\"), Href = URLHelper.getActionUrlRelation(\"{0}\", \"Form\"), Title = \"Edit\", Css = \"btn btn-warning btn-circle refreshTable\", Icon = BaseUIIconText.fa.fa_fa_pencil, Extra = extraRelation }});" + Environment.NewLine, controller.Replace("Controller", ""));
                        form.Append($"table.Links.Add(new CsTableLink() {{ ButtonType = CsTableLinkType.functionButton, Href = URLHelper.getActionUrl(\"{controller.Replace("Controller", "")}\", \"ToggleActive\"), Title = \"Active/Inactive\", Css = \"btn btn-warning btn-circle refreshTable\", Icon = BaseUIIconText.fa.fa_fa_asterisk }});" + Environment.NewLine);
                        form.AppendFormat("	table.Links.Add(new CsTableLink() {{ ButtonType = CsTableLinkType.deleteButton, Href = URLHelper.getActionUrl(\"{0}\", \"Delete\"), Title = \"Delete\", Css = \"btn btn-danger btn-circle refreshTable\", Icon = BaseUIIconText.fa.fa_fa_trash_o }});" + Environment.NewLine, controller.Replace("Controller", ""));
                    }
                    form.Append($"table.buildDataTable(filter);" + Environment.NewLine);
                    form.AppendFormat("	foreach (DataRow item in table.Data.Rows) " + Environment.NewLine);
                    form.AppendFormat("{{ " + Environment.NewLine);
                    foreach (DataRow item in columns.Rows)
                    {
                        dbProperties properties = new dbProperties(item);


                        if (!string.IsNullOrEmpty(properties.AlterText))
                        {

                            string[] fkeys = properties.Fk.Split(';');
                            string pluraly = fkeys[1];
                            bool isGuidR = Helper.getData($"select * from INFORMATION_SCHEMA.COLUMNS where DATA_TYPE='uniqueidentifier' and COLUMN_NAME='{fkeys[2]}' and TABLE_NAME = '{properties.TableName}'", db).Rows.Count > 0;
                            pluraly = pluralize.Pluralize(pluraly);
                            form.AppendFormat("	if (item[\"{0}\"] != null) " + Environment.NewLine, properties.Name);
                            form.AppendFormat("{{ " + Environment.NewLine);
                            form.AppendFormat(" if (item[\"{0}\"].ToString() != \"\")" + Environment.NewLine, properties.Name);
                            form.AppendFormat(" {{" + Environment.NewLine);
                            if (properties.Type == "int")
                                form.AppendFormat("    int {0} = int.Parse(item[\"{0}\"].ToString()); " + Environment.NewLine, properties.Name);
                            else if (isGuidR)
                                form.AppendFormat("    Guid {0} = Guid.Parse(item[\"{0}\"].ToString()); " + Environment.NewLine, properties.Name);
                            else
                                form.AppendFormat("    string {0} = item[\"{0}\"].ToString(); " + Environment.NewLine, properties.Name);

                            form.AppendFormat("    item[\"{0}\"] = db.{1}.FirstOrDefault(d => d.{2} == {0}).{3}; " + Environment.NewLine, properties.Name, pluraly, fkeys[3], properties.AlterText);
                            form.AppendFormat(" }} " + Environment.NewLine);
                            form.AppendFormat("else" + Environment.NewLine);
                            form.AppendFormat("     item[\"{0}\"] = \"Not Set\"; " + Environment.NewLine, properties.Name);
                            form.AppendFormat("}} " + Environment.NewLine);
                        }
                        else if (properties.Type == "bit")
                        {
                            form.AppendFormat("	if (item[\"{0}\"] != null) " + Environment.NewLine, properties.Name);
                            form.AppendFormat("{{ " + Environment.NewLine);
                            form.AppendFormat("    item[\"{0}\"] = (item[\"{0}\"].ToString() == \"1\" || item[\"{0}\"].ToString() == \"True\") ? (\"Yes\") : (\"No\"); " + Environment.NewLine, properties.Name);
                            form.AppendFormat("}} " + Environment.NewLine);
                        }
                        else if (properties.Type == "money")
                        {
                            form.AppendFormat("	if (item[\"{0}\"] != null) " + Environment.NewLine, properties.Name);
                            form.AppendFormat("{{ " + Environment.NewLine);
                            form.Append($"item[\"{properties.Name}\"] = Helper.Money(item[\"{properties.Name}\"].ToString());" + Environment.NewLine);
                            form.AppendFormat("}} " + Environment.NewLine);
                        }
                        else if (properties.Name.ToLower().Contains("file") || properties.Name.ToLower().Contains("image"))
                        {
                            form.AppendFormat("	if (item[\"{0}\"] != null) " + Environment.NewLine, properties.Name);
                            form.AppendFormat("{{ " + Environment.NewLine);
                            form.AppendFormat("    item[\"{0}\"] = \"#file&&&\" + \"{0}<>\" + item[\"{0}\"].ToString() + \"<>{1}\";  " + Environment.NewLine, properties.Name, controller.Replace("Controller", ""));
                            form.AppendFormat("}} " + Environment.NewLine);
                        }
                        else if (properties.Name.ToLower().Contains("color"))
                        {
                            form.AppendFormat("	if (item[\"{0}\"] != null) " + Environment.NewLine, properties.Name);
                            form.AppendFormat("{{ " + Environment.NewLine);
                            string color = "string.Format(\"colorColumn={{0}}\", item[\"{0}\"].ToString());";
                            form.AppendFormat("    item[\"{0}\"] = " + color + "  " + Environment.NewLine, properties.Name, controller.Replace("Controller", ""));

                            form.AppendFormat("}} " + Environment.NewLine);
                        }
                        else if (properties.Name.ToLower().Contains("prototype") && properties.Type == "prototype")
                        {
                            form.AppendFormat("	if (item[\"{0}\"] != null) " + Environment.NewLine, properties.Name);
                            form.AppendFormat("{{ " + Environment.NewLine);
                            form.AppendFormat("    item[\"{0}\"] = \"\";  " + Environment.NewLine, properties.Name, controller.Replace("Controller", ""));
                            form.AppendFormat("}} " + Environment.NewLine);
                        }
                    }
                    form.AppendFormat("}} " + Environment.NewLine);
                    form.AppendFormat("	table.buildDefaultJson();" + Environment.NewLine);
                    if (!report)
                        form.AppendFormat("	table.buildLinks();" + Environment.NewLine);
                    form.AppendFormat("	return Json(table.Json, JsonRequestBehavior.AllowGet);" + Environment.NewLine);
                    form.AppendFormat("}}" + Environment.NewLine);
                    form.Append($"public FileResult getCSV(string from = \"\")" + Environment.NewLine);
                    form.AppendFormat("{{" + Environment.NewLine);

                    form.Append($"FormCollection form = new FormCollection();" + Environment.NewLine);
                    form.Append($"            foreach (string key in Request.QueryString.AllKeys)" + Environment.NewLine);
                    form.Append($"if (key != \"limit\" && key != \"offset\")" + Environment.NewLine);
                    form.Append($"                form.Add(key, Request.QueryString[key]);" + Environment.NewLine);
                    form.Append($"            var wb = new XLWorkbook();" + Environment.NewLine);

                    form.AppendFormat("	JsonCsTableHelper table = new JsonCsTableHelper(db, form, Request.QueryString);" + Environment.NewLine);
                    form.AppendFormat("	table.TableName = \"[{0}]\";" + Environment.NewLine, tableName);
                    if (!report)
                    {
                        form.AppendFormat("	table.PrimaryKey = \"{0}\";" + Environment.NewLine, primaryKey);
                    }
                    else
                        form.AppendFormat("	table.PrimaryKey = \"" + columns.Rows[0]["name"] + "\";" + Environment.NewLine);


                    form.Append($"            string[] hides = table.hideColumns;" + Environment.NewLine);
                    form.Append($"            List<string> tohide = hides.ToList();" + Environment.NewLine);
                    form.Append($"            //tohide.Add(\"column\");" + Environment.NewLine);
                    form.Append($"            //tohide.Remove(\"creationDate\");" + Environment.NewLine);

                    form.Append($"            if (!string.IsNullOrEmpty(from))" + Environment.NewLine);
                    form.Append($"            {{" + Environment.NewLine);
                    form.Append($"                string[] relation = from.Split(':');" + Environment.NewLine);
                    form.Append($"                table.AdditionalWhere = $\"{{relation[0]}}='{{relation[1]}}'\";" + Environment.NewLine);
                    form.Append($"                tohide.Add(relation[0]);" + Environment.NewLine);
                    form.Append($"            }}" + Environment.NewLine);


                    form.Append($"            table.hideColumns = tohide.ToArray();" + Environment.NewLine);


                    foreach (DataRow item in columns.Rows)
                    {
                        dbProperties properties = new dbProperties(item);
                        string camelSepate = Helper.Capitalize(properties.Name.Replace("ID", ""));

                        if (string.IsNullOrEmpty(properties.AlterText))
                            form.AppendFormat("table.ReplacedText.Add(\"{0}\", (\"{1}\")); " + Environment.NewLine, properties.Name, camelSepate.Replace(" ", " "));
                        else
                        {
                            string[] fkeys = properties.Fk.Split(';');
                            form.AppendFormat("table.ReplacedText.Add(\"{0}\", (\"{1}\")); " + Environment.NewLine, properties.Name, Helper.Capitalize(properties.Name.Replace("ID", "")).Replace(" ", " "), fkeys[1].Replace("Base", ""));
                        }
                    }
                    form.AppendFormat("	table.SearchCondition = \"");
                    GetSearchConditions(columns, form);
                    form.AppendFormat("\";" + Environment.NewLine);
                    form.AppendFormat("	table.VarName = \"cs{0}\";" + Environment.NewLine, tableName);
                    form.AppendFormat("	table.ReferenceText = \"{0}\";" + Environment.NewLine, withoutPrefix);
                    if (!report)
                    {
                        form.AppendFormat("	table.Links.Add(new CsTableLink() {{ ButtonType = CsTableLinkType.button, Href = URLHelper.getActionUrl(\"{0}\", \"Form\"), Title = \"Edit\", Css = \"btn btn-warning btn-circle refreshTable\", Icon = BaseUIIconText.fa.fa_fa_pencil }});" + Environment.NewLine, controller.Replace("Controller", ""));
                        form.AppendFormat("	table.Links.Add(new CsTableLink() {{ ButtonType = CsTableLinkType.deleteButton, Href = URLHelper.getActionUrl(\"{0}\", \"Delete\"), Title = \"Delete\", Css = \"btn btn-danger btn-circle refreshTable\", Icon = BaseUIIconText.fa.fa_fa_trash_o }});" + Environment.NewLine, controller.Replace("Controller", ""));
                    }
                    form.Append($"if (Request.QueryString.AllKeys.Contains(\"filter\"))" + Environment.NewLine);
                    form.Append($"                table.buildDataTable(Request.QueryString[\"filter\"]);" + Environment.NewLine);
                    form.Append($"            else" + Environment.NewLine);
                    form.Append($"                table.buildDataTable(\"\");" + Environment.NewLine);

                    form.AppendFormat("	foreach (DataRow item in table.Data.Rows) " + Environment.NewLine);
                    form.AppendFormat("{{ " + Environment.NewLine);
                    int a = 0;
                    foreach (DataRow item in columns.Rows)
                    {
                        dbProperties properties = new dbProperties(item);
                        if (!string.IsNullOrEmpty(properties.AlterText))
                        {

                            string[] fkeys = properties.Fk.Split(';');
                            string pluraly = fkeys[1];
                            bool isGuidR = Helper.getData($"select * from INFORMATION_SCHEMA.COLUMNS where DATA_TYPE='uniqueidentifier' and COLUMN_NAME='{fkeys[2]}' and TABLE_NAME = '{properties.TableName}'", db).Rows.Count > 0;
                            pluraly = pluralize.Pluralize(pluraly);
                            form.AppendFormat("	if (item[\"{0}\"] != null) " + Environment.NewLine, properties.Name);
                            form.AppendFormat("{{ " + Environment.NewLine);
                            form.AppendFormat(" if (item[\"{0}\"].ToString() != \"\")" + Environment.NewLine, properties.Name);
                            form.AppendFormat(" {{" + Environment.NewLine);
                            if (properties.Type == "int")
                                form.AppendFormat("    int {0} = int.Parse(item[\"{0}\"].ToString()); " + Environment.NewLine, properties.Name);
                            else if (isGuidR)
                                form.AppendFormat("    Guid {0} = Guid.Parse(item[\"{0}\"].ToString()); " + Environment.NewLine, properties.Name);
                            else
                                form.AppendFormat("    string {0} = item[\"{0}\"].ToString(); " + Environment.NewLine, properties.Name);
                            form.AppendFormat("    item[\"{0}\"] = db.{1}.FirstOrDefault(d => d.{2} == {0}).{3}; " + Environment.NewLine, properties.Name, pluraly, fkeys[3], properties.AlterText);
                            form.AppendFormat(" }} " + Environment.NewLine);
                            form.AppendFormat("else" + Environment.NewLine);
                            form.AppendFormat("     item[\"{0}\"] = \"Not Set\"; " + Environment.NewLine, properties.Name);
                            form.AppendFormat("}} " + Environment.NewLine);
                        }
                        else if (properties.Type == "bit")
                        {
                            form.AppendFormat("	if (item[\"{0}\"] != null) " + Environment.NewLine, properties.Name);
                            form.AppendFormat("{{ " + Environment.NewLine);
                            form.AppendFormat("    item[\"{0}\"] = (item[\"{0}\"].ToString() == \"1\" || item[\"{0}\"].ToString() == \"True\") ? (\"Yes\") : (\"No\"); " + Environment.NewLine, properties.Name);
                            form.AppendFormat("}} " + Environment.NewLine);
                        }
                        else if (properties.Type == "money")
                        {
                            form.AppendFormat("	if (item[\"{0}\"] != null) " + Environment.NewLine, properties.Name);
                            form.AppendFormat("{{ " + Environment.NewLine);
                            form.Append($"item[\"{properties.Name}\"] = Helper.Money(item[\"{properties.Name}\"].ToString());" + Environment.NewLine);
                            form.AppendFormat("}} " + Environment.NewLine);
                        }
                        else if (properties.Name.ToLower().Contains("file") || properties.Name.ToLower().Contains("image"))
                        {
                            form.AppendFormat("	if (item[\"{0}\"] != null) " + Environment.NewLine, properties.Name);
                            form.AppendFormat("{{ " + Environment.NewLine);
                            form.AppendFormat("    item[\"{0}\"] = \"#file&&&\" + \"{0}<>\" + item[\"{0}\"].ToString() + \"<>{1}\";  " + Environment.NewLine, properties.Name, controller.Replace("Controller", ""));
                            form.AppendFormat("}} " + Environment.NewLine);
                        }
                        else if (properties.Name.ToLower().Contains("color"))
                        {
                            form.AppendFormat("	if (item[\"{0}\"] != null) " + Environment.NewLine, properties.Name);
                            form.AppendFormat("{{ " + Environment.NewLine);
                            string color = "string.Format(\"colorColumn={{0}}\", item[\"{0}\"].ToString());";
                            form.AppendFormat("    item[\"{0}\"] = " + color + "  " + Environment.NewLine, properties.Name, controller.Replace("Controller", ""));

                            form.AppendFormat("}} " + Environment.NewLine);
                        }
                        else if (properties.Name.ToLower().Contains("prototype") && properties.Type == "prototype")
                        {
                            form.AppendFormat("	if (item[\"{0}\"] != null) " + Environment.NewLine, properties.Name);
                            form.AppendFormat("{{ " + Environment.NewLine);
                            form.AppendFormat("    item[\"{0}\"] = \"\";  " + Environment.NewLine, properties.Name, controller.Replace("Controller", ""));
                            form.AppendFormat("}} " + Environment.NewLine);
                        }
                    }
                    form.AppendFormat("}} " + Environment.NewLine);

                    form.AppendFormat("	wb.Worksheets.Add(table.Data, \"{0}\");" + Environment.NewLine, Helper.Capitalize(tableName, 2));
                    form.AppendFormat("	using (var memoryStream = new MemoryStream())" + Environment.NewLine);
                    form.AppendFormat("	{{" + Environment.NewLine);
                    form.AppendFormat("		wb.SaveAs(memoryStream);" + Environment.NewLine);
                    form.AppendFormat("		byte[] byteInfo = memoryStream.ToArray();" + Environment.NewLine);
                    form.AppendFormat("		return File(byteInfo, System.Net.Mime.MediaTypeNames.Application.Octet, String.Format(\"{0} {1} {{0}}.xlsx\", DateTime.Now.ToShortDateString()));" + Environment.NewLine, Helper.Capitalize(tableName, 2), new BaseConfiguration().appName);
                    form.AppendFormat("	}}" + Environment.NewLine);
                    form.AppendFormat("}}" + Environment.NewLine);



                    form.Append($"[HttpGet]" + Environment.NewLine);
                    form.Append($"        [AllowAnonymous]" + Environment.NewLine);
                    form.Append($"        public JsonResult getAll(FormCollection form, string columns = \"['*']\", string @operator = \"AND\")" + Environment.NewLine);
                    form.Append($"        {{" + Environment.NewLine);
                    form.Append($"            List<string> where = new List<string>();" + Environment.NewLine);

                    form.Append($"            foreach (string item in Request.QueryString) if (!form.AllKeys.Contains(item)) form.Add(item, Request.QueryString[item]);" + Environment.NewLine);

                    form.Append($"            if (form.Count > 0)" + Environment.NewLine);
                    form.Append($"                foreach (string item in form.AllKeys)" + Environment.NewLine);
                    form.Append($"                    where.Add($\"{{item}}='{{form[item]}}'\");" + Environment.NewLine);
                    form.Append($"            List<string> sel = JsonConvert.DeserializeObject<List<string>>(columns);" + Environment.NewLine);
                    form.Append($"            string whereCondition = where.Count > 0 ? \" where \" + string.Join($\" {{@operator}} \", where) : \"\";" + Environment.NewLine);
                    form.Append($"            DataTable data = Helper.getData($\"SELECT [{{string.Join(\"],[\", sel)}}] FROM [{tableName}] {{whereCondition}} \".Replace(\"[*]\", \"*\"), db);" + Environment.NewLine);
                    form.Append($"            List<string> ncolumns = new List<string>();" + Environment.NewLine);
                    form.Append($"            foreach (DataColumn item in data.Columns)" + Environment.NewLine);
                    form.Append($"                ncolumns.Add(item.ColumnName);" + Environment.NewLine);
                    foreach (DataRow item in relations.Rows)
                    {
                        string pluralName = item["child"].ToString().Substring(prefix, item["child"].ToString().Length - prefix);
                        string pluralItem = pluralize.Pluralize(pluralName);
                        form.Append($"                ncolumns.Add(\"{pluralItem}\");" + Environment.NewLine);
                    }
                    form.Append($"            List<object> rows = new List<object>();" + Environment.NewLine);
                    form.Append($"            foreach (DataRow item in data.Rows)" + Environment.NewLine);
                    form.Append($"            {{" + Environment.NewLine);
                    form.Append($"                List<object> adds = item.ItemArray.ToList();" + Environment.NewLine);
                    foreach (DataRow item in relations.Rows)
                    {
                        string pluralName = item["child"].ToString();
                        string friend = item["fk"].ToString();
                        form.Append($"adds.Add(Helper.Regular(db, item, \"{pluralName}\", $\" where [{friend}] = '@id@'\"));" + Environment.NewLine);
                    }
                    form.Append($"                rows.Add(adds.ToArray());" + Environment.NewLine);
                    form.Append($"            }}" + Environment.NewLine);
                    form.Append($"            return Json(new {{ columns = ncolumns, rows = rows, count = data.Rows.Count }}, JsonRequestBehavior.AllowGet);" + Environment.NewLine);
                    form.Append($"        }}" + Environment.NewLine);
                    form.Append($"        [HttpGet]" + Environment.NewLine);
                    form.Append($"        [AllowAnonymous]" + Environment.NewLine);
                    if (isGuid)
                        form.Append($"        public JsonResult get(string id)" + Environment.NewLine);
                    else if (isChar)
                        form.Append($"        public JsonResult get(string id)" + Environment.NewLine);
                    else
                        form.Append($"        public JsonResult get(int id)" + Environment.NewLine);

                    form.Append($"        {{" + Environment.NewLine);
                    form.Append($"            {tableName} model = db.{plural}.Find(id);" + Environment.NewLine);
                    form.Append($"            if (model != null)" + Environment.NewLine);
                    form.Append($"                return Json(model, JsonRequestBehavior.AllowGet);" + Environment.NewLine);
                    form.Append($"            else" + Environment.NewLine);
                    form.Append($"                return Json(new {{ Message = \"This record no longer exists\" }}, JsonRequestBehavior.AllowGet);" + Environment.NewLine);
                    form.Append($"        }}" + Environment.NewLine);
                    if (!report)
                    {
                        form.Append($"        [HttpPut]" + Environment.NewLine);
                        form.Append($"        [AllowAnonymous]" + Environment.NewLine);
                        form.Append($"        public JsonResult create({tableName} model)" + Environment.NewLine);
                        form.Append($"        {{" + Environment.NewLine);
                        form.Append($"            BoolString validation = model.BeforeSave(db);" + Environment.NewLine);
                        form.Append($"            if (validation.BoolValue)" + Environment.NewLine);
                        form.Append($"                return Json(new {{ Message = validation.StringValue }});" + Environment.NewLine);
                        form.Append($"            db.{plural}.Add(model);" + Environment.NewLine);
                        form.Append($"            db.SaveChanges();" + Environment.NewLine);
                        form.Append($"            validation = model.AfterSave(db);" + Environment.NewLine);
                        form.Append($"            if (validation.BoolValue)" + Environment.NewLine);
                        form.Append($"                return Json(new {{ Message = validation.StringValue }});" + Environment.NewLine);
                        form.Append($"            return Json(new {{ id = model.{primaryKey}, MessageSucess = \"That { Helper.Capitalize(tableName, 2)} saved successfully.\" }});" + Environment.NewLine);
                        form.Append($"        }}" + Environment.NewLine);
                        form.Append($"        [HttpPost]" + Environment.NewLine);
                        form.Append($"        [AllowAnonymous]" + Environment.NewLine);
                        form.Append($"        public JsonResult update(int id, {tableName} model)" + Environment.NewLine);
                        form.Append($"        {{" + Environment.NewLine);
                        form.Append($"            BoolString validation = model.BeforeEdit(db);" + Environment.NewLine);
                        form.Append($"            if (validation.BoolValue)" + Environment.NewLine);
                        form.Append($"                return Json(new {{ Message = validation.StringValue }});" + Environment.NewLine);
                        form.Append($"            db.Entry(model).State = System.Data.Entity.EntityState.Modified;" + Environment.NewLine);
                        form.Append($"            db.SaveChanges();" + Environment.NewLine);
                        form.Append($"            validation = model.AfterEdit(db);" + Environment.NewLine);
                        form.Append($"            if (validation.BoolValue)" + Environment.NewLine);
                        form.Append($"                return Json(new {{ Message = validation.StringValue }});" + Environment.NewLine);
                        form.Append($"            return Json(new {{ id = model.{primaryKey}, MessageSucess = \"That {Helper.Capitalize(tableName, 2)} saved successfully.\" }});" + Environment.NewLine);
                        form.Append($"        }}" + Environment.NewLine);
                        form.Append($"        [HttpDelete]" + Environment.NewLine);
                        form.Append($"        [AllowAnonymous]" + Environment.NewLine);
                        form.Append($"        public JsonResult remove(int id = 0)" + Environment.NewLine);
                        form.Append($"        {{" + Environment.NewLine);
                        form.Append($"            try" + Environment.NewLine);
                        form.Append($"            {{" + Environment.NewLine);
                        form.Append($"                {tableName} model = db.{plural}.Find(id);" + Environment.NewLine);
                        form.Append($"                if (model != null)" + Environment.NewLine);
                        form.Append($"                {{" + Environment.NewLine);
                        form.Append($"                    BoolString validation = model.BeforeDelete(db);" + Environment.NewLine);
                        form.Append($"                    if (validation.BoolValue)" + Environment.NewLine);
                        form.Append($"                        return Json(new {{ Message = validation.StringValue }}, JsonRequestBehavior.AllowGet);" + Environment.NewLine);
                        form.Append($"                    db.{plural}.Remove(model);" + Environment.NewLine);
                        form.Append($"                    db.SaveChanges();" + Environment.NewLine);
                        form.Append($"                    validation = model.AfterDelete(db);" + Environment.NewLine);
                        form.Append($"                    if (validation.BoolValue)" + Environment.NewLine);
                        form.Append($"                        return Json(new {{ Message = validation.StringValue }}, JsonRequestBehavior.AllowGet);" + Environment.NewLine);
                        form.Append($"                    return Json(new {{ id = model.{primaryKey}, MessageSucess = \"That {Helper.Capitalize(tableName, 2)} deleted successfully.\" }}, JsonRequestBehavior.AllowGet);" + Environment.NewLine);
                        form.Append($"                }}" + Environment.NewLine);
                        form.Append($"                return Json(new {{ Message = \"This record no longer exists\" }}, JsonRequestBehavior.AllowGet);" + Environment.NewLine);
                        form.Append($"            }}" + Environment.NewLine);
                        form.Append($"            catch (Exception ex)" + Environment.NewLine);
                        form.Append($"            {{" + Environment.NewLine);
                        form.Append($"                return Json(new {{ Message = Helper.ModeralException(ex).Replace(\"@table\", \"{Helper.Capitalize(tableName, 2)}\") }}, JsonRequestBehavior.AllowGet);" + Environment.NewLine);
                        form.Append($"            }}" + Environment.NewLine);
                        form.Append($"        }}" + Environment.NewLine);

                    }


                    form.AppendFormat("	}}" + Environment.NewLine);
                    form.AppendFormat("}}" + Environment.NewLine);



                    string path = System.Web.HttpContext.Current.Server.MapPath(string.Format("~/Controllers/{0}.cs", controller));
                    if (isController)
                    {
                        FileStream myFile = System.IO.File.Create(path);
                        myFile.Close();
                        StreamWriter write = new StreamWriter(path);
                        write.Write(form.ToString());
                        write.Close();
                    }
                    form = new StringBuilder();

                    //Index
                    form.AppendFormat("@model Admin.Models.{0}" + Environment.NewLine, tableName);
                    form.AppendFormat("@using Admin.BaseClass.UI" + Environment.NewLine);
                    form.AppendFormat("@using Admin.Models;" + Environment.NewLine);

                    form.AppendFormat("     @{{bool allowExport = false; bool saveConfig = false; string cookieName = \"\" ; int id = 0; bool enableControl = true;}}" + Environment.NewLine);
                    form.AppendFormat("         @foreach (var item in (List<BaseDynamicList>) ViewBag.dynamicList)" + Environment.NewLine);
                    form.AppendFormat("         {{" + Environment.NewLine);
                    form.AppendFormat("             if (item.name == ViewBag.tableName)" + Environment.NewLine);
                    form.AppendFormat("             {{" + Environment.NewLine);
                    form.AppendFormat("                 allowExport = item.allowExport;" + Environment.NewLine);
                    form.AppendFormat("                 cookieName = item.cookieKey;" + Environment.NewLine);
                    form.AppendFormat("                 id = item.id;" + Environment.NewLine);
                    form.AppendFormat("                 saveConfig = item.allowSaveConfig;" + Environment.NewLine);
                    form.AppendFormat("                 enableControl = item.enableControl;" + Environment.NewLine);
                    form.AppendFormat("                 break;" + Environment.NewLine);
                    form.AppendFormat("             }}" + Environment.NewLine);
                    form.AppendFormat("         }}" + Environment.NewLine);


                    form.Append($"<div>" + Environment.NewLine);
                    form.Append($"    <div class=\"col-md-6 pl0 \">" + Environment.NewLine);
                    form.Append($"        <h2 class=\"tile_seccion\">{Helper.Capitalize(tableName, 2)} List</h2>" + Environment.NewLine);
                    form.Append($"    </div>" + Environment.NewLine);
                    form.Append($"    <div class=\"col-md-6 pr0\" style='text-align:right'>" + Environment.NewLine);

                    form.AppendFormat("<input id=\"refi\" type=\"hidden\" value=\"{0}\"/>" + Environment.NewLine, withoutPrefix);
                    if (!report)
                        form.AppendFormat("<button class=\"btn-no-styles baseDynamicModal\" data-loading-text=\"<i class='fa fa-refresh fa-spin'></i>\"  data-action=\"@Url.Action(\"Form\", \"{0}\", new {{ from = ViewBag.from }})\" title=\"@(\"Add New\")\"><i class=\"fa fa-plus-circle\"></i></button>" + Environment.NewLine, controller.Replace("Controller", ""));
                    form.AppendFormat("<button class=\"btn-no-styles refreshTable\" onclick=\"refreshMe{1}@(ViewBag.from.Split(':')[0])();\" data-action=\"@Url.Action(\"Form\", \"{0}\")\" title=\"@(\"Refresh Table\")\"><i class=\"fa fa-refresh\"></i></button>" + Environment.NewLine, controller.Replace("Controller", ""), tableName);
                    form.Append($"<button id=\"btnFilter@(ViewBag.from.Split(':')[0])\" class=\"btn-no-styles baseDynamicModal\" data-loading-text=\"<i class='fa fa-refresh fa-spin'></i>\" data-action=\"@Url.Action(\"Filters\", \"BaseDeveloper\", new {{ tableName=\"{tableName}\", from=ViewBag.from   }})\" title=\"@(\"Filter\")\" ><i class=\"fa fa-filter\" ></i></button>" + Environment.NewLine);
                    form.Append($"<button class=\"btn-no-styles\" id=\"configAdmColumns\" title=\"@(\"Columns\")\" ><i class=\"fa fa-list\" ></i></button>" + Environment.NewLine);
                    form.AppendFormat("@if (allowExport)" + Environment.NewLine);
                    form.AppendFormat("{{" + Environment.NewLine);
                    form.AppendFormat("      <button class=\"btn btn-@(ViewBag.config.secundaryColor)\" onclick=\"exportCSV{0}();\" title=\"@(\"Export Excel\")\"><i class=\"@BaseUIIconText.glyphicon.glyphicon_glyphicon_export\"></i></button>" + Environment.NewLine, tableName);
                    form.AppendFormat("}}" + Environment.NewLine);

                    form.AppendFormat("</div>" + Environment.NewLine);

                    form.Append($"    <div id=\"SearchForm\" action=\"#\" class=\"smart-form\" style=\"width: 100%;\">" + Environment.NewLine);
                    form.Append($"        <fieldset style=\"padding: 0 0 0 0 !important;\">" + Environment.NewLine);
                    form.Append($"            <div class=\"row\">" + Environment.NewLine);
                    form.Append($"                @Html.BaseTextBox(\"table_generalSearch\" + (string)(ViewBag.from.Split(':')[0]), \"\", \"General Search\", new {{ icon1 = new BaseTextBoxIcon() {{ Icon = Admin.BaseClass.UI.BaseUIIcon.fa.fa_fa_search, Position = BaseTextBoxIconPosition.icon_prepend }} }}, 10, new {{ placeholder = (Helper.Capitalize(\"Search texts\")) }}, \"\", \"\", false, new MyHtmlHelpers.BaseToolTip(\"This value looks for the match between all fields\"))" + Environment.NewLine);
                    form.Append($"                @Html.BaseSpiner(\"table_generalLimit\" + (string)(ViewBag.from.Split(':')[0]), 20, \"Limit Per Page\", 2, MyHtmlHelpers.spinnerDirection.both, \"\", \"number\", 1, 2147483647, 1, 1, \"n\")" + Environment.NewLine);
                    form.Append($"            </div>" + Environment.NewLine);
                    form.Append($"        </fieldset>" + Environment.NewLine);
                    form.Append($"    </div>" + Environment.NewLine);
                    form.AppendFormat("</div>");

                    form.Append($"<div class=\"panel\" style=\"border:none\">" + Environment.NewLine);
                    form.Append($"    <div class=\"panel-body pl0 pr0\">" + Environment.NewLine);
                    form.AppendFormat("        <div id=\"cs{2}Div@(ViewBag.from.Split(':')[0])\" data-url=\"@Url.Action(\"data\", \"{0}\", new {{ from = ViewBag.from }})\" class=\"\"></div>" + Environment.NewLine, controller.Replace("Controller", ""), "", tableName);
                    form.Append($"    </div>" + Environment.NewLine);
                    form.Append($"</div>      " + Environment.NewLine);



                    form.AppendFormat("" + Environment.NewLine);
                    form.AppendFormat("<script type=\"text/javascript\">" + Environment.NewLine);
                    form.AppendFormat("     pageSetUp();" + Environment.NewLine);
                    form.AppendFormat("     var questionSaveConfig{0} = \"@saveConfig\";" + Environment.NewLine, tableName);
                    form.AppendFormat("     if (questionSaveConfig{0} == \"False\") {{" + Environment.NewLine, tableName);
                    form.AppendFormat("         var ck = \"@cookieName\";" + Environment.NewLine);
                    form.AppendFormat("         delete_cookie(ck);" + Environment.NewLine);
                    form.AppendFormat("     }}" + Environment.NewLine);
                    form.AppendFormat("" + Environment.NewLine);
                    form.AppendFormat("    function refreshMe{0}@(ViewBag.from.Split(':')[0])() {{" + Environment.NewLine, tableName);
                    form.AppendFormat("        cs{0}@(ViewBag.from.Split(':')[0]).data('CStable').update();" + Environment.NewLine, tableName);
                    form.AppendFormat("    }}" + Environment.NewLine);
                    form.Append($"    $(\"#table_generalSearch@(ViewBag.from.Split(':')[0])\").keypress(function (e) {{" + Environment.NewLine);
                    form.Append($"        if (e.which == 13) {{" + Environment.NewLine);
                    form.Append($"            generalFilterSearch@(ViewBag.from.Split(':')[0]) = $(this).val();" + Environment.NewLine);
                    form.Append($"            LoadTable{tableName}@(ViewBag.from.Split(':')[0])();" + Environment.NewLine);
                    form.Append($"        }}" + Environment.NewLine);
                    form.Append($"    }});" + Environment.NewLine);
                    form.Append($"    $(\"#table_generalLimit@(ViewBag.from.Split(':')[0])\").keypress(function (e) {{" + Environment.NewLine);
                    form.Append($"        if (e.which == 13) {{" + Environment.NewLine);
                    form.Append($"            generalFilterlimit@(ViewBag.from.Split(':')[0]) = $(this).val();" + Environment.NewLine);
                    form.Append($"            LoadTable{tableName}@(ViewBag.from.Split(':')[0])();" + Environment.NewLine);
                    form.Append($"        }}" + Environment.NewLine);
                    form.Append($"    }});" + Environment.NewLine);
                    form.AppendFormat("" + Environment.NewLine);
                    form.AppendFormat("     var clOff{0} = [" + Environment.NewLine, tableName);
                    form.AppendFormat("     @foreach (var item in (List<BaseDynamicColumnList>) ViewBag.column)" + Environment.NewLine);
                    form.AppendFormat("     {{" + Environment.NewLine);
                    form.AppendFormat("         if(item.listID == id && !item.show)" + Environment.NewLine);
                    form.AppendFormat("         {{" + Environment.NewLine);
                    string quote = "\\";
                    form.AppendFormat("             string quote = \"{0}\"\";" + Environment.NewLine, quote);
                    form.AppendFormat("             string x = quote + item.name + quote + \", \";" + Environment.NewLine);
                    form.AppendFormat("             @Html.Raw(x) " + Environment.NewLine);
                    form.AppendFormat("         }}" + Environment.NewLine);
                    form.AppendFormat("     }}" + Environment.NewLine);
                    form.AppendFormat("     ];" + Environment.NewLine);
                    form.Append($"    selectedFilters@(ViewBag.from.Split(':')[0]) = undefined;" + Environment.NewLine);
                    form.Append($"    filterQuery@(ViewBag.from.Split(':')[0]) = \"\";" + Environment.NewLine);
                    form.Append($"    filterQueryURL@(ViewBag.from.Split(':')[0]) = \"\";" + Environment.NewLine);
                    form.Append($"    generalFilterSearch@(ViewBag.from.Split(':')[0]) = \"\";" + Environment.NewLine);
                    form.Append($"    generalFilterlimit@(ViewBag.from.Split(':')[0]) = 20;" + Environment.NewLine);
                    form.Append($"function LoadTable{tableName}@(ViewBag.from.Split(':')[0])() {{" + Environment.NewLine);
                    form.AppendFormat("    cs{0}@(ViewBag.from.Split(':')[0]) = $('#cs{0}Div@(ViewBag.from.Split(':')[0])');" + Environment.NewLine, tableName);
                    form.AppendFormat("    cs{0}@(ViewBag.from.Split(':')[0]).CStable({{" + Environment.NewLine, tableName);
                    form.AppendFormat("        ajaxUrl: cs{0}@(ViewBag.from.Split(':')[0]).data('url')," + Environment.NewLine, tableName);
                    form.AppendFormat("        pagerFirstLabel: '@(\"First\")'," + Environment.NewLine);
                    form.AppendFormat("        pagerLastLabel: '@(\"Last\")'," + Environment.NewLine);
                    form.AppendFormat("        ajaxData: {{" + Environment.NewLine);
                    form.AppendFormat("            limit: generalFilterlimit@(ViewBag.from.Split(':')[0])," + Environment.NewLine);
                    form.AppendFormat("            offset: 0," + Environment.NewLine);
                    form.Append($"cstable_search: generalFilterSearch@(ViewBag.from.Split(':')[0])" + Environment.NewLine);
                    form.AppendFormat("        }}," + Environment.NewLine);
                    form.AppendFormat("        columnasDesactivadas: clOff{0}," + Environment.NewLine, tableName);
                    form.AppendFormat("        defaultColumnOff: true," + Environment.NewLine);
                    form.AppendFormat("        current: 1," + Environment.NewLine);
                    form.AppendFormat("        pagerCount: 10," + Environment.NewLine);
                    form.Append($"            hasSearch: false," + Environment.NewLine);
                    form.Append($"            hasShow : false," + Environment.NewLine);
                    form.AppendFormat("    }});" + Environment.NewLine);
                    form.AppendFormat("    }}" + Environment.NewLine);
                    if (!report)
                        form.Append($"LoadTable{tableName}@(ViewBag.from.Split(':')[0])();" + Environment.NewLine);
                    else
                        form.Append($"$(\"#btnFilter@(ViewBag.from.Split(':')[0])\").click();" + Environment.NewLine);
                    form.AppendFormat("     function exportCSV{0}() {{" + Environment.NewLine, tableName);
                    form.AppendFormat("         var cadenaGet = Filters{0}(); //Almacena todos los Filters" + Environment.NewLine, tableName);
                    form.AppendFormat("         $.ajax({{" + Environment.NewLine);
                    form.AppendFormat("             url: \"@Url.Action(\"getCSV\", \"{0}\")\"," + Environment.NewLine, controller.Replace("Controller", ""));
                    form.AppendFormat("             data: \"from = @ViewBag.from&\" + cadenaGet + \"colActivas=\" + columnasActivasReporte{0}() + \"&tableName=@ViewBag.tableName&\" + filterQueryURL@(ViewBag.from.Split(':')[0])," + Environment.NewLine, tableName);
                    form.AppendFormat("             success: function (data) {{" + Environment.NewLine);
                    form.AppendFormat("                 window.location.href = this.url;" + Environment.NewLine);
                    form.AppendFormat("             }}" + Environment.NewLine);
                    form.AppendFormat("         }});" + Environment.NewLine);
                    form.AppendFormat("     }}" + Environment.NewLine);
                    form.AppendFormat("     //Funcion que retorna un string con los Filters aplicados, en formato GET" + Environment.NewLine);
                    form.AppendFormat("     function Filters{0}() {{" + Environment.NewLine, tableName);
                    form.AppendFormat("          var get = \"\";" + Environment.NewLine);
                    form.AppendFormat("          var array = $.map(cs{0}@(ViewBag.from.Split(':')[0]).data('CStable').options.ajaxData, function (value, index) {{" + Environment.NewLine, tableName);
                    form.AppendFormat("          return index;" + Environment.NewLine);
                    form.AppendFormat("          }});" + Environment.NewLine);
                    form.AppendFormat("          var array2 = $.map(cs{0}@(ViewBag.from.Split(':')[0]).data('CStable').options.ajaxData, function (value, index) {{" + Environment.NewLine, tableName);
                    form.AppendFormat("          return value;" + Environment.NewLine);
                    form.AppendFormat("          }});" + Environment.NewLine);
                    form.AppendFormat("          for (var i = 0; i < array.length; i++) {{" + Environment.NewLine);
                    form.AppendFormat("             get += array[i] + \"=\" + array2[i] + \"&\";" + Environment.NewLine);
                    form.AppendFormat("          }}" + Environment.NewLine);
                    form.AppendFormat("          return get;" + Environment.NewLine);
                    form.AppendFormat("     }}" + Environment.NewLine);
                    form.AppendFormat("     //Funcion que retorna un String con las columnas activas, separadas por \"-\"" + Environment.NewLine);
                    form.AppendFormat("      function columnasActivasReporte{0}() {{" + Environment.NewLine, tableName);
                    form.AppendFormat("          var ac = eval(unescape(leerCockie(\"@cookieName\")));" + Environment.NewLine);
                    form.AppendFormat("          var colActivas = \"\";" + Environment.NewLine);
                    form.AppendFormat("          if (ac != undefined) {{" + Environment.NewLine);
                    form.AppendFormat("              for (var i = 0; i < ac.length; i++) {{" + Environment.NewLine);
                    form.AppendFormat("                  if (ac[i].c) {{" + Environment.NewLine);
                    form.Append($"if (columns_cstable_cookie_header_cs{tableName}Div[ac[i].v] != undefined)" + Environment.NewLine);
                    form.AppendFormat("                      colActivas += columns_cstable_cookie_header_cs{0}Div[ac[i].v].n + \"-\";" + Environment.NewLine, tableName);
                    form.AppendFormat("                  }}" + Environment.NewLine);
                    form.AppendFormat("              }}" + Environment.NewLine);
                    form.AppendFormat("              colActivas = colActivas.substring(0, colActivas.length - 1);" + Environment.NewLine);
                    form.AppendFormat("         }}" + Environment.NewLine);
                    form.AppendFormat("         else{{" + Environment.NewLine);
                    form.AppendFormat("              for (var i = 0; i < columns_cstable_cookie_header_cs{0}Div.length; i++) {{" + Environment.NewLine, tableName);
                    form.Append($"if (columns_cstable_cookie_header_cs{tableName}Div[ac[i].v] != undefined)" + Environment.NewLine);
                    form.AppendFormat("                   colActivas += columns_cstable_cookie_header_cs{0}Div[i].n + \"-\";" + Environment.NewLine, tableName);
                    form.AppendFormat("              }}" + Environment.NewLine);
                    form.AppendFormat("              colActivas = colActivas.substring(0, colActivas.length - 1);" + Environment.NewLine);
                    form.AppendFormat("         }}" + Environment.NewLine);
                    form.AppendFormat("         return colActivas;" + Environment.NewLine);
                    form.AppendFormat("     }}" + Environment.NewLine);
                    form.AppendFormat("     //Si no existe la cookie" + Environment.NewLine);

                    form.AppendFormat("function makeHide@(ViewBag.tableName)() {{ if (!existCookie(\"@cookieName\")) {{ $(\"#updatechecks\").trigger(\"click\"); }} }}" + Environment.NewLine);
                    form.AppendFormat("     if (!existCookie(\"@cookieName\")) {{" + Environment.NewLine);
                    form.AppendFormat("         $(\"#updatechecks\").trigger(\"click\");" + Environment.NewLine);
                    form.AppendFormat("     }}" + Environment.NewLine);
                    form.AppendFormat("     //Funcion que retorna el contenido de la cookie con el nombre que le pasan" + Environment.NewLine);
                    form.AppendFormat("     function leerCockie(cname) {{" + Environment.NewLine);
                    form.AppendFormat("         var name = cname + \"=\";" + Environment.NewLine);
                    form.AppendFormat("         var ca = document.cookie.split(';');" + Environment.NewLine);
                    form.AppendFormat("             for (var i = 0; i < ca.length; i++) {{" + Environment.NewLine);
                    form.AppendFormat("                  var c = ca[i];" + Environment.NewLine);
                    form.AppendFormat("                  while (c.charAt(0) == ' ') c = c.substring(1);" + Environment.NewLine);
                    form.AppendFormat("                  if (c.indexOf(name) == 0) return c.substring(name.length, c.length);" + Environment.NewLine);
                    form.AppendFormat("             }}" + Environment.NewLine);
                    form.AppendFormat("         return \"\";" + Environment.NewLine);
                    form.AppendFormat("     }}" + Environment.NewLine);
                    form.AppendFormat("     //Delete cookie" + Environment.NewLine);
                    form.AppendFormat("     function delete_cookie(name) {{" + Environment.NewLine);
                    form.AppendFormat("         document.cookie = name + '=; expires=Thu, 01 Jan 1970 00:00:01 GMT;';" + Environment.NewLine);
                    form.AppendFormat("     }}" + Environment.NewLine);
                    form.AppendFormat("     function existCookie(cname) {{" + Environment.NewLine);
                    form.AppendFormat("         var name = cname + \"=\";" + Environment.NewLine);
                    form.AppendFormat("         var ca = document.cookie.split(';');" + Environment.NewLine);
                    form.AppendFormat("             for (var i = 0; i < ca.length; i++) {{" + Environment.NewLine);
                    form.AppendFormat("                  var c = ca[i];" + Environment.NewLine);
                    form.AppendFormat("                  while (c.charAt(0) == ' ') c = c.substring(1);" + Environment.NewLine);
                    form.AppendFormat("                  if (c.indexOf(name) == 0) return true;" + Environment.NewLine);
                    form.AppendFormat("             }}" + Environment.NewLine);
                    form.AppendFormat("         return false;" + Environment.NewLine);
                    form.AppendFormat("     }}" + Environment.NewLine);
                    form.AppendFormat("     function reset{0}(){{" + Environment.NewLine, tableName);
                    form.AppendFormat("         var ck = \"@cookieName\";" + Environment.NewLine);
                    form.AppendFormat("         delete_cookie(ck);" + Environment.NewLine);
                    form.AppendFormat("         location.reload();" + Environment.NewLine);
                    form.AppendFormat("     }}" + Environment.NewLine);
                    form.AppendFormat("</script>" + Environment.NewLine);
                    form.AppendFormat("" + Environment.NewLine);


                    string folder = System.Web.HttpContext.Current.Server.MapPath(string.Format("~/Views/{0}", controller.Replace("Controller", "")));
                    if (isIndex)
                    {
                        Directory.CreateDirectory(folder);
                        path = System.Web.HttpContext.Current.Server.MapPath(string.Format("~/Views/{0}/Index.cshtml", controller.Replace("Controller", "")));
                        FileStream myFile = System.IO.File.Create(path);
                        myFile.Close();
                        StreamWriter write = new StreamWriter(path);
                        write.Write(form.ToString());
                        write.Close();
                    }
                    form = new StringBuilder();

                    form.Append($"using Admin.CustomCode;" + Environment.NewLine);
                    form.Append($"using Admin.ModelsExtra;" + Environment.NewLine);
                    form.Append($"using System;" + Environment.NewLine);
                    form.Append($"using System.Collections.Generic;" + Environment.NewLine);
                    form.Append($"using System.Configuration;" + Environment.NewLine);
                    form.Append($"using System.Linq;" + Environment.NewLine);
                    form.Append($"using System.Web;" + Environment.NewLine);
                    form.Append($"using WebMatrix.WebData;" + Environment.NewLine);
                    form.Append($"namespace Admin.Models" + Environment.NewLine);
                    form.Append($"{{" + Environment.NewLine);
                    form.Append($"    public partial class {tableName} : IScopeble" + Environment.NewLine);
                    form.Append($"    {{" + Environment.NewLine);
                    form.Append($"        " + Environment.NewLine);
                    form.Append($"        public BoolString BeforeSave(Context db)" + Environment.NewLine);
                    form.Append($"        {{" + Environment.NewLine);
                    form.Append($"            List<string> messages = new List<string>();" + Environment.NewLine);
                    form.Append($"            return ScopeHelper.RegulateMessages(messages);" + Environment.NewLine);
                    form.Append($"        }}" + Environment.NewLine);
                    form.Append($"        public BoolString BeforeEdit(Context db)" + Environment.NewLine);
                    form.Append($"        {{" + Environment.NewLine);
                    form.Append($"            List<string> messages = new List<string>();" + Environment.NewLine);
                    form.Append($"            return ScopeHelper.RegulateMessages(messages);" + Environment.NewLine);
                    form.Append($"        }}" + Environment.NewLine);
                    form.Append($"        public BoolString BeforeCreate(Context db)" + Environment.NewLine);
                    form.Append($"        {{" + Environment.NewLine);
                    form.Append($"        active=true;" + Environment.NewLine);
                    form.Append($"            List<string> messages = new List<string>();" + Environment.NewLine);
                    form.Append($"            return ScopeHelper.RegulateMessages(messages);" + Environment.NewLine);
                    form.Append($"        }}" + Environment.NewLine);
                    form.Append($"        public BoolString BeforeDelete(Context db)" + Environment.NewLine);
                    form.Append($"        {{" + Environment.NewLine);
                    form.Append($"            List<string> messages = new List<string>();" + Environment.NewLine);
                    form.Append($"            return ScopeHelper.RegulateMessages(messages);" + Environment.NewLine);
                    form.Append($"        }}" + Environment.NewLine);

                    form.Append($"        public BoolString BeforeActive(Context db)" + Environment.NewLine);
                    form.Append($"        {{" + Environment.NewLine);
                    form.Append($"            List<string> messages = new List<string>();" + Environment.NewLine);
                    form.Append($"            return ScopeHelper.RegulateMessages(messages);" + Environment.NewLine);
                    form.Append($"        }}" + Environment.NewLine);
                    form.Append($"        public BoolString BeforeInactive(Context db)" + Environment.NewLine);
                    form.Append($"        {{" + Environment.NewLine);
                    form.Append($"            List<string> messages = new List<string>();" + Environment.NewLine);
                    form.Append($"            return ScopeHelper.RegulateMessages(messages);" + Environment.NewLine);
                    form.Append($"        }}" + Environment.NewLine);

                    form.Append($"        public BoolString AfterSave(Context db)" + Environment.NewLine);
                    form.Append($"        {{" + Environment.NewLine);
                    form.Append($"            List<string> messages = new List<string>();" + Environment.NewLine);
                    form.Append($"            return ScopeHelper.RegulateMessages(messages);" + Environment.NewLine);
                    form.Append($"        }}" + Environment.NewLine);
                    form.Append($"        public BoolString AfterEdit(Context db)" + Environment.NewLine);
                    form.Append($"        {{" + Environment.NewLine);
                    if (!report)
                        form.Append($"        Helper.Audit(db, \"{tableName}\", AuditMode.Edit, {primaryKey},this);" + Environment.NewLine);
                    form.Append($"            List<string> messages = new List<string>();" + Environment.NewLine);
                    form.Append($"            return ScopeHelper.RegulateMessages(messages);" + Environment.NewLine);
                    form.Append($"        }}" + Environment.NewLine);
                    form.Append($"        public BoolString AfterCreate(Context db)" + Environment.NewLine);
                    form.Append($"        {{" + Environment.NewLine);
                    if (!report)
                        form.Append($"        Helper.Audit(db, \"{tableName}\", AuditMode.Create, {primaryKey},this);" + Environment.NewLine);
                    form.Append($"            List<string> messages = new List<string>();" + Environment.NewLine);
                    form.Append($"            return ScopeHelper.RegulateMessages(messages);" + Environment.NewLine);
                    form.Append($"        }}" + Environment.NewLine);
                    form.Append($"        public BoolString AfterDelete(Context db)" + Environment.NewLine);
                    form.Append($"        {{" + Environment.NewLine);
                    if (!report)
                        form.Append($"        Helper.Audit(db, \"{tableName}\", AuditMode.Delete, {primaryKey},this);" + Environment.NewLine);
                    form.Append($"            List<string> messages = new List<string>();" + Environment.NewLine);
                    form.Append($"            return ScopeHelper.RegulateMessages(messages);" + Environment.NewLine);
                    form.Append($"        }}" + Environment.NewLine);

                    form.Append($"        public BoolString AfterActive(Context db)" + Environment.NewLine);
                    form.Append($"        {{" + Environment.NewLine);
                    if (!report)
                        form.Append($"        Helper.Audit(db, \"{tableName}\", AuditMode.Active, {primaryKey},this);" + Environment.NewLine);
                    form.Append($"            List<string> messages = new List<string>();" + Environment.NewLine);
                    form.Append($"            return ScopeHelper.RegulateMessages(messages);" + Environment.NewLine);
                    form.Append($"        }}" + Environment.NewLine);
                    form.Append($"        public BoolString AfterInactive(Context db)" + Environment.NewLine);
                    form.Append($"        {{" + Environment.NewLine);
                    if (!report)
                        form.Append($"        Helper.Audit(db, \"{tableName}\", AuditMode.Inactive, {primaryKey},this);" + Environment.NewLine);
                    form.Append($"            List<string> messages = new List<string>();" + Environment.NewLine);
                    form.Append($"            return ScopeHelper.RegulateMessages(messages);" + Environment.NewLine);
                    form.Append($"        }}" + Environment.NewLine);

                    form.Append($"    }}" + Environment.NewLine);
                    form.Append($"}}" + Environment.NewLine);

                    if (isModelExtra)
                    {
                        path = System.Web.HttpContext.Current.Server.MapPath($"~/ModelsExtra/{tableName}.cs");
                        FileStream myFile = System.IO.File.Create(path);
                        myFile.Close();
                        StreamWriter write = new StreamWriter(path);
                        write.Write(form.ToString());
                        write.Close();
                    }
                    form = new StringBuilder();
                    //Form
                    form.AppendFormat("@model Admin.Models.{0}" + Environment.NewLine, tableName);
                    form.AppendFormat("@using Admin.BaseClass.UI" + Environment.NewLine);
                    form.AppendFormat("@using Admin.Models;" + Environment.NewLine);

                    string width = "";
                    if (columns.Rows.Count < 8)
                    {

                    }
                    else
                        if (columns.Rows.Count > 8 && columns.Rows.Count < 10)
                        width = "data-width=\"60%\"";
                    else
                        width = "data-width=\"80%\"";
                    if (isGuid)
                        form.AppendFormat("<i " + width + " class=\"dn\" id=\"modalInfo{0}\" data-modalimg=\"@Helper.GetApplicationImage(new Admin.Models.Context())\" data-modalTitle=\"@(\"Create {0}\")\"  data-editModalTitle=\"@(\"Edit {0}\")\" data-editing=\"@(Model.{2}==new Guid()?\"False\":\"True\")\" ></i>" + Environment.NewLine, tableName.Substring(prefix, tableName.Length - prefix), tableName, primaryKey);

                    else if (isChar)
                        form.AppendFormat("<i " + width + " class=\"dn\" id=\"modalInfo{0}\" data-modalimg=\"@Helper.GetApplicationImage(new Admin.Models.Context())\" data-modalTitle=\"@(\"Create {0}\")\"  data-editModalTitle=\"@(\"Edit {0}\")\" data-editing=\"@(Model.{2}==null?\"False\":\"True\")\" ></i>" + Environment.NewLine, tableName.Substring(prefix, tableName.Length - prefix), tableName, primaryKey);
                    else
                        form.AppendFormat("<i " + width + " class=\"dn\" id=\"modalInfo{0}\" data-modalimg=\"@Helper.GetApplicationImage(new Admin.Models.Context())\" data-modalTitle=\"@(\"Create {0}\")\"  data-editModalTitle=\"@(\"Edit {0}\")\" data-editing=\"@(Model.{2}==0?\"False\":\"True\")\" ></i>" + Environment.NewLine, tableName.Substring(prefix, tableName.Length - prefix), tableName, primaryKey);
                    form.AppendFormat("<div class=\"panel panel-primary\">" + Environment.NewLine);
                    form.AppendFormat("    <div class=\"panel-heading\">" + Environment.NewLine);
                    form.AppendFormat("        <button class=\"close\" type=\"button\" data-dismiss=\"modal\" aria-hidden=\"true\"> X </button>" + Environment.NewLine);
                    form.AppendFormat("        <h3 class=\"panel-title\">{0}{1}</h3>" + Environment.NewLine, withoutPrefix, "");
                    form.AppendFormat("    </div>" + Environment.NewLine);
                    form.AppendFormat("    <div class=\"panel-body\">" + Environment.NewLine);
                    form.AppendFormat("<form action=\"@Url.Action(\"Save\", \"{0}\")\" id=\"{1}-form\" class=\"smart-form ajaxForm\" data-successMessage=\"@(\"Saved\")\" data-errorMessage=\"@(\"Error writing data try later.\")\"  >" + Environment.NewLine, controller.Replace("Controller", ""), tableName);
                    GetFields(tableName, columns, formSplit, form, report, "", hiddenplus);
                    form.AppendFormat("<footer>" + Environment.NewLine);

                    form.Append($"@if (!string.IsNullOrEmpty(ViewBag.from))" + Environment.NewLine);
                    form.Append($"                {{" + Environment.NewLine);
                    form.Append($"                    string[] relations = ViewBag.from.Split(':');" + Environment.NewLine);
                    form.Append($"                    <a class=\"btn btn-danger formClose\" title=\"@(\"Close\")\" href=\"#{tableName.Substring(0, 2)}@(relations[0].Split('_')[0])/Form/@(relations[1])\"><i class=\"glyphicon glyphicon-floppy-remove\"></i></a>" + Environment.NewLine);
                    form.Append($"                }}" + Environment.NewLine);
                    if (isGuid)
                        form.Append($"else if (ViewBag.relations.Count > 0 && Model.{primaryKey} != new Guid())" + Environment.NewLine);
                    else if (isChar)
                        form.Append($"else if (ViewBag.relations.Count > 0 && Model.{primaryKey} != null)" + Environment.NewLine);
                    else
                        form.Append($"else if (ViewBag.relations.Count > 0 && Model.{primaryKey} != 0)" + Environment.NewLine);
                    form.Append($"                {{" + Environment.NewLine);
                    form.Append($"                    <a class=\"btn btn-danger formClose fromBack\" title=\"@(\"Close\")\" href=\"#{tableName}/Index\"><i class=\"glyphicon glyphicon-floppy-remove\"></i></a>" + Environment.NewLine);
                    form.Append($"                }}" + Environment.NewLine);


                    form.AppendFormat("<button class=\"btn btn-success\" data-success=\"$('#baseDynamicModalObject').modal('hide');cs{0}@(ViewBag.from.Split(':')[0]).data('CStable').update();\" title=\"@(\"Save\")\" ><i class=\"glyphicon glyphicon-floppy-disk\"></i></button>" + Environment.NewLine, tableName);
                    form.AppendFormat("</footer>" + Environment.NewLine);
                    form.AppendFormat("</form>" + Environment.NewLine);
                    form.AppendFormat("    </div>" + Environment.NewLine);
                    form.AppendFormat("</div>" + Environment.NewLine);

                    if (isGuid)
                        form.Append($"@if (ViewBag.relations.Count > 0 && Model.{primaryKey} != new Guid())" + Environment.NewLine);
                    else if (isChar)
                        form.Append($"@if (ViewBag.relations.Count > 0 && Model.{primaryKey} != null)" + Environment.NewLine);
                    else
                        form.Append($"@if (ViewBag.relations.Count > 0 && Model.{primaryKey} != 0)" + Environment.NewLine);
                    form.Append($"{{" + Environment.NewLine);
                    form.Append($"    bool first = true;" + Environment.NewLine);
                    form.Append($"    <div class=\"panel panel-primary\">" + Environment.NewLine);
                    form.Append($"        <ul class=\"nav nav-tabs\">" + Environment.NewLine);
                    form.Append($"            @foreach (var item in (List<VWISRElation>)ViewBag.relations)" + Environment.NewLine);
                    form.Append($"                {{" + Environment.NewLine);
                    form.Append($"                <li class=\"@(first ? \"active\" : \"\")\"><a href=\"#@(item.K_Table + \"_\" + item.FK_Column)\" data-toggle=\"tab\" aria-expanded=\"@(first ? \"true\" : \"false\")\">@Helper.Capitalize(item.K_Table,2)'s @Helper.Capitalize(item.FK_Column.Split('_')[1])</a></li>" + Environment.NewLine);
                    form.Append($"                first = false;" + Environment.NewLine);
                    form.Append($"            }}" + Environment.NewLine);
                    form.Append($"        </ul>" + Environment.NewLine);
                    form.Append($"        <div class=\"panel-body\">" + Environment.NewLine);
                    form.Append($"            <div id=\"myTabContent\" class=\"tab-content\">" + Environment.NewLine);
                    form.Append($"                @{{" + Environment.NewLine);
                    form.Append($"                    first = true;" + Environment.NewLine);
                    form.Append($"                }}" + Environment.NewLine);
                    form.Append($"                @foreach (var item in (List<VWISRElation>)ViewBag.relations)" + Environment.NewLine);
                    form.Append($"                {{" + Environment.NewLine);
                    form.Append($"                    <div class=\"tab-pane fade @(first ? \"active in\" : \"\")\" id=\"@(item.K_Table + \"_\" + item.FK_Column)\">" + Environment.NewLine);
                    form.Append($"                        Loading..." + Environment.NewLine);
                    form.Append($"                    </div>" + Environment.NewLine);
                    form.Append($"                    <script>" + Environment.NewLine);
                    form.Append($"                        loadContent(\"@Url.Action(\"index\", item.K_Table, new {{ from = item.FK_Column+ \":\"+Model.{primaryKey} }})\", \"#@(item.K_Table + \"_\" + item.FK_Column)\");" + Environment.NewLine);
                    form.Append($"                    </script>" + Environment.NewLine);
                    form.Append($"first = false;" + Environment.NewLine);
                    form.Append($"                }}" + Environment.NewLine);
                    form.Append($"            </div>" + Environment.NewLine);
                    form.Append($"        </div>" + Environment.NewLine);
                    form.Append($"    </div>" + Environment.NewLine);
                    form.Append($"                    }}" + Environment.NewLine);


                    form.AppendFormat("<script>" + Environment.NewLine);
                    form.AppendFormat("function next() {{" + Environment.NewLine);
                    form.AppendFormat("        location.href = \"#{0}/Form/\" + BaseLastID;" + Environment.NewLine, controller.Replace("Controller", ""));
                    form.AppendFormat("}}" + Environment.NewLine);
                    form.AppendFormat("    pageSetUp(true);" + Environment.NewLine);
                    form.AppendFormat("    $(document).ready(function () {{" + Environment.NewLine);
                    form.AppendFormat("        validateForm(\"{0}-form\");" + Environment.NewLine, tableName);
                    form.Append($"var from = \"@ViewBag.from\";" + Environment.NewLine);
                    form.Append($"        HideRelation(\"{tableName}-form\", from);" + Environment.NewLine);
                    form.AppendFormat("    }});" + Environment.NewLine);
                    form.AppendFormat("</script>");
                    if (buildColumns) { databaseColumns(tableName); }
                    if (!report)
                    {
                        if (isForm)
                        {
                            Directory.CreateDirectory(folder);
                            path = System.Web.HttpContext.Current.Server.MapPath(string.Format("~/Views/{0}/Form.cshtml", controller.Replace("Controller", "")));
                            FileStream myFile = System.IO.File.Create(path);
                            //myFile.SetAccessControl(oFileSecurity);
                            myFile.Close();
                            StreamWriter write = new StreamWriter(path);
                            write.Write(form.ToString());
                            write.Close();

                        }
                    }
                    form = new StringBuilder();

                    Models.BaseController control = db.BaseControllers.FirstOrDefault(d => d.name == tableName);
                    if (control != null)
                    {
                        control.infoName = Helper.Capitalize(tableName, 2);
                    }
                    else
                    {
                        string controlllerSin = controller.Replace("Controller", "");
                        control = new Models.BaseController() { name = controlllerSin, fullName = controller, infoName = Helper.Capitalize(controlllerSin, prefix) };
                        control.BaseActions.Add(new Models.BaseAction() { name = "Index", displayName = ("Home"), isSystem = false });
                        if (!report)
                            control.BaseActions.Add(new Models.BaseAction() { name = "Form", displayName = ("Form"), isSystem = false });
                        if (!report)
                            control.BaseActions.Add(new Models.BaseAction() { name = "Save", displayName = ("Save Item"), isSystem = false });
                        if (!report)
                            control.BaseActions.Add(new Models.BaseAction() { name = "Delete", displayName = ("Delete Item"), isSystem = false });
                        control.BaseActions.Add(new Models.BaseAction() { name = "data", displayName = ("Grid Data"), isSystem = false });
                        control.BaseActions.Add(new Models.BaseAction() { name = "getCSV", displayName = ("Export"), isSystem = false });
                        db.BaseControllers.Add(control);
                        db.SaveChanges();
                        int actionID = int.Parse(Helper.getData("SELECT ID FROM BASEACTION WHERE NAME = 'Index' AND controllerID=" + control.id, db).Rows[0][0].ToString());
                        int? menuPadreID = null;
                        if (!string.IsNullOrEmpty(parentMenu))
                        {
                            BaseMenu padre = db.BaseMenus.FirstOrDefault(d => d.text == parentMenu);
                            if (padre != null)
                                menuPadreID = padre.id;
                            else
                            {
                                BaseMenu menuP = new BaseMenu()
                                {
                                    title = parentMenu,
                                    text = parentMenu,
                                    hidden = false,
                                    translatable = false,
                                    directLink = false,
                                    noOrder = 0,
                                };
                                db.BaseMenus.Add(menuP);
                                db.SaveChanges();
                                menuPadreID = menuP.id;
                            }
                        }
                        BaseMenu menu = new BaseMenu()
                        {
                            menuID = menuPadreID,
                            actionID = actionID,
                            title = Helper.Capitalize(tableName, prefix),
                            text = Helper.Capitalize(tableName, prefix),
                            hidden = false,
                            translatable = false,
                            directLink = false,
                            noOrder = 0,
                        };
                        db.BaseMenus.Add(menu);
                        db.SaveChanges();
                    }
                    messages.AppendLine(string.Format("The CRUD {0} was successfully generated", tableName));
                }
                catch (Exception ex)
                {
                    messages.AppendLine(ex.Message);
                }
            }
            return Json(new { message = messages.ToString() });
        }

        private void databaseColumns(string tableName)
        {
            BaseDynamicList bd = db.BaseDynamicLists.FirstOrDefault(v => v.name == tableName);
            if (bd == null)
            {
                BaseDynamicList bDynamic = new BaseDynamicList();
                bDynamic.name = tableName;
                bDynamic.nameToDisplay = tableName;
                bDynamic.cookieKey = "cstable_cookie_header_cs" + tableName + "Div";
                bDynamic.allowExport = false;
                bDynamic.allowSaveConfig = true;
                bDynamic.enableControl = true;
                db.BaseDynamicLists.Add(bDynamic);
                db.SaveChanges();
                int index = 1;
                DataTable columns = Helper.getData("select * from VWBaseNiiAll where tableName='" + tableName + "' and type!='sysname' order by column_id", db);
                foreach (DataRow item in columns.Rows)
                {
                    BaseDynamicColumnList b = new BaseDynamicColumnList();
                    b.listID = bDynamic.id;
                    b.name = item["name"].ToString();
                    b.show = true;
                    b.orderNum = index;
                    db.BaseDynamicColumnLists.Add(b);
                    db.SaveChanges();
                    index++;
                }
            }
        }

        public class HawFilters
        {
            public string tableName = "", from = "";
            public List<HAWColumns> columns = new List<HAWColumns>();

        }
        public class HAWColumns
        {
            public string COLUMN_NAME, DATA_TYPE, NUMERIC_PRECISION, NUMERIC_SCALE, TABLE_NAME, RELATION_TYPE, RELATION, LABEL;
            public bool IS_NULLABLE;
            public int CHARACTER_MAXIMUM_LENGTH;

            public string TABLE_FROM
            {
                get { return !string.IsNullOrEmpty(RELATION) ? RELATION.Split(';')[0] : ""; }
            }
            public string FK_COLUM
            {
                get { return !string.IsNullOrEmpty(RELATION) ? RELATION.Split(';')[1] : ""; }
            }
            public string PK_COLUM
            {
                get { return !string.IsNullOrEmpty(RELATION) ? RELATION.Split(';')[2] : ""; }
            }

            public string PK_COLUM_NAME
            {
                get
                {
                    DataTable data = Helper.getData($"select top 1 COLUMN_NAME from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME='{TABLE_TO}' and DATA_TYPE like '%varchar%'", new Context());
                    if (data.Rows.Count > 0)
                        return data.Rows[0][0].ToString();
                    else
                        return PK_COLUM;
                }
            }

            public string TABLE_TO
            {
                get { return !string.IsNullOrEmpty(RELATION) ? RELATION.Split(';')[3] : ""; }
            }
        }

        public HawFilters getForm(string tableName, string noallow = "", string from = "")
        {
            ViewBag.from = from;
            if (!string.IsNullOrEmpty(from))
                noallow += "(" + from.Split(':')[0] + ")";
            DataTable table = Helper.getData(string.Format("select * from VWHAWKColumn where TABLE_NAME='{0}'", tableName), db);
            HawFilters filters = new HawFilters();
            filters.from = from;
            filters.tableName = tableName;
            //DataTable addTable = Helper.getData(string.Format("select * from BaseViewRelation where tableFrom='{0}'", tableName), db);
            List<BaseViewRelation> addTable = db.BaseViewRelations.Where(d => d.tableFrom == tableName).ToList();
            foreach (BaseViewRelation item in addTable)
            {
                if (item.hide.HasValue)
                    if (item.hide.Value)
                        continue;
                if (string.IsNullOrEmpty(item.pkColumn))
                    continue;

                filters.columns.Add(new HAWColumns()
                {
                    CHARACTER_MAXIMUM_LENGTH = 0,
                    COLUMN_NAME = item.fkColumn,
                    TABLE_NAME = item.tableFrom,
                    DATA_TYPE = "int",
                    RELATION_TYPE = "dynamic",
                    RELATION = item.tableTo,
                    NUMERIC_PRECISION = item.pkColumn,
                    NUMERIC_SCALE = item.pkColumnName,
                    LABEL = item.customLabel ?? item.fkColumn
                });
            }
            foreach (DataRow item in table.Rows)
            {
                int max = 0;
                int.TryParse(item["CHARACTER_MAXIMUM_LENGTH"].ToString(), out max);
                if (!noallow.ToLower().Contains("(" + item["COLUMN_NAME"].ToString().ToLower() + ")"))
                {
                    BaseViewRelation hide = addTable.Where(d => d.hide.HasValue).FirstOrDefault(d => d.fkColumn == item["COLUMN_NAME"].ToString() && d.hide.Value);
                    if (hide != null)
                        continue;

                    if (filters.columns.Count(d => d.COLUMN_NAME == item["COLUMN_NAME"].ToString()) == 0)
                    {
                        BaseViewRelation basev = addTable.FirstOrDefault(d => d.fkColumn == item["COLUMN_NAME"].ToString() && !string.IsNullOrEmpty(d.customLabel));
                        string label = item["COLUMN_NAME"].ToString();
                        if (basev != null)
                            if (!string.IsNullOrEmpty(basev.customLabel))
                                label = basev.customLabel;

                        filters.columns.Add(new HAWColumns()
                        {
                            CHARACTER_MAXIMUM_LENGTH = max,
                            COLUMN_NAME = item["COLUMN_NAME"].ToString(),
                            TABLE_NAME = item["TABLE_NAME"].ToString(),
                            DATA_TYPE = item["DATA_TYPE"].ToString(),
                            RELATION_TYPE = item["RELATION_TYPE"].ToString(),
                            RELATION = item["RELATION"].ToString(),
                            NUMERIC_PRECISION = item["NUMERIC_PRECISION"].ToString(),
                            NUMERIC_SCALE = item["NUMERIC_SCALE"].ToString(),
                            IS_NULLABLE = (item["NUMERIC_SCALE"].ToString() == "YES" ? true : false),
                            LABEL = label
                        });
                    }
                }
            }
            return filters;
        }

        [IsView]
        public ActionResult Filters(string tableName, string noallow = "", string from = "")
        {
            ViewBag.from = from;
            return PartialView(getForm(tableName, noallow, from));
        }
        [IsView]
        public ActionResult SQL()
        {
            QueryMode q = new QueryMode("SELECT * FROM BASECONTROLLER");
            return PartialView(q);
        }

        [HttpPost]
        public ActionResult Result(string sql)
        {
            DataTable table = Helper.getData(sql, db);
            return PartialView(table);
        }
    }
}
