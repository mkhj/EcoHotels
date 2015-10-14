using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EcoHotels.Core.Common;
using EcoHotels.Core.Common.Enum;
using EcoHotels.Core.Domain.Models;
using EcoHotels.Core.Domain.Models.Commerce;
using EcoHotels.Core.Domain.Value_objects;

namespace EcoHotels.Web.UI.Models
{
    public class CheckoutBillingInformationModel
    {
        public CheckoutBillingInformationModel() { }

        //public CheckoutBillingInformationModel(Reservation reservation)
        //{
        //    Hotel = reservation.HotelName;

        //    Arrival = reservation.Arrival;
        //    Departure = reservation.Departure;

        //    Firstname = reservation.Firstname;
        //    Lastname = reservation.Lastname;
        //    PhoneNumber = reservation.PhoneNumber;
        //    SelectedCountryName = reservation.Country ?? string.Empty;
        //    Email = reservation.Email;

        //    TotalPrice = Money.Create(reservation.TotalPrice, Currency.Create("DKK", 10, "DKK", 0.0m));

        //    Rooms = reservation.Items.Select(x => new CheckoutRoomInformationModel(x)).ToList();
        //}

        public CheckoutBillingInformationModel(Reservation reservation, SearchResultHotel searchResultHotel, IEnumerable<Addon> addons, IEnumerable<Country> countries)
        {
            Hotel = reservation.HotelName;

            Arrival = reservation.Arrival;
            Departure = reservation.Departure;

            Firstname = reservation.Firstname;
            Lastname = reservation.Lastname;
            //PhoneNumber = reservation.Phonenumber;
            SelectedCountryName = reservation.Country ?? string.Empty;
            Email = reservation.Email;

            TotalPrice = Money.Create(reservation.TotalPrice, Currency.Create("DKK", 10, "DKK", 0.0m));


            Rooms = new List<CheckoutRoomInformationModel>();
            foreach (var reservationItem in reservation.Items)
            {
                var roomtype = searchResultHotel.Rooms.FirstOrDefault(x => x.RoomTypeId == reservationItem.RoomTypeId);
                var roomInformationModel = new CheckoutRoomInformationModel(reservationItem, roomtype, addons);
                Rooms.Add(roomInformationModel);
            }

            if(Rooms.Count == 1)
            {
                Rooms[0].NameOfPrimaryGuest = reservation.Fullname;
            }

            AvailableCountries = countries.Select(x => new SelectListItem { Selected = x.Name.Trim() == SelectedCountryName.Trim(), Text = x.Name.Trim(), Value = x.Name.Trim() }).ToList();
            AvailableCountries.Insert(0, new SelectListItem { Selected = false, Text = "Choose one..", Value = string.Empty });
        }

        public string Hotel { get; set; }

        [ReadOnly(true)]
        public DateTime Arrival { get; set; }

        [ReadOnly(true)]
        public DateTime Departure { get; set; }

        [ReadOnly(true)]
        public int Duration { get { return (Departure - Arrival).Days; } }

        [ReadOnly(true)]
        public Money TotalPrice { get; set; }

        #region - Contact Information -

        [Required]
        public string Firstname { get; set; }

        [Required]
        public string Lastname { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        public string SelectedCountryName { get; set; }

        [ReadOnly(true)]
        public List<SelectListItem> AvailableCountries { get; set; }

        #endregion

        public List<CheckoutRoomInformationModel> Rooms { get; set; }
        
        #region - Credit Card -

        //[Required]
        //public int CreditcardType { get; set; }

        //[Required]
        //public string CreditcardNumber { get; set; }

        //[Required]
        //public string CreditcardHolder { get; set; }

        //[Required, Range(1, 12, ErrorMessage = "ExpireMonth must be a number")]
        //public int ExpireMonth { get; set; }

        //[Required, Range(1, 99, ErrorMessage = "ExpireYear must be a number")]
        //public int ExpireYear { get; set; }

        //[Required]
        //public string CVC { get; set; }

        #endregion

    }

    public class CheckoutRoomInformationModel
    {
        public CheckoutRoomInformationModel(){}

        //public CheckoutRoomInformationModel(ReservationItem reservationItem)
        //{
        //    //Id = reservationItem.Id;
        //    NameOfPrimaryGuest = reservationItem.NameOfPrimaryGuest;
        //    ItemType = reservationItem.Description;
        //    Adults = reservationItem.TotalNumberOfGuests;
        //}

        public CheckoutRoomInformationModel(ReservationItem reservationItem, SearchResultRoom roomtype, IEnumerable<Addon> addons)
        {
            Id = Guid.NewGuid();

            Addons = addons.Select(x => 
                new CheckoutAddonItemModel(
                    x.Id, 
                    x.Name.GetText(LanguageTypeEnum.English), 
                    x.Description.GetText(LanguageTypeEnum.English), 
                    x.Price, 
                    x.PostingRhythm, 
                    x.CalculationRule)
            ).ToList();

            NameOfPrimaryGuest = reservationItem.NameOfPrimaryGuest;
            ItemType = reservationItem.Description;
            Adults = reservationItem.TotalNumberOfGuests;

            Adults = (roomtype.MaxCapacity == 1) ? 1 : 0;

            TotalPrice = roomtype.TotalPrice;

            Guests = Enumerable.Range(1, roomtype.MaxCapacity).Select(x => new SelectListItem { Selected = (x == roomtype.MaxCapacity && x == 1), Text = x.ToString(), Value = x.ToString() }).ToList();
            if (roomtype.MaxCapacity > 1)
            {
                Guests.Insert(0, new SelectListItem { Selected = false, Text = "0", Value = string.Empty });
            }
        }

        /// <summary>
        /// Cart Item Id
        /// </summary>
        [Required]
        public Guid Id { get; set; }

        [ReadOnly(true)]
        public string ItemType { get; set; }

        [Required]
        public string NameOfPrimaryGuest { get; set; }

        [Required, Range(1, int.MaxValue, ErrorMessage = "Number of guests in room must be large then one.")]
        public int Adults { get; set; }

        public List<CheckoutAddonItemModel> Addons { get; set; }

        [ReadOnly(true)]
        public List<SelectListItem> Guests { get; set; }

        [ReadOnly(true)]
        public decimal TotalPrice { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class CheckoutAddonItemModel
    {
        public CheckoutAddonItemModel(){}

        public CheckoutAddonItemModel(int id, string name, string description, decimal price, PostingRhythm postingRhythm, CalculationRule calculationRule)
        {
            Id = id;
            Name = name;
            Description = description;
            Price = price;
            PostingRhythm = (int) postingRhythm;
            CalculationRule = (int) calculationRule;
            IsSelected = false;

        }

        public int Id { get; set; }

        [ReadOnly(true)]
        public string Name { get; set; }

        [ReadOnly(true)]
        public string Description { get; set; }

        [ReadOnly(true)]
        public decimal Price { get; set; }

        [ReadOnly(true)]
        public int PostingRhythm { get; set; }

        [ReadOnly(true)]
        public int CalculationRule { get; set; }

        public bool IsSelected { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class CheckoutCompleteModel
    {
        public CheckoutCompleteModel(Reservation reservation)
        {
            ReservationId = reservation.ReservationId.ToString();
            Arrival = reservation.Arrival;
            Depature = reservation.Departure;

            HotelName = reservation.HotelName;
            HotelAddress = reservation.HotelAddress;
            HotelPhoneNumber = reservation.HotelPhoneNumber;
            HotelEmail = reservation.HotelEmail;

            Firstname = reservation.Firstname;
            Lastname = reservation.Lastname;
            PhoneNumber = reservation.PhoneNumber;
            CountryName = reservation.Country;
            Email = reservation.Email;

            Rooms = reservation.Items.Select(x => new CheckoutCompleteRoomInformationModel(x)).ToList();
        }

        [ReadOnly(true)]
        public string ReservationId { get; set; }

        [ReadOnly(true)]
        public DateTime Arrival { get; set; }

        [ReadOnly(true)]
        public DateTime Depature { get; set; }

        public int Duration
        {
            get { return (Depature - Arrival).Days; }
        }

        public decimal TotalPrice
        {
            get { return Rooms.Sum(x => x.TotalPrice); }
        }

        #region - Hotel Information -

        [ReadOnly(true)]
        public string HotelName { get; set; }

        [ReadOnly(true)]
        public string HotelAddress { get; set; }

        [ReadOnly(true)]
        public string HotelPhoneNumber { get; set; }

        [ReadOnly(true)]
        public string HotelEmail { get; set; }

        #endregion
        
        #region - Contact Information -

        [ReadOnly(true)]
        public string Firstname { get; set; }

        [ReadOnly(true)]
        public string Lastname { get; set; }

        [ReadOnly(true)]
        public string PhoneNumber { get; set; }

        [ReadOnly(true)]
        public string Email { get; set; }

        [ReadOnly(true)]
        public string CountryName { get; set; }

        #endregion

        public List<CheckoutCompleteRoomInformationModel> Rooms { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class CheckoutCompleteRoomInformationModel
    {
        public CheckoutCompleteRoomInformationModel() { }

        public CheckoutCompleteRoomInformationModel(ReservationItem reservationItem)
        {
            NameOfPrimaryGuest = reservationItem.NameOfPrimaryGuest;
            Description = reservationItem.Description;
            Adults = reservationItem.TotalNumberOfGuests;
            TotalPrice = reservationItem.TotalPriceWithoutDiscount;
            AddonsSummary = reservationItem.AddonsSummary;
            RatesPerDay = reservationItem.PricePerDate.Select(x => new CheckoutCompleteRoomRatesPerDay(x));
        }

        [ReadOnly(true)]
        public string Description { get; set; }

        [ReadOnly(true)]
        public string NameOfPrimaryGuest { get; set; }

        [ReadOnly(true)]
        public int Adults { get; set; }

        [ReadOnly(true)]
        public decimal TotalPrice { get; set; }

        [ReadOnly(true)]
        public string AddonsSummary { get; set; }

        public IEnumerable<CheckoutCompleteRoomRatesPerDay> RatesPerDay { get; set; }
    }

    public class CheckoutCompleteRoomRatesPerDay
    {
        public CheckoutCompleteRoomRatesPerDay(ReservationPrice item)
        {
            Date = item.Date;
            Price = item.Price;
        }

        [ReadOnly(true)]
        public DateTime Date { get; set; }

        [ReadOnly(true)]
        public decimal Price { get; set; }
    }
}