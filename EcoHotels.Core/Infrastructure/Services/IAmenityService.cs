using System.Collections.Generic;
using EcoHotels.Core.Domain.Models.Property;

namespace EcoHotels.Core.Infrastructure.Services
{
    public interface IAmenityService
    {
        IEnumerable<Amenity> FindAll();

        void Save(Amenity amenity);

        void Delete(Amenity amenity);

    }
}
