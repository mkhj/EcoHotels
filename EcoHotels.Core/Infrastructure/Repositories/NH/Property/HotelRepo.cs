using System;
using System.Collections.Generic;
using EcoHotels.Core.Domain.Models.Property;
using NHibernate.Criterion;

namespace EcoHotels.Core.Infrastructure.Repositories.NH.Property
{
    public class HotelRepo : Repository<Hotel>
    {

        public Hotel FindByRoom(Guid roomId)
        {
            var criteria = DetachedCriteria.For(typeof(Hotel))
                .CreateAlias("Rooms", "r")
                    .Add(Restrictions.Eq("r.Id", roomId));

            return FindOne(criteria);
        }

        public Hotel FindById(Guid organizationId, Guid id)
        {
            var criteria = DetachedCriteria.For(typeof(Hotel))
                .Add(Restrictions.Eq("Id", id))
                .Add(Restrictions.Eq("Organization.Id", organizationId));

            return FindOne(criteria);
        }

        public IEnumerable<Hotel> FindAllInActive()
        {
            var criteria = DetachedCriteria.For(typeof (Hotel))
                .Add(Restrictions.Eq("IsActive", false));

            return FindAll(criteria);
        }

        //public IEnumerable<Hotel> FindAllNonVerifiedHotels()
        //{
        //    var criteria = DetachedCriteria.For(typeof(Hotel))
        //        .Add(Restrictions.IsNull("Verified"));

        //    return FindAll(criteria);
        //}

        public IEnumerable<Hotel> FindAllHotelsToVerify()
        {
            var criteria = DetachedCriteria.For(typeof(Hotel))
                .Add(Restrictions.Or(Restrictions.IsNull("Verified"), Restrictions.LtProperty("Verified", "Modified")));

            return FindAll(criteria);
        }

        public long CountHotelsToVerify()
        {
            var criteria = DetachedCriteria.For(typeof(Hotel))
                .Add(Restrictions.Or(Restrictions.IsNull("Verified"), Restrictions.LtProperty("Verified", "Modified")));

            return Count(criteria);
        }

        public long CountInActiveHotels()
        {
            var criteria = DetachedCriteria.For(typeof(Hotel))
                .Add(Restrictions.Eq("IsActive", false));

            return Count(criteria);
        }
    }
}
