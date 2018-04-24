using System;
using System.Collections.Generic;

namespace Admin.Models
{
    public partial class BaseMessage
    {
        public int id { get; set; }
        public string code { get; set; }
        public string message { get; set; }
        public int messageCategory { get; set; }
        public virtual BaseMessageCategory BaseMessageCategory { get; set; }
    }
}
