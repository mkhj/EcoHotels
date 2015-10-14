using System;
using System.Collections.Generic;
using EcoHotels.Core.Domain.Models.Property;
using NHibernate.Criterion;

namespace EcoHotels.Core.Infrastructure.Repositories.NH.Property
{
    internal class AmenityRepo : Repository<Amenity>
    {
        public IEnumerable<Amenity> FindAllByHotelId(Guid hotelId)
        {
            var criteria = DetachedCriteria.For(typeof(Amenity))
                .Add(Restrictions.Eq("HotelId", hotelId));

            return FindAll(criteria);
        }

        public IEnumerable<Amenity> FindAllByRoomTypeId(Guid roomTypeId)
        {
            var criteria = DetachedCriteria.For(typeof(Amenity))
                .Add(Restrictions.Eq("RoomTypeId", roomTypeId));

            return FindAll(criteria);
        }
    }
}
