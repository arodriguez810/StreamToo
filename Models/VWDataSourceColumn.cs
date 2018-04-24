using System;
using System.Collections.Generic;

namespace Admin.Models
{
    public partial class VWDataSourceColumn
    {
        public int viewId { get; set; }
        public int columnId { get; set; }
        public string columnName { get; set; }
        public string viewName { get; set; }
    }
}
