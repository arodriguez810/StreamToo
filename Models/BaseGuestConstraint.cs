using System;
using System.Collections.Generic;

namespace Admin.Models
{
    public partial class BaseGuestConstraint
    {
        public string CONSTRAINT_CATALOG { get; set; }
        public string CONSTRAINT_SCHEMA { get; set; }
        public string CONSTRAINT_NAME { get; set; }
        public string CHECK_CLAUSE { get; set; }
    }
}
