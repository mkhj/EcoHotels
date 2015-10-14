using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using EcoHotels.Core.Domain.Models.Commerce;
using EcoHotels.Core.Domain.Models.Property;
using EcoHotels.Core.Domain.Value_objects;
using EcoHotels.Core.Infrastructure.NH;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NHibernate.Criterion;
using NHibernate.SqlCommand;
using NHibernate.Transform;

namespace EcoHotels.Core.Tests.Unit
{
    [TestClass]
    public class SearchTester : UnittestContext
    {

        [TestMethod]
        public void Can_find_hotels_with_available_rooms()
        {
            var searchResult = BuildSearchResult();

            Assert.IsTrue(searchResult.Hotels.Count() > 0);

            var searchResultHotel = searchResult.Hotels.First();

            Assert.IsTrue(searchResultHotel.HasAvailableRooms);
        }

        [TestMethod]
        public void Room_is_available_for_selected_days()
        {
            var searchResult = BuildSearchResult();
            var searchResultHotel = searchResult.Hotels.First();

            var searchResultRoom = searchResultHotel.Rooms.First();

            Assert.IsTrue(searchResultRoom.IsAvailable);
        }

        [TestMethod]
        public void Can_calculate_lowest_price_for_room()
        {
            var searchResult = BuildSearchResult();
            var searchResultHotel = searchResult.Hotels.First();

            var searchResultRoom = searchResultHotel.Rooms.First();

            Assert.IsTrue(searchResultRoom.LowestPricePerDay == 10);
        }

        [TestMethod]
        public void Can_calculate_total_price_for_room()
        {
            var searchResult = BuildSearchResult();
            var searchResultHotel = searchResult.Hotels.First();

            var searchResultRoom = searchResultHotel.Rooms.First();

            Assert.IsTrue(searchResultRoom.TotalPrice == 20);
        }


        private SearchResultList BuildSearchResult()
        {
            var arrival = DateTime.Now;
            var departure = DateTime.Now.AddDays(2);

            var hotel = FindHotel();
            var reservations = FindReservations(new[] { 1 }, arrival, departure);
            var roomtypeInventories = FindRoomtypeInventories(hotel.RoomTypes.Select(x => x.Id).ToArray(), arrival, departure);
            var rateCategories = FindRateCategories(new[] { 1 });

            return SearchResultList.Create(new List<Hotel> { hotel }, reservations, roomtypeInventories, rateCategories, arrival, departure);
        }

        private Hotel FindHotel()
        {
            return new Repository<Hotel>().FindBy(1);
        }

        private IEnumerable<Reservation> FindReservations(int[] hotelIds, DateTime arrival, DateTime depature)
        {
            var criteria = DetachedCriteria.For(typeof(Reservation))
                .Add(Restrictions.In("HotelId", hotelIds))
                .CreateAlias("Items", "i", JoinType.LeftOuterJoin) // equals .SetFetchMode("i", FetchMode.Eager)
                .CreateAlias("i.PricePerDate", "p")
                .Add(Restrictions.Ge("p.Date", arrival.Date))
                .Add(Restrictions.Le("p.Date", depature.Date))
                .SetResultTransformer(new DistinctRootEntityResultTransformer());

            return new Repository<Reservation>().FindAll(criteria);
        }

        private IEnumerable<RoomTypeInventory> FindRoomtypeInventories(int[] roomtypeIds, DateTime arrival, DateTime depature)
        {
            var criteria = DetachedCriteria.For(typeof(RoomTypeInventory))
                .Add(Restrictions.In("RoomTypeId", roomtypeIds))
                    .CreateAlias("Date", "d")
                        .Add(Restrictions.Ge("d.Value", arrival.Date))
                        .Add(Restrictions.Le("d.Value", depature.Date));

            return new Repository<RoomTypeInventory>().FindAll(criteria);
        }

        private IEnumerable<RateCategory> FindRateCategories(int[] hotelIds)
        {
            var criteria = DetachedCriteria.For(typeof(RateCategory))
                .Add(Restrictions.In("Hotel.Id", hotelIds));

            return new Repository<RateCategory>().FindAll(criteria);
        }
    }
}
