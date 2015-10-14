using System.Collections.Generic;
using System.Linq;
using EcoHotels.Core.Domain.Models.Commerce;
using EcoHotels.Core.Infrastructure.Cache;
using EcoHotels.Core.Infrastructure.NH;
using NHibernate.Criterion;

namespace EcoHotels.Core.Infrastructure.Services.Impl.Price
{
    public class AddonService : IAddonService
    {
        private ICacheStorage CacheStorage;
        private Repository<Addon> AddonRepo;

        public AddonService(ICacheStorage cacheStorage)
        {
            CacheStorage = cacheStorage;
            AddonRepo = new Repository<Addon>();           
        }

        public Addon FindById(int hotelId, int id)
        {
            var addons = FindAllByHotelId(hotelId);
            return addons.First(x => x.Id == id);
        }

        public IEnumerable<Addon> FindAll()
        {
            return AddonRepo.FindAll();
        }

        public IEnumerable<Addon> FindAllByHotelId(int hotelId)
        {
            var criteria = DetachedCriteria.For(typeof(Addon))
                .Add(Restrictions.Eq("Hotel.Id", hotelId));

            return AddonRepo.FindAll(criteria);

            //return AddonRepo.Session.CreateSQLQuery("SELECT Addons.* FROM Addons WHERE HotelId = :HotelId")
            //    .SetInt32("HotelId", hotelId)
            //    .List<Addon>();
        }

        public void Save(Addon addon)
        {
            if(addon.IsValid())
            {
                AddonRepo.Save(addon);
            }
        }


        public void Delete(Addon addon)
        {
            AddonRepo.Remove(addon);
        }
    }
}
