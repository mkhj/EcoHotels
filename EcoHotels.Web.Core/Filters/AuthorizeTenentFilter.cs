using System.Web.Mvc;
using Hotelr.Core.Web.Services;
using Hotelr.Web.Core.Services;

namespace Hotelr.Core.Web.Filters
{
    public sealed class AuthorizeTenentFilter : AuthorizeAttribute
    {
        public ITenantService TenantService { get; set; }

        public AuthorizeTenentFilter()
        {
            TenantService = DependencyResolver.Current.GetService(typeof(ITenantService)) as ITenantService;
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            var isValidTenent = TenantService.GetCurrentTenant().IsValid;
            if (!isValidTenent)
            {
                //filterContext.Result = new HttpUnauthorizedResult();
                base.HandleUnauthorizedRequest(filterContext);
            }

            base.OnAuthorization(filterContext);

            //bool skipAuthorization = filterContext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true)
            //|| filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true);
            //if (applyAuthorization)
            //{
            //    base.OnAuthorization(filterContext);
            //}
        }
    }
}
