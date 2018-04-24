using System;
using System.Collections.Generic;

namespace Admin.Models
{
    public partial class BaseWebSocketChannelBlackListProfile
    {
        public int Id { get; set; }
        public int profileId { get; set; }
        public int webSocketChannelId { get; set; }
        public virtual BaseProfile BaseProfile { get; set; }
        public virtual BaseWebSocketChannel BaseWebSocketChannel { get; set; }
    }
}
