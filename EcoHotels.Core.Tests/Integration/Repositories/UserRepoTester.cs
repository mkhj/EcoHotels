using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using EcoHotels.Core.Domain.Models.Security;
using EcoHotels.Core.Infrastructure.NH;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NHibernate.Criterion;

namespace EcoHotels.Core.Tests.Integration.Repositories
{
    [TestClass]
    public class UserRepoTester : UnittestContext
    {
        [TestMethod]
        public void TestMethod1()
        {
            var email = "'; DROP TABLE [Users] --";
            var expression = Restrictions.Eq("Email", email);

            
            var criteria = DetachedCriteria.For(typeof(User))
                .Add(expression);

            var user = new Repository<User>().FindOne(criteria);

            Assert.IsNull(user);
        }
    }
}
