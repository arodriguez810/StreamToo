using System;
using System.Collections.Generic;

namespace Admin.Models
{
    public partial class VWOCHouseInformationExport
    {
        public int ID { get; set; }
        public string House_ID { get; set; }
        public string Sticker_Number { get; set; }
        public string Account_Numbers { get; set; }
        public string Barcode { get; set; }
        public string Location { get; set; }
        public System.DateTime Creation_Date { get; set; }
        public string User { get; set; }
        public string Comment { get; set; }
    }
}
