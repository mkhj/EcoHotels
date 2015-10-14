using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EcoHotels.Core.Domain.Models.Commerce;
using FluentNHibernate.Mapping;

namespace EcoHotels.Core.Infrastructure.Mappings
{
    public class RateCategoryMap : ClassMap<RateCategory>
    {
        public RateCategoryMap()
        {
            Table("RateCategories");

            Id(x => x.Id).GeneratedBy.Identity();
            Map(x => x.Name);
            Map(x => x.Description);
            Map(x => x.Color);

            References(x => x.Hotel, "HotelId")
                .Cascade.None();

            HasMany(x => x.Items)
                .Table("Rates")
                .KeyColumn("RateCategoryId")
                .Inverse()
                .AsSet()
                .Cascade.AllDeleteOrphan();
        }
    }
}
