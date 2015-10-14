using System;
using System.Globalization;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using EcoHotels.Core.Domain.Models.Property;
using EcoHotels.Core.Domain.Models.Security;
using EcoHotels.Core.Helpers;
using EcoHotels.Core.Infrastructure;
using EcoHotels.Core.Infrastructure.Mappings;
using EcoHotels.Core.Infrastructure.NH;
using EcoHotels.Core.Infrastructure.Services;
using EcoHotels.Extensions;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Testing;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EcoHotels.Core.Tests
{
    [TestClass]
    public class UnitTest1
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
        public void CreateUser()
        {
            var organization = Organization.Create("Hotel Guldsmeden", "sdfsdf");

            var user = User.Create(organization, "drevil@mysite.com", "123456", RolesEnum.Backend);
            organization.Users.Add(user);            
        }

        [TestMethod]
        public void nHibernate_unit_test()
        {
            var unityServiceContainer = new UnityServiceContainer();

            var service = (IOrganizationService)unityServiceContainer.TryGetService(typeof(IOrganizationService));
            var organization = service.FindById(0);

            Assert.IsNotNull(organization);
        }

        [TestMethod]
        public void Login()
        {


            //var criteria = DetachedCriteria.For(typeof(User))
            //    .Add(Restrictions.Eq("Email", "info@iloveecohotels.com".Trim()));

            //var user = criteria.GetExecutableCriteria(session).UniqueResult<User>();

            //var shouldBeInvalid = PasswordHelper.ValidatePassword("123456", user.Password);
            //Assert.IsFalse(shouldBeInvalid);

            //var isValid = PasswordHelper.ValidatePassword("123456", user.Password);
            //Assert.IsTrue(isValid);
        }

        [TestMethod]
        public void Test_first_level_cache()
        {
            //var log4netConfig = new FileInfo(@"C:\SourceCode\EcoHotels\EcoHotels.Core.Tests\Config\Log4net-unittest.config");
            //if (log4netConfig.Exists)
            //{
            //    log4net.Config.XmlConfigurator.Configure(log4netConfig);
            //}

            //var session = SessionFactory.OpenSession();

            //session.BeginTransaction();
            //CurrentSessionContext.Bind(session);

            //var criteria = DetachedCriteria.For(typeof(User))
            //    .Add(Restrictions.Eq("Email", "info@iloveecohotels.com".Trim()));

            //var user1 = session.Load<User>("92E89484-A2FD-4834-B4E8-A06900C82946".ToGuid());
            //var user2 = session.Load<User>("92E89484-A2FD-4834-B4E8-A06900C82946".ToGuid());
            
            //var session2 = CurrentSessionContext.Unbind(SessionFactory);
            //session2.Transaction.Commit();

            //session2.Close();
            //session2.Dispose();
        }

        [TestMethod]
        public void Test2()
        {
            //var cultureInfo = CultureInfo.GetCultureInfo("SWE");
            //Console.WriteLine(cultureInfo.LCID);

            var regionCulture = new RegionInfo("AO");
            Console.WriteLine(regionCulture.ISOCurrencySymbol);
        }

        [TestMethod]
        public void Test()
        {
            Console.WriteLine("{0,-30}, {1,-12}, {2,-12}, {3,-12}", "English", "Name", "Currency", "TwoIso");

            foreach (var cultureInfo in CultureInfo.GetCultures(CultureTypes.SpecificCultures))
            {

                if (!cultureInfo.Equals(CultureInfo.InvariantCulture) && !cultureInfo.IsNeutralCulture)
                {
                    var regionCulture = new RegionInfo(cultureInfo.LCID);

                    // cultureInfo.Name == da-DK


                    var content = string.Format("{0,-30}, {1,-12}, {2,-12}, {3,-12}, {4,-4}",
                        regionCulture.EnglishName, 
                        regionCulture.TwoLetterISORegionName, 
                        regionCulture.ISOCurrencySymbol, 
                        cultureInfo.TwoLetterISOLanguageName,
                        cultureInfo.LCID);

                    Console.WriteLine(content);

                    //if (regionCulture.ISOCurrencySymbol == isoSymbol)
                    //{
                    //    return cultureInfo.LCID;

                    //    //return new NumberFormatInfo
                    //    //{
                    //    //    CurrencyNegativePattern = 1, // cultureInfo.NumberFormat.CurrencyNegativePattern,
                    //    //    CurrencyPositivePattern = 1, // cultureInfo.NumberFormat.CurrencyPositivePattern,
                    //    //    CurrencyGroupSizes = NumberFormatInfo.InvariantInfo.CurrencyGroupSizes, // cultureInfo.NumberFormat.CurrencyGroupSizes,
                    //    //    CurrencyDecimalDigits = cultureInfo.NumberFormat.CurrencyDecimalDigits,
                    //    //    CurrencyDecimalSeparator = cultureInfo.NumberFormat.CurrencyDecimalSeparator,
                    //    //    CurrencyGroupSeparator = cultureInfo.NumberFormat.CurrencyGroupSeparator,
                    //    //    CurrencySymbol = isoSymbol
                    //    //};
                    //}
                }
            }
        }


        [TestMethod]
        public void TestMethod1()
        {
            var random = new RNGCryptoServiceProvider();
            var salt = new byte[32]; //256 bits
            random.GetBytes(salt);

            Console.WriteLine(Convert.ToBase64String(salt));
            Console.WriteLine(BytesToHex(salt));

            Assert.Inconclusive();
        }

        //Converts a byte array to a hex string
        private static string BytesToHex(byte[] toConvert)
        {
            var s = new StringBuilder(toConvert.Length * 2);
            foreach (var b in toConvert)
            {
                s.Append(b.ToString("x2"));
            }
            return s.ToString();
        }
    }
}
