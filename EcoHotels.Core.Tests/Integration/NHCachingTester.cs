using System;
using System.Diagnostics;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using EcoHotels.Core.Domain.Models;
using EcoHotels.Core.Infrastructure.Repositories.NH;
using EcoHotels.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NHibernate.Criterion;

namespace EcoHotels.Core.Tests.Integration
{
    /// <summary>
    /// Summary description for NHCachingTester
    /// </summary>
    /// <seealso cref="http://nhibernate.hibernatingrhinos.com/28/first-and-second-level-caching-in-nhibernate"/>
    [TestClass]
    public class NHCachingTester : UnittestContext
    {

        [TestMethod]
        public void Will_fetch_from_cache_when_using_a_Query()
        {
            //var repo = new Repository();
            //var entityNotInCache = repo.FindByISOCode("DK");

            //Assert.AreEqual(0, Statistics.QueryCacheHitCount);

            //var entityShouldBeInCache = repo.FindByISOCode("DK");

            //Assert.AreEqual(1, Statistics.QueryCacheHitCount);
        }

        [TestMethod]
        public void Will_fetch_from_First_Level_Cache_when_using_Id_in_same_SessionScope()
        {
            //var repo = new CountryRepo();
            //var entityNotInCache = repo.FindBy(61);

            //Assert.AreEqual(0, Statistics.SecondLevelCacheHitCount);

            //var entityShouldBeInCache = repo.FindBy(61);

            //Assert.AreEqual(0, Statistics.SecondLevelCacheHitCount);
        }

        [TestMethod]
        public void Will_fetch_from_Second_Level_Cache_when_using_Id_with_different_SessionScopes()
        {
            using (var session = Session.SessionFactory.OpenSession())
            {
                var entityNotInCache = session.Get<Country>(61);

                Assert.IsNotNull(entityNotInCache);

                Assert.AreEqual(0, Statistics.SecondLevelCacheHitCount);
            }

            using(var session = Session.SessionFactory.OpenSession())
            {
                var entityShouldBeInCache = session.Get<Country>(61);

                Assert.IsNotNull(entityShouldBeInCache);

                Assert.AreEqual(1, Statistics.SecondLevelCacheHitCount);
            }
        }

        [TestMethod]
        public void test()
        {
            //Console.WriteLine(DateTime.Now.ToUnixTimestamp());

            //var repo = new CountryRepo();

            //using (var session = Session.SessionFactory.OpenSession())
            //{
            //    var entitiesNotInCache = session.CreateCriteria<Country>().List();

            //    Assert.AreEqual(0, Statistics.SecondLevelCacheHitCount);
            //}          

            

            //using (var session = Session.SessionFactory.OpenSession())
            //{
            //    var entityShouldBeInCache = session.Get<Country>(61);

            //    Assert.IsNotNull(entityShouldBeInCache);

            //    Assert.AreEqual(1, Statistics.SecondLevelCacheHitCount);
            //}

            //using (var session = Session.SessionFactory.OpenSession())
            //{
            //    var entityShouldBeInCache = session.Get<Country>(61);

            //    Assert.AreEqual(2, Statistics.SecondLevelCacheHitCount);

            //    entityShouldBeInCache.Name = entityShouldBeInCache.Name + "12";

            //    session.Save(entityShouldBeInCache);
            //    session.Flush();
            //}

            //using (var session = Session.SessionFactory.OpenSession())
            //{
            //    var entityShouldBeInCache = session.Get<Country>(62);

            //    Assert.IsNotNull(entityShouldBeInCache);

            //    Assert.AreEqual(2, Statistics.SecondLevelCacheHitCount);
            //}

            

            //using (var session = Session.SessionFactory.OpenSession())
            //{
            //    var entityShouldBeInCache = DetachedCriteria.For(typeof(Country))
            //        .SetCacheable(true)
            //        .Add(Restrictions.Eq("Alpha2Code", "DK"))
            //        .GetExecutableCriteria(session).UniqueResult<Country>();

            //    Assert.IsNotNull(entityShouldBeInCache);

            //    Assert.AreEqual(2, Statistics.SecondLevelCacheHitCount);
            //}

            //var entityShouldBeInCache = repo.FindByISOCode("DK");

            //Assert.AreEqual(1, Statistics.SecondLevelCacheHitCount);
        }
    }
}
