using System.Collections.Generic;
using EcoHotels.Core.Domain.Models.Property;

namespace EcoHotels.Core.Infrastructure.Services
{
    public interface IRoomTypeService
    {
        RoomType FindById(int hotelId, int id);

        RoomType FindByIdAndOrganizationId(int id, int organizationId);

        IEnumerable<RoomType> FindAllByHotelId(int id);

        void Delete(RoomType roomType);

        void Save(RoomType roomType);

        void Save(List<RoomType> roomType);
    }
}
