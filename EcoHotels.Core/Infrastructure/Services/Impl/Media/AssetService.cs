using System.Collections.Generic;
using System.Linq;
using EcoHotels.Core.Domain.Models.Media;
using EcoHotels.Core.Infrastructure.Cache;
using EcoHotels.Core.Infrastructure.NH;
using NHibernate.Criterion;

namespace EcoHotels.Core.Infrastructure.Services.Impl.Media
{
    public class AssetService : IAssetService
    {
        private ICacheStorage CacheStorage;

        private Repository<Asset> AssetRepo;

        public AssetService(ICacheStorage cacheStorage)
        {
            CacheStorage = cacheStorage;

            AssetRepo = new Repository<Asset>();
        }

        public Asset FindById(int assetId)
        {
            return AssetRepo.FindBy(assetId);
        }

        public IEnumerable<Asset> FindByIds(int hotelId, IEnumerable<int> ids)
        {
            var criteria = DetachedCriteria.For(typeof(Asset))
                .Add(Restrictions.In("Id", ids.ToArray()))
                .CreateAlias("Category", "c")
                    .Add(Restrictions.Eq("c.HotelId", hotelId));

            return AssetRepo.FindAll(criteria);
        }

        public IEnumerable<Asset> FindAllByHotelId(int hotelId)
        {
            var criteria = DetachedCriteria.For(typeof(Asset))
                .CreateAlias("Category", "c")
                    .Add(Restrictions.Eq("c.HotelId", hotelId));

            return AssetRepo.FindAll(criteria);
        }

        public IEnumerable<Asset> FindAll()
        {
            return AssetRepo.FindAll();
        }

        public void Delete(Asset asset)
        {
            AssetRepo.Remove(asset);
        }


        public void Save(Asset asset)
        {
            if(asset.IsValid())
            {
                AssetRepo.Save(asset);
            }
        }

    }
}
