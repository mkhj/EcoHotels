using System;
using System.Collections.Generic;
using System.Web.Mvc;
using EcoHotels.Core.Domain.Models.Security;
using EcoHotels.Web.Core.Services;

namespace EcoHotels.Web.Core.Attributes.Security
{
    public class AuthorizeAdmin : AuthorizeAttribute
    {

        private IAppService AppService;

        public AuthorizeAdmin()
        {
            AppService = DependencyResolver.Current.GetService(typeof(IAppService)) as IAppService;

            var roles = new List<RolesEnum>
                            {
                                RolesEnum.Backend,
                                RolesEnum.Supporter,
                                RolesEnum.Organization,
                            };

            Roles = string.Join(",", roles);
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            var id = AppService.GetCurrentOrganizationId();
            if(id != 0)
            {
                base.OnAuthorization(filterContext);   
                return;
            }
            this.HandleUnauthorizedRequest(filterContext);
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new RedirectResult("/admin/account/logon");
        }
    }
}
