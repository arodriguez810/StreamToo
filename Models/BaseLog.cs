using System;
using System.Collections.Generic;

namespace Admin.Models
{
    public partial class BaseLog
    {
        public System.DateTime created { get; set; }
        public int basedynamicList_entity { get; set; }
        public Nullable<int> entityId { get; set; }
        public Nullable<System.Guid> entityIdU { get; set; }
        public string entityIdS { get; set; }
        public System.DateTime date { get; set; }
        public int baseUser_user { get; set; }
        public int baseLogAction_action { get; set; }
        public string version { get; set; }
        public virtual BaseDynamicList BaseDynamicList { get; set; }
        public virtual BaseLogAction BaseLogAction { get; set; }
        public virtual BaseUser BaseUser { get; set; }
    }
}
