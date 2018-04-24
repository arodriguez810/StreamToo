namespace Admin.BaseClass.Format
{
    public class Directories
    {

        static public string AttachmentDirName
        {
            get { return "files"; }
        }

        static public string VirtualPath
        {
            get { return System.Web.Hosting.HostingEnvironment.ApplicationVirtualPath; }
        }

        static public string EmailsPath
        {
            get { return UploadPath + "/" + "Emails/"; }
        }

        static public string EmailsPathPipe
        {
            get { return UploadPathPipe + "/" + "Emails/"; }
        }

        static public string UserImages
        {
            get { return UploadPath + "/" + "UserImages/"; }
        }

        static public string ApplicationLogos
        {
            get { return UploadPath + "/" + "ApplicationLogos/"; }
        }

        static public string ApplicationLogosPipe
        {
            get { return UploadPathPipe + "/" + "ApplicationLogos/"; }
        }

        static public string UserImagesPipe
        {
            get { return UploadPathPipe + "/" + "UserImages/"; }
        }

        static public string UploadPath
        {
            get { return "~/Uploads"; }
        }

        static public string UploadPathPipe
        {
            get { return "/Uploads"; }
        }
        

    }
}