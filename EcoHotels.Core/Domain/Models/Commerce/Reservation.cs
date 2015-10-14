using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EcoHotels.Core.Infrastructure;

namespace EcoHotels.Core.Domain.Models.Commerce
{
    public enum ReservationType
    {
        Person,

        Corporation,

        Bonus
    }

    [Serializable]
    public class Reservation : EntityBase<Reservation>
    {
        protected internal Reservation()
        {
            HotelName = string.Empty;
            HotelAddress = string.Empty;
            HotelPhoneNumber = string.Empty;
            HotelEmail = string.Empty;

            CreditCardHolder = string.Empty;
            CreditCardNumber = string.Empty;
            CreditCardMonth = string.Empty;
            CreditCardYear = string.Empty;
            CreditCardCvc = string.Empty;

            Created = DateTime.Now;
            Discounts = new List<ReservationDiscount>();
            Items = new List<ReservationItem>();
        }

        public static Reservation Create(DateTime arrival, DateTime departure, string currencySymbol, ReservationType type, string ipaddress)
        {
            return new Reservation
                       {
                           Arrival = arrival,
                           Departure = departure,
                           CurrencySymbol = currencySymbol,
                           IpAddress = ipaddress,
                           Type = type
                       };
        }

        /// <summary>
        /// Auto-generated Id
        /// </summary>
        public virtual int ReservationId { get; protected internal set; }

        public virtual ReservationType Type { get; set; }

        public virtual Customer Customer { get; set; }

        public virtual string IpAddress { get; protected internal set; }
        
        #region - Hotel Information -

        public virtual int HotelId { get; set; }

        public virtual string HotelName { get; set; }

        public virtual string HotelAddress { get; set; }

        public virtual string HotelPhoneNumber { get; set; }

        public virtual string HotelEmail { get; set; }

        #endregion
        
        public virtual DateTime Arrival { get; set; }

        public virtual DateTime Departure { get; set; }

        public virtual string CurrencySymbol { get; set; }

        #region - Personal Information -

        public virtual string Firstname { get; set; }

        public virtual string Lastname { get; set; }

        public virtual string Fullname { get { return Firstname + " " + Lastname; } }

        public virtual string PhoneNumber { get; set; }

        public virtual string Country { get; set; }

        public virtual string Email { get; set; }

        #endregion
        
        #region - Creditcard information -

        public virtual string CreditCardHolder { get; set; }

        public virtual string CreditCardNumber { get; set; }

        public virtual string CreditCardMonth { get; set; }

        public virtual string CreditCardYear { get; set; }

        public virtual string CreditCardCvc { get; set; }

        /// <summary>
        /// Replaces all but the 4 last digits with a star "*"
        /// </summary>
        public virtual string CreditcardNumberObfuscated
        {
            get
            {
                var lastFourDigtigs = CreditCardNumber.Substring(CreditCardNumber.Length - 4);

                var builder = new StringBuilder();
                for (int i = 0; i < CreditCardNumber.Length - 4; i++)
                {
                    builder.Append("*");
                }

                builder.Append(" ");
                builder.Append(lastFourDigtigs);

                return builder.ToString();
            }
        }

        #endregion

        public virtual ICollection<ReservationDiscount> Discounts { get; protected internal set; }

        public virtual ICollection<ReservationItem> Items { get; protected internal set; }
        
        
        public virtual DateTime Created { get; protected internal set; }

        public virtual DateTime? Modified { get; set; }

        public virtual DateTime? Cancelled { get; set; }


        public virtual bool HasBeenCancelled { get { return Cancelled != null; } }

        public virtual decimal TotalPrice
        {
            get { return Items.Sum(x => x.TotalPriceWithoutDiscount); }
        }

        public virtual int Duration
        {
            get { return (Departure - Arrival).Days; }
        }

        protected override void Validate()
        {
        }
    }
}
