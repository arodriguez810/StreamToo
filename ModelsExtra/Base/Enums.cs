using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Admin.ModelsExtra
{
    public class Enums
    {
        public enum ESMQueue
        {
            Sales = 1,
            SmartBar = 2,
            CustomerService = 3,
            Greater = 4
        }
        public enum ESMStatus
        {
            Complete = 1,
            Canceled = 2,
            Working = 3,
            Processing = 4,
            Send_to_CSR = 5,
        }
        public enum EEmployeeType
        {
            Greater = 1,
            Vendor_Agent = 2,
            Smart_Agent = 3,
            Customer_Service = 5,
        }
    }
}