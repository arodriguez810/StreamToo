using Admin.BaseClass;
using Admin.BaseClass.App;
using Admin.CustomCode;
using Admin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebMatrix.WebData;

namespace Admin.Controllers
{
    [AllowAnonymous]
    public class BaseServiceController : BaseController
    {

        public class Register
        {
            public int id { get; set; }
            public string CaptchaInputText { get; set; }
            public string email { get; set; }
            public string password { get; set; }
            public string fullName { get; set; }
            public string phone { get; set; }
            public string username { get; set; }
        }

        public class Login
        {
            public string email { get; set; }
            public string password { get; set; }
        }

        [CaptchaMvc.Attributes.CaptchaVerify("Captcha is not valid")]
        [HttpPost]
        public JsonResult OnRegister(Register model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return Json(new { status = "error", message = $"Captcha is invalid!." });
                else if (db.BaseAccounts.Count(d => d.email == model.email) > 0)
                    return Json(new { status = "error", message = $"This user already exists" });
                else if (db.BaseAccounts.Count(d => d.username == model.username) > 0)
                    return Json(new { status = "error", message = $"This username already exists" });
                BaseAccount account = new BaseAccount();
                account.email = model.email;
                account.fullName = model.fullName;
                account.phone = model.phone;
                account.username = model.username;
                account.registered = false;
                account.creationDate = DateTime.Now;
                account.password = Permission.CalculateMD5Hash(model.password);
                account.token = Permission.CalculateMD5Hash(account.creationDate.ToString() + model.email);
                db.BaseAccounts.Add(account);
                db.SaveChanges();
                SendMail mail = new SendMail();
                mail.To.Add(model.email);
                BaseEmail text = db.BaseEmails.FirstOrDefault(d => d.code == "Register");
                Dictionary<string, string> vars = new Dictionary<string, string>();
                vars.Add("@fullname@", model.fullName);
                var url = URLHelper.getAbsoluteUrlNoHome("BaseService", "ValidateAccount", new string[] { $"token={account.token}" });
                vars.Add("@link@", $"<a href={url}>here</a>");
                mail.Body = Helper.ReplaceDictionary(text.bodyHTML, vars);
                mail.Subject = text.subject;
                mail.Send();
                return Json(new { status = "success", message = $"Your account has been registered, check your mail for next steps" });
            }
            catch (Exception ex)
            {
                return Json(new { status = "error", message = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult OnProfile(Register model)
        {
            try
            {
                if (db.BaseAccounts.Count(d => d.email == model.email && d.id != model.id) > 0)
                    return Json(new { status = "error", message = $"This user already exists" });
                else if (db.BaseAccounts.Count(d => d.username == model.username && d.id != model.id) > 0)
                    return Json(new { status = "error", message = $"This username already exists" });
                bool logoff = false;
                BaseAccount account = db.BaseAccounts.Find(model.id);
                if (account.email != model.email)
                    logoff = true;
                account.email = model.email;
                account.fullName = model.fullName;
                account.phone = model.phone;
                db.SaveChanges();
                return Json(new { status = "success", message = $"Your account has been updated successfully", process = logoff ? "$('#logout').click();" : "location.reload();" });
            }
            catch (Exception ex)
            {
                return Json(new { status = "error", message = ex.Message });
            }
        }

        [AllowAnonymous]
        public ActionResult ValidateAccount(string token)
        {
            try
            {
                var account = db.BaseAccounts.FirstOrDefault(x => x.token == token && !x.registered);
                account.registered = true;
                db.SaveChanges();

                SendMail mail = new SendMail();
                mail.To.Add(account.email);
                BaseEmail text = db.BaseEmails.FirstOrDefault(d => d.code == "RegisterSuccess");
                Dictionary<string, string> vars = new Dictionary<string, string>();
                vars.Add("@fullname@", account.fullName);
                mail.Body = Helper.ReplaceDictionary(text.bodyHTML, vars);
                mail.Subject = text.subject;
                mail.Send();
                WebSecurity.Login(account.email, account.password + "/force/force", persistCookie: false);
                return RedirectToAction("Index", "Send");
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Send");
            }
        }

        [HttpPost]
        public JsonResult OnLogin(Register model)
        {
            try
            {
                var password = Permission.CalculateMD5Hash(model.password);
                var user = db.BaseAccounts.FirstOrDefault(d => d.email == model.email && d.password == password);
                if (user == null)
                    return Json(new { status = "error", message = $"Username or password are incorrect." });
                WebSecurity.Login(model.email, model.password, persistCookie: false);
                return Json(new { status = "success", message = $"Your account has been registered, check your mail for next steps" });
            }
            catch (Exception ex)
            {
                return Json(new { status = "error", message = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult OnLogaout()
        {
            try
            {
                WebSecurity.Logout();
                HttpCookie cookie = Request.Cookies["userLoginCookieERP"];
                if (cookie != null)
                {
                    cookie.Expires = DateTime.Now.AddDays(-1d);
                    ControllerContext.HttpContext.Response.Cookies.Add(cookie);
                }
                return Json(new { status = "success", message = $"Logout successfully." });
            }
            catch (Exception ex)
            {
                return Json(new { status = "error", message = ex.Message });
            }
        }
    }
}
