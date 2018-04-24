using System;
using System.Collections.Generic;

namespace Admin.Models
{
    public partial class BaseTemplateVariable
    {
        public int id { get; set; }
        public int templateDataSourceID { get; set; }
        public Nullable<int> dataSourceColumnID { get; set; }
        public string description { get; set; }
        public virtual BaseTemplateDataSource BaseTemplateDataSource { get; set; }
    }
}
