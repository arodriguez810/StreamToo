using Admin.Models;
using System.Text;
using System.Web.Mvc;

namespace Admin.BaseClass.UI
{
    public class Widget : BaseControl
    {
        BaseWidget baseWidget;

        public BaseWidget BaseWidget
        {
            get { return baseWidget; }
            set { baseWidget = value; }
        }
        public Widget(string _id, string _title, string _icon, ViewContext context)
        {
            this.icon = BaseUIIcon.fa.fa_fa_list_alt;
            string action = context.RouteData.Values["action"].ToString();
            string controller = context.RouteData.Values["controller"].ToString();
            this.Icon = _icon;
            this.Id = _id;
            this.Title = _title;
        }

        int width = 12;

        public int Width
        {
            get { return width; }
            set { width = value; }
        }

        bool data_widget_editbutton = true,
             data_widget_colorbutton = true,
             data_widget_togglebutton = true,
             data_widget_deletebutton = true,
             data_widget_fullscreenbutton = true,
             data_widget_custombutton = false,
             data_widget_collapsed = true,
             data_widget_sortable = true;

        public bool Data_widget_sortable
        {
            get { return data_widget_sortable; }
            set { data_widget_sortable = value; }
        }

        public bool Data_widget_collapsed
        {
            get { return data_widget_collapsed; }
            set { data_widget_collapsed = value; }
        }

        public bool Data_widget_custombutton
        {
            get { return data_widget_custombutton; }
            set { data_widget_custombutton = value; }
        }

        public bool Data_widget_fullscreenbutton
        {
            get { return data_widget_fullscreenbutton; }
            set { data_widget_fullscreenbutton = value; }
        }

        public bool Data_widget_deletebutton
        {
            get { return data_widget_deletebutton; }
            set { data_widget_deletebutton = value; }
        }

        public bool Data_widget_togglebutton
        {
            get { return data_widget_togglebutton; }
            set { data_widget_togglebutton = value; }
        }

        public bool Data_widget_colorbutton
        {
            get { return data_widget_colorbutton; }
            set { data_widget_colorbutton = value; }
        }

        public bool Data_widget_editbutton
        {
            get { return data_widget_editbutton; }
            set { data_widget_editbutton = value; }
        }

        string icon = "", data_widget_load = "";

        public string Data_widget_load
        {
            get { return data_widget_load; }
            set { data_widget_load = value; }
        }

        public string beginHeader()
        {
            StringBuilder html = new StringBuilder();
            string properties = "";
            properties += addProperty(this.data_widget_editbutton, "data-widget-editbutton");
            properties += addProperty(this.data_widget_colorbutton, "data-widget-colorbutton");
            properties += addProperty(this.data_widget_togglebutton, "data-widget-togglebutton");
            properties += addProperty(this.data_widget_deletebutton, "data-widget-deletebutton");
            properties += addProperty(this.data_widget_fullscreenbutton, "data-widget-fullscreenbutton");
            properties += addProperty(this.data_widget_custombutton, "data-widget-custombutton");
            properties += addProperty(this.data_widget_collapsed, "data-widget-collapsed");
            properties += addProperty(this.data_widget_sortable, "data-widget-sortable");
            properties += !string.IsNullOrEmpty(data_widget_load) ? string.Format("data-widget-load=\"{0}\"", data_widget_load) : "";

            html.AppendLine(string.Format("<article class=\"col-xs-12 col-sm-{0} col-md-{0} col-lg-{0}\">", this.width));
            html.AppendLine(string.Format("<div class=\"jarviswidget\" id=\"{0}\" {1} >", this.Id, properties));
            html.AppendLine("<header>");
            html.AppendLine(string.Format("<span class=\"widget-icon\"> <i class=\"fa {0}\"></i> </span>", this.icon));
            html.AppendLine(string.Format("<h2>{0}</h2>", (this.Title)));
            return html.ToString();
        }

        public string beginCustomButton()
        {
            StringBuilder html = new StringBuilder();
            html.AppendLine("<div class=\"widget-toolbar\" role=\"menu\">");
            return html.ToString();
        }

        public string endHeader()
        {
            StringBuilder html = new StringBuilder();
            html.AppendLine("</header>");
            html.AppendLine("<div>");
            html.AppendLine("<div class=\"jarviswidget-editbox\">");
            html.AppendLine("<input type=\"text\" class=\"form-control\">");
            html.AppendLine("<span class=\"note\"><i class=\"fa fa-check text-success\"></i> " + ("Change title to update and save instantly") + "!</span>");
            html.AppendLine("</div>");
            html.AppendLine(string.Format("<div class=\"widget-body {0}\">", this.Class));
            return html.ToString();
        }

        public string footerBegin()
        {
            return "<div class=\"widget-footer\">";
        }

        public string Dcontinue()
        {
            return "</div>";
        }

        public string end()
        {
            StringBuilder html = new StringBuilder();
            html.AppendLine("</div>");
            html.AppendLine("</div>");
            html.AppendLine("</div>");
            html.AppendLine("</article>");
            return html.ToString();
        }

        public Widget clone(string id, string _title, ViewContext context)
        {
            Widget clone = new Widget(id, _title, this.icon, context);
            clone.Class = this.Class;
            clone.Data_widget_collapsed = this.Data_widget_collapsed;
            clone.Data_widget_colorbutton = this.Data_widget_colorbutton;
            clone.Data_widget_custombutton = this.Data_widget_custombutton;
            clone.Data_widget_deletebutton = this.Data_widget_deletebutton;
            clone.Data_widget_editbutton = this.Data_widget_editbutton;
            clone.Data_widget_fullscreenbutton = this.Data_widget_fullscreenbutton;
            clone.Data_widget_sortable = this.Data_widget_sortable;
            clone.Data_widget_togglebutton = this.Data_widget_togglebutton;
            return clone;
        }

        string addProperty(bool property, string text)
        {
            string thisProperty = "";
            if (!property)
                thisProperty += " " + text + "=\"false\" ";
            else
                return "";
            return thisProperty;
        }
    }
}