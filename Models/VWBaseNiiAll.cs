using System;
using System.Collections.Generic;

namespace Admin.Models
{
    public partial class VWBaseNiiAll
    {
        public int column_id { get; set; }
        public string name { get; set; }
        public short max_length { get; set; }
        public byte precision { get; set; }
        public byte scale { get; set; }
        public Nullable<bool> is_nullable { get; set; }
        public bool is_identity { get; set; }
        public bool is_computed { get; set; }
        public string tableName { get; set; }
        public string type { get; set; }
        public string fk { get; set; }
        public string alterText { get; set; }
    }
}
