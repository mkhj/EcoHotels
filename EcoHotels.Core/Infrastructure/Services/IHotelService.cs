using System.Collections.Generic;
using EcoHotels.Core.Domain.Models.Property;

namespace EcoHotels.Core.Infrastructure.Services
{
    public interface IHotelService
    {
        Hotel FindById(int id);

        Hotel FindById(int organizationId, int id);

        IEnumerable<Hotel> FindAll();

        IEnumerable<Hotel> FindAllInActive();

        IEnumerable<Hotel> FindAllHotelsToVerify();

        long CountHotelsToVerify();

        long CountInActiveHotels();

        void Save(Hotel hotel);

        void Delete(Hotel hotel);

    }
}
