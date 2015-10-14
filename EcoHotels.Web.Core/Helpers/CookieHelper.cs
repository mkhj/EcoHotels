using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using EcoHotels.Extensions;

namespace EcoHotels.Web.Core.Helpers
{
    public class CookieHelper
    {
        private const string VERSION = "v1";

        public const string ECOHOTELS_LANGUAGE_COOKIE = "EcoHotels.Language";

        public const string ECOHOTELS_CURRENCY_COOKIE = "EcoHotels.Currency";

        public static void SetCookie(string name, string data)
        {
            // Create a cookie and add the encrypted ticket to the cookie as data.
            var cookie = new HttpCookie(GetCookieKey(name), data);
            cookie.Expires = DateTime.Now.AddYears(10);
            cookie.HttpOnly = true;

            HttpContext.Current.Response.Cookies.Add(cookie);
        }

        public static string GetCookie(string name)
        {
            var httpCookie = HttpContext.Current.Request.Cookies[GetCookieKey(name)];
            if(httpCookie.IsNull())
            {
                return string.Empty;
            }

            return httpCookie.Value;
        }

        public static void RemoveCookie(string name)
        {
            var httpCookie = HttpContext.Current.Request.Cookies[GetCookieKey(name)];
            if (httpCookie.IsNotNull())
            {
                httpCookie.Domain = "dev.iloveecohotels.com";
                httpCookie.Expires = DateTime.Now.AddDays(-1);
                HttpContext.Current.Response.Cookies.Add(httpCookie);
            }
        }

        private static string GetCookieKey(string name)
        {
            return string.Concat(name, ".", VERSION);
        }
    }
}
