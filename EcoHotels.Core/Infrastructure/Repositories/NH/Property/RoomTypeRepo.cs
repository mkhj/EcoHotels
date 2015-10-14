using System;
using System.Collections.Generic;
using EcoHotels.Core.Domain.Models.Property;

namespace EcoHotels.Core.Infrastructure.Repositories.NH.Property
{
    public class RoomTypeRepo : Repository<RoomType>
    {
        public RoomType FindByIdAndOrganizationId(Guid id, Guid organizationId)
        {
            var sql = @"SELECT RoomTypes.*
                        FROM Organizations INNER JOIN
                            Hotels ON Organizations.Id = Hotels.OrganizationId INNER JOIN
                                RoomTypes ON Hotels.Id = RoomTypes.HotelId
                        WHERE (RoomTypes.Id = :Id) AND (Organizations.Id = :OrganizationId)";

            return Session.CreateSQLQuery(sql)
                .SetGuid("Id", id)
                .SetGuid("OrganizationId", organizationId)
                .UniqueResult<RoomType>();
        }

        public IEnumerable<RoomType> FindAllByHotelId(Guid hotelId)
        {
            return Session.CreateSQLQuery("FROM RoomType r WHERE r.HotelId = :HotelId")
                .SetGuid("HotelId", hotelId)
                .List<RoomType>();
        }


    }
}
