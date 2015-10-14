using System.Collections.Generic;
using System.Linq;
using EcoHotels.Core.Domain.Models.Property;
using EcoHotels.Core.Infrastructure.NH;
using NHibernate.Criterion;

namespace EcoHotels.Core.Infrastructure.Services.Impl.Property
{
    public class RoomTypeService : IRoomTypeService
    {
        private IRateService RateService;

        private Repository<RoomType> RoomTypeRepo;

        public RoomTypeService(IRateService rateService)
        {
            RateService = rateService;

            RoomTypeRepo = new Repository<RoomType>();            
        }

        public RoomType FindById(int id)
        {
            return RoomTypeRepo.FindBy(id);
        }

        public RoomType FindById(int hotelId, int id)
        {
            var roomTypes = FindAllByHotelId(hotelId);
            return roomTypes.First(x => x.Id == id);
        }

        public IEnumerable<RoomType> FindAllById(IEnumerable<int> ids)
        {
        var criteria = DetachedCriteria.For(typeof(RoomType))
            .Add(Restrictions.In("Id", ids.ToArray()));

            return RoomTypeRepo.FindAll(criteria);
        }



        public RoomType FindByIdAndOrganizationId(int id, int organizationId)
        {
            var sql = @"SELECT RoomTypes.*
                        FROM Organizations INNER JOIN
                            Hotels ON Organizations.Id = Hotels.OrganizationId INNER JOIN
                                RoomTypes ON Hotels.Id = RoomTypes.HotelId
                        WHERE (RoomTypes.Id = :Id) AND (Organizations.Id = :OrganizationId)";

            return RoomTypeRepo.Session.CreateSQLQuery(sql)
                .SetInt32("Id", id)
                .SetInt32("OrganizationId", organizationId)
                .UniqueResult<RoomType>();
        }

        public IEnumerable<RoomType> FindAllByHotelId(int hotelId)
        {
            var criteria = DetachedCriteria.For(typeof (RoomType))
                .Add(Restrictions.Eq("Hotel.Id", hotelId));

            return RoomTypeRepo.FindAll(criteria);
        }

        public void Delete(RoomType roomType)
        {
            RoomTypeRepo.Remove(roomType);
        }

        public void Save(RoomType roomType)
        {
            if (roomType.IsValid())
            {
                RoomTypeRepo.Save(roomType);
            }
        }

        public void Save(List<RoomType> roomTypes)
        {
            roomTypes.ForEach(Save);
        }
    }
}
