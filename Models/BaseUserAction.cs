using System;
using System.Collections.Generic;

namespace Admin.Models
{
    public partial class BaseUserAction
    {
        public int actionID { get; set; }
        public int userID { get; set; }
        public bool forever { get; set; }
        public Nullable<System.DateTime> untilDate { get; set; }
        public string password { get; set; }
        public Nullable<bool> passwordAccess { get; set; }
        public Nullable<int> leftSeconds { get; set; }
        public virtual BaseAction BaseAction { get; set; }
        public virtual BaseUser BaseUser { get; set; }
    }
}
