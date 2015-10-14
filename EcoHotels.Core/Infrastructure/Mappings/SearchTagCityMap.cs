using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EcoHotels.Core.Domain.Models.Tags;
using FluentNHibernate.Mapping;

namespace EcoHotels.Core.Infrastructure.Mappings
{
    public class SearchTagCityMap : ClassMap<SearchTagCity>
    {
        public SearchTagCityMap()
        {
            Table("SearchTagCities");

            Id(x => x.Id).GeneratedBy.Identity();
            Map(x => x.Name);

            References(x => x.Country, "CountryId")
                .Cascade.None();

            HasMany(x => x.Hotels)
                .Table("Hotels")
                .KeyColumn("SearchTagCityId")
                .LazyLoad()
                .Inverse()
                .AsSet()
                .Cascade.AllDeleteOrphan();
        }
    }
}
