using System;
using System.Collections.Generic;
using System.Linq;
using EcoHotels.Core.Domain.Models.Commerce;
using EcoHotels.Core.Domain.Models.Property;
using EcoHotels.Core.Domain.Models.Tags;
using EcoHotels.Core.Domain.Value_objects;
using EcoHotels.Core.Infrastructure.NH;
using EcoHotels.Extensions;
using NHibernate.Criterion;
using NHibernate.SqlCommand;
using NHibernate.Transform;

namespace EcoHotels.Core.Infrastructure.Services.Impl
{
    public class SearchService : ISearchService
    {
        private Repository<SearchTagCity> SearchTagCityRepo;
        private Repository<Hotel> HotelRepo;
        private Repository<RateCategory> RateCategoryRepo;
        private Repository<RoomTypeInventory> RoomTypeInventoryRepo;
        private Repository<Reservation> ReservationRepo;

        public SearchService()
        {
            SearchTagCityRepo = new Repository<SearchTagCity>();
            HotelRepo = new Repository<Hotel>();
            RateCategoryRepo = new Repository<RateCategory>();
            RoomTypeInventoryRepo = new Repository<RoomTypeInventory>();
            ReservationRepo = new Repository<Reservation>();
        }

        public SearchResultHotel FindByHotel(int hotelId)
        {
            var hotel = HotelRepo.FindBy(hotelId);
            return SearchResultHotel.Create(hotel);
        }

        public SearchResultHotel FindByHotel(int hotelId, DateTime arrival, DateTime depature)
        {
            var hotel = HotelRepo.FindBy(hotelId);
            var hotelIds = new[] {hotel.Id};

            var roomtypeIds = hotel.RoomTypes.Select(x => x.Id).ToArray();

            var roomTypeInventories = FindRoomtypeInventories(roomtypeIds, arrival, depature);

            var rateCategories = FindRateCategories(hotelIds);

            var reservations = FindReservations(hotelIds, arrival, depature);

            var searchResult = SearchResultList.Create(new List<Hotel>{hotel}, reservations, roomTypeInventories, rateCategories, arrival, depature);

            return searchResult.Hotels.FirstOrDefault();
        }

        public SearchResultList FindByCity(int searchTagCityId, DateTime arrival, DateTime depature)
        {
            var hotels = FindHotels(searchTagCityId);

            var hotelIds = hotels.Select(x => x.Id).ToArray();
            var roomtypeIds = hotels.SelectMany(x => x.RoomTypes).Select(x => x.Id).ToArray();

            var roomTypeInventories = FindRoomtypeInventories(roomtypeIds, arrival, depature);

            var rateCategories = FindRateCategories(hotelIds);

            var reservations = FindReservations(hotelIds, arrival, depature);

            return SearchResultList.Create(hotels, reservations, roomTypeInventories, rateCategories, arrival, depature);
        }

        private ICollection<Hotel> FindHotels(int searchTagCityId)
        {
            var criteria = DetachedCriteria.For(typeof(SearchTagCity))
                                //.SetFetchMode("Hotels", FetchMode.Eager)
                                .Add(Restrictions.Eq("Id", searchTagCityId));

            var searchTagCities = SearchTagCityRepo.FindFirst(criteria);
            if (searchTagCities.IsNull())
            {
                throw new NullReferenceException("A Search tag for the given city was not found.");
            }

            return searchTagCities.Hotels;
        }

        private IEnumerable<Reservation> FindReservations(int[] hotelIds, DateTime arrival, DateTime depature)
        {
            var criteria = DetachedCriteria.For(typeof (Reservation))                
                .Add(Restrictions.In("HotelId", hotelIds))
                .CreateAlias("Items", "i", JoinType.LeftOuterJoin) // equals .SetFetchMode("i", FetchMode.Eager)
                .CreateAlias("i.PricePerDate", "p")
                .Add(Restrictions.Ge("p.Date", arrival.Date))
                .Add(Restrictions.Le("p.Date", depature.Date))
                .SetResultTransformer(new DistinctRootEntityResultTransformer());

            return ReservationRepo.FindAll(criteria);
        }

        private IEnumerable<RateCategory> FindRateCategories(int[] hotelIds)
        {
            var criteria = DetachedCriteria.For(typeof(RateCategory))
                .Add(Restrictions.In("Hotel.Id", hotelIds));

            return RateCategoryRepo.FindAll(criteria);
        }

        private IEnumerable<RoomTypeInventory> FindRoomtypeInventories(int[] roomtypeIds, DateTime arrival, DateTime depature)
        {
            var criteria = DetachedCriteria.For(typeof(RoomTypeInventory))
                .Add(Restrictions.In("RoomTypeId", roomtypeIds))
                    .CreateAlias("Date", "d")
                        .Add(Restrictions.Ge("d.Value", arrival.Date))
                        .Add(Restrictions.Le("d.Value", depature.Date));

            return RoomTypeInventoryRepo.FindAll(criteria);
        }
    }
}
