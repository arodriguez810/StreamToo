using WebMatrix.WebData;

namespace Admin
{
    public static class WebSecurityConfig
    {
        public static void RegisterWebSec()
        {
            WebSecurity.InitializeDatabaseConnection("Context", "BaseUser", "ID", "username", false);
        }
    }
}