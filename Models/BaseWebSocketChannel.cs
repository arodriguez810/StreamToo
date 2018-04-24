using System;
using System.Collections.Generic;

namespace Admin.Models
{
    public partial class BaseWebSocketChannel
    {
        public BaseWebSocketChannel()
        {
            this.BaseWebSocketChannelBlackListProfiles = new List<BaseWebSocketChannelBlackListProfile>();
            this.BaseWebSocketChannelBlackListUsers = new List<BaseWebSocketChannelBlackListUser>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsGlobal { get; set; }
        public string ServerCode { get; set; }
        public string ParametersName { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public Nullable<System.TimeSpan> StartTime { get; set; }
        public Nullable<System.TimeSpan> EndTime { get; set; }
        public string DataEmitExample { get; set; }
        public string ClientCode { get; set; }
        public Nullable<bool> NoDateLimit { get; set; }
        public Nullable<bool> NoTimeLimit { get; set; }
        public virtual ICollection<BaseWebSocketChannelBlackListProfile> BaseWebSocketChannelBlackListProfiles { get; set; }
        public virtual ICollection<BaseWebSocketChannelBlackListUser> BaseWebSocketChannelBlackListUsers { get; set; }
    }
}
