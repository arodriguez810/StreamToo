using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WebMatrix.WebData;
using Admin.Filters;
using Admin.Models;
using Admin.CustomCode;
using Admin.BaseClass;
using Admin.BaseClass.App;
using Admin.BaseModels.ViewModels;

namespace Admin.Controllers
{
    //[Authorize]
    //[InitializeSimpleMembership]
    [PreventRepost]
    public class AccountController : BaseController
    {
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            HttpCookie cookie = Request.Cookies["userLoginCookieERP"];
            if (WebSecurity.IsAuthenticated)
            {
                BaseUser user = db.BaseUsers.Find(WebSecurity.CurrentUserId);
                if (!user.BaseUserStatu.canLogin)
                {
                    LogOff();
                }
            }

            if (cookie != null)
            {
                string username = cookie.Value.Split('¬')[0].Trim();
                string cookievalue = cookie.Value;
                BaseUser user = db.BaseUsers.Where(x => x.username == username).FirstOrDefault();
                if (user != null)
                {
                    db.Entry(user).State = EntityState.Modified;
                    db.SaveChanges();
                    FormsAuthentication.SetAuthCookie(user.username, true);
                    return RedirectToAction("Index", "BaseHome");
                }
            }
            else
            {
                ViewBag.ReturnUrl = returnUrl;
                if (WebSecurity.IsAuthenticated)
                {
                    return RedirectToAction("Index", "BaseHome");
                }
            }
            return PartialView();
        }
        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(LoginModel model, string returnUrl, FormCollection form)
        {
            if (ModelState.IsValid && WebSecurity.Login(model.username, model.password, persistCookie: model.RememberMe))
            {
                BaseUser user = db.BaseUsers.FirstOrDefault(usr => usr.username == model.username);
                user.superAdminShowHiddenMenu = false;
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                string UserPass = model.username + ":" + model.password;
                /*creando cookie*/
                var chx = form["RememberMe2"];
                if (chx == "on")
                {
                    HttpCookie cookie = Request.Cookies["userLoginCookieERP"];
                    if (cookie == null)
                    {
                        cookie = new HttpCookie("userLoginCookieERP");
                        cookie.Value = model.username + "¬" + Permission.CalculateMD5Hash(UserPass);
                        cookie.Expires = DateTime.Now.AddDays(30);
                        ControllerContext.HttpContext.Response.Cookies.Add(cookie);
                    }
                }

                //returnUrl = returnUrl.Replace("#", "<$>");
                if (!string.IsNullOrEmpty(returnUrl))
                {
                    return RedirectToAction("Index", "BaseHome");
                }
                else
                if (user.BaseProfiles.Count > 0)
                {
                    if (user.BaseProfiles.First().BaseAction != null)
                        return Redirect(URLHelper.getActionUrl(user.BaseProfiles.First().BaseAction));
                }
                else
                if (user.BaseAction == null)
                {
                    if (user.BaseAction != null)
                        return Redirect(URLHelper.getActionUrl(user.BaseAction));
                }
                return RedirectToAction("Index", "BaseHome");
            }

            // If we got this far, something failed, redisplay form
            ModelState.AddModelError("", ("The user name or password are invalid."));
            return RedirectToAction("Login", "Account", new { i = true, email = model.username });
        }
        [AllowAnonymous]
        public ActionResult Forgot(string returnUrl)
        {
            return PartialView();
        }
        [AllowAnonymous]
        public ActionResult ChangePassword(string token)
        {
            BaseUser user = db.BaseUsers.FirstOrDefault(d => d.token == token);
            if (user != null)
                return PartialView(user);
            BaseAccount client = db.BaseAccounts.FirstOrDefault(d => d.token == token);
            if (client != null)
                return PartialView(new BaseUser() { ID = client.id });
            throw new HttpException(404, "Token not available");
        }
        [HttpPost]
        [AllowAnonymous]
        public ActionResult ChangePassword(BaseUser model)
        {
            BaseUser user = db.BaseUsers.Find(model.ID);
            if (user != null)
            {
                BaseConfiguration condfig = new BaseConfiguration();
                user.password = Permission.CalculateMD5Hash(model.password);
                user.userStatusID = 1;
                user.token = Permission.CalculateMD5Hash(DateTime.Now.ToString());
                SendMail mail = new SendMail();
                string link = $"<a href='{ URLHelper.getAbsoluteUrlNoHome("Account", "Login")}'>{condfig.appName}</a>";
                mail.Body = Helper.BaseMessage("Email - Forgot password confirmation Body").Replace("@user@", user.FullName).Replace("@link@", link);
                mail.Subject = Helper.BaseMessage("Email - Forgot password confirmation Subject").Replace("@appName@", condfig.appName);
                mail.To.Add(user.username);
                mail.Send();
                db.SaveChanges();
                return RedirectToAction("Login", "Account");
            }
            BaseAccount client = db.BaseAccounts.Find(model.ID);
            if (client != null)
            {
                BaseConfiguration condfig = new BaseConfiguration();
                client.password = Permission.CalculateMD5Hash(model.password);
                client.token = Permission.CalculateMD5Hash(DateTime.Now.ToString());
                SendMail mail = new SendMail();
                string link = $"<a href='{ URLHelper.getAbsoluteUrlNoHome("Account", "Login")}'>{condfig.appName}</a>";
                mail.Body = Helper.BaseMessage("Email - Forgot password confirmation Body").Replace("@user@", client.fullName).Replace("@link@", link);
                mail.Subject = Helper.BaseMessage("Email - Forgot password confirmation Subject").Replace("@appName@", condfig.appName);
                mail.To.Add(client.email);
                mail.Send();
                db.SaveChanges();
                return RedirectToAction("Index", "Send");
            }
            return RedirectToAction("Login", "Account");

        }
        [HttpPost]
        [AllowAnonymous]
        public JsonResult Forgot(LoginModel model, FormCollection form)
        {
            BaseUser user = db.BaseUsers.FirstOrDefault(usr => usr.username == model.username);
            if (user != null)
            {
                if (user.BaseUserStatu.canLogin || user.userStatusID == 4)
                {
                    user.userStatusID = 4;
                    user.token = Permission.CalculateMD5Hash(user.username + user.password);
                    SendMail mail = new SendMail();
                    string link = $"<a href='{ URLHelper.getAbsoluteUrlNoHome("Account", "ChangePassword", $"token={user.token}")}'>here</a>";
                    mail.Body = Helper.BaseMessage("Email - Forgot password instructions Body").Replace("@user@", user.FullName).Replace("@link@", link);
                    mail.Subject = Helper.BaseMessage("Email - Forgot password instructions Subject").Replace("@company@", new BaseConfiguration().appName);
                    mail.To.Add(user.username);
                    mail.Send();
                    db.SaveChanges();
                }
            }
            BaseAccount cllient = db.BaseAccounts.FirstOrDefault(usr => usr.email == model.username);
            if (cllient != null)
            {
                if (cllient.registered)
                {
                    cllient.token = Permission.CalculateMD5Hash(cllient.email + cllient.password);
                    SendMail mail = new SendMail();
                    string link = $"<a href='{ URLHelper.getAbsoluteUrlNoHome("Account", "ChangePassword", $"token={cllient.token}")}'>here</a>";
                    mail.Body = Helper.BaseMessage("Email - Forgot password instructions Body").Replace("@user@", cllient.fullName).Replace("@link@", link);
                    mail.Subject = Helper.BaseMessage("Email - Forgot password instructions Subject").Replace("@company@", new BaseConfiguration().appName);
                    mail.To.Add(cllient.email);
                    mail.Send();
                    db.SaveChanges();
                }
            }
            return Json(new { status = "success", message = "Instructions have been sent." });
        }

        public ActionResult LogOff(int id = 0)
        {
            WebSecurity.Logout();
            HttpCookie cookie = Request.Cookies["userLoginCookieERP"];
            if (cookie != null)
            {
                cookie.Expires = DateTime.Now.AddDays(-1d);
                ControllerContext.HttpContext.Response.Cookies.Add(cookie);
            }
            return RedirectToAction("Login", "Account");
        }
        [HttpPost]
        public ActionResult LogOff()
        {
            WebSecurity.Logout();
            HttpCookie cookie = Request.Cookies["userLoginCookieERP"];
            if (cookie != null)
            {
                cookie.Expires = DateTime.Now.AddDays(-1d);
                ControllerContext.HttpContext.Response.Cookies.Add(cookie);
            }
            return RedirectToAction("Login", "Account");
        }
        public string replaceAcentos(string content)
        {
            return content.Replace("á", "a").Replace("é", "e").Replace("í", "i").Replace("ó", "o").Replace("ú", "u");
        }
        #region Helpers
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "BaseHome");
            }
        }

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
        }

        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "User name already exists. Please enter a different user name.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "A user name for that e-mail address already exists. Please enter a different e-mail address.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }
        #endregion


    }
}