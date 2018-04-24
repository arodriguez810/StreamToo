using System;
using System.Collections.Generic;

namespace Admin.Models
{
    public partial class BaseTimeZone
    {
        public int id { get; set; }
        public string abbreviation { get; set; }
        public string fullName { get; set; }
        public string location { get; set; }
        public string timeZone { get; set; }
    }
}
