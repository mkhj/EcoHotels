using System.Collections.Generic;
using EcoHotels.Core.Domain.Models.Commerce;
using EcoHotels.Core.Domain.Models.Property;

namespace EcoHotels.Core.Infrastructure.Services
{
    public interface IRateService
    {
        void Save(RateCategory category);

        void Delete(RateCategory category);

        RateCategory FindBy(int id);

        IEnumerable<RateCategory> FindByHotel(int hotelId);

        RateCategory FindBy(int id, int hotelId);
        void Save(IEnumerable<RateCategory> categories);
        void EnsureRatesForNewRoomType(RoomType roomType);
        void RemoveRatesForRoomType(RoomType roomType);
    }
}
