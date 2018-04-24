using System.Collections.Generic;
using System.Text;

namespace Admin.BaseClass.UI
{
    /// <summary>
    /// Esta clase usa: id, icon y title
    /// </summary>
    public class Tab : BaseControl
    {

    }

    public enum TabStyle
    {
        nav_tabs = 0,
        nav_pills = 1
    }

    /// <summary>
    /// Esta clase usa: id
    /// </summary>
    public class Tabs : BaseControl
    {

        List<Tab> allTabs;

        public List<Tab> AllTabs
        {
            get { return allTabs; }
            set { allTabs = value; }
        }
        TabStyle tabstyle = TabStyle.nav_tabs;

        int tabActive = 1;
        string idActive = "";
        public int TabActive
        {
            get { return tabActive; }
            set { tabActive = value; }
        }

        public Tabs(string _id)
        {
            this.Id = _id;
            allTabs = new List<Tab>();
        }

        public string tabsStyleToString()
        {
            return this.tabstyle.ToString().Replace("_", "-");
        }

        public string html()
        {
            StringBuilder html = new StringBuilder();
            html.AppendLine(string.Format("<ul class=\"nav pull-right {0}\" id=\"{1}\">", this.tabsStyleToString(), this.Id));
            int first = 1;
            foreach (var item in this.allTabs)
            {
                if (this.tabActive == first)
                {
                    idActive = item.Id;
                }
                html.AppendLine(string.Format("<li class=\"{0}\">", this.tabActive == first ? "active" : ""));
                html.AppendLine(string.Format("<a href=\"#{0}\" data-toggle=\"tab\"> <i class=\"fa fa-lg {1}\"></i> <span class=\"hidden-mobile hidden-tablet\"> {2} </span> </a>",item.Id,item.Icon,item.Title));
                html.AppendLine("</li>");
                first++;
            }
            html.AppendLine("</ul>");
            return html.ToString();
        }

        public string beginTabs()
        {
            StringBuilder html = new StringBuilder();
            html.AppendLine("<div class=\"tab-content padding-10\">");
            return html.ToString();
        }
        public string begin(string tabID, string animation = "fade")
        {
            StringBuilder html = new StringBuilder();
            html.AppendLine(string.Format("<div id=\"{0}\" class=\"tab-pane {1} {2}\">", tabID, animation ,idActive == tabID ? "active in" : ""));
            return html.ToString();
        }

    }
}