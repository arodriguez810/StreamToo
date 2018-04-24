using System;
using System.Collections.Generic;

namespace Admin.Models
{
    public partial class BaseProfileMenu
    {
        public int menuID { get; set; }
        public int userProfileID { get; set; }
        public int noOrder { get; set; }
        public virtual BaseMenu BaseMenu { get; set; }
        public virtual BaseProfile BaseProfile { get; set; }
    }
}
