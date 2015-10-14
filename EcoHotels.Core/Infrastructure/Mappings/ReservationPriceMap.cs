using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using EcoHotels.Core.Domain.Models.Commerce;

namespace EcoHotels.Core.Infrastructure.Mappings
{
    public class ReservationPriceMap : ClassMap<ReservationPrice>
    {
        public ReservationPriceMap()
        {
            Table("ReservationPrices");

            Id(x => x.Id).GeneratedBy.Identity();
            Map(x => x.Date);
            Map(x => x.Price);

            References(x => x.ReservationItem, "ReservationItemId")
                .Cascade.None();
        }
    }
}
