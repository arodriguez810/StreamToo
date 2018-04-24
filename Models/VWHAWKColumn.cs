using System;
using System.Collections.Generic;

namespace Admin.Models
{
    public partial class VWHAWKColumn
    {
        public string TABLE_NAME { get; set; }
        public string COLUMN_NAME { get; set; }
        public string IS_NULLABLE { get; set; }
        public string DATA_TYPE { get; set; }
        public Nullable<int> CHARACTER_MAXIMUM_LENGTH { get; set; }
        public Nullable<byte> NUMERIC_PRECISION { get; set; }
        public Nullable<int> NUMERIC_SCALE { get; set; }
        public string RELATION_TYPE { get; set; }
        public string RELATION { get; set; }
    }
}
