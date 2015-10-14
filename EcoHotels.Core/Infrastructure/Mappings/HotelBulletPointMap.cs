using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EcoHotels.Core.Common.Enum;
using EcoHotels.Core.Domain.Models.Property;
using FluentNHibernate.Mapping;

namespace EcoHotels.Core.Infrastructure.Mappings
{
    public class HotelBulletPointMap : ClassMap<HotelBulletPoint>
    {
        public HotelBulletPointMap()
        {
            Table("HotelBulletPoints");

            Id(x => x.Id).GeneratedBy.Identity();

            Map(x => x.Type).CustomType(typeof(HotelBulletPointEnum));
            Map(x => x.OrderId);

            References(x => x.Hotel, "HotelId")
                .Cascade.None();

            References(x => x.Text, "TextMultiLanguageId")
                .Cascade.All();
        }
    }
}
