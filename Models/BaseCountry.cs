using System;
using System.Collections.Generic;

namespace Admin.Models
{
    public partial class BaseCountry
    {
        public int id { get; set; }
        public string officialShortForm { get; set; }
        public string officialLongForm { get; set; }
        public string ISOCode { get; set; }
        public string ISOShort { get; set; }
        public string ISOLong { get; set; }
        public string UNCode { get; set; }
        public string CapitalMajorCity { get; set; }
    }
}
