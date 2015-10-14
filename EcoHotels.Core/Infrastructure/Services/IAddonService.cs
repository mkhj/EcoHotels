using System.Collections.Generic;
using EcoHotels.Core.Domain.Models.Commerce;

namespace EcoHotels.Core.Infrastructure.Services
{
    public interface IAddonService
    {
        Addon FindById(int hotelId, int id);

        IEnumerable<Addon> FindAllByHotelId(int hotelId);

        IEnumerable<Addon> FindAll();

        void Save(Addon addon);

        void Delete(Addon addon);
    }
}
