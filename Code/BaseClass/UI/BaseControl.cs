namespace Admin.BaseClass.UI
{
    public class BaseControl
    {
        string id = "", @class = "", title = "", icon = "", href = "", onclick = "", color = "", text = "";

        public string Text
        {
            get { return text; }
            set { text = value; }
        }

        public string Color
        {
            get { return color; }
            set { color = value; }
        }

        public string Onclick
        {
            get { return onclick; }
            set { onclick = value; }
        }

        public string Href
        {
            get { return href; }
            set { href = value; }
        }

        public string Icon
        {
            get { return icon; }
            set { icon = value; }
        }

        public string Title
        {
            get { return title; }
            set { title = value; }
        }

        public string Class
        {
            get { return @class; }
            set { @class = value; }
        }

        public string Id
        {
            get { return id; }
            set { id = value; }
        }


        public string close()
        {
            return "</div>";
        }
    }
}