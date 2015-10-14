using System;
using System.Collections.Generic;
using System.Linq;
using EcoHotels.Core.Domain.Models.Media;
using NHibernate.Criterion;

namespace EcoHotels.Core.Infrastructure.Repositories.NH.Media
{
    internal class AssetRepo : Repository<Asset>
    {
        public IEnumerable<Asset> FindByOrganizationId(Guid organizationId)
        {
            var criteria = DetachedCriteria.For(typeof(Asset))
                .Add(Restrictions.Eq("OrganizationId", organizationId));

            return FindAll(criteria);
        }

        public Asset FindById(Guid hotelId, Guid id)
        {
            var restriction1 = new Conjunction()
                .Add(Restrictions.Eq("HotelId", hotelId))
                .Add(Restrictions.Eq("Id", id));

            var criteria = DetachedCriteria.For(typeof(Asset))
                .Add(restriction1);

            return FindOne(criteria);
        }

        public IEnumerable<Asset> FindByIds(Guid organizationId, IEnumerable<Guid> ids)
        {
            var criteria = DetachedCriteria.For(typeof (Asset))
                .Add(Restrictions.In("Id", ids.ToArray()))
                .CreateAlias("Category", "c")
                    .Add(Restrictions.Eq("c.OrganizationId", organizationId));

            return FindAll(criteria);
        }
    }
}
