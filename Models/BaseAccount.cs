using System;
using System.Collections.Generic;

namespace Admin.Models
{
    public partial class BaseAccount
    {
        public int id { get; set; }
        public string username { get; set; }
        public string phone { get; set; }
        public string email { get; set; }
        public bool registered { get; set; }
        public System.DateTime creationDate { get; set; }
        public string password { get; set; }
        public string token { get; set; }
        public string fullName { get; set; }
    }
}
