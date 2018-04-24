using System.Collections.Generic;

namespace Admin.Models.ViewModels
{
    public class JsonCsTable
    {
        public JsonCsTable()
        {
            columns = new List<Column>();
            rows = new List<Row>();
            header = new List<Row>();
        }
        public List<Row> header { get; set; }
        public List<Column> columns { get; set; }
        public List<Row> rows { get; set; }        
        public string count { get; set; }
        public string query { get; set; }
        public string infoData { get; set; }
    }

    public class Column
    {
        public string t { get; set; }
        public string p { get; set; }
        public string n { get; set; }
        public string tc { get; set; }
    }

    public class Row
    {
        public Row()
        {
            d = new List<D>();
        }
        public List<D> d { get; set; }
        public string p { get; set; }
    }

    public class D
    {
        public string d { get; set; }
        public string p { get; set; }
        public string column { get; set; }
    }
}