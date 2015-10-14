using System;
using EcoHotels.Core.Domain.Models.Property;
using NHibernate.Criterion;

namespace EcoHotels.Core.Infrastructure.Repositories.NH.Property
{
    public class OrganizationRepo : Repository<Organization>
    {
        public Organization FindByHotelId(Guid id)
        {
            var criteria = DetachedCriteria.For(typeof(Organization))
                .CreateAlias("Hotels", "h")
                    .Add(Restrictions.Eq("h.Id", id));

            return FindOne(criteria);
        }
    }
}
