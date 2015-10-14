using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using EcoHotels.Core.Domain.Models.Security;
using EcoHotels.Core.Infrastructure.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EcoHotels.Core.Tests.Integration.Services
{
    [TestClass]
    public class UserServiceTester : UnittestContext
    {
        [TestMethod]
        public void Can_persist_entity()
        {
            var organizationService = (IOrganizationService)UnityServiceContainer.GetService(typeof(IOrganizationService));

            var organization = organizationService.FindById(1);

            var user = User.Create(organization, "drevil@mysite.com", "1234", RolesEnum.Backend);
            user.IsActive = true;

            organization.Users.Add(user);

            organizationService.Save(organization);
        }
    }
}
