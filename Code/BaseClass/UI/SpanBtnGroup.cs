using System.Collections.Generic;
using System.Text;

namespace Admin.BaseClass.UI
{
    public class SpanBtnGroup
    {
        string divider = "<li class=\"divider\"></li>";
        string toggleClass = "btn btn-default btn-xs dropdown-toggle";
        string spanToggleClass = "caret single";

        List<string> links = new List<string>();

        public List<string> Links
        {
            get { return links; }
            set { links = value; }
        }

        public string getHtml()
        {
            StringBuilder html = new StringBuilder();
            
            html.AppendLine("	<a href=\"#\" data-toggle=\"dropdown\" class=\"" + toggleClass + "\"><span class=\"" + spanToggleClass + "\"></span></a>");
            html.AppendLine("	<ul class=\"dropdown-menu\">");
            foreach (var link in links)
            {
                if (link.ToLower().Contains("divider"))
                {
                    html.AppendLine(divider);
                    continue;
                }
                html.AppendLine("		<li>");
                html.AppendLine("			" + link + "");
                html.AppendLine("		</li>");
            }
            html.AppendLine("	</ul>");
            return html.ToString();
        }
    }
}