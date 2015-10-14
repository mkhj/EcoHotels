using System;
using System.Collections.Generic;
using EcoHotels.Core.Infrastructure.NH;
using EcoHotels.Core.Domain.Models.Property;
using EcoHotels.Core.Infrastructure.Cache;
using NHibernate.Criterion;

namespace EcoHotels.Core.Infrastructure.Services.Impl.Property
{
    public class HotelService : IHotelService
    {
        private ICacheStorage CacheStorage;

        private Repository<Hotel> HotelRepo;

        public HotelService(ICacheStorage cacheStorage)
        {
            CacheStorage = cacheStorage;

            HotelRepo = new Repository<Hotel>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Hotel FindById(int id)
        {
            return HotelRepo.FindBy(id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="organizationId"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public Hotel FindById(int organizationId, int id)
        {
            var criteria = DetachedCriteria.For(typeof(Hotel))
                .Add(Restrictions.Eq("Id", id))
                .Add(Restrictions.Eq("Organization.Id", organizationId));

            return HotelRepo.FindOne(criteria);
        }

        public IEnumerable<Hotel> FindAll()
        {
            return HotelRepo.FindAll();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Hotel> FindAllInActive()
        {
            var criteria = DetachedCriteria.For(typeof(Hotel))
                .Add(Restrictions.Eq("IsActive", false));

            return HotelRepo.FindAll(criteria);
        }

        public IEnumerable<Hotel> FindAllHotelsToVerify()
        {
            var criteria = DetachedCriteria.For(typeof(Hotel))
                .Add(Restrictions.Or(Restrictions.IsNull("Verified"), Restrictions.LtProperty("Verified", "Modified")));

            return HotelRepo.FindAll(criteria);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public long CountHotelsToVerify()
        {
            var criteria = DetachedCriteria.For(typeof(Hotel))
                .Add(Restrictions.Or(Restrictions.IsNull("Verified"), Restrictions.LtProperty("Verified", "Modified")));

            return HotelRepo.Count(criteria);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public long CountInActiveHotels()
        {
            var criteria = DetachedCriteria.For(typeof(Hotel))
                .Add(Restrictions.Eq("IsActive", false));

            return HotelRepo.Count(criteria);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="hotels"></param>
        public void Save(IEnumerable<Hotel> hotels)
        {
            foreach (var hotel in hotels)
            {
                Save(hotel);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hotel"></param>
        public void Save(Hotel hotel)
        {
            if (hotel.IsValid())
            {
                hotel.PageInformation.LastModified = DateTime.Now;
                HotelRepo.Save(hotel);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hotel"></param>
        public void Delete(Hotel hotel)
        {
            HotelRepo.Remove(hotel);
        }
    }


}
