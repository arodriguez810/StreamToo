using System;
using System.Collections.Generic;

namespace Admin.Models
{
    public partial class BaseTemplatePage
    {
        public int id { get; set; }
        public int templateID { get; set; }
        public int pageNumber { get; set; }
        public virtual BaseTemplate BaseTemplate { get; set; }
    }
}
