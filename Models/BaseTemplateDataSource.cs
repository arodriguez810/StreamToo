using System;
using System.Collections.Generic;

namespace Admin.Models
{
    public partial class BaseTemplateDataSource
    {
        public BaseTemplateDataSource()
        {
            this.BaseTemplateVariables = new List<BaseTemplateVariable>();
            this.BaseTemplateTables = new List<BaseTemplateTable>();
        }

        public int id { get; set; }
        public int templateID { get; set; }
        public int dataSourceID { get; set; }
        public virtual BaseTemplate BaseTemplate { get; set; }
        public virtual ICollection<BaseTemplateVariable> BaseTemplateVariables { get; set; }
        public virtual ICollection<BaseTemplateTable> BaseTemplateTables { get; set; }
    }
}
