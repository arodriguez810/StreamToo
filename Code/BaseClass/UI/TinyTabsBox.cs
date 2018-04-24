using System.Collections.Generic;
using System.Text;

namespace Admin.BaseClass.UI
{
    public class TinyTabs : BaseControl
    {
    }
    public class TinyTabsBox
    {
        int headNumber = 0;
        string welcomeMessage = "", footerMessage = "", sicon = "", id = "", welcomeTitle = "", welcomeIcon = "";

        public string Sicon
        {
            get { return sicon; }
            set { sicon = value; }
        }

        public string Id
        {
            get { return id; }
            set { id = value; }
        }

        public string WelcomeIcon
        {
            get { return welcomeIcon; }
            set { welcomeIcon = value; }
        }

        public string WelcomeTitle
        {
            get { return welcomeTitle; }
            set { welcomeTitle = value; }
        }

        List<TinyTabs> tabs;

        public List<TinyTabs> Tabs
        {
            get { return tabs; }
            set { tabs = value; }
        }

        public TinyTabsBox()
        {
            tabs = new List<TinyTabs>();
        }

        public string FooterMessage
        {
            get { return footerMessage; }
            set { footerMessage = value; }
        }

        public string WelcomeMessage
        {
            get { return welcomeMessage; }
            set { welcomeMessage = value; }
        }

        public int HeadNumber
        {
            get { return headNumber; }
            set { headNumber = value; }
        }

        public string Icon()
        {
            return string.Format("<span id=\"{0}\" class=\"{0}-dropdown\"><i class=\"{1}\"></i><b class=\"badge\">{2}</b></span>"
                ,this.id
                ,this.sicon
                ,this.headNumber
                );
        }

        public string Tabsty()
        {
            StringBuilder html = new StringBuilder();
            html.AppendLine("<div class=\"btn-group btn-group-justified\" data-toggle=\"buttons\">");
            foreach (var item in this.Tabs)
            {
                html.AppendLine("	<label class=\"btn btn-default\">");
                html.AppendLine(string.Format("		<input type=\"radio\" name=\"{0}\" id=\"{1}\">",this.id,item.Href));
                html.AppendLine(item.Text);
                html.AppendLine("	</label>");
            }
            html.AppendLine("</div>");
            return html.ToString();
        }

        public string WelcomeMessageHtml()
        {
            StringBuilder html = new StringBuilder();
            html.AppendLine("<div class=\"ajax-notifications custom-scroll\">");
            html.AppendLine("	<div class=\"alert alert-transparent\">");
            html.AppendLine("		<h4>"+this.welcomeTitle+"</h4>");
            html.AppendLine(this.welcomeMessage);
            html.AppendLine("	</div>");
            html.AppendLine("	<i class=\"" + this.welcomeIcon + " fa-4x fa-border\"></i>");
            html.AppendLine("</div>");
            return html.ToString();
        }

        public string FooterHtml()
        {
            StringBuilder html = new StringBuilder();
            html.AppendLine("<span>"+footerMessage);
            html.AppendLine("	<button type=\"button\" data-loading-text=\"<i class='fa fa-refresh fa-spin'></i> "+"Loading..."+"\" class=\"btn btn-xs btn-default pull-right\">");
            html.AppendLine("		<i class=\"fa fa-refresh\"></i>");
            html.AppendLine("	</button>");
            html.AppendLine("</span>");
            return html.ToString();
        }

        public string HTML()
        {
            StringBuilder html = new StringBuilder();
            html.AppendLine(this.Icon());
            html.AppendLine("<div class=\"ajax-dropdown\">");
            html.AppendLine(this.Tabsty());
            html.AppendLine(this.WelcomeMessageHtml());
            html.AppendLine(this.FooterHtml());
            html.AppendLine("</div>");
            return html.ToString();
        }
    }
}