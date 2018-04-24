using System;
using System.Collections.Generic;

namespace Admin.Models
{
    public partial class BaseUserSession
    {
        public int userID { get; set; }
        public System.DateTime created { get; set; }
        public string ip { get; set; }
        public virtual BaseUser BaseUser { get; set; }
    }
}
