using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EcoHotels.Core.Domain.Models.Commerce;
using FluentNHibernate.Mapping;

namespace EcoHotels.Core.Infrastructure.Mappings
{
    public class RatePriceMap : ClassMap<RatePrice>
    {
        public RatePriceMap()
        {
            Table("RatePrices");

            Id(x => x.Id).GeneratedBy.Identity();
            Map(x => x.RoomTypeId);
            //Map(x => x.RateCategoryId);
            Map(x => x.Value);
            Map(x => x.IsActive);

            References(x => x.Category, "RateCategoryId")
                .Cascade.None();
        }
    }
}
