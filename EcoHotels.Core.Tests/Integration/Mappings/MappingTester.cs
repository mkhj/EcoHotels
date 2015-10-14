using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using EcoHotels.Core.Domain.Models.Security;
using FluentNHibernate.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EcoHotels.Core.Tests.Integration.Mappings
{
    [TestClass]
    public class MappingTester : UnittestContext
    {
        [TestMethod]
        public void TestMethod1()
        {
            // class that can test mappings easily
            var persistenceSpecification = new PersistenceSpecification<User>(Session);

            var mappings = persistenceSpecification.VerifyTheMappings();
        }
    }
}
