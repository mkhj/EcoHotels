using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using EcoHotels.Core.Domain.Models.Commerce;

namespace EcoHotels.Core.Infrastructure.Mappings
{
    public class ReservationItemMap : ClassMap<ReservationItem>
    {
        public ReservationItemMap()
        {
            Table("ReservationItems");

            Id(x => x.Id).GeneratedBy.Identity();
            Map(x => x.RoomTypeId);
            Map(x => x.Description);
            Map(x => x.NameOfPrimaryGuest, "GuestName");
            Map(x => x.Adults);
            Map(x => x.Children);
            Map(x => x.Babies);
            Map(x => x.Type).CustomType(typeof(ReservationItemType));

            References(x => x.Reservation, "ReservationId")
                .Cascade.None();

            HasMany(x => x.PricePerDate)
                .Table("ReservationPrices")
                .KeyColumn("ReservationItemId")
                .Inverse()
                .AsSet()
                .Cascade.All();

            HasMany(x => x.Addons)
                .Table("ReservationAddons")
                .KeyColumn("ReservationItemId")
                .Inverse()
                .AsSet()
                .Cascade.All();
        }
    }
}
