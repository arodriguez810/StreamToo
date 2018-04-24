using System;
using System.Collections.Generic;

namespace Admin.Models
{
    public partial class BaseTemplateTable
    {
        public int id { get; set; }
        public string alias { get; set; }
        public Nullable<int> templateDataSourceID { get; set; }
        public virtual BaseTemplateDataSource BaseTemplateDataSource { get; set; }
    }
}
