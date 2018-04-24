using System;
using System.Collections.Generic;

namespace Admin.Models
{
    public partial class BaseController
    {
        public BaseController()
        {
            this.BaseActions = new List<BaseAction>();
        }

        public int id { get; set; }
        public string name { get; set; }
        public string fullName { get; set; }
        public string infoName { get; set; }
        public string Discriminator { get; set; }
        public virtual ICollection<BaseAction> BaseActions { get; set; }
    }
}
