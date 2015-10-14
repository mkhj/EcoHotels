using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using EcoHotels.Core.Domain.Models.Security;
using EcoHotels.Web.Core.Services;

namespace EcoHotels.Web.Core.Attributes.Security
{
    public class AuthorizeCustomer : AuthorizeAttribute
    {
        public AuthorizeCustomer()
        {
            var roles = new List<RolesEnum>
                            {
                                RolesEnum.Customer,
                                RolesEnum.Backend
                            };

            Roles = string.Join(",", roles);
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new RedirectResult("/");
        }
    }
}
