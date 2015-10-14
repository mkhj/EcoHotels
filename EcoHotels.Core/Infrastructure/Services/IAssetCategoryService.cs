using System.Collections.Generic;
using EcoHotels.Core.Domain.Models.Media;

namespace EcoHotels.Core.Infrastructure.Services
{
    public interface IAssetCategoryService
    {
        AssetCategory FindById(int id, int hotelId);

        IEnumerable<AssetCategory> FindAllByHotelId(int hotelId);

        void Save(AssetCategory assetCategory);

        void Delete(AssetCategory assetCategory);
    }
}
