using System;
using System.Collections.Generic;

namespace Admin.Models
{
    public partial class BaseLanguage
    {
        public int id { get; set; }
        public string languageCultureName { get; set; }
        public string displayName { get; set; }
        public string cultureCode { get; set; }
        public string ISO639xValue { get; set; }
        public string flagImage { get; set; }
    }
}
