using System;
using EcoHotels.Core.Infrastructure;

namespace EcoHotels.Core.Domain.Models.Commerce
{
    [Serializable]
    public class ReservationPrice : EntityBase<ReservationPrice>
    {
        protected internal ReservationPrice(){}

        protected internal ReservationPrice(ReservationItem reservationItem, DateTime date, decimal price)
        {
            ReservationItem = reservationItem;
            Date = date.Date;
            Price = price;
        }

        public static ReservationPrice Create(ReservationItem reservationItem, DateTime date, decimal price)
        {
            return new ReservationPrice(reservationItem, date, price);
        }

        public virtual ReservationItem ReservationItem { get; protected internal set; }

        public virtual DateTime Date { get; protected internal set; }

        public virtual decimal Price { get; protected internal set; }

        protected override void Validate()
        {
            
        }
    }
}
