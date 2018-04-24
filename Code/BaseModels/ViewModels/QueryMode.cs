namespace Admin.BaseModels.ViewModels
{
    public class QueryMode
    {
        public QueryMode(string text)
        {
            this.text = text;
        }
        string text;
        public string Text
        {
            get { return text; }
            set { text = value; }
        }
    }
}