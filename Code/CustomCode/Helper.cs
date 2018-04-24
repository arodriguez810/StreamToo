using Admin.BaseClass.Format;
using Admin.BaseModels.ViewModels;
using Admin.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using WebMatrix.WebData;

namespace Admin.CustomCode
{
    public class BoolString
    {
        bool boolValue;
        public bool BoolValue
        {
            get { return boolValue; }
            set { boolValue = value; }
        }
        string stringValue;
        public string StringValue
        {
            get { return stringValue; }
            set { stringValue = value; }
        }
    }

    enum EncryptStrenght
    {
        poor = 10,
        regular = 20,
        good = 30,
        verygood = 50,
        ultra = 60,
        mega = 80,
        optimize = 100
    }

    public enum AuditMode
    {
        Create = 1,
        Edit = 2,
        Active = 3,
        Inactive = 4,
        Delete = 5
    }
    public class Helper
    {
        

        static public void Audit(Context db, string tableName, AuditMode action, int id, object model)
        {
            BaseDynamicList list = db.BaseDynamicLists.FirstOrDefault(d => d.name == tableName);
            if (list != null)
            {
                list.BaseLogs.Add(new BaseLog()
                {
                    created = DateTime.Now,
                    baseLogAction_action = (int)action,
                    baseUser_user = WebSecurity.CurrentUserId,
                    date = DateTime.Now,
                    entityId = id,
                    version = JsonConvert.SerializeObject(model, 
                    new JsonSerializerSettings() { Formatting = Formatting.None, ReferenceLoopHandling = ReferenceLoopHandling.Ignore })

                });
                db.SaveChanges();
            }
        }

        static public void Audit(Context db, string tableName, AuditMode action, Guid id, object model)
        {
            BaseDynamicList list = db.BaseDynamicLists.FirstOrDefault(d => d.name == tableName);
            if (list != null)
            {
                list.BaseLogs.Add(new BaseLog()
                {
                    created = DateTime.Now,
                    baseLogAction_action = (int)action,
                    baseUser_user = WebSecurity.CurrentUserId,
                    date = DateTime.Now,
                    entityIdU = id,
                    version = JsonConvert.SerializeObject(model,
                    new JsonSerializerSettings() { Formatting = Formatting.None, ReferenceLoopHandling = ReferenceLoopHandling.Ignore })
                });
                db.SaveChanges();
            }
        }

        static public void Audit(Context db, string tableName, AuditMode action, string id, object model)
        {
            BaseDynamicList list = db.BaseDynamicLists.FirstOrDefault(d => d.name == tableName);
            if (list != null)
            {
                list.BaseLogs.Add(new BaseLog()
                {
                    created = DateTime.Now,
                    baseLogAction_action = (int)action,
                    baseUser_user = WebSecurity.CurrentUserId,
                    date = DateTime.Now,
                    entityIdS = id,
                    version = JsonConvert.SerializeObject(model,
                    new JsonSerializerSettings() { Formatting = Formatting.None, ReferenceLoopHandling = ReferenceLoopHandling.Ignore })
                });
                db.SaveChanges();
            }
        }
        static public bool IsDigitsOnly(string str)
        {
            foreach (char c in str)
            {
                if (c < '0' || c > '9')
                    return false;
            }

            return true;
        }

        static public bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        public static object Regular(DataTable model)
        {
            List<string> ncolumns = new List<string>();
            foreach (DataColumn item in model.Columns)
                ncolumns.Add(item.ColumnName);
            List<object> rows = new List<object>();
            foreach (DataRow item in model.Rows)
            {
                rows.Add(item.ItemArray);
            }
            Dictionary<string, string> add = new Dictionary<string, string>();
            return new { columns = ncolumns, rows = rows, count = model.Rows.Count };
        }

        public static object Regular(Context db, DataRow row, string table, string where = "", string orderby = "", string schema = "dbo")
        {
            foreach (DataColumn item in row.Table.Columns)
                where = where.Replace($"@{item.ColumnName}@", row[item].ToString());

            DataTable model = Helper.getData($"SELECT * FROM {schema}.[{table}] {where} {orderby}", db);
            List<string> ncolumns = new List<string>();
            foreach (DataColumn item in model.Columns)
                ncolumns.Add(item.ColumnName);
            List<object> rows = new List<object>();
            foreach (DataRow item in model.Rows)
            {
                rows.Add(item.ItemArray);
            }
            Dictionary<string, string> add = new Dictionary<string, string>();
            return new { columns = ncolumns, rows = rows, count = model.Rows.Count };
        }

        public static object Regular(Context db, string table, string where = "", string orderby = "", string schema = "dbo")
        {
            DataTable model = Helper.getData($"SELECT * FROM {schema}.[{table}] {where} {orderby}", db);
            List<string> ncolumns = new List<string>();
            foreach (DataColumn item in model.Columns)
                ncolumns.Add(item.ColumnName);
            List<object> rows = new List<object>();
            foreach (DataRow item in model.Rows)
            {
                rows.Add(item.ItemArray);
            }
            Dictionary<string, string> add = new Dictionary<string, string>();
            return new { columns = ncolumns, rows = rows, count = model.Rows.Count };
        }

        static EncryptStrenght strenght = EncryptStrenght.regular;
        public static string Encrypt(string word, int id)
        {
            EISEncrypt sm = new EISEncrypt();
            if (!word.Contains("[sparklock]"))
                return sm.EncryptToString($"{word}&&&***{id}") + "[sparklock]";
            else
                return word;
        }
        public static string Decrypt(string word, int id)
        {
            EISEncrypt sm = new EISEncrypt();
            if (word.Contains("[sparklock]"))
                return sm.DecryptString(word.Replace("[sparklock]", "")).Replace($"&&&***{id}", "");
            else
                return word;
        }

        public static string ImageForm(int recordID, string path = "", string url = "")
        {
            if (File.Exists(Path.Combine(path, $"files/forms/{recordID}.png")))
            {
                if (url.Contains("no"))
                    return $"<img style=\"width: 400px;\" src=\"files/forms/{recordID}.png?{Guid.NewGuid()}\" />";
                else
                    return $"<img style=\"width: 400px;\" src=\"../../files/forms/{recordID}.png?{Guid.NewGuid()}\" />";
            }
            else
            {
                return "";
            }
        }

        public static string ReplaceDictionary(string toreplace, Dictionary<string, string> list)
        {
            string result = toreplace;
            foreach (var item in list)
                result = result.Replace(item.Key, item.Value);
            return result;
        }
        static public string Capitalize(string word, int index = 0, bool separateMayus = true)
        {
            if (word.StartsWith("Base"))
            {
                word = word.Replace("Base", "");
                index = 0;
            }
            word = word.Substring(index, word.Length - index);
            List<string> letters = new List<string>();
            for (int i = 65; i <= 90; i++)
                letters.Add(((char)i).ToString());
            string firstPart = word[0].ToString().ToUpper();
            string secondPart = word.Substring(1, word.Length - 1);
            string truSecond = "";
            if (separateMayus)
                for (int i = 0; i < secondPart.Length; i++)
                {
                    Char before = i > 0 ? secondPart[i - 1] : '~';
                    Char after = i < secondPart.Length - 1 ? secondPart[i + 1] : '~';
                    Char current = secondPart[i];
                    if ((Char.IsUpper(current) && !Char.IsUpper(before)) || (Char.IsUpper(current) && Char.IsLower(after)))
                        truSecond += " " + current;
                    else
                        truSecond += current;
                }
            return firstPart + truSecond;
        }
        static public string InnerException(Exception exception)
        {
            Exception newException = exception;
            while (newException.InnerException != null)
            {
                newException = newException.InnerException;
            }
            return newException.Message;
        }
        static public string ModeralException(string ex)
        {
            string messageFinal = ex;
            if (ex.Contains("Cannot insert duplicate key row in object"))
            {
                //messageFinal = "Cannot insert duplicate key row in object " + SplitString("'. ", messageFinal)[1].Split('.')[0];
                messageFinal = "Cannot insert duplicate key";
            }
            else
                if (ex.Contains("DELETE statement conflicted with the REFERENCE constraint"))
            {
                messageFinal = SplitString(", table \"", messageFinal)[1];
                messageFinal = SplitString("\", column ", messageFinal)[0].Replace("dbo.", "");

            }
            return messageFinal;
        }
        static public string ModeralException(Exception exception)
        {
            string ex = InnerException(exception);
            string messageFinal = ex;
            if (ex.Contains("Cannot insert duplicate key row in object"))
            {
                //messageFinal = "Failed to save @table, " + SplitString("'. ", messageFinal)[1].Split('.')[0];
                messageFinal = "This @table already exists.";
            }
            else
                if (ex.Contains("DELETE statement conflicted with the REFERENCE constraint"))
            {
                messageFinal = SplitString(", table \"", messageFinal)[1];
                string[] division = SplitString("\", column '", messageFinal);
                messageFinal = "This record is being used in field \"" + division[1].Replace("The statement has been terminated.", "").TrimEnd() + "\"";
                messageFinal = messageFinal.Replace("'.", "") + " of " + Helper.Capitalize(division[0].Replace("dbo.", ""), 2);
            }
            return messageFinal;
        }
        static public string[] SplitString(string separator, string str)
        {
            return str.Split(new string[] { separator }, StringSplitOptions.RemoveEmptyEntries);
        }
        public static string currentPassword = "";
        public static string currentAction = "";
        public static string currentController = "";
        public static DateTime? currentExpired = null;

        static public void resetCurrents()
        {
            currentPassword = "";
            currentExpired = null;
        }

        public static string Money(decimal value)
        {
            try
            {
                return "$" + value.ToString("N", System.Globalization.CultureInfo.GetCultureInfo("en-US"));
            }
            catch (Exception)
            {
                return "$0.00";
            }
        }

        public static string Money(string value)
        {
            try
            {
                decimal parsed = decimal.Parse(value);
                return "$" + parsed.ToString("N", System.Globalization.CultureInfo.GetCultureInfo("en-US"));
            }
            catch (Exception)
            {
                return "Not Set";
            }
        }

        //public static Dictionary<string, object> dataTableToDictionary(DataTable dt)
        //{
        //    return dt.AsEnumerable().ToDictionary<DataRow, string, object>(row => row.Field<string>(0), row => row.Field<object>(1));
        //}

        public static string firstUpperCase(string word)
        {
            return char.ToUpper(word[0]) + word.Substring(1);
        }

        public static string firstUpperCaseAndTs(string word)
        {
            return (char.ToUpper(word[0]) + word.Substring(1));
        }

        public static BaseUser GetUser(Context db = null)
        {
            Context dbss = new Context();
            //DataTable userInfo = Helper.getData("SELECT * FROM [USER] WHERE id=" + WebSecurity.CurrentUserId, db);
            //DataRow data = userInfo.Rows[0];
            //int validadorIint = 0;
            //User userito = new User();
            //userito.address = data["address"].ToString();
            //userito.companyID = 4;
            //userito.confirmationToken = data["confirmationToken"].ToString();
            //userito.cookie = data["cookie"].ToString();
            //userito.country = data["country"].ToString();
            //int.TryParse(data["defaultActionID"].ToString(), out validadorIint); userito.defaultActionID = validadorIint;
            //int.TryParse(data["departmentID"].ToString(), out validadorIint); userito.departmentID = validadorIint;
            //userito.email = data["email"].ToString();
            //userito.externalUser = data["externalUser"].ToString() == "True" ? true : false;
            //int.TryParse(data["failedAttempts"].ToString(), out validadorIint); userito.failedAttempts = validadorIint;
            //userito.firstTime = data["firstTime"].ToString() == "True" ? true : false;
            //userito.ID = WebSecurity.CurrentUserId;
            //userito.identification = data["identification"].ToString();
            //userito.imageUrl = data["imageUrl"].ToString();
            //userito.lastIP = data["lastIP"].ToString();
            //userito.lastName = data["lastName"].ToString();
            //userito.lastName2 = data["lastName2"].ToString();
            //userito.name = data["name"].ToString();
            //userito.password = data["password"].ToString();
            //userito.phone = data["phone"].ToString();
            //userito.phone2 = data["phone2"].ToString();
            //userito.province = data["province"].ToString();
            //userito.superUser = data["superUser"].ToString() == "True" ? true : false;
            //int.TryParse(data["TipoDocumentoID"].ToString(), out validadorIint); userito.TipoDocumentoID = validadorIint;
            //userito.username = data["username"].ToString();
            //int.TryParse(data["userStatusID"].ToString(), out validadorIint); userito.userStatusID = validadorIint;
            return dbss.BaseUsers.Where(d => d.ID == WebSecurity.CurrentUserId).First();
            //return userito;
        }

        /// <summary>
        /// Metodo utilizado para almacenar los nombres de los controladores
        /// y sus acciones asociadas en la base de datos.
        /// </summary>
        public static void InsertControllerAndActions()
        {
            ControllerActionProvider cp = new ControllerActionProvider();
            using (Context db = new Context())
            {

                List<BaseController> Controllers = db.BaseControllers.ToList();
                List<string> existingName = new List<string>();
                foreach (BaseController controller in Controllers)
                {
                    if (existingName.Contains(controller.name))
                    {
                        foreach (BaseAction item in controller.BaseActions.ToList())
                        {
                            db.BaseActions.Remove(item);
                        }
                        db.BaseControllers.Remove(controller);
                        db.SaveChanges();
                    }
                    else
                    {
                        existingName.Add(controller.name);
                    }
                }

                foreach (string controllerName in cp.GetControllerNames())
                {
                    if (controllerName.Contains("User"))
                    {

                    }
                    bool insertNew = false;
                    Admin.Models.BaseController c = db.BaseControllers.FirstOrDefault(t => t.fullName == controllerName);
                    if (c == null)
                    {
                        insertNew = true;
                        c = new Admin.Models.BaseController();
                        c.fullName = controllerName;
                        c.infoName = controllerName.Replace("Controller", "");
                        c.name = c.infoName;
                    }


                    List<Admin.Models.BaseAction> actions = new List<Admin.Models.BaseAction>();
                    string[] lstActions = cp.GetActionNames(controllerName).Distinct().ToArray();

                    foreach (string action in lstActions)
                    {
                        Admin.Models.BaseAction a = new Admin.Models.BaseAction();
                        a.name = action;
                        a.displayName = Helper.Capitalize(action);
                        if (!c.BaseActions.Select(d => d.name).Contains(action))
                        {
                            c.BaseActions.Add(a);
                        }
                    }

                    List<BaseAction> actionsToDelete = c.BaseActions.ToList();
                    foreach (BaseAction item in actionsToDelete)
                    {
                        if (!lstActions.Contains(item.name))
                        {
                            Helper.executeNonQUery("UPDATE BaseMenu Set actionID=null where actionID=" + item.id.ToString(), db);
                            Helper.executeNonQUery("UPDATE BaseWidget Set actionID=null where actionID=" + item.id.ToString(), db);
                            db.BaseActions.Remove(item);
                            try
                            {
                                db.SaveChanges();
                            }
                            catch
                            {

                            }
                        }
                    }

                    List<string> existingActionName = new List<string>();
                    foreach (BaseAction action in actionsToDelete)
                    {
                        if (existingActionName.Contains(action.name))
                        {
                            //db.BaseActions.Remove(action);
                            //db.SaveChanges();
                        }
                        else
                        {
                            existingActionName.Add(action.name);
                        }
                    }

                    if (insertNew)
                    {
                        db.BaseControllers.Add(c);
                        db.SaveChanges();
                    }
                    else
                    {
                        db.Entry(c).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                }
            }
        }

        public static string getFileNameWithOutFormat(string url)
        {
            if (string.IsNullOrEmpty(url))
                return "N/A";
            string[] fileName = Path.GetFileName(url).Split(new string[] { "$$" }, StringSplitOptions.None);
            string labelText = Path.GetFileName(url);
            if (fileName.Length > 1)
            {
                labelText = fileName[1];
            }
            return labelText;
        }

        /// <summary>
        /// Helper function to get the filename of a server repository item without its file extension,
        /// and without the format headers used to the convention names.
        /// </summary>
        /// <param name="url">The url to get the Filename from filesystem</param>
        /// <param name="returnNull">Boolean condition to allow a null value to be returned</param>
        /// <returns></returns>
        public static string getFileNameWithOutFormat(string url, bool returnNull)
        {
            if (string.IsNullOrEmpty(url))
                return returnNull ? null : "N/A";
            string[] fileName = Path.GetFileName(url).Split(new string[] { "$$" }, StringSplitOptions.None);
            string labelText = Path.GetFileName(url);
            if (fileName.Length > 1)
            {
                labelText = fileName[1];
            }
            return labelText;
        }

        public static void updateObjectFields(object obj)
        {
            foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(obj))
            {

                if (descriptor.PropertyType.Name == "String"
                    && descriptor.GetValue(obj) == null
                    )
                {
                    PropertyInfo prop = obj.GetType()
                        .GetProperty(descriptor.Name, BindingFlags.Public | BindingFlags.Instance);
                    if (prop.CanWrite)
                    {
                        prop.SetValue(obj, "", null);
                    }
                }
            }
        }

        public static string GetApplicationImage(Context db)
        {
            BaseConfiguration configuration = new BaseConfiguration();
            return Directories.VirtualPath + Directories.ApplicationLogosPipe + "default.png";
        }

        public static string getImage(string @class, string field, string name, bool cache = false)
        {
            string cacheString = $"?nocache={new Random().Next(0, 5000)}";
            return $"{System.Web.Hosting.HostingEnvironment.ApplicationVirtualPath}/files/{@class}/{field}/{name}{((!cache) ? cacheString : "")}";
        }

        public static string getAllProperties(System.Type myclass, string separator)
        {
            new List<int>();
            string fields = "";
            PropertyInfo[] propertyInfos = myclass.GetProperties(BindingFlags.Public | BindingFlags.Static);
            foreach (PropertyInfo property in propertyInfos)
            {
                fields += property.Name + separator;
            }
            fields += "**";
            fields = fields.Replace(separator + "**", "");
            return fields;
        }

        static void Transfer(object source, object target)
        {
            var sourceType = source.GetType();
            var targetType = target.GetType();

            var sourceParameter = Expression.Parameter(typeof(object), "source");
            var targetParameter = Expression.Parameter(typeof(object), "target");

            var sourceVariable = Expression.Variable(sourceType, "castedSource");
            var targetVariable = Expression.Variable(targetType, "castedTarget");

            var expressions = new List<Expression>();

            expressions.Add(Expression.Assign(sourceVariable, Expression.Convert(sourceParameter, sourceType)));
            expressions.Add(Expression.Assign(targetVariable, Expression.Convert(targetParameter, targetType)));

            foreach (var property in sourceType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                if (!property.CanRead)
                    continue;

                var targetProperty = targetType.GetProperty(property.Name, BindingFlags.Public | BindingFlags.Instance);
                if (targetProperty != null
                        && targetProperty.CanWrite
                        && targetProperty.PropertyType.IsAssignableFrom(property.PropertyType))
                {
                    expressions.Add(
                        Expression.Assign(
                            Expression.Property(targetVariable, targetProperty),
                            Expression.Convert(
                                Expression.Property(sourceVariable, property), targetProperty.PropertyType)));
                }
            }

            var lambda =
                Expression.Lambda<Action<object, object>>(
                    Expression.Block(new[] { sourceVariable, targetVariable }, expressions),
                    new[] { sourceParameter, targetParameter });

            var del = lambda.Compile();

            del(source, target);
        }

        public static void executeNonQUery(string Query, Context db)
        {
            using (var cmd = db.Database.Connection.CreateCommand())
            {
                if (db.Database.Connection.State == ConnectionState.Closed)
                    db.Database.Connection.Open();
                cmd.CommandText = Query;
                cmd.ExecuteNonQuery();
                db.Database.Connection.Close();
            }
        }

        public static void executeNonQUeryNoState(string Query, Context db)
        {
            using (var cmd = db.Database.Connection.CreateCommand())
            {
                cmd.CommandText = Query;
                cmd.ExecuteNonQuery();
            }
        }

        public static string BaseMessage(Context db, string code)
        {
            BaseMessage message = db.BaseMessages.FirstOrDefault(d => d.code == code);
            if (message != null)
                return db.BaseMessages.FirstOrDefault(d => d.code == code).message.Replace("[[", "{").Replace("]]", "}");
            else
            {
                message = new BaseMessage();
                message.code = code;
                message.message = code;
                message.messageCategory = 1;
                db.BaseMessages.Add(message);
                db.SaveChanges();
                return code;
            }
        }

        public static string BaseMessage(string code)
        {
            Context db = new Context();
            BaseMessage message = db.BaseMessages.FirstOrDefault(d => d.code == code);
            if (message != null)
                return db.BaseMessages.FirstOrDefault(d => d.code == code).message.Replace("[[", "{").Replace("]]", "}");
            else
            {
                message = new BaseMessage();
                message.code = code;
                message.message = code;
                message.messageCategory = 1;
                db.BaseMessages.Add(message);
                db.SaveChanges();
                return code;
            }
        }

        public static DataTable getData(string Query, Context db, params string[] hideColumns)
        {
            var cmd = db.Database.Connection.CreateCommand();
            if (db.Database.Connection.State == ConnectionState.Closed)
                db.Database.Connection.Open();
            cmd.CommandText = Query;
            DbDataReader reader = cmd.ExecuteReader();
            DataTable table = new DataTable();
            if (reader.HasRows)
            {
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    if (!hideColumns.Contains(reader.GetName(i)))
                        table.Columns.Add(reader.GetName(i));
                }
                while (reader.Read())
                {
                    List<object> datos = new List<object>();
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        if (!hideColumns.Contains(reader.GetName(i)))
                            datos.Add(reader[i]);
                    }
                    table.Rows.Add(datos.ToArray());
                }

                reader.Close();
            }
            db.Database.Connection.Close();
            return table;
        }

        public static void Autenticate(string path)
        {
            MenuAuthorize.ProcessLogin(path);
        }

        public static void executeNonQuerySQL(string Query, SqlConnection db)
        {
            using (SqlCommand cmd = new SqlCommand(Query, db))
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();
                cmd.CommandText = Query;
                cmd.ExecuteNonQuery();
                db.Close();
            }
        }

        public static DataTable getDataSQL(string Query, SqlConnection db)
        {
            SqlCommand cmd = new SqlCommand(Query, db);
            if (db.State == ConnectionState.Closed)
                db.Open();
            cmd.CommandText = Query;
            DbDataReader reader = cmd.ExecuteReader();


            DataTable table = new DataTable();
            if (reader.HasRows)
            {
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    table.Columns.Add(reader.GetName(i));
                }
                while (reader.Read())
                {
                    List<object> datos = new List<object>();
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        datos.Add(reader[i]);
                    }
                    table.Rows.Add(datos.ToArray());
                }

                reader.Close();
            }
            db.Close();
            return table;
        }

        public static TargetType Transfer<SourceType, TargetType>(SourceType source, object db = null)
            where TargetType : class, new()
        {
            TargetType target = new TargetType();
            if (db != null)
            {
                target = (TargetType)db;
            }
            Transfer(source, target);

            return target;
        }

        public enum InlineChartsTypes
        {
            pie = 1,
            bar = 2,
            line = 3
        }

        /// <summary>
        /// Count occurrences of strings.
        /// </summary>
        public static int CountStringOccurrences(string text, string pattern)
        {
            // Loop through all instances of the string 'text'.
            int count = 0;
            int i = 0;
            while ((i = text.IndexOf(pattern, i)) != -1)
            {
                i += pattern.Length;
                count++;
            }
            return count;
        }

        public static string ServerDateFormat(string date, int type = 1)
        {
            if (date == null || date.Length == 0)
                return date;

            switch (type)
            {
                case 1:
                    {
                        return Convert.ToDateTime(date).ToString("yyyy-MM-dd", System.Globalization.CultureInfo.GetCultureInfo("en-US"));
                    }
                case 2:
                    {
                        return Convert.ToDateTime(date).ToString("yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.GetCultureInfo("en-US"));
                    }
            }

            return date;
        }

        public static SqlConnection GetSqlConnection(Context db)
        {
            var conStr = db.Database.Connection.ConnectionString;
            return new SqlConnection(conStr);
        }

        public static List<TableSchema> GetRiskTables(Context db)
        {
            var lista = new List<TableSchema>();
            string sql = "SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'guest'";

            var cn = GetSqlConnection(db);

            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                if (cn.State == ConnectionState.Closed)
                {
                    cn.Open();
                }

                cmd.CommandText = sql;

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var tmpObj = new TableSchema();
                        tmpObj.Catalog = reader.GetString(0);
                        tmpObj.Schema = reader.GetString(1);
                        tmpObj.Name = reader.GetString(2);
                        tmpObj.Type = reader.GetString(3);

                        lista.Add(tmpObj);
                    }
                }

                cn.Close();
            }

            return lista;
        }
    }

    public class TableSchema
    {
        public string Catalog { get; set; }
        public string Schema { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
    }
}