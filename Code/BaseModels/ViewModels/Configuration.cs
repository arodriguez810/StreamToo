using System.Configuration;

namespace Admin.BaseModels.ViewModels
{
    public partial class BaseConfiguration
    {

        System.Configuration.Configuration configuration;


        public BaseConfiguration()
        {
            configuration = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~");
            //ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
        }


        public string languague
        {
            get { return ConfigurationManager.AppSettings["languague"].ToString(); }
            set
            {
                configuration.AppSettings.Settings["languague"].Value = value;
                configuration.Save(ConfigurationSaveMode.Full);
            }
        }


        public string AvaibleCoolDownInSeconds
        {
            get { return ConfigurationManager.AppSettings["AvaibleCoolDownInSeconds"].ToString(); }
            set
            {
                configuration.AppSettings.Settings["AvaibleCoolDownInSeconds"].Value = value;
                configuration.Save(ConfigurationSaveMode.Full);
            }
        }

        public string PullOrder
        {
            get { return ConfigurationManager.AppSettings["PullOrder"].ToString(); }
            set
            {
                configuration.AppSettings.Settings["PullOrder"].Value = value;
                configuration.Save(ConfigurationSaveMode.Full);
            }
        }

        public string name
        {
            get { return ConfigurationManager.AppSettings["name"].ToString(); }
            set
            {
                configuration.AppSettings.Settings["name"].Value = value;
                configuration.Save(ConfigurationSaveMode.Full);
            }
        }

        public string styles
        {
            get { return ConfigurationManager.AppSettings["styles"].ToString(); }
            set
            {
                configuration.AppSettings.Settings["styles"].Value = value;
                configuration.Save(ConfigurationSaveMode.Full);
            }
        }

        public string colors
        {
            get { return ConfigurationManager.AppSettings["colors"].ToString(); }
            set
            {
                configuration.AppSettings.Settings["colors"].Value = value;
                configuration.Save(ConfigurationSaveMode.Full);
            }
        }

        public string address
        {
            get { return ConfigurationManager.AppSettings["address"].ToString(); }
            set
            {
                configuration.AppSettings.Settings["address"].Value = value;
                configuration.Save(ConfigurationSaveMode.Full);
            }
        }

        public string companyEmail
        {
            get { return ConfigurationManager.AppSettings["companyEmail"].ToString(); }
            set
            {
                configuration.AppSettings.Settings["companyEmail"].Value = value;
                configuration.Save(ConfigurationSaveMode.Full);
            }
        }

        public string appName
        {
            get { return ConfigurationManager.AppSettings["appName"].ToString(); }
            set
            {
                configuration.AppSettings.Settings["appName"].Value = value;
                configuration.Save(ConfigurationSaveMode.Full);
            }
        }

        public string copyRight
        {
            get { return ConfigurationManager.AppSettings["copyRight"].ToString(); }
            set
            {
                configuration.AppSettings.Settings["copyRight"].Value = value;
                configuration.Save(ConfigurationSaveMode.Full);
            }
        }

        public decimal version
        {
            get { return decimal.Parse(ConfigurationManager.AppSettings["version"].ToString()); }
            set
            {
                configuration.AppSettings.Settings["version"].Value = value.ToString();
                configuration.Save(ConfigurationSaveMode.Full);
            }
        }

        public int SessionTimeOut
        {
            get { return int.Parse(ConfigurationManager.AppSettings["SessionTimeOut"].ToString()); }
            set
            {
                configuration.AppSettings.Settings["SessionTimeOut"].Value = value.ToString();
                configuration.Save(ConfigurationSaveMode.Full);
            }
        }

        public string SiteID
        {
            get { return ConfigurationManager.AppSettings["SiteID"].ToString(); }
            set
            {
                configuration.AppSettings.Settings["SiteID"].Value = value;
                configuration.Save(ConfigurationSaveMode.Full);
            }
        }

        public string css
        {
            get { return ConfigurationManager.AppSettings["css"].ToString(); }
            set
            {
                configuration.AppSettings.Settings["css"].Value = value;
                configuration.Save(ConfigurationSaveMode.Full);
            }
        }

        public string primaryColor
        {
            get { return ConfigurationManager.AppSettings["primaryColor"].ToString(); }
            set
            {
                configuration.AppSettings.Settings["primaryColor"].Value = value;
                configuration.Save(ConfigurationSaveMode.Full);
            }
        }

        public string secundaryColor
        {
            get { return ConfigurationManager.AppSettings["secundaryColor"].ToString(); }
            set
            {
                configuration.AppSettings.Settings["secundaryColor"].Value = value;
                configuration.Save(ConfigurationSaveMode.Full);
            }
        }

        public string server
        {
            get { return ConfigurationManager.AppSettings["server"].ToString(); }
            set
            {
                configuration.AppSettings.Settings["server"].Value = value;
                configuration.Save(ConfigurationSaveMode.Full);
            }
        }

        public string email
        {
            get { return ConfigurationManager.AppSettings["email"].ToString(); }
            set
            {
                configuration.AppSettings.Settings["email"].Value = value;
                configuration.Save(ConfigurationSaveMode.Full);
            }
        }

        public string password
        {
            get { return ConfigurationManager.AppSettings["password"].ToString(); }
            set
            {
                configuration.AppSettings.Settings["password"].Value = value;
                configuration.Save(ConfigurationSaveMode.Full);
            }
        }

        public int port
        {
            get { return int.Parse(ConfigurationManager.AppSettings["port"].ToString()); }
            set
            {
                configuration.AppSettings.Settings["port"].Value = value.ToString();
                configuration.Save(ConfigurationSaveMode.Full);
            }
        }

        public bool smtpSSL
        {
            get { return ConfigurationManager.AppSettings["smtpSSL"].ToString() == "true" ? true : false; }
            set
            {
                configuration.AppSettings.Settings["smtpSSL"].Value = value ? "true" : "false";
                configuration.Save(ConfigurationSaveMode.Full);
            }
        }

        public string timeZone
        {
            get { return ConfigurationManager.AppSettings["timeZone"].ToString(); }
            set
            {
                configuration.AppSettings.Settings["timeZone"].Value = value;
                configuration.Save(ConfigurationSaveMode.Full);
            }
        }

        public string dateformat
        {
            get { return ConfigurationManager.AppSettings["dateformat"].ToString(); }
            set
            {
                configuration.AppSettings.Settings["dateformat"].Value = value;
                configuration.Save(ConfigurationSaveMode.Full);
            }
        }

        public string dateOutFormat
        {
            get { return ConfigurationManager.AppSettings["dateOutFormat"].ToString(); }
            set
            {
                configuration.AppSettings.Settings["dateOutFormat"].Value = value;
                configuration.Save(ConfigurationSaveMode.Full);
            }
        }

        public string timeFormat
        {
            get { return ConfigurationManager.AppSettings["timeFormat"].ToString(); }
            set
            {
                configuration.AppSettings.Settings["timeFormat"].Value = value;
                configuration.Save(ConfigurationSaveMode.Full);
            }
        }

        public string timeOutFormat
        {
            get { return ConfigurationManager.AppSettings["timeOutFormat"].ToString(); }
            set
            {
                configuration.AppSettings.Settings["timeOutFormat"].Value = value;
                configuration.Save(ConfigurationSaveMode.Full);
            }
        }

        public string dateTimePickerFormat
        {
            get { return ConfigurationManager.AppSettings["dateTimePickerFormat"].ToString(); }
            set
            {
                configuration.AppSettings.Settings["dateTimePickerFormat"].Value = value;
                configuration.Save(ConfigurationSaveMode.Full);
            }
        }

        public string commsoftRoot
        {
            get { return ConfigurationManager.AppSettings["commsoftRoot"].ToString(); }
            set
            {
                configuration.AppSettings.Settings["commsoftRoot"].Value = value;
                configuration.Save(ConfigurationSaveMode.Full);
            }
        }

        public string commsoftAuthentication
        {
            get { return ConfigurationManager.AppSettings["commsoftAuthentication"].ToString(); }
            set
            {
                configuration.AppSettings.Settings["commsoftAuthentication"].Value = value;
                configuration.Save(ConfigurationSaveMode.Full);
            }
        }

        public string commsoftStatementSearchAddress
        {
            get { return ConfigurationManager.AppSettings["commsoftStatementSearchAddress"].ToString(); }
            set
            {
                configuration.AppSettings.Settings["commsoftStatementSearchAddress"].Value = value;
                configuration.Save(ConfigurationSaveMode.Full);
            }
        }
    }
}