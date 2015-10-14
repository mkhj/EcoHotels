using System.Collections.Generic;
using EcoHotels.Core.Domain.Models.Media;
using EcoHotels.Core.Infrastructure.NH;
using NHibernate.Criterion;

namespace EcoHotels.Core.Infrastructure.Services.Impl.Media
{
    public class AssetCategoryService : IAssetCategoryService
    {
        private Repository<AssetCategory> AssetCategoryRepo;

        public AssetCategoryService()
        {
            AssetCategoryRepo = new Repository<AssetCategory>();
        }

        public AssetCategory FindById(int id, int hotelId)
        {
            var criteria = DetachedCriteria.For(typeof(AssetCategory))
                .Add(Restrictions.Eq("Id", id))
                .Add(Restrictions.Eq("HotelId", hotelId));

            return AssetCategoryRepo.FindOne(criteria);
        }

        public IEnumerable<AssetCategory> FindAllByHotelId(int hotelId)
        {
            var criteria = DetachedCriteria.For(typeof(AssetCategory))
                .Add(Restrictions.Eq("HotelId", hotelId));

            return AssetCategoryRepo.FindAll(criteria);
        }

        public void Save(AssetCategory assetCategory)
        {
            if(assetCategory.IsValid())
            {
                AssetCategoryRepo.Save(assetCategory);   
            }
        }

        public void Delete(AssetCategory assetCategory)
        {
            AssetCategoryRepo.Remove(assetCategory);
        }
    }
}
