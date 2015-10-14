using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web.Mvc;
using EcoHotels.Core.Infrastructure.Services;
using EcoHotels.Extensions;
using EcoHotels.Web.Core.Helpers;
using EcoHotels.Web.Core.Services;

namespace EcoHotels.Web.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class LocalizedAttribute : ActionFilterAttribute
    {
        private ILanguageService LanguageService;
        private ILocationService LocationService;
        private ICurrencyService CurrencyService;

        public LocalizedAttribute()
        {
            LanguageService = DependencyResolver.Current.GetService(typeof(ILanguageService)) as ILanguageService;
            LocationService = DependencyResolver.Current.GetService(typeof(ILocationService)) as ILocationService;
            CurrencyService = DependencyResolver.Current.GetService(typeof(ICurrencyService)) as ICurrencyService;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            #region - Language -

            var lang = (filterContext.RouteData.Values["lang"] ?? string.Empty).ToString();
            if(lang.IsNullOrEmpty())
            {
                filterContext.Result = new HttpStatusCodeResult(404);
                base.OnActionExecuting(filterContext);
                return;
            }

            //var language = LanguageService.FindByShortName(lang);
            //if(language.IsNull())
            //{
            //    filterContext.Result = new HttpStatusCodeResult(404, "lalalal");
            //    base.OnActionExecuting(filterContext);
            //    return;
            //}

            //var ci = new CultureInfo(language.Shortname);
            //Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture(ci.Name);
            //Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(ci.Name);

            //filterContext.HttpContext.Items.Add("CURRENT_LANGUAGE", language);

            #endregion

            #region - Currency -

            var currencyId = CookieHelper.GetCookie(CookieHelper.ECOHOTELS_CURRENCY_COOKIE);
            var currency = CurrencyService.FindById(currencyId.ToInt());

            if (currency.IsNull())
            {
                var country = LocationService.FindLocationByIp(filterContext.HttpContext.Request.UserHostAddress);
                filterContext.HttpContext.Items.Add("CURRENT_CURRENCY", country.Currency);
                base.OnActionExecuting(filterContext);
                return;
            }

            filterContext.HttpContext.Items.Add("CURRENT_CURRENCY", currency);
            base.OnActionExecuting(filterContext);

            #endregion
        }
    }
}
