using System;
using System.Collections.Generic;

namespace Admin.Models
{
    public partial class VWOCHouseInformation
    {
        public int id { get; set; }
        public string houseId { get; set; }
        public string accountNumbers { get; set; }
        public string stickerNumber { get; set; }
        public string location { get; set; }
        public string baseUser_User { get; set; }
        public string comment { get; set; }
        public string barCode { get; set; }
        public System.DateTime creationDate { get; set; }
    }
}
