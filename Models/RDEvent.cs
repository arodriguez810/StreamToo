using System;
using System.Collections.Generic;

namespace Admin.Models
{
    public partial class RDEvent
    {
        public int id { get; set; }
        public bool active { get; set; }
        public System.DateTime creationDate { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public Nullable<int> channel_channel { get; set; }
        public Nullable<System.DateTime> dateFrom { get; set; }
        public Nullable<System.DateTime> dateTo { get; set; }
        public Nullable<System.TimeSpan> timeFrom { get; set; }
        public Nullable<System.TimeSpan> timeTo { get; set; }
        public Nullable<bool> isAllDay { get; set; }
        public virtual RDChannel RDChannel { get; set; }
    }
}
