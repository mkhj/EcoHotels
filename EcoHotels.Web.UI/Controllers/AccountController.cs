using System.Web.Mvc;
using EcoHotels.Core.Common.Enum;
using EcoHotels.Core.Helpers;
using EcoHotels.Core.Infrastructure.Services;
using EcoHotels.Extensions;
using EcoHotels.Web.Core.Attributes.Security;
using EcoHotels.Web.Core.Models;
using EcoHotels.Web.Core.Services;
using EcoHotels.Web.UI.Areas.Admin.Models;
using EcoHotels.Web.UI.Models;
using Microsoft.Practices.Unity;

namespace EcoHotels.Web.UI.Controllers
{
    public class AccountController : Controller
    {
        #region - Services -

        [Dependency]
        public IAppService AppService { get; set; }

        [Dependency]
        public ICustomerService CustomerService { get; set; }

        [Dependency]
        public ICountryService CountryService { get; set; }

        [Dependency]
        public IFormsAuthenticationService AuthenticationService { get; set; }

        #endregion

        #region - Authentication -

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult LogOn()
        {
            if (Request.IsAuthenticated)
            {
                return Redirect("/");
            }

            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult LogOn(LogOnModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Error"] = "Invalid email/password. Please try again.";
            }

            var customer = CustomerService.FindByEmail(model.Email);

            var isNotValid = customer == null || !PasswordHelper.ValidatePassword(model.Password, customer.Password);
            if (isNotValid)
            {
                ViewData["Error"] = "Invalid email/password. Please try again.";
                return View();
            }

            var authCookie = AuthenticationService.CreateAuthCookie(customer.Id.ToString(), customer.Role.ToString());
            Response.Cookies.Add(authCookie);

            return RedirectToAction("index", "reservation");
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

            return RedirectToAction("index", "home");
        }

        #endregion

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Create(int i)
        {
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [AuthorizeCustomer, HttpGet]
        public ActionResult Index()
        {
            var customer = CustomerService.FindById(User.Identity.Name.ToInt());
            var countries = CountryService.FindAll();

            return View(new EditCustomerInformationModel(customer, countries));
        }

        [AuthorizeCustomer, HttpPost, ValidateAntiForgeryToken]
        public ActionResult Index(EditCustomerInformationModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new JsonResultError("Data is not valid."));
            }

            var customer = CustomerService.FindById(User.Identity.Name.ToInt());

            customer.Firstname = model.Firstname;
            customer.Lastname = model.Lastname;
            customer.Email = model.Email;
            customer.Gender = model.SelectedGenderType.ConvertToEnum<GenderTypeEnum>();
            customer.Country = model.SelectedCountryName;

            if(model.HasValidBirthday())
            {
                customer.Birthday = model.GetBirthday();
            }
            else
            {
                customer.Birthday = null;
            }

            CustomerService.Save(customer);

            return Json(new JsonResultSuccess("Profile data has been updated."));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [AuthorizeCustomer, HttpGet]
        public ActionResult Password()
        {
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [AuthorizeCustomer, HttpPost, ValidateAntiForgeryToken]
        public ActionResult Password(EditCustomerPasswordModel model)
        {
            if (!ModelState.IsValid)
            {
                if(string.Compare(model.NewPassword, model.ConfirmPassword) != 0)
                {
                    return Json(new JsonResultError("'New Password' and 'Confirm Password' do not match."));
                }

                return Json(new JsonResultError("Data is not valid."));
            }

            if (model.NewPassword.IsNotNullOrEmpty() && model.NewPassword.Length <= 5)
            {
                return Json(new JsonResultError("Password should be longer then 5 characters."));
            }

            var customer = CustomerService.FindById(User.Identity.Name.ToInt());
            customer.SetNewPassword(model.NewPassword);

            return Json(new JsonResultSuccess("Password has been updated."));
        }


        //[AuthorizeCustomer, HttpGet]
        //public ActionResult BillingInformation()
        //{
        //    return View();
        //}

        //[AuthorizeCustomer, HttpPost, ValidateAntiForgeryToken]
        //public ActionResult BillingInformation(object model)
        //{
        //    return View();
        //}
    }
}
