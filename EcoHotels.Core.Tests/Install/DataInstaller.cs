using System;
using System.Globalization;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using EcoHotels.Core.Domain.Models;
using EcoHotels.Core.Domain.Models.Commerce;
using EcoHotels.Core.Domain.Models.Property;
using EcoHotels.Core.Infrastructure.NH;
using EcoHotels.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NHibernate;
using NHibernate.Criterion;

namespace EcoHotels.Core.Tests.Install
{
    [TestClass]
    public class DataInstaller
    {
        public ISession Session { get { return SessionFactory.GetCurrentSession(); }
        }

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

        [TestMethod]
        public void Insert_dates_into_calendar()
        {
            var startDate = new DateTime(2012, 12, 1);

            for(var i = 0; i <= 365 * 5; i++)
            {
                var date = Date.Create(startDate.AddDays(i));
                Session.SaveOrUpdate(date);
            }

            Session.Flush();
        }

        [TestMethod]
        public void insert_inventory_data()
        {
            var date = Session.Get<Date>(2);

            var inventory = RoomTypeInventory.Create(0, date);

            Session.Save(inventory);

            Session.Flush();
        }

        [TestMethod]
        public void insert_inventory_data2()
        {
            //var date = Session.Get<RoomTypeInventory>(1);

            //var criteria = DetachedCriteria.For(typeof (RoomTypeInventory))

            var criteria = DetachedCriteria.For(typeof(RoomTypeInventory))
                .CreateAlias("Date", "h")
                    .Add(Restrictions.Ge("h.Value", DateTime.Now.AddDays(0).Date))
                    .Add(Restrictions.Le("h.Value", DateTime.Now.AddDays(20).Date));

            var list = criteria.GetExecutableCriteria(Session).List<RoomTypeInventory>();

            var a = 0;
        }

        [TestMethod]
        public void insert_system_amenities()
        {
            var amenity1 = Amenity.Create("Bar");
            var amenity2 = Amenity.Create("Free Wifi");
            var amenity3 = Amenity.Create("Gym");
            var amenity4 = Amenity.Create("Handicap-accessible");
            var amenity5 = Amenity.Create("Parking on site");
            var amenity6 = Amenity.Create("Pet-friendly");
            var amenity7 = Amenity.Create("Pool");
            var amenity8 = Amenity.Create("Restuarant");
            var amenity9 = Amenity.Create("Room Service");
            var amenity10 = Amenity.Create("Spa");


            Session.Save(amenity1);
            Session.Save(amenity2);
            Session.Save(amenity3);
            Session.Save(amenity4);
            Session.Save(amenity5);
            Session.Save(amenity6);
            Session.Save(amenity7);
            Session.Save(amenity8);
            Session.Save(amenity9);
            Session.Save(amenity10);

            Session.Flush();
        }
    }
}
