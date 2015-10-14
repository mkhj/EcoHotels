using System;
using System.Collections.Generic;
using EcoHotels.Core.Domain.Models.Property;
using EcoHotels.Core.Domain.Models.Security;
using NHibernate.Criterion;
using NHibernate.Transform;

namespace EcoHotels.Core.Infrastructure.Repositories.NH
{
    internal class UserRepo : Repository<User>
    {
        public User FindById(Guid organizationId, Guid id)
        {
            var criteria = DetachedCriteria.For(typeof (User))
                .Add(Restrictions.Eq("Id", id))
                .Add(Restrictions.Eq("Organization.Id", organizationId));

            return FindOne(criteria);
        }

        public User FindByEmail(string email)
        {
            var criteria = DetachedCriteria.For(typeof(User))
                .Add(Restrictions.Eq("Email", email.Trim()));

            return FindOne(criteria);
        }

        public bool IsEmailUnique(string email)
        {
            var criteria = DetachedCriteria.For(typeof(User))
                .Add(Restrictions.Eq("Email", email.Trim()));

            return !Exists(criteria);
        }
    }
}
