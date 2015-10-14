using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using EcoHotels.Core.Infrastructure.Services;
using EcoHotels.Core.Infrastructure.Services.Impl;
using EcoHotels.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EcoHotels.Core.Tests.Integration.Services
{
    [TestClass]
    public class ReservationServiceTester : UnittestContext
    {
        public IReservationService ReservationService = new ReservationService(null);

        [TestMethod]
        public void Can_fetch_reservation_by_hotel_and_travel_dates()
        {
            var hotelId = 0; // "27EFFBF2-3A36-4A7F-9FE5-A0B90117D63B".ToGuid();
            var arrival = new DateTime(2012, 9, 20);
            var departure = new DateTime(2012, 9, 25);

            var reservations = ReservationService.FindBy(hotelId, arrival, departure);


            Assert.IsTrue(reservations.Count() > 0);
        }
    }
}
