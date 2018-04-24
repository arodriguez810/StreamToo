using Admin.CustomCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;

namespace Admin.Code.CustomCode
{
    public class TableClass
    {
        private List<KeyValuePair<String, Type>> _fieldInfo = new List<KeyValuePair<String, Type>>();
        private string _className = String.Empty;
        private string _prefix = String.Empty;
        private object _values = null;

        private Dictionary<Type, String> dataMapper
        {
            get
            {
                Dictionary<Type, String> dataMapper = new Dictionary<Type, string>();
                dataMapper.Add(typeof(int), "BIGINT");
                dataMapper.Add(typeof(string), "NVARCHAR(MAX)");
                dataMapper.Add(typeof(bool), "BIT");
                dataMapper.Add(typeof(DateTime), "DATETIME");
                dataMapper.Add(typeof(float), "FLOAT");
                dataMapper.Add(typeof(decimal), "DECIMAL(18,0)");
                dataMapper.Add(typeof(Guid), "UNIQUEIDENTIFIER");

                dataMapper.Add(typeof(int[]), "NVARCHAR(MAX)");
                dataMapper.Add(typeof(string[]), "NVARCHAR(MAX)");
                dataMapper.Add(typeof(bool[]), "NVARCHAR(MAX)");
                dataMapper.Add(typeof(DateTime[]), "NVARCHAR(MAX)");
                dataMapper.Add(typeof(float[]), "NVARCHAR(MAX)");
                dataMapper.Add(typeof(decimal[]), "NVARCHAR(MAX)");
                dataMapper.Add(typeof(Guid[]), "NVARCHAR(MAX)");

                return dataMapper;
            }
        }
        public List<KeyValuePair<String, Type>> Fields
        {
            get { return this._fieldInfo; }
            set { this._fieldInfo = value; }
        }
        public string ClassName
        {
            get { return this._className; }
            set { this._className = value; }
        }
        public TableClass(Type t, string prefix = "", object values = null)
        {
            this._className = t.Name;
            this._prefix = prefix;
            this._values = values;
            foreach (PropertyInfo p in t.GetProperties())
            {
                KeyValuePair<String, Type> field = new KeyValuePair<String, Type>(p.Name, p.PropertyType);
                this.Fields.Add(field);
            }
        }


        public string createFieldsScriptInsert(Models.Context db)
        {
            System.Text.StringBuilder script = new StringBuilder();
            for (int i = 0; i < this.Fields.Count; i++)
            {
                KeyValuePair<String, Type> field = this.Fields[i];
                if (dataMapper.ContainsKey(field.Value))
                {
                    string query = $"{_prefix}{field.Key}";
                }
                else
                {
                    TableClass addTypes = new TableClass(field.Value, _prefix + field.Key + "_");
                    script.Append(addTypes.createFieldsScript(db));
                }

                script.Append(Environment.NewLine);
            }
            return script.ToString();
        }

        public string createFieldsScript(Models.Context db)
        {
            System.Text.StringBuilder script = new StringBuilder();
            for (int i = 0; i < this.Fields.Count; i++)
            {
                KeyValuePair<String, Type> field = this.Fields[i];

                if (dataMapper.ContainsKey(field.Value))
                {
                    string query = "\t " + $"ALTER TABLE report.Device ADD {_prefix}{field.Key} {dataMapper[field.Value]};";
                    try
                    {
                        Helper.executeNonQUery(query, db);
                        script.Append(query);
                    }
                    catch
                    {

                    }
                }
                else
                {
                    // Complex Type? 
                    TableClass addTypes = new TableClass(field.Value, _prefix + field.Key + "_");
                    script.Append(addTypes.createFieldsScript(db));
                }

                script.Append(Environment.NewLine);
            }
            return script.ToString();
        }
        public string CreateTableScript()
        {
            Models.Context db = new Models.Context();
            try
            {
                Helper.executeNonQUery("create table report.Device(runID int)", db);
            }
            catch
            {

            }
            System.Text.StringBuilder script = new StringBuilder();
            script.AppendLine(createFieldsScript(db));
            return script.ToString();
        }
    }
}