using System.Collections.Generic;
using EcoHotels.Core.Domain.Models.Property;
using EcoHotels.Core.Infrastructure.Cache;
using EcoHotels.Core.Infrastructure.NH;

namespace EcoHotels.Core.Infrastructure.Services.Impl.Property
{
    public class AmenityService : IAmenityService
    {
        private ICacheStorage CacheStorage;

        private Repository<Amenity> AmenityRepo { get; set; }

        public AmenityService(ICacheStorage cacheStorage)
        {            
            CacheStorage = cacheStorage;

            AmenityRepo = new Repository<Amenity>();
        }

        public IEnumerable<Amenity> FindAll()
        {
            return AmenityRepo.FindAll();
        }

        public void Save(Amenity amenity)
        {
            if (amenity.IsValid())
            {
                AmenityRepo.Save(amenity);
            }
        }

        public void Delete(Amenity amenity)
        {
            AmenityRepo.Remove(amenity);
        }
    }
}
