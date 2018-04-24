namespace Admin.Models.ViewModels
{
    public class PermissionsModelAdmin
    {
        bool isUser = true;

        public bool IsUser
        {
            get { return isUser; }
            set { isUser = value; }
        }

        public PermissionsModelAdmin()
        {

        }
    }
}