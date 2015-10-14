using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EcoHotels.Core.Common.Enum;
using EcoHotels.Core.Domain.Models.Commerce;
using NHibernate.Criterion;

namespace EcoHotels.Core.Infrastructure.Repositories.NH
{
    internal class CustomerRepo : Repository<Customer>
    {
        public Customer FindByEmail(string email)
        {
            var criteria = DetachedCriteria.For(typeof(Customer))
                .Add(Restrictions.Eq("Email", email.Trim()));

            return FindOne(criteria);
        }

        public Customer FindByExternalId(string externalId, AccountTypeEnum accountTypeEnum)
        {
            var criteria = DetachedCriteria.For(typeof(Customer))
                .Add(Restrictions.Eq("ExternalId", externalId.Trim()))
                .Add(Restrictions.Eq("AccountType", accountTypeEnum));

            return FindOne(criteria);
        }

        public bool IsEmailUnique(string email)
        {
            var criteria = DetachedCriteria.For(typeof(Customer))
                .Add(Restrictions.Eq("Email", email.Trim()));

            return !Exists(criteria);
        }
    }
}
