using System;
using System.Collections.Generic;

namespace Admin.Models
{
    public partial class BaseWebSocketChannelBlackListUser
    {
        public int Id { get; set; }
        public int userID { get; set; }
        public int webSocketChannelid { get; set; }
        public virtual BaseUser BaseUser { get; set; }
        public virtual BaseWebSocketChannel BaseWebSocketChannel { get; set; }
    }
}
