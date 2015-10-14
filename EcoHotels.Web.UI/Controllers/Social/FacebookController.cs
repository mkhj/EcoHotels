using System.Web.Mvc;
using EcoHotels.Core.Common.Enum;
using EcoHotels.Core.Domain.Models.Commerce;
using EcoHotels.Core.Infrastructure.Services;
using EcoHotels.Extensions;
using EcoHotels.Web.Core.Services;
using EcoHotels.Web.Core.Social;
using Microsoft.Practices.Unity;

namespace EcoHotels.Web.UI.Controllers.Social
{
    /// <summary>
    /// </summary>
    public class FacebookController : Controller
    {
        #region - Services -

        [Dependency]
        public ICustomerService CustomerService { get; set; }

        [Dependency]
        public IFormsAuthenticationService AuthenticationService { get; set; }

        #endregion

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Logon(FacebookUserdata user)
        {
            if (user.IsNotNull())
            {
                var customer = CustomerService.FindByExternalId(user.Id, AccountTypeEnum.Facebook);
                if (customer.IsNull())
                {
                    customer = Customer.Create(user.Email, string.Empty, user.Id, AccountTypeEnum.Facebook);
                    customer.Firstname = user.First_name;
                    customer.Lastname = user.Last_name;

                    if (user.HasValidBirthday)
                    {
                        customer.Birthday = user.ConvertBirthdayToDatetime();
                    }

                    switch (user.Gender.ToLower())
                    {
                        case "male":
                            customer.Gender = GenderTypeEnum.Male;
                            break;
                        case "female":
                            customer.Gender = GenderTypeEnum.Female;
                            break;
                        default:
                            break;
                    }


                    CustomerService.Save(customer);
                }

                var authCookie = AuthenticationService.CreateAuthCookie(customer.Id.ToString(), customer.Role.ToString());
                Response.Cookies.Add(authCookie);

                return Json(new { status = "success" });
            }

            return Json(new { status = "error" });
        }
    }
}
