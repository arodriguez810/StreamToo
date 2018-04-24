using System.Collections.Generic;

namespace Admin.Models.ViewModels
{
    public class JSONMorris
    {

        public JSONMorris()
        {
            this.labels = new List<string>();
            this.ykeys = new List<string>();
            this.data = new List<Dictionary<string, object>>();
            this.element = "";
            this.colors = new List<string>();
            //this.xLabelAngle = 0;
            //this.resize = "false";
            this.hideHover = "auto";
            this.xLabelAngle = 40;
        }

        public string element;
        public List<Dictionary<string, object>> data;
        public string hoverCallback;
        public string xkey;
        public List<string> ykeys;
        public List<string> labels;
        public bool parseTime = false;
        public List<string> colors { get; set; }
        public string formatter;
        public int xLabelAngle;
        //public string resize;
        public string hideHover;
        

    }

}