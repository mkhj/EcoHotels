using System;
using System.Collections.Generic;
using EcoHotels.Core.Domain.Models.Localization;
using EcoHotels.Core.Domain.Models.Property;
using NHibernate.Criterion;
using NHibernate.Transform;

namespace EcoHotels.Core.Infrastructure.Repositories.NH
{
    public class LanguageRepo : Repository<Language>
    {
        public IEnumerable<Language> FindByOrganizationId(Guid id)
        {
            var criteria = DetachedCriteria.For(typeof(Organization))
                .Add(Restrictions.Eq("Id", id))
                .CreateAlias("AvailableLanguages", "a")
                .SetProjection(Projections.ProjectionList()
                    .Add(Projections.Property("a.Id"), "Id")
                    .Add(Projections.Property("a.Name"), "Name")
                )
                .SetResultTransformer(Transformers.AliasToBean(typeof(Language)));

            return FindAll(criteria);
        }

        public IEnumerable<Language> FindAllActive()
        {
            var criteria = DetachedCriteria.For(typeof(Language))
                .Add(Restrictions.Eq("IsActive", true));

            return FindAll(criteria);
        }

        public Language FindByShortName(string shortname)
        {
            var criteria = DetachedCriteria.For(typeof(Language))
                .Add(Restrictions.Eq("Shortname", shortname));

            return FindOne(criteria);
        }
    }
}
