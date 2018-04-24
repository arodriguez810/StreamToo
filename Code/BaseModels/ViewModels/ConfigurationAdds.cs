using System.Configuration;

namespace Admin.BaseModels.ViewModels
{
    public partial class BaseConfiguration
    {
        public bool ActiveTwilio
        {
            get { return ConfigurationManager.AppSettings["ActiveTwilio"].ToString() == "True" ? true : false; }
            set
            {
                configuration.AppSettings.Settings["ActiveTwilio"].Value = value ? "True" : "False";
                configuration.Save(ConfigurationSaveMode.Full);
            }
        }

        public string AccountSid
        {
            get { return ConfigurationManager.AppSettings["AccountSid"].ToString(); }
            set
            {
                configuration.AppSettings.Settings["AccountSid"].Value = value;
                configuration.Save(ConfigurationSaveMode.Full);
            }
        }
        public string AuthToken
        {
            get { return ConfigurationManager.AppSettings["AuthToken"].ToString(); }
            set
            {
                configuration.AppSettings.Settings["AuthToken"].Value = value;
                configuration.Save(ConfigurationSaveMode.Full);
            }
        }
        public string FromNumber
        {
            get { return ConfigurationManager.AppSettings["FromNumber"].ToString(); }
            set
            {
                configuration.AppSettings.Settings["FromNumber"].Value = value;
                configuration.Save(ConfigurationSaveMode.Full);
            }
        }


        public int maxLength
        {
            get { return int.Parse(ConfigurationManager.AppSettings["maxLength"].ToString()); }
            set
            {
                configuration.AppSettings.Settings["maxLength"].Value = value.ToString();
                configuration.Save(ConfigurationSaveMode.Full);
            }
        }
        

        public string MessagePrefix
        {
            get { return ConfigurationManager.AppSettings["MessagePrefix"].ToString(); }
            set
            {
                configuration.AppSettings.Settings["MessagePrefix"].Value = value;
                configuration.Save(ConfigurationSaveMode.Full);
            }
        }
    }
}