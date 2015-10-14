using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EcoHotels.Core.Domain.Models.Media;
using NHibernate.Criterion;

namespace EcoHotels.Core.Infrastructure.Repositories.NH.Media
{
    internal class AssetCategoryRepo : Repository<AssetCategory>
    {
        public AssetCategory FindById(Guid id, Guid organizationId)
        {
            var criteria = DetachedCriteria.For(typeof(AssetCategory))
                .Add(Restrictions.Eq("Id", id))
                .Add(Restrictions.Eq("OrganizationId", organizationId));

            return FindOne(criteria);
        }

        public IEnumerable<AssetCategory> FindByOrganizationId(Guid organizationId)
        {
            var criteria = DetachedCriteria.For(typeof(AssetCategory))
                .Add(Restrictions.Eq("OrganizationId", organizationId));

            return FindAll(criteria);
        }
    }
}
