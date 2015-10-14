using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EcoHotels.Core.Common.Enum;
using EcoHotels.Core.Domain.Models.Commerce;
using EcoHotels.Core.Domain.Models.Property;
using EcoHotels.Extensions;

namespace EcoHotels.Core.Domain.Value_objects
{
    public class SearchResultList
    {
        private SearchResultList(IEnumerable<SearchResultHotel> hotels, DateTime arrival, DateTime departure)
        {
            Hotels = hotels;
            Arrival = arrival;
            Departure = departure;
        }

        public static SearchResultList Create(IEnumerable<Hotel> hotels, IEnumerable<Reservation> reservations, IEnumerable<RoomTypeInventory> inventories, IEnumerable<RateCategory> rateCategories, DateTime arrival, DateTime departure)
        {
            var result = new List<SearchResultHotel>();
            foreach (var hotel in hotels)
            {
                var currentReservationForHotel = reservations.Where(x => x.HotelId == hotel.Id);

                var resultHotel = SearchResultHotel.Create(hotel, currentReservationForHotel, inventories, rateCategories, arrival, departure);
                result.Add(resultHotel);
            }

            return new SearchResultList(result, arrival, departure);
        }

        public DateTime Arrival { get; private set; }

        public DateTime Departure { get; private set; }

        public IEnumerable<SearchResultHotel> Hotels { get; private set; }

        public int NumberOfHotelsWithAvailableRooms
        {
            get { return Hotels.SelectMany(x => x.Rooms).Count(x => x.IsAvailable); }
        }
    }

    public class SearchResultHotel
    {
        private SearchResultHotel(Hotel hotel, IEnumerable<SearchResultRoom> searchResultRooms, DateTime arrival, DateTime departure)
        {
            HotelId = hotel.Id;

            Arrival = arrival;
            Departure = departure;

            Name = hotel.Name;
            PhoneNumber = hotel.Phone;
            Email = hotel.Email;
            Address = hotel.Address1 + " " + hotel.Address2;
            City = hotel.City;
            Zipcode = hotel.Zipcode;
            Country = hotel.Country;

            About = hotel.About.GetText(LanguageTypeEnum.English);

            CheckIn = hotel.CheckIn + " " + hotel.CheckInAMPM;
            CheckOut = hotel.CheckOut + " " + hotel.CheckOutAMPM;
            MinimumStay = hotel.MinimumStay;
            MinimumCheckInAge = hotel.MinimumCheckInAge;

            Amenities = hotel.Amenities.Select(x => x.Name);

            Rooms = searchResultRooms;

            Photos = hotel.Assets.Select(x => x.GenerateUrl(580, 358) + x.GenerateCroppingQuery());
            Thumbnails = hotel.Assets.Select(x => x.GenerateUrl(77, 44) + x.GenerateCroppingQuery());
        }

        public static SearchResultHotel Create(Hotel hotel)
        {
            return null;
        }

        internal static SearchResultHotel Create(Hotel hotel, IEnumerable<Reservation> reservations, IEnumerable<RoomTypeInventory> inventories, IEnumerable<RateCategory> rateCategories, DateTime arrival, DateTime departure)
        {
            var ratePrices = rateCategories.SelectMany(x => x.Items);
            var reservationItems = reservations.SelectMany(x => x.Items);

            var searchResultRooms = new List<SearchResultRoom>();
            
            foreach (var roomType in hotel.RoomTypes)
            {
                var currentInventoriesForRoomtype = inventories.Where(x => x.RoomTypeId == roomType.Id);
                var prices = ratePrices.Where(x => x.RoomTypeId == roomType.Id);
                var items = reservationItems.Where(x => x.RoomTypeId == roomType.Id);

                var resultRoom = SearchResultRoom.Create(roomType, items, currentInventoriesForRoomtype, prices, arrival, departure);
                searchResultRooms.Add(resultRoom);
                
            }

            return new SearchResultHotel(hotel, searchResultRooms, arrival, departure);
        }

        public int HotelId { get; private set; }

        public DateTime Arrival { get; private set; }

        public DateTime Departure { get; private set; }

        public string Name { get; private set; }

        public string Email { get; private set; }

        public string PhoneNumber { get; private set; }
        
        public string Address { get; private set; }

        public string City { get; private set; }

        public string Zipcode { get; private set; }
        
        public string Country { get; private set; }

        
        public string About { get; private set; }

        #region - Policies -

        public string CheckIn { get; private set; }

        public string CheckOut { get; private set; }

        public int MinimumStay { get; private set; }

        public int MinimumCheckInAge { get; private set; }

        #endregion

        public IEnumerable<string> Amenities { get; set; }

        public IEnumerable<SearchResultRoom> Rooms { get; set; }

        public IEnumerable<string> Photos { get; set; }

        public IEnumerable<string> Thumbnails { get; set; }

        public bool HasAvailableRooms
        {
            get { return Rooms.Any(x => x.IsAvailable); }
        }

        public decimal FindCheapestPrice()
        {
            return Rooms.Where(x => x.IsAvailable).Min(x => x.LowestPricePerDay);
        }
    }

    public class SearchResultRoom
    {
        internal static SearchResultRoom Create(RoomType roomType, IEnumerable<ReservationItem> reservationItems, IEnumerable<RoomTypeInventory> inventories, IEnumerable<RatePrice> prices, DateTime arrival, DateTime depature)
        {
            var minimumQuantity = int.MaxValue;
            var hasAvailableQuantity = false;
            var inventoryPricePerDays = new List<SearchResultRoomPrice>();

            var reservationPrices = reservationItems.SelectMany(x => x.PricePerDate);
            var days = (depature - arrival).Days;

            for (var i = 0; i < days; i++)
            {
                var date = arrival.AddDays(i);

                var availableQuantity = FindAvailableQuantity(roomType.Quantity, date, inventories, reservationPrices);
                var price = FindPrice(roomType.RackRate, date, inventories, prices);

                var inventoryPricePerDay = SearchResultRoomPrice.Create(date, availableQuantity, price);
                inventoryPricePerDays.Add(inventoryPricePerDay);

                minimumQuantity = Math.Min(availableQuantity, minimumQuantity);
                hasAvailableQuantity = availableQuantity > 0;

                if(hasAvailableQuantity == false)
                {
                    // The room is not available for one of the selected days
                    break;
                }
            }

            var resultRoom = new SearchResultRoom
                                 {
                                     RoomTypeId = roomType.Id,
                                     Name = roomType.Name.GetText(LanguageTypeEnum.English),
                                     Description = roomType.Description.GetText(LanguageTypeEnum.English),
                                     IsAvailable = hasAvailableQuantity,
                                     LowestPricePerDay = inventoryPricePerDays.Min(x => x.Price),
                                     TotalPrice = inventoryPricePerDays.Sum(x => x.Price),
                                     AvailableQuantity = minimumQuantity,
                                     MaxCapacity = roomType.Capacity,
                                     Prices = inventoryPricePerDays,
                                     Thumbnails = roomType.Assets.Select(x => x.GenerateUrl(250, 155) + x.GenerateCroppingQuery())
                                 };

            return resultRoom;
        }

        private static int FindAvailableQuantity(int defaultQuantity, DateTime date, IEnumerable<RoomTypeInventory> inventories, IEnumerable<ReservationPrice> reservationPrices)
        {
            // if no days a found qty is zero else qty = number of days
            var quantityUsedForGivenDate = reservationPrices.Where(x => x.Date.Date == date.Date).Count();

            var availableQuantity = 0;
            var currentInventory = inventories.FirstOrDefault(x => x.Date.Value.Date == date.Date);
            if (currentInventory.IsNotNull())
            {
                availableQuantity = (currentInventory.Quantity - quantityUsedForGivenDate);
            }
            else
            {
                availableQuantity = (defaultQuantity - quantityUsedForGivenDate);
            }

            return availableQuantity;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rackrate"></param>
        /// <param name="date"></param>
        /// <param name="inventories">Inventories for a given roomtype</param>
        /// <param name="prices">Prices for a given roomtype</param>
        /// <returns></returns>
        private static decimal FindPrice(decimal rackrate, DateTime date, IEnumerable<RoomTypeInventory> inventories, IEnumerable<RatePrice> prices)
        {
            var currentInventory = inventories.FirstOrDefault(x => x.Date.Value.Date == date.Date);
            if(currentInventory.IsNotNull())
            {
                var ratePrice = prices.FirstOrDefault(x => x.Category.Id == currentInventory.RateCategoryId);
                if (ratePrice.IsNotNull() && ratePrice.IsActive)
                {
                    return ratePrice.Value;
                }
            }

            return rackrate;
        }

        public int RoomTypeId { get; private set; }

        public string Name { get; private set; }

        public string Description { get; private set; }

        public int MaxCapacity { get; private set; }

        public int AvailableQuantity { get; private set; }

        public bool IsAvailable { get; private set; }

        public decimal LowestPricePerDay { get; private set; }

        public decimal TotalPrice { get; private set; }

        public IEnumerable<SearchResultRoomPrice> Prices { get; private set; }

        public IEnumerable<string> Thumbnails { get; set; }
    }

    public class SearchResultRoomPrice
    {
        private SearchResultRoomPrice(DateTime date, int available, decimal price)
        {
            Date = date;
            Available = available;
            Price = price;
        }

        public static SearchResultRoomPrice Create(DateTime date, int available, decimal price)
        {
            return new SearchResultRoomPrice(date, available, price);
        }

        public DateTime Date { get; private set; }

        public int Available { get; private set; }

        public decimal Price { get; private set; }
    }
}
