using System;
using System.Collections.Generic;

namespace Admin.Models
{
    public partial class BaseSystemTable
    {
        public int id { get; set; }
        public string tableName { get; set; }
        public string why { get; set; }
    }
}
