using System.Collections.Generic;
using System.Linq;
using EcoHotels.Core.Common;
using EcoHotels.Core.Domain.Models.Commerce;
using EcoHotels.Core.Domain.Models.Property;
using EcoHotels.Core.Infrastructure.Cache;
using EcoHotels.Core.Infrastructure.NH;
using EcoHotels.Extensions;
using NHibernate.Criterion;
using NHibernate.Linq;

namespace EcoHotels.Core.Infrastructure.Services.Impl.Price
{

    public class RateService : IRateService
    {
        private Repository<RateCategory> RateCategoryRepo { get; set; }

        public RateService(ICacheStorage cacheStorage)
        {            
            RateCategoryRepo = new Repository<RateCategory>();
        }

        public RateCategory FindBy(int id)
        {
            return RateCategoryRepo.FindBy(id);
        }

        public RateCategory FindBy(int id, int hotelId)
        {
            var criteria = DetachedCriteria.For(typeof(RateCategory))
                    .Add(Restrictions.Eq("Id", id))
                    .Add(Restrictions.Eq("Hotel.Id", hotelId));

            return RateCategoryRepo.FindOne(criteria);
        }

        public IEnumerable<RateCategory> FindByHotel(int hotelId)
        {
            var criteria = DetachedCriteria.For(typeof(RateCategory))
                    .Add(Restrictions.Eq("Hotel.Id", hotelId));

            return RateCategoryRepo.FindAll(criteria);
        }

        public void EnsureRatesForNewRoomType(RoomType roomType)
        {
            Check.Require(roomType.Id > 0, "Roomtype has not been persisted.");
            Check.Require(roomType.Hotel.IsNotNull(), "Roomtype needs to be associated with a hotel.");

            var categories = FindByHotel(roomType.Hotel.Id);
            foreach (var category in categories)
            {
                var roomtypeHasNotBeenAdded = !category.Items.Any(x => x.RoomTypeId == roomType.Id);
                if (roomtypeHasNotBeenAdded)
                {
                    var ratePrice = RatePrice.Create(category, roomType.Id, 0.0m, false);
                    category.Items.Add(ratePrice);   
                }
            }

            Save(categories);
        }

        public void RemoveRatesForRoomType(RoomType roomType)
        {
            Check.Require(roomType.Id > 0, "Roomtype has not been persisted.");
            Check.Require(roomType.Hotel.IsNotNull(), "Roomtype needs to be associated with a hotel.");

            RateCategoryRepo.Session.CreateSQLQuery(
                "UPDATE [RoomTypeInventory] SET [RateCategoryId] = NULL WHERE RoomTypeId = :RoomTypeId")
                .SetInt32("RoomTypeId", roomType.Id)
                .ExecuteUpdate();

            //NOTE: consider deleting them directly instead for selecting first. (will this affect caching?)

            var categories = FindByHotel(roomType.Hotel.Id);
            foreach (var category in categories)
            {
                var ratePrice = category.Items.FirstOrDefault(x => x.RoomTypeId == roomType.Id);
                if(ratePrice.IsNotNull())
                {
                    category.Items.Remove(ratePrice);
                }
            }

            Save(categories);


        }

        public void Delete(RateCategory category)
        {
            RateCategoryRepo.Remove(category);
        }

        public void Save(RateCategory category)
        {
            if(category.IsValid())
            {
                RateCategoryRepo.Save(category);
            }
        }

        public void Save(IEnumerable<RateCategory> categories)
        {
            categories.ForEach(Save);
        }
    }
}
