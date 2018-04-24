using System;
using System.Collections.Generic;

namespace Admin.Models
{
    public partial class BaseDynamicList
    {
        public BaseDynamicList()
        {
            this.BaseDynamicColumnLists = new List<BaseDynamicColumnList>();
            this.BaseLogs = new List<BaseLog>();
        }

        public int id { get; set; }
        public string name { get; set; }
        public string nameToDisplay { get; set; }
        public string cookieKey { get; set; }
        public bool allowSaveConfig { get; set; }
        public bool allowExport { get; set; }
        public bool enableControl { get; set; }
        public bool allowLog { get; set; }
        public bool audit { get; set; }
        public virtual ICollection<BaseDynamicColumnList> BaseDynamicColumnLists { get; set; }
        public virtual ICollection<BaseLog> BaseLogs { get; set; }
    }
}
