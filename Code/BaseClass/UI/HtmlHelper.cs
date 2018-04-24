using Admin.BaseModels.ViewModels;
using Admin.CustomCode;
using Admin.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;

namespace Admin.BaseClass.UI
{
    public partial class UIHtmlHelper
    {

        public static string PreviewFile(string name, string value, string controller)
        {
            StringBuilder html = new StringBuilder();
            if (!string.IsNullOrEmpty(value))
            {
                html.AppendFormat("<a class=\"btn btn-link\" data-target=\"#{0}Preview\" data-toggle=\"modal\" href=\"javascript:void(0);\">Preview</a>", name);

                html.AppendFormat("<div class=\"modal fade showmore\" id='{0}Preview'>", name);
                html.AppendFormat("             <div class=\"modal-dialog\">");
                html.AppendFormat("                  <div class=\"modal-content\">");
                html.AppendFormat("                      <div class='modal-header'>");
                html.AppendFormat("                        <button type='button' class='close' data-dismiss='modal' aria-hidden='true'>    &times;</button>");
                html.AppendFormat("                          <h4 class='modal-title' id='myModalLabel'>{0}'s document</h4>", Helper.Capitalize(name));
                html.AppendFormat("                                    </div>");
                html.AppendFormat("                                    <div class='modal-body'>");
                html.AppendFormat("                                        <object data=\"files/{0}/{1}/{2}\" style=\"width: 100%; min-height: 400px;\"></object>", controller, name, value);
                html.AppendFormat("                                    </div>");
                html.AppendFormat("                                </div>");
                html.AppendFormat("                            </div>");
                html.AppendFormat("                        </div>");
            }
            return html.ToString();
        }

        public static string MenuDivider
        {
            get { return "<li class=\"divider\"></li>"; }
        }

        public static string getTreeMenus(BaseMenu menu)
        {
            StringBuilder html = new StringBuilder();
            if (menu.BaseMenu1.Count() > 0)
            {
                BaseConfiguration configuration = new BaseConfiguration();
                html.AppendLine("<ul class=\"label-" + (configuration.primaryColor) + "\">");
                foreach (var submenu in menu.BaseMenu1.OrderBy(d => d.noOrder))
                {
                    html.AppendLine("   <li class=\"label-" + (configuration.primaryColor) + "\">");
                    string classIfParent = submenu.BaseMenu1.Count() > 0 ? "menu-item-parent" : "";

                    string dataAction = !submenu.directLink ? submenu.hreftext : submenu.href;

                    html.AppendLine(string.Format("<a href=\"{0}\" title=\"{1}\"><i class=\"{2} {3} {6}\"></i><span class=\"{5}\">{4}</span>",
                        dataAction, submenu.realTitle, submenu.css, submenu.icon, submenu.realText, classIfParent, ""));
                    if (!string.IsNullOrEmpty(submenu.badgeQuery))
                    {
                        html.AppendLine(string.Format("<span class=\"Badge{0} ajaxBadge badge pull-right {1} inbox-badge\">{{0}}</span>",
                            submenu.id,
                            submenu.badgeColor
                            ));
                    }
                    else if (!string.IsNullOrEmpty(submenu.badgeText))
                    {
                        html.AppendLine(string.Format("<span class=\"badge pull-right {0} inbox-badge\">{1}</span>",
                            submenu.badgeColor,
                            submenu.realBadgeText
                            ));
                    }
                    html.AppendLine("</a>");
                    html.AppendLine(getTreeMenus(submenu));
                    html.AppendLine("   </li>");
                }
                html.AppendLine("</ul>");
            }
            return html.ToString();
        }

        public static string getTreeMenus(BaseMenu menu, List<BaseMenu> userMenu)
        {
            StringBuilder html = new StringBuilder();
            if (menu.BaseMenu1.Count() > 0)
            {
                BaseConfiguration configuration = new BaseConfiguration();
                html.AppendLine("<ul class=\"label-" + (configuration.primaryColor) + "\">");
                foreach (var submenu in menu.BaseMenu1.OrderBy(d => d.noOrder))
                {
                    if (userMenu.Where(d => d.id == submenu.id).Count() > 0)
                    {
                        html.AppendLine("   <li class=\"label-" + (configuration.primaryColor) + "\">");
                        string classIfParent = submenu.BaseMenu1.Count() > 0 ? "menu-item-parent" : "";
                        string dataAction = !submenu.directLink ? submenu.hreftext : submenu.href;
                        html.AppendLine(string.Format("<a href=\"{0}\" title=\"{1}\"><i class=\"{2} {3} {6}\"></i><span class=\"{5}\">{4}</span>",
                            dataAction, submenu.realTitle, submenu.css, submenu.icon, submenu.realText, classIfParent, ""));
                        if (!string.IsNullOrEmpty(submenu.badgeQuery))
                        {
                            html.AppendLine(string.Format("<span class=\"Badge{0} ajaxBadge badge pull-right {1} inbox-badge\">{{0}}</span>",
                                submenu.id,
                                submenu.badgeColor
                                ));
                        }
                        else if (!string.IsNullOrEmpty(submenu.badgeText))
                        {
                            html.AppendLine(string.Format("<span class=\"badge pull-right {0} inbox-badge\">{1}</span>",
                                submenu.badgeColor,
                                submenu.realBadgeText
                                ));
                        }
                        html.AppendLine("</a>");
                        html.AppendLine(getTreeMenus(submenu, userMenu));
                        html.AppendLine("   </li>");
                    }
                }
                html.AppendLine("</ul>");
            }
            return html.ToString();
        }

        public static string getTreeMenusCrud(ICollection<BaseMenu> menu)
        {
            StringBuilder html = new StringBuilder();
            if (menu.Count > 0)
            {
                html.AppendFormat("<ol>", "");
                foreach (var item in menu.OrderBy(d => d.noOrder))
                {
                    html.AppendFormat("<li class=\"dd-item\" data-id=\"{0}\">", item.id);
                    html.AppendFormat("<div class=\"dd-handle dd3-handle\">", item.id);
                    html.AppendFormat(".");
                    html.AppendFormat("</div>");
                    html.AppendFormat("<div class=\"dd3-content\">");

                    html.AppendFormat(item.text ?? "", "");
                    html.AppendFormat("<div class=\"pull-right\">");
                    html.AppendFormat(" \n<button class=\"btn btn-info btn-xs baseDynamicModal\" data-action=\"{1}\" title=\"{0}\" ><i class=\"fa fa-edit\"></i></button>", ("Edit"), URLHelper.getActionUrl("BaseMenu", "Form") + "/" + item.id.ToString());

                    html.AppendFormat(" \n<button");
                    html.AppendFormat("	data-message=\"{0}\" ", ("Are you sure to delete this menu?"));
                    html.AppendFormat("	data-errormessage=\"{0}\"", ("Could not delete this menu"));
                    html.AppendFormat("	data-title=\"\"", "Delete Menu");
                    html.AppendFormat("	data-buttons=\"[{1}][{0}]\"", ("Yes"), ("No"));
                    html.AppendFormat("	data-btnactions=\"{0}=>%$elem.parent().parent().remove();&{1}=>\"", ("Yes"), ("No"));
                    html.AppendFormat("	class=\"btn btn-danger btn-xs delete\" data-action=\"{0}\" title=\"{1}\">",
                        URLHelper.getActionUrl("BaseMenu", "Delete") + "/" + item.id.ToString(), ("Delete"));
                    html.AppendFormat("	<i class=\"fa fa-trash-o\"></i>");
                    html.AppendFormat("</button>");
                    html.AppendFormat("</div>");

                    html.AppendFormat("</div>");
                    html.AppendLine(getTreeMenusCrud(item.BaseMenu1));
                    html.AppendFormat("</li>");

                }
                html.AppendFormat("</ol>");
            }
            return html.ToString();
        }

        public static string RemoveHtml(string source)
        {
            return Regex.Replace(source, "<.*?>", string.Empty);
        }

    }

    public enum BaseTextBoxIconPosition
    {
        icon_prepend = 0,
        icon_append = 1
    }

    public class BaseTextBoxIcon
    {
        string icon = null;
        public string Icon
        {
            get { return icon; }
            set { icon = value; }
        }
        BaseTextBoxIconPosition position;
        public BaseTextBoxIconPosition Position
        {
            get { return position; }
            set { position = value; }
        }
    }

    public static class MyHtmlHelpers
    {



        public static string colLgText(int length)
        {
            if (length < 13)
                //return string.Format(" class='col col-{0}' ", length);                           
                return string.Format(" col col-lg-{0} col-xs-{0}", length);
            else
                //return " class='col col-10' ";
                return " col col-lg-10 col-xs-10 ";
        }

        public static void BeginControl(StringBuilder html, int length, string labelClass, string labelText, string xtraclass = "")
        {
            html.AppendFormat("<section class=\"{0} {1}\">", colLgText(length), xtraclass);
            if (!string.IsNullOrEmpty(labelText))
            {
                if (labelText.Contains("pbold"))
                {
                    html.AppendFormat("<label class='label'><strong>{0}</strong></label>", (labelText.Replace("pbold", "")));
                }
                else
                {
                    html.AppendFormat("<label class='label'>{0}</label>", (labelText));
                }
            }
            html.AppendFormat("<label class=\"{0}\"> ", labelClass);
        }
        public static void BeginControl2(StringBuilder html, int length, string labelClass, string labelText, string xtraclass = "")
        {
            html.AppendFormat("<section class=\"{0} {1}\">", colLgText(length), xtraclass);
            if (!string.IsNullOrEmpty(labelText))
                html.AppendFormat("<label class=\"{0}\"><label class='label'>{1}</label> ", labelClass, (labelText));
        }

        public static void EndControl(StringBuilder html, string note = "")
        {
            html.AppendFormat("</label>");
            html.AppendFormat("<div class=\"note\">");
            html.AppendFormat(note);
            html.AppendFormat("</div>");
            html.AppendFormat("</section>");
        }
        public static IDictionary<string, object> RefillAttrs(object htmlAttributes, string mask, string validation)
        {
            IDictionary<string, object> AttrsBefore = new RouteValueDictionary(htmlAttributes);
            Dictionary<string, object> Attrs = new Dictionary<string, object>();
            foreach (var item in AttrsBefore)
            {
                Attrs.Add(item.Key.Replace("_", "-"), item.Value);
            }
            if (!Attrs.ContainsKey("autocomplete"))
                Attrs.Add("autocomplete", "off");

            if (!string.IsNullOrEmpty(mask))
            {
                string[] placeMask = mask.Split(';');
                Attrs.Add("data-mask", placeMask[0]);
                if (placeMask.Count() > 1)
                    Attrs.Add("data-mask-placeholder", placeMask[1]);
            }
            if (!string.IsNullOrEmpty(validation))
            {
                Attrs.Add("data-validation", validation);
            }
            return Attrs;
        }
        public enum TooltipDir
        {
            right,
            left,
            top,
            bottom
        }
        public enum BaseSelector
        {
            radio = 0,
            checkbox = 1
        }
        public class BaseToolTip
        {

            public BaseToolTip(string pText = "")
            {
                this.text = pText;
            }
            TooltipDir vertical = TooltipDir.top, horizontal = TooltipDir.right;

            public TooltipDir Horizontal
            {
                get { return horizontal; }
                set { horizontal = value; }
            }

            public TooltipDir Vertical
            {
                get { return vertical; }
                set { vertical = value; }
            }

            string text, icon = BaseUIIcon.fa.fa_fa_exclamation, colorClass = "txt-color-blue";

            public string ColorClass
            {
                get { return colorClass; }
                set { colorClass = value; }
            }

            public string Text
            {
                get { return text; }
                set { text = value; }
            }

            public void set(StringBuilder html)
            {
                html.AppendFormat("<b class=\"tooltip tooltip-{1}-{0}\">", horizontal.ToString(), vertical.ToString());
                html.AppendFormat("	<i class=\"{0} {1}\"></i> ", icon, colorClass);
                html.AppendFormat((text));
                html.AppendFormat("</b>");
            }

            public BaseToolTip clone(string newText)
            {
                BaseToolTip bushin = new BaseToolTip();
                bushin.colorClass = this.colorClass;
                bushin.horizontal = this.horizontal;
                bushin.icon = this.icon;
                bushin.text = newText;
                bushin.vertical = this.vertical;
                return bushin;
            }
        }
        public enum BaseScaleSliderType
        {
            single = 0,
            @double = 1
        }
        public enum sliderClassColor
        {
            danger = 0,
            success = 1,
            warning = 2,
            info = 3
        }
        public enum sliderHandle
        {
            round = 0,
            triangle = 1,
            squar = 2,
        }
        public enum sliderSelection
        {
            after = 0,
            before = 1
        }
        public enum spinnerDirection
        {
            left = 0,
            right = 1,
            both = 2
        }

        public static string generateHidden(string name, object value)
        {
            return "";//string.Format("<input type='hidden' name='{0}' id='{0}' value='{1}'/>",name+"_old",value);
        }

        public static MvcHtmlString BaseTextBox<TModel>(this HtmlHelper<TModel> htmlHelper, string name,
            object value, string label, object icons, int col, object htmlAttributes, string validations, string note = "", bool textArea = false, BaseToolTip tooltip = null,
            string xtraClassSection = "")
        {
            value = value == null ? "" : value;
            IDictionary<string, object> Ricons = new RouteValueDictionary(icons);
            StringBuilder html = new StringBuilder();

            IDictionary<string, object> attrs = RefillAttrs(htmlAttributes, "", validations);
            BeginControl(html, col, textArea ? "textarea" : "input", label, xtraClassSection);
            if (icons != null)
            {
                foreach (var item in Ricons)
                {
                    BaseTextBoxIcon icon = (BaseTextBoxIcon)item.Value;
                    html.AppendFormat("<i class=\"{1} {0}\"></i>", icon.Icon, icon.Position.ToString().Replace("_", "-"));
                }
            }

            if (textArea)
            {
                try
                {
                    html.AppendFormat(htmlHelper.TextArea(name, value.ToString().Replace("\"", "\\\""), attrs).ToHtmlString());
                }
                catch
                {

                }
            }
            else
                html.AppendFormat(htmlHelper.TextBox(name, value.ToString().Replace("\"", "\\\""), attrs).ToHtmlString());
            if (tooltip != null)
                tooltip.set(html);
            EndControl(html, note);
            html.AppendLine(generateHidden(name, value));
            return MvcHtmlString.Create(html.ToString());
        }
        public static MvcHtmlString BasePassword<TModel>(this HtmlHelper<TModel> htmlHelper, string name,
            object value, string label, object icons, int col, object htmlAttributes, string validations, string note = "", bool textArea = false, BaseToolTip tooltip = null,
            string xtraClassSection = "")
        {
            value = value == null ? "" : value;
            IDictionary<string, object> Ricons = new RouteValueDictionary(icons);
            StringBuilder html = new StringBuilder();

            IDictionary<string, object> attrs = RefillAttrs(htmlAttributes, "", validations);

            BeginControl(html, col, textArea ? "textarea" : "input", label, xtraClassSection);
            if (icons != null)
            {
                foreach (var item in Ricons)
                {
                    BaseTextBoxIcon icon = (BaseTextBoxIcon)item.Value;
                    html.AppendFormat("<i class=\"{1} {0}\"></i>", icon.Icon, icon.Position.ToString().Replace("_", "-"));
                }
            }
            html.AppendFormat(htmlHelper.Password(name, value, attrs).ToHtmlString());
            if (tooltip != null)
                tooltip.set(html);
            EndControl(html, note);
            return MvcHtmlString.Create(html.ToString());
        }
        public static MvcHtmlString BaseTextBox<TModel>(
            this HtmlHelper<TModel> htmlHelper,
            string name,
            object value,
            object icons,
            int col,
            object htmlAttributes,
            string validations = "",
            string note = "",
            bool textArea = false,
            BaseToolTip tooltip = null,
            string mask = "",
            string xtraClassSection = "")
        {
            value = value == null ? "" : value;
            IDictionary<string, object> Ricons = new RouteValueDictionary(icons);
            IDictionary<string, object> attrs = RefillAttrs(htmlAttributes, "", validations);

            StringBuilder html = new StringBuilder();
            BeginControl(html, col, textArea ? "textarea" : "input", "", xtraClassSection);
            if (icons != null)
            {
                foreach (var item in Ricons)
                {
                    BaseTextBoxIcon icon = (BaseTextBoxIcon)item.Value;
                    html.AppendFormat("<i class=\"{1} {0}\"></i>", icon.Icon, icon.Position.ToString().Replace("_", "-"));
                }
            }
            if (textArea)
                html.AppendFormat(htmlHelper.TextArea(name, value.ToString(), attrs).ToHtmlString());
            else
                html.AppendFormat(htmlHelper.TextBox(name, value, attrs).ToHtmlString());
            if (tooltip != null)
                tooltip.set(html);
            EndControl(html, note);
            html.AppendLine(generateHidden(name, value));
            return MvcHtmlString.Create(html.ToString());
        }

        public static MvcHtmlString BaseTextBox<TModel>(
            this HtmlHelper<TModel> htmlHelper,
            string name,
            object value,
            object icons,
            int col,
            string validations = "",
            string note = "",
            bool textArea = false, BaseToolTip tooltip = null,
            string xtraClassSection = "")
        {
            value = value == null ? "" : value;
            IDictionary<string, object> Ricons = new RouteValueDictionary(icons);
            StringBuilder html = new StringBuilder();
            BeginControl(html, col, textArea ? "textarea" : "input", Helper.Capitalize(name), xtraClassSection);
            if (icons != null)
            {
                foreach (var item in Ricons)
                {
                    BaseTextBoxIcon icon = (BaseTextBoxIcon)item.Value;
                    html.AppendFormat("<i class=\"{1} {0}\"></i>", icon.Icon, icon.Position.ToString().Replace("_", "-"));
                }
            }
            if (textArea)
                html.AppendFormat(htmlHelper.TextArea(name, value.ToString()).ToHtmlString());
            else
                html.AppendFormat(htmlHelper.TextBox(name, value, new { autocomplete = "off" }).ToHtmlString());
            if (tooltip != null)
                tooltip.set(html);
            EndControl(html, note);
            html.AppendLine(generateHidden(name, value));
            return MvcHtmlString.Create(html.ToString());
        }
        public static MvcHtmlString BaseTextBox<TModel>(
            this HtmlHelper<TModel> htmlHelper,
            string name,
            object value,
            object icons,
            string validations = "",
            string note = "",
            bool textArea = false, BaseToolTip tooltip = null,
            string xtraClassSection = "")
        {
            value = value == null ? "" : value;
            IDictionary<string, object> Ricons = new RouteValueDictionary(icons);
            StringBuilder html = new StringBuilder();
            BeginControl(html, 6, textArea ? "textarea" : "input", (name), xtraClassSection);
            if (icons != null)
            {
                foreach (var item in Ricons)
                {
                    BaseTextBoxIcon icon = (BaseTextBoxIcon)item.Value;
                    html.AppendFormat("<i class=\"{1} {0}\"></i>", icon.Icon, icon.Position.ToString().Replace("_", "-"));
                }
            }
            if (textArea)
                html.AppendFormat(htmlHelper.TextArea(name, value.ToString()).ToHtmlString());
            else
                html.AppendFormat(htmlHelper.TextBox(name, value, new { autocomplete = "off" }).ToHtmlString());
            if (tooltip != null)
                tooltip.set(html);
            EndControl(html, note);
            html.AppendLine(generateHidden(name, value));
            return MvcHtmlString.Create(html.ToString());
        }


        public static MvcHtmlString BaseTextBox<TModel>(
            this HtmlHelper<TModel> htmlHelper,
            string name,
            object value,
            int col,
            string label,
            string validations = "",
            string note = "",
            bool textArea = false, BaseToolTip tooltip = null,
            string xtraClassSection = "")
        {
            value = value == null ? "" : value;
            StringBuilder html = new StringBuilder();
            BeginControl(html, col, textArea ? "textarea" : "input", (label), xtraClassSection);
            if (textArea)
                html.AppendFormat(htmlHelper.TextArea(name, value.ToString()).ToHtmlString());
            else
                html.AppendFormat(htmlHelper.TextBox(name, value, new { autocomplete = "off" }).ToHtmlString());
            if (tooltip != null)
                tooltip.set(html);
            EndControl(html, note);
            html.AppendLine(generateHidden(name, value));
            return MvcHtmlString.Create(html.ToString());
        }

        public static MvcHtmlString BaseTextBox<TModel>(
            this HtmlHelper<TModel> htmlHelper,
            string name,
            object value,
            string validations = "",
            string note = "",
            bool textArea = false, BaseToolTip tooltip = null,
            string xtraClassSection = "")
        {
            value = value == null ? "" : value;
            StringBuilder html = new StringBuilder();
            BeginControl(html, 6, textArea ? "textarea" : "input", (name), xtraClassSection);
            if (textArea)
                html.AppendFormat(htmlHelper.TextArea(name, value.ToString()).ToHtmlString());
            else
                html.AppendFormat(htmlHelper.TextBox(name, value, new { autocomplete = "off" }).ToHtmlString());
            if (tooltip != null)
                tooltip.set(html);
            EndControl(html, note);
            html.AppendLine(generateHidden(name, value));
            return MvcHtmlString.Create(html.ToString());
        }
        public static MvcHtmlString BaseFile<TModel>(
            this HtmlHelper<TModel> htmlHelper, string name, string label, string placeHolder, string @class, int length, string icon, string validations = "", string note = "",
            string xtraClassSection = "", BaseToolTip tooltip = null)
        {
            BaseConfiguration config = new BaseConfiguration();
            StringBuilder html = new StringBuilder();
            html.AppendFormat("<section class=\"{0}\">", xtraClassSection);
            html.AppendFormat("	<label class=\"label\">{0}</label>", (label));
            html.AppendFormat("	<label class=\"input input-file\" for=\"file\">");
            if (!string.IsNullOrEmpty(icon))
                html.AppendFormat("<i class=\"{1} {0}\"></i>", icon, BaseTextBoxIconPosition.icon_prepend.ToString().Replace("_", "-"));
            html.AppendFormat("	<div class=\"button{2}\"><input {4} class='fileInput " + config.primaryColor + "' type=\"file\" onchange=\"this.parentNode.nextSibling.value = this.value\" name=\"{3}\" id=\"{3}\">{0}</div><input class='fileInput btn-" + config.primaryColor + "' name=\"{3}fileName\" type=\"text\" readonly=\"\" placeholder=\"         {1}\">", ("Browse"), placeHolder, @class, name, !string.IsNullOrEmpty(validations) ? "data-validation=\"" + validations + "\"" : "");

            if (tooltip != null)
                tooltip.set(html);

            html.AppendFormat("	</label>");
            html.AppendFormat("</section>");
            return MvcHtmlString.Create(html.ToString());
        }

        public static MvcHtmlString BaseFileMobile<TModel>(
            this HtmlHelper<TModel> htmlHelper, string name, string label, string placeHolder, string @class, int length, string icon, string validations = "", string note = "",
            string xtraClassSection = "", BaseToolTip tooltip = null)
        {
            BaseConfiguration config = new BaseConfiguration();
            StringBuilder html = new StringBuilder();
            html.AppendFormat("<section class=\"{0}\">", xtraClassSection);
            html.AppendFormat("	<label class=\"label\">{0}</label>", (label));
            html.AppendFormat("	<label class=\"input input-file\" for=\"file\">");
            if (!string.IsNullOrEmpty(icon))
                html.AppendFormat("<i class=\"{1} {0}\"></i>", icon, BaseTextBoxIconPosition.icon_prepend.ToString().Replace("_", "-"));
            html.AppendFormat("	<div class=\"button{2}\"><input {4} class='fileInput " + config.primaryColor + "' type=\"file\" onchange=\"this.parentNode.nextSibling.value = this.value\" name=\"{3}\" id=\"{3}\">{0}</div><input class='fileInput btn-" + config.primaryColor + "' name=\"{3}fileName\" type=\"text\" readonly=\"\" placeholder=\"{1}\" accept=\"image/*;capture=camera\"  >", ("Browse"), placeHolder, @class, name, !string.IsNullOrEmpty(validations) ? "data-validation=\"" + validations + "\"" : "");

            if (tooltip != null)
                tooltip.set(html);

            html.AppendFormat("	</label>");
            html.AppendFormat("</section>");
            return MvcHtmlString.Create(html.ToString());
        }

        public static MvcHtmlString BaseFile<TModel>(
            this HtmlHelper<TModel> htmlHelper, string name, string label, string placeHolder, int length, string icon, string validations = "", string note = "",
            string xtraClassSection = "", BaseToolTip tooltip = null)
        {
            BaseConfiguration config = new BaseConfiguration();
            StringBuilder html = new StringBuilder();
            html.AppendFormat("<section class=\"{0} {1}\">", colLgText(length), xtraClassSection);
            if (!string.IsNullOrEmpty(label))
                html.AppendFormat("	<label class=\"label\">{0}</label>", (label));
            html.AppendFormat("	<label class=\"input input-file\" for=\"file\">");
            if (!string.IsNullOrEmpty(icon))
                html.AppendFormat("<i class=\"{1} {0}\"></i>", icon, BaseTextBoxIconPosition.icon_prepend.ToString().Replace("_", "-"));
            //html.AppendFormat("	<div class=\"button{2}\"><input class='fileInput' type=\"file\" onchange=\"this.parentNode.nextSibling.value = this.value\" name=\"{3}\" id=\"{3}\">{0}</div><input class='fileInput' type=\"text\" readonly=\"\" placeholder=\"         {1}\">", ("Browse"), placeHolder, "", name);
            html.AppendFormat("	<div class=\"button{2}\"><input {4} class='fileInput btn-" + config.primaryColor + "' type=\"file\" onchange=\"this.parentNode.nextSibling.value = this.value\" name=\"{3}\" id=\"{3}\">{0}</div><input class='fileInput " + config.primaryColor + "' name=\"{3}fileName\" type=\"text\" readonly=\"\" placeholder=\"         {1}\">", ("Browse"), placeHolder, "", name, !string.IsNullOrEmpty(validations) ? "data-validation=\"" + validations + "\"" : "");
            if (tooltip != null)
                tooltip.set(html);
            html.AppendFormat("	</label>");
            html.AppendFormat("</section>");
            return MvcHtmlString.Create(html.ToString());
        }



        public static MvcHtmlString BaseFile<TModel>(
            this HtmlHelper<TModel> htmlHelper, string name, string label, int length, string icon, string validations = "", string note = "",
            string xtraClassSection = "", string value = "", string controller = "", BaseToolTip tooltip = null)
        {
            BaseConfiguration config = new BaseConfiguration();
            StringBuilder html = new StringBuilder();
            html.AppendFormat("<section class=\"{0} {1}\">", colLgText(length), xtraClassSection);
            if (!string.IsNullOrEmpty(label))
                html.AppendFormat("	<label class=\"label\">{0}</label>", (label));
            html.AppendFormat("	<label class=\"input input-file\" for=\"file\">");
            if (!string.IsNullOrEmpty(icon))
                html.AppendFormat("<i class=\"{1} {0}\"></i>", icon, BaseTextBoxIconPosition.icon_prepend.ToString().Replace("_", "-"));
            html.AppendFormat("	<div class=\"button{2} btn-" + config.primaryColor + "\"><input   type=\"file\" onchange=\"this.parentNode.nextSibling.value = this.value\" name=\"{3}\" id=\"{3}\">{0}</div><input {4} class='fileInput " + config.primaryColor + "' name=\"{3}fileName\" type=\"text\" readonly=\"\" placeholder=\"         {1}\">", ("Browse"), ("Include some files"), "", name, !string.IsNullOrEmpty(validations) ? "data-validation=\"" + validations + "\"" : "");
            html.AppendFormat("	</label>");
            if (!string.IsNullOrEmpty(value))
            {
                html.AppendFormat("<a class=\"btn btn-link\" data-target=\"#{0}Preview\" data-toggle=\"modal\" href=\"javascript:void(0);\">Preview</a>", name);

                html.AppendFormat("<div class=\"modal fade showmore\" id='{0}Preview'>", name);
                html.AppendFormat("             <div class=\"modal-dialog\">");
                html.AppendFormat("                  <div class=\"modal-content\">");
                html.AppendFormat("                      <div class='modal-header'>");
                html.AppendFormat("                        <button type='button' class='close' data-dismiss='modal' aria-hidden='true'>    &times;</button>");
                html.AppendFormat("                          <h4 class='modal-title' id='myModalLabel'>{0}'s document</h4>", Helper.Capitalize(name));
                html.AppendFormat("                                    </div>");
                html.AppendFormat("                                    <div class='modal-body'>");
                html.AppendFormat("                                        <object data=\"files/{0}/{1}/{2}\" style=\"width: 100%; min-height: 400px;\"></object>", controller, name, value);
                html.AppendFormat("                                    </div>");
                html.AppendFormat("                                </div>");
                html.AppendFormat("                            </div>");
                html.AppendFormat("                        </div>");


            }

            html.AppendFormat("</section>");
            return MvcHtmlString.Create(html.ToString());
        }


        public static MvcHtmlString BaseDropDownList<TModel>(
             this HtmlHelper<TModel> htmlHelper,
           string name,
           string tableName,
           string whereCondition,
           string label,
           string optionLabel,
           List<object> selectds,
           string validations = "",
            string groupby = "",
           string note = "")
        {
            return BaseDropDownList(htmlHelper, name, tableName, "id", "name", whereCondition, 6, label, optionLabel, "", Admin.BaseClass.UI.BaseUIIcon.fa.fa_fa_arrow_circle_down, selectds, false, "Refresh", groupby, validations);
        }

        public static MvcHtmlString BaseDropDownList<TModel>(
             this HtmlHelper<TModel> htmlHelper,
           string name,
           string tableName,
           string valueColumn,
           string textColumn,
           string whereCondition,
           string label,
           string optionLabel,
           List<object> selectds,
           string groupby = "",
           string note = "",
            string xtraClassSection = "")
        {
            return BaseDropDownList(htmlHelper, name, tableName, valueColumn, textColumn, whereCondition, 6, label, optionLabel, "",
                Admin.BaseClass.UI.BaseUIIcon.fa.fa_fa_arrow_circle_down, selectds, false, "Refresh", groupby, "");
        }


        public static MvcHtmlString BaseDropDownList<TModel>(
           this HtmlHelper<TModel> htmlHelper,
           string name,
           string tableName,
           string valueColumn,
           string textColumn,
           string whereCondition,
           int col,
           string label,
           string optionLabel,
           object @class,
           string icon,
           List<object> selectds,
           bool multiple,
           string refresh,
           string groupby = "",
           string validations = "",
           string note = "",
           object htmlAttributes = null,
           string xtraClassSection = "",
           bool na = false)
        {
            BaseConfiguration config = new BaseConfiguration();
            selectds = selectds == null ? new List<object>() : selectds;
            IDictionary<string, object> Ricons = new RouteValueDictionary(htmlAttributes);
            string attrs = "";
            foreach (KeyValuePair<string, object> item in Ricons)
            {
                attrs += string.Format(" {0}=\"{1}\" ", item.Key.Replace("_", "-"), item.Value);
            }
            if (!string.IsNullOrEmpty(validations))
            {
                attrs += string.Format(" data-validation=\"{0}\" ", validations);
            }
            StringBuilder html = new StringBuilder();
            html.AppendFormat("<section class=\"{0} {1}\">", colLgText(col), xtraClassSection);
            if (!string.IsNullOrEmpty(label))
            {
                if (label.Contains("pbold"))
                {
                    html.AppendFormat("<label class='label'><strong>{0}</strong></label>", (label.Replace("pbold", "")));
                }
                else
                {
                    html.AppendFormat("<label class='label'>{0}</label>", (label));
                }
            }
            html.AppendFormat("<div>");
            html.AppendFormat("<select {10} name=\"{8}\" id=\"{8}\"  {6} data-optionlabel=\"{9}\" data-action=\"{7}\" class='baseSelect {0} {5} select2' data-value=\"{1}\" data-text=\"{2}\" data-table=\"{3}\" data-where=\"{4}\" data-groupby=\"{11}\">",
                @class, valueColumn, textColumn, tableName, whereCondition,
                (multiple) ? "custom-scroll" : "", (multiple) ? "multiple=\"\"" : "", URLHelper.getActionUrl("BaseGeneral", "UpdateDropDown"), name, optionLabel, attrs, groupby);
            Context context = new Context();
            DataTable @default = Helper.getData(string.Format("select  top 1 COLUMN_NAME,DATA_TYPE from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME='{0}' and DATA_TYPE='nvarchar'", tableName), context);
            if (@default.Rows.Count == 0)
            {
                @default = Helper.getData(string.Format("select COLUMN_NAME,DATA_TYPE from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME='{0}' and COLUMN_NAME='name'", tableName), context);
            }
            string orderRichBy = "";
            if (@default.Rows.Count > 0)
                orderRichBy = " order by " + @default.Rows[0][0].ToString();


            DataTable data = Helper.getData(string.Format("SELECT {0} as value, {1} as text {4} from {2} {3} {6} {5}",
                valueColumn, textColumn, tableName, whereCondition
                , !string.IsNullOrEmpty(groupby) ? " ," + groupby + " as groupby " : ""
                , !whereCondition.ToLower().Contains("order by") ? (!string.IsNullOrEmpty(groupby) ? " order by " + groupby : "") : ""
                , string.IsNullOrEmpty(groupby) && !whereCondition.ToLower().Contains("order by") ? orderRichBy : ""), context);
            if (!multiple && !string.IsNullOrEmpty(optionLabel))
                html.AppendFormat("	<option value=\"\">{0}</option>", !string.IsNullOrEmpty(optionLabel) ? (optionLabel) : "");

            string groupCheck = "";
            bool first = true;
            if (na)
            {
                bool selected = selectds.Count(d => d.ToString() == "NULL") > 0;
                html.AppendFormat("	<option value=\"{0}\" {2} >{1}</option>", "NULL", "Not Sets", selected ? "selected" : "");
            }
            foreach (DataRow item in data.Rows)
            {
                if (!string.IsNullOrEmpty(groupby))
                {
                    if (groupCheck != item["groupby"].ToString())
                    {
                        groupCheck = item["groupby"].ToString();
                        if (!first)
                            html.AppendFormat("</optgroup>");
                        html.AppendFormat("<optgroup label=\"{0}\">", item["groupby"].ToString());
                        first = false;
                    }
                }
                bool selected = selectds.Count(d => d.ToString() == item["value"].ToString()) > 0;
                html.AppendFormat("	<option value=\"{0}\" {2} >{1}</option>", item["value"], item["text"], selected ? "selected" : "");
            }
            html.AppendFormat("</select>");
            html.AppendFormat("</div>");
            html.AppendFormat("<div>");
            if (string.IsNullOrEmpty(refresh))
            {
                html.AppendFormat("<button class=\"btn btn-" + config.primaryColor + " baseDropDownRefresh\" >");
                html.AppendFormat("	<i class=\"{1}\"></i>{0}", " ", BaseUIIcon.fa.fa_fa_refresh);
                html.AppendFormat("</button>");
            }
            html.AppendFormat(note);
            html.AppendFormat("</div>");
            html.AppendFormat("</section>");
            return MvcHtmlString.Create(html.ToString());
        }

        public static MvcHtmlString BaseDropDownListExtra<TModel>(
           this HtmlHelper<TModel> htmlHelper,
           string name,
           string tableName,
           string valueColumn,
           string textColumn,
           string whereCondition,
           int col,
           string label,
           string optionLabel,
           object @class,
           string icon,
           List<object> selectds,
           bool multiple,
           string refresh,
           string groupby = "",
           string validations = "",
           string note = "",
           object htmlAttributes = null,
           string xtraClassSection = "",
           string extraTextColumn = "")
        {
            selectds = selectds == null ? new List<object>() : selectds;
            IDictionary<string, object> Ricons = new RouteValueDictionary(htmlAttributes);
            string attrs = "";
            foreach (KeyValuePair<string, object> item in Ricons)
            {
                attrs += string.Format(" {0}=\"{1}\" ", item.Key.Replace("_", "-"), item.Value);
            }
            if (!string.IsNullOrEmpty(validations))
            {
                attrs += string.Format(" data-validation=\"{0}\" ", validations);
            }

            StringBuilder html = new StringBuilder();
            html.AppendFormat("<section class=\"{0} {1}\">", colLgText(col), xtraClassSection);
            if (!string.IsNullOrEmpty(label))
                html.AppendFormat("<label class='label'>{0}</label>", (label));
            html.AppendFormat("<div>");
            html.AppendFormat("<select {10} name=\"{8}\" id=\"{8}\"  {6} data-optionlabel=\"{9}\" data-action=\"{7}\" class='baseSelect {0} {5} select2' data-value=\"{1}\" data-text=\"{2}\" data-table=\"{3}\" data-where=\"{4}\" data-groupby=\"{11}\">",
                @class, valueColumn, textColumn, tableName, whereCondition,
                (multiple) ? "custom-scroll" : "", (multiple) ? "multiple=\"\"" : "", URLHelper.getActionUrl("BaseGeneral", "UpdateDropDown"), name, optionLabel, attrs, groupby);

            Context context = new Context();
            string extraColumnConsulting = extraTextColumn != "" ? ", " + extraTextColumn + " as extraColumn" : "";
            DataTable data = Helper.getData(string.Format("SELECT {0} as value, {1} as text {4} " + extraColumnConsulting + " from {2} {3} {5}",
                valueColumn, textColumn, tableName, whereCondition
                , !string.IsNullOrEmpty(groupby) ? " ," + groupby + " as groupby " : "",
                !string.IsNullOrEmpty(groupby) ? " order by " + groupby : ""), context);
            if (!multiple && !string.IsNullOrEmpty(optionLabel))
                html.AppendFormat("	<option value=\"\">{0}</option>", !string.IsNullOrEmpty(optionLabel) ? (optionLabel) : "");

            string groupCheck = "";
            bool first = true;
            foreach (DataRow item in data.Rows)
            {
                if (!string.IsNullOrEmpty(groupby))
                {
                    if (groupCheck != item["groupby"].ToString())
                    {
                        groupCheck = item["groupby"].ToString();
                        if (!first)
                            html.AppendFormat("</optgroup>");
                        html.AppendFormat("<optgroup label=\"{0}\">", item["groupby"].ToString());
                        first = false;
                    }
                }
                bool selected = selectds.Count(d => d.ToString() == item["value"].ToString()) > 0;
                string textToShow = item["text"].ToString() + " (" + item["extraColumn"].ToString() + ")";
                html.AppendFormat("	<option value=\"{0}\" {2} >{1}</option>", item["value"], textToShow, selected ? "selected" : "");
            }
            html.AppendFormat("</select>");
            html.AppendFormat("</div>");
            html.AppendFormat("<div>");
            if (!string.IsNullOrEmpty(refresh))
            {
                html.AppendFormat("<button class=\"btn btn-success baseDropDownRefresh\" >");
                html.AppendFormat("	<i class=\"{1}\"></i>{0}", (refresh), BaseUIIcon.fa.fa_fa_refresh);
                html.AppendFormat("</button>");
            }
            html.AppendFormat(note);
            html.AppendFormat("</div>");
            html.AppendFormat("</section>");
            return MvcHtmlString.Create(html.ToString());
        }

        public static MvcHtmlString BaseDropDownList<TModel>(
           this HtmlHelper<TModel> htmlHelper,
           string name,
           Dictionary<string, string> data,
           int col,
           string label,
           string optionLabel,
           object @class,
           string icon,
           List<object> selectds,
           bool multiple,
           string validations = "",
           string note = "",
            string xtraClassSection = "")
        {

            StringBuilder html = new StringBuilder();
            BeginControl(html, col, "select " + (multiple ? "select-multiple" : ""), label, xtraClassSection);
            string attrs = "";
            if (!string.IsNullOrEmpty(validations))
            {
                attrs += string.Format(" data-validation=\"{0}\" ", validations);
            }
            html.AppendFormat("<select {10} name=\"{8}\" id=\"{8}\"  {6} data-optionlabel=\"{9}\" data-action=\"{7}\" class='baseSelect {0} {5} select2 " + xtraClassSection + "' data-value=\"{1}\" data-text=\"{2}\" data-table=\"{3}\" data-where=\"{4}\">",
                @class, "", "", "", "",
                (multiple) ? "custom-scroll" : "", (multiple) ? "multiple=\"\"" : "", URLHelper.getActionUrl("BaseGeneral", "UpdateDropDown"), name, optionLabel, attrs);

            Context context = new Context();
            if (!multiple)
                html.AppendFormat("	<option selected disabled=\"\" value=\"0\">{0}</option>", !string.IsNullOrEmpty(optionLabel) ? (optionLabel) : "");

            foreach (KeyValuePair<string, string> item in data)
            {
                bool selected = selectds.Count(d => d.ToString() == item.Key) > 0;
                html.AppendFormat("	<option value=\"{0}\" {2} >{1}</option>", item.Key, item.Value, selected ? "selected" : "");
            }
            html.AppendFormat("</select>");
            if (!string.IsNullOrEmpty(icon) && !multiple)
                html.AppendFormat("<i class=\"{0}\"></i>", icon);

            html.AppendFormat("</label>");
            html.AppendFormat("<div class=\"{0}\">", !string.IsNullOrEmpty(note) ? "note" : "");
            html.AppendFormat(note);
            html.AppendFormat("</div>");
            html.AppendFormat("</section>");
            return MvcHtmlString.Create(html.ToString());
        }


        public static MvcHtmlString BaseDropDownListNoDisabled<TModel>(
          this HtmlHelper<TModel> htmlHelper,
          string name,
          Dictionary<string, string> data,
          int col,
          string label,
          string optionLabel,
          object @class,
          string icon,
          List<object> selectds,
          bool multiple,
          string validations = "",
          string note = "",
           string xtraClassSection = "")
        {

            StringBuilder html = new StringBuilder();
            BeginControl(html, col, "select " + (multiple ? "select-multiple" : ""), label, xtraClassSection);
            string attrs = "";
            if (!string.IsNullOrEmpty(validations))
            {
                attrs += string.Format(" data-validation=\"{0}\" ", validations);
            }
            html.AppendFormat("<select {10} name=\"{8}\" id=\"{8}\"  {6} data-optionlabel=\"{9}\" data-action=\"{7}\" class='baseSelect {0} {5} select2 " + xtraClassSection + "' data-value=\"{1}\" data-text=\"{2}\" data-table=\"{3}\" data-where=\"{4}\">",
                @class, "", "", "", "",
                (multiple) ? "custom-scroll" : "", (multiple) ? "multiple=\"\"" : "", URLHelper.getActionUrl("BaseGeneral", "UpdateDropDown"), name, optionLabel, attrs);

            Context context = new Context();
            if (!multiple)
                html.AppendFormat("	<option selected value=\"0\">{0}</option>", !string.IsNullOrEmpty(optionLabel) ? (optionLabel) : "");

            foreach (KeyValuePair<string, string> item in data)
            {
                bool selected = selectds.Count(d => d.ToString() == item.Key) > 0;
                html.AppendFormat("	<option value=\"{0}\" {2} >{1}</option>", item.Key, item.Value, selected ? "selected" : "");
            }
            html.AppendFormat("</select>");
            if (!string.IsNullOrEmpty(icon) && !multiple)
                html.AppendFormat("<i class=\"{0}\"></i>", icon);

            html.AppendFormat("</label>");
            html.AppendFormat("<div class=\"{0}\">", !string.IsNullOrEmpty(note) ? "note" : "");
            html.AppendFormat(note);
            html.AppendFormat("</div>");
            html.AppendFormat("</section>");
            return MvcHtmlString.Create(html.ToString());
        }


        public static MvcHtmlString BaseSelectorList<TModel>(
            this HtmlHelper<TModel> htmlHelper,
            BaseSelector type,
            string name,
           string tableName,
           string valueColumn,
           string textColumn,
           string whereCondition,
           int col,
           int row,
           string label,
           object selectd,
           string refresh = "Refresh",
           string toggleON = "",
           string toggleOff = "",
           string validations = "",
           object htmlAttributes = null,
           string note = "",
            string xtraClassSection = "")
        {
            int coler = 12 / (col > 4 ? 4 : col);
            bool toggle = !string.IsNullOrEmpty(toggleOff) && !string.IsNullOrEmpty(toggleON);
            StringBuilder html = new StringBuilder();
            html.AppendFormat("<section>");
            html.AppendFormat("<label class=\"label\">{0}</label>", label);

            IDictionary<string, object> Ricons = new RouteValueDictionary(htmlAttributes);
            string attrs = "";
            foreach (KeyValuePair<string, object> item in Ricons)
            {
                attrs += string.Format(" {0}=\"{1}\" ", item.Key, item.Value);
            }
            if (!string.IsNullOrEmpty(validations))
            {
                attrs += string.Format(" data-validation=\"{0}\" ", validations);
            }

            Context context = new Context();
            DataTable data = Helper.getData(string.Format("SELECT {0} as value, {1} as text from {2} {3}", valueColumn, textColumn, tableName, whereCondition), context);
            html.AppendFormat("<div class=\"row baseSelectorButtons\" data-action=\"{0}\" data-value=\"{1}\" data-text=\"{2}\"  data-table=\"{3}\" data-where=\"{4}\" data-col=\"{5}\" data-row=\"{6}\" data-toggleoff=\"{7}\" data-toggleon=\"{8}\" data-name=\"{9}\" data-type=\"{10}\" data-toggle=\"{11}\" >",
                URLHelper.getActionUrl("BaseGeneral", "UpdateRadioButton"), valueColumn, textColumn, tableName, whereCondition, col, row, toggleOff, toggleON, name, type.ToString(), toggle);

            int index = 1;
            bool wasClose = true;
            foreach (DataRow item in data.Rows)
            {
                if (index == 1 && wasClose)
                {
                    html.AppendFormat("	<div class=\"col col-{0}\">", coler);
                    wasClose = false;
                }
                html.AppendFormat("<label class=\"{0}\">", toggle ? "toggle" : type.ToString());
                html.AppendFormat("<input {4} type=\"{3}\" {0} name=\"{1}\" value=\"{2}\" >", selectd.ToString() == item["value"].ToString() ? "checked=\"checked\"" : "", name, item["value"].ToString(), type.ToString(), attrs);
                if (toggle)
                    html.AppendFormat("<i data-swchoff-text=\"{0}\" data-swchon-text=\"{1}\"></i>", (toggleOff), (toggleON));
                else
                    html.AppendFormat("<i></i>");
                html.AppendFormat("{0}</label>", item["text"]);
                if (index % row == 0)
                {
                    html.AppendFormat("</div>");
                    wasClose = true;
                }
            }
            html.AppendFormat("</div>");
            html.AppendFormat("<div class=\"{0}\">", !string.IsNullOrEmpty(note) ? "note" : "");
            if (!string.IsNullOrEmpty(refresh))
            {
                html.AppendFormat("<button class=\"btn btn-success baseSelectorButtonsRefresh\" >");
                html.AppendFormat("	<i class=\"{1}\"></i>{0}", (refresh), BaseUIIcon.fa.fa_fa_refresh);
                html.AppendFormat("</button>");
            }
            html.AppendFormat(note);
            html.AppendFormat("</div>");
            html.AppendFormat("</section>");
            return MvcHtmlString.Create(html.ToString());
        }

        public static MvcHtmlString BaseSelectorList<TModel>(
            this HtmlHelper<TModel> htmlHelper,
            BaseSelector type,
            string name,
            Dictionary<string, string> data,
           int col,
           int row,
           string label,
           object selectd,
           string toggleON = "",
           string toggleOff = "",
           string validations = "",
           object htmlAttributes = null,
           string note = "",
            string xtraClassSection = "")
        {
            IDictionary<string, object> Ricons = new RouteValueDictionary(htmlAttributes);
            string attrs = "";
            foreach (KeyValuePair<string, object> item in Ricons)
            {
                attrs += string.Format(" {0}=\"{1}\" ", item.Key, item.Value);
            }
            if (!string.IsNullOrEmpty(validations))
            {
                attrs += string.Format(" data-validation=\"{0}\" ", validations);
            }

            int coler = 12 / (col > 4 ? 4 : col);
            bool toggle = !string.IsNullOrEmpty(toggleOff) && !string.IsNullOrEmpty(toggleON);
            StringBuilder html = new StringBuilder();
            html.AppendFormat("<section>");
            html.AppendFormat("<label class=\"label\">{0}</label>", label);
            html.AppendFormat("<div class=\"row baseSelectorButtons\" data-action=\"{0}\" data-value=\"{1}\" data-text=\"{2}\"  data-table=\"{3}\" data-where=\"{4}\" data-col=\"{5}\" data-row=\"{6}\" data-toggleoff=\"{7}\" data-toggleon=\"{8}\" data-name=\"{9}\" data-type=\"{10}\" data-toggle=\"{11}\" >",
                URLHelper.getActionUrl("BaseGeneral", "UpdateRadioButton"), "", "", "", "", col, row, toggleOff, toggleON, name, type.ToString(), toggle);

            int index = 1;
            bool wasClose = true;
            foreach (KeyValuePair<string, string> item in data)
            {
                if (index == 1 && wasClose)
                {
                    html.AppendFormat("	<div class=\"col col-{0}\">", coler);
                    wasClose = false;
                }
                html.AppendFormat("<label class=\"{0}\">", toggle ? "toggle" : type.ToString());
                html.AppendFormat("<input {4} type=\"{3}\" {0} name=\"{1}\" value=\"{2}\" >", selectd.ToString() == item.Value ? "checked=\"checked\"" : "", name, item.Value, type.ToString(), attrs);
                if (toggle)
                    html.AppendFormat("<i data-swchoff-text=\"{0}\" data-swchon-text=\"{1}\"></i>", (toggleOff), (toggleON));
                else
                    html.AppendFormat("<i></i>");
                html.AppendFormat("{0}</label>", item.Value);
                if (index % row == 0)
                {
                    html.AppendFormat("</div>");
                    wasClose = true;
                }
            }
            html.AppendFormat("</div>");
            html.AppendFormat("<div class=\"{0}\">", !string.IsNullOrEmpty(note) ? "note" : "");
            html.AppendFormat(note);
            html.AppendFormat("</div>");
            html.AppendFormat("</section>");
            return MvcHtmlString.Create(html.ToString());
        }

        public static MvcHtmlString BaseRating<TModel>(
            this HtmlHelper<TModel> htmlHelper,
            string name, int length, string label, int rating, string icon, string validation = "", string note = "",
            string xtraClassSection = "")
        {
            StringBuilder html = new StringBuilder();
            html.AppendFormat("<section>");
            html.AppendFormat("<div class=\"rating\">");
            for (int i = length; i > 0; i--)
            {
                bool selected = i >= rating && rating > 0;
                html.AppendFormat("<input {3} {2} type=\"radio\" id=\"{0}-{1}\" name=\"{0}\">", name, i, selected ? "checked=\"checked\"" : "", !string.IsNullOrEmpty(validation) ? "data-validation=\"" + validation + "\"" : "");
                html.AppendFormat("<label for=\"{0}-{2}\"><i class=\"{1}\"></i></label>", name, icon, i);
            }
            html.AppendFormat((label));
            html.AppendFormat("</div>");
            html.AppendFormat("</section>");

            return MvcHtmlString.Create(html.ToString());
        }

        public static MvcHtmlString BaseCheckBox<TModel>(
            this HtmlHelper<TModel> htmlHelper,
            string name,
           bool? value,
           string text,
           string label,
           int col,
           string toggleON = "",
           string toggleOff = "",
           string validations = "",
           string note = "",
            string xtraClassSection = "")
        {
            value = value == null ? false : value;
            StringBuilder html = new StringBuilder();
            bool toggle = (!string.IsNullOrEmpty(toggleOff) && !string.IsNullOrEmpty(toggleON));
            BeginControl(html, col, toggle ? "toggle" : "checkbox", label, xtraClassSection);
            html.AppendFormat("<input id=\"{0}\" class=\"baseCheck\" type=\"checkbox\" value=\"{1}\" name=\"{0}\" data-val=\"{1}\">", name, value.ToString().ToLower());

            if (!string.IsNullOrEmpty(toggleOff) && !string.IsNullOrEmpty(toggleON))
                html.AppendFormat("<i data-swchoff-text=\"{0}\" data-swchon-text=\"{1}\"></i>", (toggleOff), (toggleON));
            else
                html.AppendFormat("<i></i>");
            html.AppendFormat("{0}</label>", text);
            EndControl(html, note);
            html.AppendFormat("<input type=\"hidden\" value=\"false\" name=\"{0}\">", name, value.ToString().ToLower());
            return MvcHtmlString.Create(html.ToString());
        }

        public static MvcHtmlString BaseCheckBox2<TModel>(
            this HtmlHelper<TModel> htmlHelper,
            string name,
           bool? value,
           string text,
           string label,
           int col,
           string toggleON = "",
           string toggleOff = "",
           string validations = "",
           string note = "",
            string xtraClassSection = "")
        {
            value = value == null ? false : value;
            StringBuilder html = new StringBuilder();
            bool toggle = (!string.IsNullOrEmpty(toggleOff) && !string.IsNullOrEmpty(toggleON));
            BeginControl2(html, col, toggle ? "toggle" : "checkbox", label, xtraClassSection);
            html.AppendFormat("<input id=\"{0}\" class=\"baseCheck\" type=\"checkbox\" value=\"{1}\" name=\"{0}\" data-val=\"{1}\">", name, value.ToString().ToLower());

            if (!string.IsNullOrEmpty(toggleOff) && !string.IsNullOrEmpty(toggleON))
                html.AppendFormat("<i data-swchoff-text=\"{0}\" data-swchon-text=\"{1}\"></i>", (toggleOff), (toggleON));
            else
                html.AppendFormat("<i></i>");
            html.AppendFormat("{0}</label>", text);
            EndControl(html, note);
            html.AppendFormat("<input type=\"hidden\" value=\"false\" name=\"{0}\">", name, value.ToString().ToLower());
            return MvcHtmlString.Create(html.ToString());
        }

        public static MvcHtmlString BaseDateTimePick<TModel>(
         this HtmlHelper<TModel> htmlHelper,
         string name,
         DateTime? value,
         string label,
         int col,
         string placeHolder = "",
         string validations = "",
         string child = "",
         string parent = "",
         bool pickTime = true,
         DateTime? minDate = null,
         DateTime? maxDate = null,
         string dateFormat = "",
         string xtraClassSection = "", BaseToolTip tooltip = null)
        {
            BaseConfiguration config = new BaseConfiguration();
            if (!value.HasValue)
                value = DateTime.Now;
            if (value.Value.ToString() == "1/1/0001 12:00:00 AM")
                value = DateTime.Now;
            StringBuilder html = new StringBuilder();
            html.AppendFormat("<section  class=\"{0} {1}\">", colLgText(col), xtraClassSection);
            if (!string.IsNullOrEmpty(label))
            {
                html.AppendFormat("<label class=\"label\">{0}</label>", label);
            }
            string dataProperties = "";
            if (!pickTime)
            {
                dataProperties = string.Format("{0}",
                       (string.IsNullOrEmpty(dateFormat) ? "data-dateFormat='YYYY/MM/DD'" : "data-dateFormat='" + dateFormat + "'") +
                       (minDate.HasValue ? "data-minDate='" + minDate.Value.ToString(config.dateOutFormat + " " + config.timeFormat) + "'" : "") +
                       (maxDate.HasValue ? "data-maxDate='" + maxDate.Value.ToString(config.dateOutFormat + " " + config.timeFormat) + "'" : "")
                   );
            }
            else
            {
                dataProperties = string.Format("{0}",
                   (string.IsNullOrEmpty(dateFormat) ? "data-dateFormat='" + config.dateTimePickerFormat + "'" : "data-dateFormat='" + dateFormat + "'") +
                   (minDate.HasValue ? "data-minDate='" + minDate.Value.ToString(config.dateOutFormat + " " + config.timeFormat) + "'" : "") +
                   (maxDate.HasValue ? "data-maxDate='" + maxDate.Value.ToString(config.dateOutFormat + " " + config.timeFormat) + "'" : "")
               );
            }

            html.AppendFormat("	<label class=\"input\"> <i class=\"icon-append fa fa-calendar\"></i>");
            html.AppendFormat("		<input  value=\"{2}\" {6} {3} {4} {5} type=\"text\" placeholder=\"{0}\" id=\"{1}\" name=\"{1}\" class=\"datetimepicker\">", placeHolder, name, (value.HasValue) ? value.Value.ToString(config.dateOutFormat + " H:m") : "", !string.IsNullOrEmpty(validations) ? "data-validation=\"" + validations + "\"" : "",
                !string.IsNullOrEmpty(child) ? "data-child=\"" + child + "\"" : "",
            !string.IsNullOrEmpty(parent) ? "data-parent=\"" + parent + "\"" : "",
            dataProperties);

            if (tooltip != null)
                tooltip.set(html);

            html.AppendFormat("	</label>");
            html.AppendFormat("</section>");
            return MvcHtmlString.Create(html.ToString());
        }


        public static MvcHtmlString BaseDateTime<TModel>(
           this HtmlHelper<TModel> htmlHelper,
           string name,
           DateTime? value,
           string label,
           int col,
           string placeHolder = "",
           string validations = "",
           string child = "",
           string parent = "",
           BaseToolTip tt = null,
           string mask = "",
           string dateFormat = "",
           string prevText = "",
           string nextText = "",
           string defaultDate = "",
           bool changeMonth = true,
           bool changeYear = true,
           int numberOfMonths = 1,
           bool showButtonPanel = true,
           string minDate = "",
           string maxDate = "",
            string xtraClassSection = "", BaseToolTip tooltip = null)
        {
            BaseConfiguration config = new BaseConfiguration();
            if (value.Value.ToString() == "1/1/0001 12:00:00 AM")
                value = DateTime.Now;
            StringBuilder html = new StringBuilder();
            html.AppendFormat("<section  class=\"{0} {1}\">", colLgText(col), xtraClassSection);
            if (!string.IsNullOrEmpty(label))
            {
                html.AppendFormat("<label class=\"label\">{0}</label>", label);
            }

            string dataProperties = string.Format("{0}",
                (string.IsNullOrEmpty(dateFormat) ? "data-dateFormat='" + config.dateformat + "'" : "data-dateFormat='" + dateFormat + "'") +
                (!string.IsNullOrEmpty(prevText) ? "data-prevText='" + prevText + "'" : "") +
                (!string.IsNullOrEmpty(nextText) ? "data-nextText='" + nextText + "'" : "") +
                (!string.IsNullOrEmpty(defaultDate) ? "data-defaultDate='" + defaultDate + "'" : "") +
                (!string.IsNullOrEmpty(minDate) ? "data-minDate='" + minDate + "'" : "") +
                (!string.IsNullOrEmpty(maxDate) ? "data-maxDate='" + maxDate + "'" : "") +
                (!string.IsNullOrEmpty(mask) ? "data-mask='" + mask + "'" : "") +
                (!string.IsNullOrEmpty(mask) ? "data-mask-placeholder='" + "." + "'" : "") +
                ("data-changeMonth='" + changeMonth.ToString().ToLower() + "'") +
                ("data-changeYear='" + changeYear.ToString().ToLower() + "'") +
                ("data-showButtonPanel='" + showButtonPanel.ToString().ToLower() + "'") +
                ("data-numberOfMonths='" + numberOfMonths + "'")
            );

            html.AppendFormat("	<label class=\"input\"> <i class=\"icon-append fa fa-calendar\"></i>");
            html.AppendFormat("		<input  value=\"{2}\" {6} {3} {4} {5} type=\"text\" placeholder=\"{0}\" id=\"{1}\" name=\"{1}\" class=\"datetime\">", placeHolder, name, (value.HasValue) ? value.Value.ToString(config.dateOutFormat) : DateTime.Now.ToString(config.dateOutFormat), !string.IsNullOrEmpty(validations) ? "data-validation=\"" + validations + "\"" : "",
                !string.IsNullOrEmpty(child) ? "data-child=\"" + child + "\"" : "",
            !string.IsNullOrEmpty(parent) ? "data-parent=\"" + parent + "\"" : "",
            dataProperties);
            if (tt != null) tt.set(html);

            if (tooltip != null)
                tooltip.set(html);

            html.AppendFormat("	</label>");
            html.AppendFormat("</section>");
            return MvcHtmlString.Create(html.ToString());
        }

        public static MvcHtmlString BaseScaleSlider<TModel>(this HtmlHelper<TModel> htmlHelper,
            string name, object initValue, object lastValue,
            BaseScaleSliderType type, object step, string postFix, object from, object to, int col, string label = "",
            bool hasGrid = true, string validations = "",
            string xtraClassSection = "")
        {
            StringBuilder html = new StringBuilder();
            html.AppendFormat("<section>");
            html.AppendFormat("<input id=\"{0}\" name=\"{0}\" value=\"{1};{2}\" data-type=\"{3}\" data-step=\"{4}\" data-postfix=\"{5}\" data-from=\"{6}\" data-to=\"{7}\" data-hasgrid=\"{8}\" type=\"text\" class=\"ionSlider\" >"
                , name, initValue, lastValue, type.ToString(), step, postFix, from, to, hasGrid ? "true" : "false", !string.IsNullOrEmpty(validations) ? "data-validation=\"" + validations + "\"" : "");
            html.AppendFormat("</section>");
            return MvcHtmlString.Create(html.ToString());
        }

        public static MvcHtmlString BaseScaleSlider2<TModel>(this HtmlHelper<TModel> htmlHelper,
           string name, sliderClassColor color, object min, object max, object step, int col, object fromValue = null,
           object toValue = null, sliderHandle handle = sliderHandle.round, sliderSelection selection = sliderSelection.before,
           string label = "", string validations = "",
            string xtraClassSection = "")
        {
            StringBuilder html = new StringBuilder();
            html.AppendFormat("<section class=\"{0} {1}\">", colLgText(col), xtraClassSection);
            if (!string.IsNullOrEmpty(label))
                html.AppendFormat("<label>{0}</label></br>", label);
            html.AppendFormat("<input class=\"slider slider-{0}\" {8} id=\"{1}\" name=\"{1}\" data-slider-min=\"{2}\" data-slider-max=\"{3}\" data-slider-step=\"{4}\" data-slider-value=\"{5}\" data-slider-handle=\"{6}\" type=\"text\" data-slider-selection=\"{7}\">",
                color.ToString(), name, min, max, step,
                ((fromValue != null && toValue != null) ? string.Format("[{0},{1}]", fromValue, toValue) : (fromValue != null) ? fromValue : "0"),
                handle.ToString(), selection.ToString(),
                 !string.IsNullOrEmpty(validations) ? "data-validation=\"" + validations + "\"" : ""
            );
            html.AppendFormat("</section>");
            return MvcHtmlString.Create(html.ToString());
        }

        public static MvcHtmlString BaseTimePicker<TModel>(
           this HtmlHelper<TModel> htmlHelper,
           string name,
           TimeSpan? value,
           string label,
           int col,
           string placeHolder = "",
           string validations = "",
            BaseToolTip tooltip = null,
           bool show = true,
           int minuteStep = 1,
           bool showSeconds = false,
           bool showMeridian = false,
            bool defaultTime = false,
            bool showInputs = false,
            bool disableFocus = true,
            string xtraClassSection = "")
        {
            BaseConfiguration config = new BaseConfiguration();
            string dataProperties = string.Format("{0}",
                ("data-minuteStep='" + minuteStep + "'") +
                ("data-show='" + show.ToString().ToLower() + "'") +
                ("data-showSeconds='" + showSeconds.ToString().ToLower() + "'") +
                ("data-showMeridian='" + showMeridian.ToString().ToLower() + "'") +
                ("data-defaultTime='" + defaultTime.ToString().ToLower() + "'") +
                ("data-showInputs='" + showInputs.ToString().ToLower() + "'") +
                ("data-disableFocus='" + disableFocus.ToString().ToLower() + "'")
            );

            StringBuilder html = new StringBuilder();
            html.AppendFormat("<section class=\"{0} {1}\">", colLgText(col), xtraClassSection);
            if (!string.IsNullOrEmpty(label))
            {
                html.AppendFormat("<label class=\"label\">{0}</label>", label);
            }

            if (tooltip != null) tooltip.set(html);

            string meridian = (value.HasValue) ? (value.Value.Hours > 12 ? "PM" : "AM") : "AM";
            html.AppendFormat("<div class=\"input-group\">");
            html.AppendFormat("	<input is-open=\"$parent.opened\" class=\"form-control timepickerClass " + "" + "\" {3} {2} id=\"{0}\" name=\"{0}\" placeholder=\"{1}\" type=\"text\" value=\"{4} {5}\">", name, placeHolder,
                !string.IsNullOrEmpty(validations) ? "data-validation=\"" + validations + "\"" : "",
                dataProperties, (value.HasValue) ? value.Value.ToString(config.timeFormat) : "", value);
            html.AppendFormat("	<span class=\"input-group-addon\"><i class=\"fa fa-clock-o\"></i></span>");
            html.AppendFormat("</div>");
            html.AppendFormat("</section>");
            return MvcHtmlString.Create(html.ToString());
        }

        public static MvcHtmlString BaseSpiner<TModel>(
           this HtmlHelper<TModel> htmlHelper,
           string name,
           object value,
           string label,
           int col,
           spinnerDirection spinnerdirection = spinnerDirection.both,
           string placeHolder = "",
           string validations = "",
           object min = null,
           object max = null,
           object step = null,
           object start = null,
           string numberFormat = "n",
            string xtraClassSection = "",
            BaseToolTip tooltip = null)
        {
            float.Parse((value == null ? "0" : value.ToString()));
            min = min == null ? 0 : min;
            max = max == null ? 100 : max;
            step = step == null ? "0.01" : step;
            start = start == null ? 1 : start;
            string dataProperties = string.Format("{0}",
                ("data-min='" + min.ToString() + "'") +
                ("data-max='" + max.ToString() + "'") +
                ("data-step='" + step.ToString() + "'") +
                ("data-start='" + start.ToString() + "'") +
                ("data-numberFormat='" + numberFormat + "'")
            );

            StringBuilder html = new StringBuilder();
            html.AppendFormat("<section class=\"{0} {1}\">", colLgText(col), xtraClassSection);
            if (!string.IsNullOrEmpty(label))
            {
                if (label.Contains("pbold"))
                {
                    html.AppendFormat("<label class=\"label\"><strong>{0}<strong></label>", label.Replace("pbold", ""));
                }
                else
                    html.AppendFormat("<label class=\"label\">{0}</label>", label);
            }

            html.AppendFormat("<div class=\"form-group\">");
            html.AppendFormat("	<input class=\"form-control spinner spinner-{0} text-right " + (name.Contains("generalLimit") ? "blockletter" : "") + "\" name=\"{1}\" id=\"{1}\" value=\"{2}\" type=\"text\" {3} {4} placeholder=\"{5}\">", spinnerdirection.ToString(), name, (validations.Contains("scale") && !validations.Contains("scale:0")) ? decimal.Parse(value == null ? "0" : value.ToString()).ToString("N", System.Globalization.CultureInfo.GetCultureInfo("en-US")) : value, dataProperties,
                !string.IsNullOrEmpty(validations) ? "data-validation=\"" + validations + "\"" : "", placeHolder);
            html.AppendFormat("</div>");

            html.AppendFormat("</section>");
            return MvcHtmlString.Create(html.ToString());
        }

        public static MvcHtmlString BaseSpinerNumber<TModel>(
           this HtmlHelper<TModel> htmlHelper,
           string name,
           object value,
           string label,
           int col,
           spinnerDirection spinnerdirection = spinnerDirection.both,
           string placeHolder = "",
           string validations = "",
           object min = null,
           object max = null,
           object step = null,
           object start = null,
           string numberFormat = "n",
            string xtraClassSection = "",
            BaseToolTip tooltip = null)
        {
            float.Parse((value == null ? "0" : value.ToString()));
            min = min == null ? 0 : min;
            max = max == null ? 100 : max;
            step = step == null ? "0.01" : step;
            start = start == null ? 1 : start;
            string dataProperties = string.Format("{0}",
                ("data-min='" + min.ToString() + "'") +
                ("data-max='" + max.ToString() + "'") +
                ("data-step='" + step.ToString() + "'") +
                ("data-start='" + start.ToString() + "'") +
                ("data-numberFormat='" + numberFormat + "'")
            );

            StringBuilder html = new StringBuilder();
            html.AppendFormat("<section class=\"{0} {1}\">", colLgText(col), xtraClassSection);
            if (!string.IsNullOrEmpty(label))
            {
                if (label.Contains("pbold"))
                {
                    html.AppendFormat("<label class=\"label\"><strong>{0}<strong></label>", label.Replace("pbold", ""));
                }
                else
                    html.AppendFormat("<label class=\"label\">{0}</label>", label);
            }

            html.AppendFormat("<div class=\"form-group\">");
            html.AppendFormat("	<input class=\"form-control text-right " + (name.Contains("generalLimit") ? "blockletter" : "") + "\" name=\"{1}\" id=\"{1}\" value=\"{2}\" type=\"number\" {3} {4} placeholder=\"{5}\">", spinnerdirection.ToString(), name, (validations.Contains("scale") && !validations.Contains("scale:0")) ? decimal.Parse(value == null ? "0" : value.ToString()).ToString("N", System.Globalization.CultureInfo.GetCultureInfo("en-US")) : value, dataProperties,
                !string.IsNullOrEmpty(validations) ? "data-validation=\"" + validations + "\"" : "", placeHolder);
            html.AppendFormat("</div>");

            html.AppendFormat("</section>");
            return MvcHtmlString.Create(html.ToString());
        }

        public static MvcHtmlString BaseColorPicker<TModel>(
           this HtmlHelper<TModel> htmlHelper,
           string name,
           string value,
           string label,
           int col,
           bool rgb = false,
           string placeHolder = "",
           string validations = "",
            string xtraClassSection = "", BaseToolTip tooltip = null)
        {
            value = value == null ? "" : value;
            StringBuilder html = new StringBuilder();
            html.AppendFormat("<section class=\"{0} {1}\">", colLgText(col), xtraClassSection);
            if (!string.IsNullOrEmpty(label))
            {
                html.AppendFormat("<label class=\"label\">{0}</label>", label);
            }
            html.AppendFormat("<input placeholder=\"{5}\" class=\"form-control colorpicker\" name=\"{1}\" id=\"{1}\" value=\"{2}\" type=\"text\" {3} {5} placeholder=\"{6}\" >", "", name, value, "", (rgb ? "data-color-format=\"rgba\"" : ""), placeHolder,
                !string.IsNullOrEmpty(validations) ? "data-validation=\"" + validations + "\"" : "");
            html.AppendFormat("</section>");
            return MvcHtmlString.Create(html.ToString());
        }

        public static MvcHtmlString BaseTagsInput<TModel>(
           this HtmlHelper<TModel> htmlHelper,
           string name,
           string values,
           string label,
           int col,
           string placeHolder = "",
           string validations = "",
            string xtraClassSection = "", BaseToolTip tooltip = null)
        {
            StringBuilder html = new StringBuilder();
            html.AppendFormat("<section class=\"{0} {1}\">", colLgText(col), xtraClassSection);
            if (!string.IsNullOrEmpty(label))
            {
                html.AppendFormat("<label class=\"label\">{0}</label>", label);
            }
            string valuesy = values;
            html.AppendFormat("<label class=\"inpupt\">");
            html.AppendFormat("<input placeholder=\"{2}\" class=\"form-control tagsinput\" data-role=\"tagsinput\" name=\"{0}\" id=\"{0}\" value=\"{1}\" type=\"text\" {3}>", name, valuesy, placeHolder,
                !string.IsNullOrEmpty(validations) ? "data-validation=\"" + validations + "\"" : "");
            html.AppendFormat("</label>");
            html.AppendFormat("</section>");
            return MvcHtmlString.Create(html.ToString());
        }

        public static MvcHtmlString BaseKnob<TModel>(
           this HtmlHelper<TModel> htmlHelper,
           string name,
           object value,
           string label,
           int col,
           int with = 120,
           int height = 120,
           int min = 0,
           int max = 100,
           string color = "#428BCA",
           string placeHolder = "",
           string validations = "",
            string xtraClassSection = "", BaseToolTip tooltip = null)
        {
            string dataProperties = string.Format("{0}",
                ("data-min='" + min.ToString() + "'") +
                (max != 0 ? "data-max='" + max.ToString() + "'" : "")
            );
            StringBuilder html = new StringBuilder();
            html.AppendFormat("<section class=\"{0} {1}\">", colLgText(col), xtraClassSection);
            if (!string.IsNullOrEmpty(label))
            {
                html.AppendFormat("<label class=\"label\">{0}</label>", label);
            }
            html.AppendFormat("<input {3} placeholder=\"{2}\" name=\"{0}\" class=\"knob\" data-width=\"{4}\" data-height=\"{5}\" data-displayinput=\"true\" value=\"{1}\" data-displayprevious=\"true\" data-fgcolor=\"{6}\" {7}>",
                name, value, placeHolder, !string.IsNullOrEmpty(validations) ? "data-validation=\"" + validations + "\"" : "", with, height, color, dataProperties);
            html.AppendFormat("</section>");
            return MvcHtmlString.Create(html.ToString());
        }

        public static MvcHtmlString BaseAlertButton<TModel>(this HtmlHelper<TModel> htmlHelper, string title, string fullAction, string btnClass, string alertTitle, string errormessage, string promptMessage, string[] buttons, string[] buttonActions)
        {
            StringBuilder form = new StringBuilder();
            string buttonsWithActions = "";
            for (int i = 0; i < buttons.Length; i++)
            {
                buttonsWithActions += string.Format("{0}=>{1}{2}", buttons[i], buttonActions[i], (i != buttons.Length - 1) ? "&" : "");
            }

            form.AppendFormat("	title=\"{0}\" ", title);
            form.AppendFormat("	data-action=\"{0}\" ", fullAction);
            form.AppendFormat("	class=\"{0} delete\" ", btnClass);
            form.AppendFormat("	data-btnactions=\"{0}\" ", buttonsWithActions);
            form.AppendFormat("	data-buttons=\"{0}\" ", "[" + string.Join("][", buttons) + "]");
            form.AppendFormat("	data-title=\"{0}\" data-errormessage=\"{1}\" data-message=\"{2}\"", alertTitle, errormessage, promptMessage);
            return MvcHtmlString.Create(form.ToString());
        }

    }
}