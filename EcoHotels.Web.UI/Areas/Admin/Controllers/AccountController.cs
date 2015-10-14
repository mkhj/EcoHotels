using System;
using System.Resources;
using System.Web.Mvc;
using EcoHotels.Core.Domain.Models.Security;
using EcoHotels.Core.Email;
using EcoHotels.Core.Helpers;
using EcoHotels.Core.Infrastructure.Services;
using EcoHotels.Extensions;
using EcoHotels.Web.Core.Models;
using EcoHotels.Web.Core.Services;
using EcoHotels.Web.UI.Areas.Admin.Models;
using Microsoft.Practices.Unity;

namespace EcoHotels.Web.UI.Areas.Admin.Controllers
{
    public class AccountController : Controller
    {
        #region - Services -

        [Dependency]
        public IAppService AppService { get; set; }

        [Dependency]
        public IUserService UserService { get; set; }

        [Dependency]
        public IFormsAuthenticationService AuthenticationService { get; set; }

        [Dependency]
        public IEmailService EmailService { get; set; }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet, OutputCache(CacheProfile = "BasicOutputCache")]
        public ActionResult Index()
        {
            return RedirectToAction("logon"); 
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet, OutputCache(CacheProfile = "BasicOutputCache")]
        public ActionResult LogOn()
        {
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult LogOn(LogOnModel model)
        {
            if(!ModelState.IsValid)
            {
                ViewData["Error-login"] = "Invalid email/password. Please try again.";
                return View();
            }

            var user = UserService.FindByEmail(model.Email);

            var isNotValid = user == null || !user.IsActive || !PasswordHelper.ValidatePassword(model.Password, user.Password);
            if (isNotValid)
            {
                ViewData["Error-login"] = "Invalid email/password. Please try again.";
                return View();
            }

            AppService.SetCurrentOrganizationId(user.Organization.Id);

            var authCookie = AuthenticationService.CreateAuthCookie(user.Id.ToString(), user.Role.ToString());
            Response.Cookies.Add(authCookie);

            return RedirectToAction("index", "dashboard");
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult LogOff()
        {
            AuthenticationService.SignOut();
            AppService.AbondonSession();

            return RedirectToAction("logon", "account");
        }


        [HttpGet]
        public ActionResult Reset()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Reset(ResetAccountModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Error"] = "Invalid email/password. Please try again.";
                return View();
            }

            var user = UserService.FindByEmail(model.Email);

            var isNotValid = user == null || !user.IsActive;
            if (isNotValid)
            {
                ViewData["Error"] = "Invalid email/password. Please try again.";
                return View();
            }

            var timestamp = DateTime.Now.Ticks;

            var key = string.Concat(user.Id, model.Email, timestamp);
            var token = PasswordHelper.HashPassword(key);

            var template = EmailService.CreateEmail("admin_user_account_reset_html");
            template.Firstname = user.Firstname;
            template.Lastname = user.Lastname;
            template.Email = user.Email;
            template.Time = timestamp;
            template.Token = token;

            EmailService.Send(model.Email, "Reset your password", template);

            ViewData["success"] = "An e-mail has been sent to you.";

            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult VerifyReset(VerifyResetModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Error"] = "Data not valid";
                return View();
            }

            var user = UserService.FindByEmail(model.Email);
            if (user.IsNull() || !user.IsActive)
            {
                ViewData["Error"] = "Data not valid";
                return View();
            }

            var token = string.Concat(user.Id, model.Email, model.Time);
            if (!PasswordHelper.ValidatePassword(token, model.Token))
            {
                ViewData["Error"] = "Data not valid";
                return View();
            }

            if (!model.IsTimeValid())
            {
                ViewData["Error"] = "Your token has expired, please reset again.";
                return View();
            }

            if(Session["USER_ID_RESET_VERIFIER"] == null)
            {
                Session["USER_ID_RESET_VERIFIER"] = user.Id;
            }

            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult VerifyReset(string password)
        {
            if (password.IsNullOrEmpty() || password.Length < 6)
            {
                ViewData["Error"] = "Data not valid";
                return View();
            }

            if (Session["USER_ID_RESET_VERIFIER"] == null)
            {
                ViewData["Error"] = "Session expired. Please reset again.";
                return View();
            }

            var userId = Session["USER_ID_RESET_VERIFIER"].ToString().ToInt();

            var user = UserService.FindById(userId);
            var isNotValid = user == null || !user.IsActive;
            if (isNotValid)
            {
                ViewData["Error"] = "Data not valid";
                return View();
            }

            user.SetNewPassword(password);
            UserService.Save(user);

            AppService.SetCurrentOrganizationId(user.Organization.Id);

            var authCookie = AuthenticationService.CreateAuthCookie(user.Id.ToString(), user.Role.ToString());
            Response.Cookies.Add(authCookie);

            return RedirectToAction("index", "dashboard");
        }
    }
}
