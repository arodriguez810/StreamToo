using System;
using System.Collections.Generic;

namespace Admin.Models
{
    public partial class BaseColumn
    {
        public int id { get; set; }
        public int basetable_table { get; set; }
        public string columnName { get; set; }
        public string dataType { get; set; }
        public bool allowNull { get; set; }
        public string defaultValue { get; set; }
        public int ordinalPosition { get; set; }
        public int characterMaximumLength { get; set; }
        public int numericPrecision { get; set; }
        public int numericScale { get; set; }
        public bool isIndex { get; set; }
        public bool isUnique { get; set; }
        public bool active { get; set; }
        public string referenceTable { get; set; }
        public string referenceField { get; set; }
        public virtual BaseTable BaseTable { get; set; }
    }
}
