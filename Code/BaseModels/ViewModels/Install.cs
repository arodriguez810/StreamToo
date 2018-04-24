namespace Admin.BaseModels.ViewModels
{
    public class Install
    {
        string serverName, databaseName, user, password, companyName;
        bool styles = true, graphs = true, adminColumns = true, examples = true, rawQueries = true, apis = true;

        public bool Apis
        {
            get { return apis; }
            set { apis = value; }
        }

        public bool RawQueries
        {
            get { return rawQueries; }
            set { rawQueries = value; }
        }

        public bool Examples
        {
            get { return examples; }
            set { examples = value; }
        }

        public bool AdminColumns
        {
            get { return adminColumns; }
            set { adminColumns = value; }
        }

        public bool Graphs
        {
            get { return graphs; }
            set { graphs = value; }
        }

        public bool Styles
        {
            get { return styles; }
            set { styles = value; }
        }

        public string CompanyName
        {
            get { return companyName; }
            set { companyName = value; }
        }

        public string Password
        {
            get { return password; }
            set { password = value; }
        }

        public string User
        {
            get { return user; }
            set { user = value; }
        }

        public string DatabaseName
        {
            get { return databaseName; }
            set { databaseName = value; }
        }

        public string ServerName
        {
            get { return serverName; }
            set { serverName = value; }
        }

    }
}