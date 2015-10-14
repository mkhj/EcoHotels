using System;
using System.Web.Mvc;
using EcoHotels.Core.Infrastructure.Services;
using EcoHotels.Extensions;

namespace EcoHotels.Web.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class CurrencyAttribute : ActionFilterAttribute
    {
        private ICurrencyService CurrencyService { get; set; }

        public CurrencyAttribute()
        {
            CurrencyService = DependencyResolver.Current.GetService(typeof(ICurrencyService)) as ICurrencyService;
        }

        public override void  OnActionExecuting(ActionExecutingContext filterContext)
        {
            var httpCookie = filterContext.HttpContext.Request.Cookies["EcoHotels.Currency"];
            if(httpCookie.IsNull())
            {
                //CurrencyService.FindById()
                //httpCookie.Value;
            }

            base.OnActionExecuting(filterContext);
        }
    }
}
