using System;
using System.Collections.Generic;

namespace Admin.Models
{
    public partial class BaseLogAction
    {
        public BaseLogAction()
        {
            this.BaseLogs = new List<BaseLog>();
        }

        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public virtual ICollection<BaseLog> BaseLogs { get; set; }
    }
}
