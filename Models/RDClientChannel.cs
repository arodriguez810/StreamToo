using System;
using System.Collections.Generic;

namespace Admin.Models
{
    public partial class RDClientChannel
    {
        public System.Guid id { get; set; }
        public int channel_channel { get; set; }
        public int visitCount { get; set; }
        public System.DateTime lastVisit { get; set; }
        public int client_client { get; set; }
        public bool isFavorite { get; set; }
        public Nullable<bool> active { get; set; }
        public Nullable<System.DateTime> creationDate { get; set; }
        public virtual RDChannel RDChannel { get; set; }
        public virtual RDClient RDClient { get; set; }
    }
}
