using System;
using System.Collections.Generic;

namespace Admin.Models
{
    public partial class RDCategory
    {
        public RDCategory()
        {
            this.RDChannels = new List<RDChannel>();
        }

        public int id { get; set; }
        public bool active { get; set; }
        public System.DateTime creationDate { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public virtual ICollection<RDChannel> RDChannels { get; set; }
    }
}
