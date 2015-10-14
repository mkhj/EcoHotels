using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using EcoHotels.Core.Domain.Models.Property;
using EcoHotels.Core.Infrastructure.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EcoHotels.Core.Tests.Integration.Services
{
    [TestClass]
    public class OrganizationServiceTester : UnittestContext
    {
        [TestMethod, ]
        public void Can_persist_entity()
        {
            var organizationService = (IOrganizationService)UnityServiceContainer.GetService(typeof(IOrganizationService));
            var currencyService = (ICurrencyService)UnityServiceContainer.GetService(typeof(ICurrencyService));

            var organization = Organization.Create("I Love Eco Hotels", "9999999999");

            var hotel = Hotel.Create("My hotel", "sdfsdfsdf", organization);
            hotel.Currency = currencyService.FindById(5);

            organization.Hotels.Add(hotel);

            organizationService.Save(organization);

            Assert.IsTrue(organization.Id > 0);
        }

        [TestMethod]
        public void Can_find_entity_by_id()
        {
            var organizationService = (IOrganizationService)UnityServiceContainer.GetService(typeof(IOrganizationService));

            var organization = organizationService.FindById(1);

            Assert.IsNotNull(organization);

            Assert.IsTrue(organization.Hotels.Count > 0);
        }

        [TestMethod]
        public void Can_delete_entity()
        {
            var organizationService = (IOrganizationService)UnityServiceContainer.GetService(typeof(IOrganizationService));

            var organization = organizationService.FindById(4);

            Assert.IsNotNull(organization);

            organizationService.Delete(organization);
        }
    }
}
