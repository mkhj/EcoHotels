using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EcoHotels.Core.Domain.Models.Media;
using FluentNHibernate.Mapping;

namespace EcoHotels.Core.Infrastructure.Mappings
{
    public class AssetCategoryMap : ClassMap<AssetCategory>
    {
        public AssetCategoryMap()
        {
            Table("AssetCategories");

            Id(x => x.Id).GeneratedBy.Identity();
            Map(x => x.HotelId);
            Map(x => x.Name);

            #region - HasMany -

            HasMany(x => x.Items)
                .Table("Assets")
                .KeyColumn("AssetCategoryId")
                .LazyLoad()
                .Inverse()
                .AsSet()
                .Cascade.AllDeleteOrphan();

            #endregion

        }
    }
}
