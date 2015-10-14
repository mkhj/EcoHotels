using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using EcoHotels.Core.Infrastructure.Mappings;
using EcoHotels.Core.Infrastructure.NH;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NHibernate;
using NHibernate.Stat;

namespace EcoHotels.Core.Tests
{
    [TestClass]
    public class UnittestContext
    {
        static UnittestContext()
        {
            var log4netConfig = new FileInfo(@"C:\SourceCode\EcoHotels\EcoHotels.Core.Tests\Config\Log4net-unittest.config");
            if (log4netConfig.Exists)
            {
                log4net.Config.XmlConfigurator.Configure(log4netConfig);
            }            
        }

        public IStatistics Statistics
        {
            get { return SessionFactory.Statistics; }
        }

        public ISession Session
        {
            get { return SessionFactory.GetCurrentSession(); }
        }

        public UnityServiceContainer UnityServiceContainer { get; private set; }

        /// <summary>
        /// Runs before every test
        /// </summary>
        [TestInitialize]
        public void CreateSessionFactory()
        {
            UnityServiceContainer = new UnityServiceContainer();

            Console.WriteLine("Starting NH Session");
            SessionFactory.BeginTransaction();

            // class that can test mappings easily
            // new PersistenceSpecification<User>(session);
        }

        /// <summary>
        /// Runs after every test
        /// </summary>
        [TestCleanup]
        public void TestCleanup()
        {
            Console.WriteLine("Ending NH Session");
            SessionFactory.EndTransaction();
        }
    }
}
