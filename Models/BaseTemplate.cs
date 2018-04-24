using System;
using System.Collections.Generic;

namespace Admin.Models
{
    public partial class BaseTemplate
    {
        public BaseTemplate()
        {
            this.BaseTemplateDataSources = new List<BaseTemplateDataSource>();
            this.BaseTemplatePages = new List<BaseTemplatePage>();
        }

        public int id { get; set; }
        public string code { get; set; }
        public string html { get; set; }
        public bool isHeader { get; set; }
        public string styles { get; set; }
        public virtual ICollection<BaseTemplateDataSource> BaseTemplateDataSources { get; set; }
        public virtual ICollection<BaseTemplatePage> BaseTemplatePages { get; set; }
    }
}
