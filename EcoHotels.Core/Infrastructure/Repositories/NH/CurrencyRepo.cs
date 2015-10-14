using System;
using System.Collections.Generic;
using NHibernate.Criterion;

namespace EcoHotels.Core.Infrastructure.Repositories.NH
{
    internal class CurrencyRepo : Repository<Domain.Value_objects.Currency>
    {
        public IEnumerable<Domain.Value_objects.Currency> FindAllByOrganizationId(Guid organizationId)
        {
            var criteria = DetachedCriteria.For(typeof(Domain.Value_objects.Currency))
                .Add(Restrictions.Eq("OrganizationId", organizationId));

            return FindAll(criteria);
        }

        public Domain.Value_objects.Currency FindByISOSymbol(string symbol)
        {
            var criteria = DetachedCriteria.For(typeof(Domain.Value_objects.Currency))
                .Add(Restrictions.Eq("ISOCurrencySymbol", symbol));

            return FindOne(criteria);
        }
    }
}
