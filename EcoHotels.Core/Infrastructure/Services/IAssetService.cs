using System.Collections.Generic;
using EcoHotels.Core.Domain.Models.Media;

namespace EcoHotels.Core.Infrastructure.Services
{
    public interface IAssetService
    {
        Asset FindById(int assetId);

        IEnumerable<Asset> FindByIds(int hotelId, IEnumerable<int> ids);

        IEnumerable<Asset> FindAllByHotelId(int hotel);

        IEnumerable<Asset> FindAll();

        void Save(Asset asset);

        void Delete(Asset asset);
    }
}
