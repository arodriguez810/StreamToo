using System;
using System.Collections.Generic;

namespace Admin.Models
{
    public partial class BaseDisabled
    {
        public string type { get; set; }
        public int id { get; set; }
        public int userID { get; set; }
        public System.DateTime date { get; set; }
    }
}
