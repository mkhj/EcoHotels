using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EcoHotels.Core.Domain.Models.Commerce;

namespace EcoHotels.Web.UI.Models
{
    public class ReservationModel
    {
        public ReservationModel(Reservation reservation)
        {
            Id = reservation.Id;

            HotelName = reservation.HotelName;
            HotelAddress = reservation.HotelAddress;
            HotelPhoneNumber = reservation.HotelPhoneNumber;
            HotelEmail = reservation.HotelEmail;

            Arrival = reservation.Arrival;
            Departure = reservation.Departure;

            Fullname = reservation.Firstname + " " + reservation.Lastname;
            Email = reservation.Email;

            CreditcardHolder = reservation.CreditCardHolder;
            CreditcardNumber = reservation.CreditcardNumberObfuscated;
            ExpireMonth = reservation.CreditCardMonth;
            ExpireYear = reservation.CreditCardYear;
            CVC = reservation.CreditCardCvc;
        }

        public int Id { get; set; }

        public DateTime Arrival { get; set; }

        public DateTime Departure { get; set; }

        #region - Hotel Information -

        public string HotelName { get; set; }

        public string HotelAddress { get; set; }

        public string HotelPhoneNumber { get; set; }

        public string HotelEmail { get; set; }

        public string HotelCancellationPolicy { get; set; }

        #endregion

        public IEnumerable<object> Rooms { get; set; }

        #region - Billing information -

        public string Fullname { get; set; }

        public string Email { get; set; }
        
        #endregion


        #region - Credit card -

        public int CreditcardType { get; set; }

        public string CreditcardNumber { get; set; }

        public string CreditcardHolder { get; set; }

        public string ExpireMonth { get; set; }

        public string ExpireYear { get; set; }

        public string CVC { get; set; }

        #endregion
    }

    public class ReservationListModel
    {
        public ReservationListModel(){}

        public ReservationListModel(IEnumerable<Reservation> reservations)
        {
            Items = reservations.Select(x => new ReservationListItemModel(x));
        }

        public IEnumerable<ReservationListItemModel> Items { get; set; }
    }

    public class ReservationListItemModel
    {
        public ReservationListItemModel(Reservation reservation)
        {
            Id = reservation.Id;
            ReserverationId = reservation.ReservationId.ToString();
            HotelName = reservation.HotelName;
            Arrival = reservation.Arrival;
            Departure = reservation.Departure;
        }

        public int Id { get; set; }

        public string ReserverationId { get; set; }

        public string HotelName { get; set; }

        public DateTime Arrival { get; set; }

        public DateTime Departure { get; set; }
    }
}