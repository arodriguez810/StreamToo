using System;
using System.Collections.Generic;

namespace Admin.Models
{
    public partial class RDChannel
    {
        public RDChannel()
        {
            this.RDEvents = new List<RDEvent>();
            this.RDProgrammings = new List<RDProgramming>();
            this.RDClientChannels = new List<RDClientChannel>();
        }

        public int id { get; set; }
        public bool active { get; set; }
        public System.DateTime creationDate { get; set; }
        public string name { get; set; }
        public string displayName { get; set; }
        public string description { get; set; }
        public string url { get; set; }
        public int orderNum { get; set; }
        public bool isTemporal { get; set; }
        public string iconImage { get; set; }
        public string logoImage { get; set; }
        public Nullable<int> category_category { get; set; }
        public Nullable<int> type_type { get; set; }
        public virtual RDCategory RDCategory { get; set; }
        public virtual RDType RDType { get; set; }
        public virtual ICollection<RDEvent> RDEvents { get; set; }
        public virtual ICollection<RDProgramming> RDProgrammings { get; set; }
        public virtual ICollection<RDClientChannel> RDClientChannels { get; set; }
    }
}
