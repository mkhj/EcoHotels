using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using EcoHotels.Core.Domain.Models.Commerce;
using EcoHotels.Core.Infrastructure.NH;
using EcoHotels.Core.Infrastructure.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EcoHotels.Core.Tests.Integration.Services
{
    [TestClass]
    public class RateServiceTester : UnittestContext
    {

        [TestMethod]
        public void Can_persist_entity()
        {
            var service = (IRateService) UnityServiceContainer.GetService(typeof (IRateService));

            var category = RateCategory.Create(null, "lalala", "sdfsdfsdf");

            var ratePrice = RatePrice.Create(category, 0, 0.0m, true);

            category.Items.Add(ratePrice);

            service.Save(category);
        }

        [TestMethod]
        public void Can_delete_entity()
        {
            var service = (IRateService)UnityServiceContainer.GetService(typeof(IRateService));

            var category = RateCategory.Create(null, "lalala", "sdfsdfsdf");

            var ratePrice = RatePrice.Create(category, 0, 0.0m, true);

            category.Items.Add(ratePrice);

            service.Save(category);

            service.Delete(category);

            var rateCategory = service.FindBy(category.Id);

            Assert.IsNull(rateCategory);
        }
    }
}
