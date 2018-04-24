using System;
using System.Collections.Generic;

namespace Admin.Models
{
    public partial class RDProgramming
    {
        public int id { get; set; }
        public bool active { get; set; }
        public System.DateTime creationDate { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public Nullable<int> channel_channel { get; set; }
        public string iconImage { get; set; }
        public System.TimeSpan timeFrom { get; set; }
        public System.TimeSpan timeTo { get; set; }
        public virtual RDChannel RDChannel { get; set; }
    }
}
