using System;
using System.Collections.Generic;

namespace Admin.Models
{
    public partial class BaseUserMenu
    {
        public int menuID { get; set; }
        public int userID { get; set; }
        public int noOrder { get; set; }
        public virtual BaseMenu BaseMenu { get; set; }
        public virtual BaseUser BaseUser { get; set; }
    }
}
