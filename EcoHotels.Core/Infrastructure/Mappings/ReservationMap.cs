using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using EcoHotels.Core.Domain.Models.Commerce;

namespace EcoHotels.Core.Infrastructure.Mappings
{
    public class ReservationMap : ClassMap<Reservation>
    {
        public ReservationMap()
        {
            Table("Reservations");

            Id(x => x.Id).GeneratedBy.Identity();
            //Map(x => x.ReservationId).Generated.Insert();
            Map(x => x.IpAddress);
            
            Map(x => x.HotelId);
            Map(x => x.HotelName);
            Map(x => x.HotelAddress);
            Map(x => x.HotelPhoneNumber);
            Map(x => x.HotelEmail);
            
            Map(x => x.Arrival);
            Map(x => x.Departure);
            Map(x => x.CurrencySymbol);
            Map(x => x.Firstname);
            Map(x => x.Lastname);
            Map(x => x.PhoneNumber);
            Map(x => x.Country);
            Map(x => x.Email);

            Map(x => x.CreditCardHolder);
            Map(x => x.CreditCardNumber);
            Map(x => x.CreditCardMonth);
            Map(x => x.CreditCardYear);
            Map(x => x.CreditCardCvc);

            Map(x => x.Type).CustomType(typeof(ReservationType));
            Map(x => x.Created);
            Map(x => x.Modified).Nullable();
            Map(x => x.Cancelled).Nullable();

            #region - BelongsTo -

            References(x => x.Customer, "CustomerId")
                .Nullable()
                .Cascade.None();

            #endregion

            HasMany(x => x.Items)
                .Table("ReservationItems")
                .KeyColumn("ReservationId")
                .Inverse()
                .AsSet()
                .Cascade.All();
        }
    }
}
