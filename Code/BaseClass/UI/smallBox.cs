using System.Collections.Generic;
using System.Text;

namespace Admin.BaseClass.UI
{
    public class smallBoxButtons
    {
        string onclick = "", href = "javascript:void(0);", @class = "btn btn-sm", text = "", btnStyle = "btn-primary";

        public string BtnStyle
        {
            get { return btnStyle; }
            set { btnStyle = value; }
        }

        public string Text
        {
            get { return text; }

            set { text = value; }
        }

        public string Class
        {
            get { return @class; }
            set { @class = value; }
        }

        public string Href
        {
            get { return href; }
            set { href = value; }
        }

        public string Onclick
        {
            get { return onclick; }
            set { onclick = value; }
        }
        bool translatable = true;

        public bool Translatable
        {
            get { return translatable; }
            set { translatable = value; }
        }
    }
    public class smallBox
    {
        string title = "", message = "", color = "#C46A69", icon = "", animation = "";
        int? timeout = null;

        public int? Timeout
        {
            get { return timeout; }
            set { timeout = value; }
        }

        public string Animation
        {
            get { return animation; }
            set { animation = value; }
        }

        public string Icon
        {
            get { return icon; }
            set { icon = value; }
        }

        public string Color
        {
            get { return color; }
            set { color = value; }
        }
        bool translatable = true;

        public bool Translatable
        {
            get { return translatable; }
            set { translatable = value; }
        }

        public string Message
        {
            get { return message; }
            set { message = value; }
        }

        public string Title
        {
            get { return title; }
            set { title = value; }
        }
        List<smallBoxButtons> buttons;

        public List<smallBoxButtons> Buttons
        {
            get { return buttons; }
            set { buttons = value; }
        }

        public smallBox()
        {
            buttons = new List<smallBoxButtons>();
        }

        public string script()
        {
            StringBuilder script = new StringBuilder();
            script.AppendLine("$.smallBox({");
            script.AppendLine(string.Format("color: \"{0}\",", this.color));
            script.AppendLine(string.Format("title: \"{0}\",", this.title));
            if (timeout.HasValue)
                script.AppendLine(string.Format("timeout: {0},", this.timeout));
            script.AppendLine(string.Format("icon: \"fa {0} {1}\",", this.icon, this.animation));
            string alertButtons = "<p class='text-align-right'>";
            foreach (var button in buttons)
                alertButtons += string.Format("<a href='{0}' onclick='{1}' class='{2} {4}'>{3}</a>", button.Href, button.Onclick, button.Class, button.Text,button.BtnStyle);
            alertButtons += "</p>";
            script.AppendLine(string.Format("content: \"{0} {1} \"", this.Message, alertButtons));
            script.AppendLine("});");
            return script.ToString();
        }

        public smallBox Clone()
        {
            smallBox clone = new smallBox();
            clone.Animation = this.Animation;
            foreach (smallBoxButtons button in Buttons)
            {
                clone.Buttons.Add(button);
            }
            clone.Color = this.Color;
            clone.Icon = this.Icon;
            clone.Message = this.Message;
            clone.Timeout = this.Timeout;
            clone.Title = this.Title;
            clone.Translatable = this.Translatable;
            return clone;
        }
    }
}