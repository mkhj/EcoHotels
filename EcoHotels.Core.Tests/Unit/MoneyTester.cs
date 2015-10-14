using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using EcoHotels.Core.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EcoHotels.Core.Domain.Value_objects;

namespace EcoHotels.Core.Tests.Unit
{
    [TestClass]
    public class MoneyTester
    {
        [TestMethod]
        public void can_convert_from_usd_to_dkk()
        {
            var USD = Currency.Create("USD", 0, "USD", 589.97m);
            var DKK = Currency.Create("DKK", 0, "DKK", 100);

            var usdMoney = Money.Create(99, USD);
            var dkkMoney = usdMoney.ConvertTo(DKK);

            Assert.AreEqual(dkkMoney.Value, 584.07m);
        }

        [TestMethod]
        public void can_convert_from_dkk_to_usd()
        {
            var USD = Currency.Create("USD", 0, "USD", 589.97m);
            var DKK = Currency.Create("DKK", 0, "DKK", 100);

            var dkkMoney = Money.Create(584.07m, DKK);
            var usdMoney = dkkMoney.ConvertTo(USD);

            Assert.AreEqual(usdMoney.Value, 99.0m);
        }

        [TestMethod]
        public void can_convert_from_usd_to_nok()
        {
            var USD = Currency.Create("USD", 0, "USD", 589.97m);
            var NOK = Currency.Create("NOK", 0, "NOK", 98.77m);

            var usdMoney = Money.Create(99, USD);
            var nokMoney = usdMoney.ConvertTo(NOK);

            Assert.AreEqual(nokMoney.Value, 591.344m);
        }
    }
}
