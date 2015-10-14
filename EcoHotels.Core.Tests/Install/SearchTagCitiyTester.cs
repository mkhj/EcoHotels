using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using EcoHotels.Core.Domain.Models.Tags;
using EcoHotels.Core.Infrastructure.NH;
using EcoHotels.Core.Infrastructure.Repositories.NH;
using EcoHotels.Core.Infrastructure.Repositories.NH.Tags;
using EcoHotels.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EcoHotels.Core.Tests.Install
{
    [TestClass]
    public class SearchTagCitiyTester
    {
        #region - Test Setup -

        [TestInitialize]
        public void ClassInit()
        {
            SessionFactory.BeginTransaction();
        }

        [TestCleanup]
        public void ClassCleanup()
        {
            SessionFactory.EndTransaction();
        }

        #endregion

        [TestMethod]
        public void TestMethod2()
        {
            Console.WriteLine(DateTime.Now.Date.ToUnixTimestamp());
        }

        [TestMethod]
        public void TestMethod1()
        {
            //var countryRepo = new CountryRepo();

            //var repo = new SearchTagCityRepo();

            //var denmark = countryRepo.FindBy(61);
            //repo.Save(SearchTagCity.Create("Copenhagen", denmark));
            //repo.Save(SearchTagCity.Create("Aarhus", denmark));


            //var norway = countryRepo.FindBy(166);
            //repo.Save(SearchTagCity.Create("Oslo", norway));


            //var uk = countryRepo.FindBy(235);
            //repo.Save(SearchTagCity.Create("London", uk));

            //var france = countryRepo.FindBy(76);
            //repo.Save(SearchTagCity.Create("Paris", france));
        }
    }
}
