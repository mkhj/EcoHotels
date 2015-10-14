using System;
using System.Collections.Generic;
using EcoHotels.Core.Domain.Models.Price;
using NHibernate.Criterion;

namespace EcoHotels.Core.Infrastructure.Repositories.NH.Rates
{
    internal class AddonRepo : Repository<Addon>
    {
        public IEnumerable<Addon> FindAllByHotelId(Guid hotelId)
        {
            var criteria = DetachedCriteria.For(typeof(Addon))
                .Add(Restrictions.Eq("HotelId", hotelId));

            return FindAll(criteria);
        }
    }
}
