using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using EcoHotels.Core.Infrastructure.Services;
using EcoHotels.Extensions;
using EcoHotels.Web.Core.Cookies;
using EcoHotels.Web.Core.Helpers;
using EcoHotels.Web.Core.Services;
using Microsoft.Practices.Unity;

namespace EcoHotels.Web.UI.Controllers
{
    public class LocationController : Controller
    {
        #region - Services -

        [Dependency]
        public ILocationService LocationService { get; set; }

        [Dependency]
        public ILanguageService LanguageService { get; set; }

        #endregion

        public ActionResult Index()
        {
            //TODO: Consider what to do if language.IsActive == false

            var languageId = CookieHelper.GetCookie(CookieHelper.ECOHOTELS_LANGUAGE_COOKIE);
            var language = LanguageService.FindById(languageId.ToInt());

            if (language.IsNull())
            {
                var ip4Address = GetIP4Address(Request);
                var country = LocationService.FindLocationByIp(ip4Address);

                CookieHelper.SetCookie(CookieHelper.ECOHOTELS_LANGUAGE_COOKIE, country.Language.Id.ToString());
                CookieHelper.SetCookie(CookieHelper.ECOHOTELS_CURRENCY_COOKIE, country.Currency.Id.ToString());

                return RedirectPermanent("/" + country.Language.Shortname + "/");
            }

            return RedirectPermanent("/" + language.Shortname + "/");
        }

        [NonAction]
        private string GetIP4Address(HttpRequestBase request)
        {
            var IP4Address = string.Empty;

            foreach (var IPA in Dns.GetHostAddresses(request.UserHostAddress))
            {
                if (IPA.AddressFamily.ToString() == "InterNetwork")
                {
                    IP4Address = IPA.ToString();
                    break;
                }
            }

            if (IP4Address != string.Empty)
            {
                return IP4Address;
            }

            foreach (var IPA in Dns.GetHostAddresses(Dns.GetHostName()))
            {
                if (IPA.AddressFamily.ToString() == "InterNetwork")
                {
                    IP4Address = IPA.ToString();
                    break;
                }
            }

            return IP4Address;
        }


    }


}
