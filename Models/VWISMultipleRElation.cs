using System;
using System.Collections.Generic;

namespace Admin.Models
{
    public partial class VWISMultipleRElation
    {
        public string K_Table { get; set; }
        public string FK_Column { get; set; }
        public string PK_Table { get; set; }
        public string PK_Column { get; set; }
        public string Constraint_Name { get; set; }
    }
}
