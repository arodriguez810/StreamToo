using System;
using System.Collections.Generic;

namespace Admin.Models
{
    public partial class BaseEmail
    {
        public int id { get; set; }
        public string code { get; set; }
        public string subject { get; set; }
        public string bodyHTML { get; set; }
        public bool isHtml { get; set; }
    }
}
