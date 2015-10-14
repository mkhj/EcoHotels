using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HeadWorks.Extensions;
using EcoHotels.Core.Domain.Models.Inventory;
using EcoHotels.Core.Domain.Models.Localization;
using EcoHotels.Core.Domain.Models.Price;
using EcoHotels.Core.Domain.Models.Property;

namespace EcoHotels.Core.Domain.Models.Search
{
    [Serializable]
    public class RoomAvailability
    {
        private IEnumerable<Season> Seasons;
        private IEnumerable<InventoryStatus> InventoryStatus;

        private DateTime Arrival;
        private DateTime Departure;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="roomType"></param>
        /// <param name="seasons"></param>
        /// <param name="inventoryStatus"></param>
        /// <param name="arrival"></param>
        /// <param name="departure"></param>
        public RoomAvailability(RoomType roomType, IList<Season> seasons, IList<InventoryStatus> inventoryStatus, DateTime arrival, DateTime departure)
        {            
            Seasons = seasons;
            InventoryStatus = inventoryStatus;
            Arrival = arrival;
            Departure = departure;

            RoomType = roomType;
            Duration = (Departure - Arrival).Days;
        }

        /// <summary>
        /// 
        /// </summary>
        public RoomType RoomType { get; private set; }

        public int Duration { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool IsAvailable()
        {
            for (var i = 0; i < Duration; i++)
            {
                var date = Arrival.AddDays(i);

                var usedQuantity = FindUsedQuantity(date);
                var availableQuantity = FindAvailableQuantity(date);

                // matches the total of bookings with how many rooms that are available with in the seasons
                if (usedQuantity >= availableQuantity)
                {
                    // no need to look any further.
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public int FindUsedQuantity(DateTime dateTime)
        {
            var status = InventoryStatus.Where(x => x.Date == dateTime).FirstOrDefault();
            return status.IsNotNull() ? status.Used : 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public int FindAvailableQuantity(DateTime dateTime)
        {
            var inventoryItem = FindInventoryLevel(dateTime.Date);
            return (inventoryItem.IsNotNull()) ? inventoryItem.Value : RoomType.Quantity;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IList<RatePerDay> FindRatesPerDay()
        {
            //TODO: Should be an Money entity and we also need to calculate Bonus points

            var result = new List<RatePerDay>();
            for (var i = 0; i < Duration; i++)
            {
                var date = Arrival.AddDays(i);

                var bestAvailablePrice = FindBestAvailablePrice(date);

                result.Add(new RatePerDay(date, bestAvailablePrice));

                ////TODO: Rates - Promotions, Qualifying and Channels
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public double CalculateAveragePrice()
        {
            return CalculateTotalPrice() / Duration;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public double CalculateTotalPrice()
        {
            if (!IsAvailable())
            {
                throw new ApplicationException("Room is not available, so can not calculate price.");
            }

            var rates = FindRatesPerDay();
            return rates.Sum(x => x.Price);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        private double FindBestAvailablePrice(DateTime date)
        {
            var activeSeasons = FindAllActiveSeasons();

            foreach (var season in activeSeasons)
            {
                if (season.Dates.Any(x => x.Value == date) && season.BestAvailableRate.IsNotNull())
                {
                    return season.BestAvailableRate.GetEffectiveRateValue(RoomType.Id, date.DayOfWeek);
                }
            }

            return RoomType.RackRate;
        }


        /// <summary>
        /// Finds all the seasons that can be used for the given Arrival and Departure daterange.
        /// </summary>
        /// <returns></returns>
        private IList<Season> FindAllActiveSeasons()
        {
            var seasons =  from season in Seasons
                           from date in season.Dates
                           where date.Value >= Arrival && date.Value <= Departure
                           select season;

            return seasons.Distinct().ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        private InventoryItem FindInventoryLevel(DateTime dateTime)
        {
            var season = Seasons.Where(x => x.HasDateInSeason(dateTime)).FirstOrDefault();
            if(season != null)
            {
                return season.Inventory.RoomsAvailable.Where(x => x.RoomType == RoomType).FirstOrDefault();
            }

            return null;
        }
    }

    [Serializable]
    public class RatePerDay
    {
        public RatePerDay(DateTime date, double price)
        {
            Date = date.Date;
            Price = price;
        }

        public DateTime Date { get; set; }

        public double Price { get; set; }
    }
}
