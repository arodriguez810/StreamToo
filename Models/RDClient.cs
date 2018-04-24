using System;
using System.Collections.Generic;

namespace Admin.Models
{
    public partial class RDClient
    {
        public RDClient()
        {
            this.RDClientChannels = new List<RDClientChannel>();
        }

        public int id { get; set; }
        public bool active { get; set; }
        public System.DateTime creationDate { get; set; }
        public string email { get; set; }
        public string name { get; set; }
        public string lastName { get; set; }
        public Nullable<System.DateTime> birthDate { get; set; }
        public string location { get; set; }
        public string device { get; set; }
        public virtual ICollection<RDClientChannel> RDClientChannels { get; set; }
    }
}
