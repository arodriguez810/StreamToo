using System;
using System.Collections.Generic;

namespace Admin.Models
{
    public partial class BaseMessageCategory
    {
        public BaseMessageCategory()
        {
            this.BaseMessages = new List<BaseMessage>();
        }

        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public virtual ICollection<BaseMessage> BaseMessages { get; set; }
    }
}
