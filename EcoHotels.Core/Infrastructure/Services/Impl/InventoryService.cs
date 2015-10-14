using System;
using System.Collections.Generic;
using System.Linq;
using EcoHotels.Core.Domain.Models.Commerce;
using EcoHotels.Core.Domain.Models.Property;
using EcoHotels.Core.Infrastructure.NH;
using NHibernate.Criterion;

namespace EcoHotels.Core.Infrastructure.Services.Impl
{
    public class InventoryService : IInventoryService
    {
        private Repository<RoomTypeInventory> InventoryRepo;

        private IReservationService ReservationService;

        private ICalendarService CalendarService;

        public InventoryService(ICalendarService calendarService, IReservationService reservationService)
        {
            InventoryRepo = new Repository<RoomTypeInventory>();

            CalendarService = calendarService;
            ReservationService = reservationService;
        }

        public IEnumerable<RoomTypeInventory> EnsureInventories(Hotel hotel, IEnumerable<RoomTypeInventory> inventories, int year, int month)
        {
            var daysInMonth = DateTime.DaysInMonth(year, month);

            var startDate = new DateTime(year, month, 1);
            var endDate = new DateTime(year, month, daysInMonth);

            var dates = CalendarService.FindAllDaysInMonthBy(year, month);

            var reservations = ReservationService.FindBy(hotel.Id, startDate, endDate);
            var reservationItems = reservations.SelectMany(x => x.Items);

            var roomtypes = hotel.RoomTypes;

            var result = new List<RoomTypeInventory>();

            for (var i = 1; i <= daysInMonth; i++)
            {
                var date = dates.First(x => x.Value.Day == i);
               
                foreach (var roomType in roomtypes)
                {
                    var reservationsItemsForRoomtype = reservationItems.Where(x => x.RoomTypeId == roomType.Id);
                    var numberOfReservations = reservationsItemsForRoomtype.SelectMany(x => x.PricePerDate).Count(x => x.Date.Day == i);

                    if (!inventories.Any(x => x.RoomTypeId == roomType.Id && x.Date.Value.Day == i))
                    {
                        var inventory = RoomTypeInventory.Create(roomType.Id, date);

                        // Available Quantity
                        inventory.Quantity = roomType.Quantity + numberOfReservations;

                        result.Add(inventory);
                    }
                }
            }

            return result;
        }

        public IEnumerable<RoomTypeInventory> FindBy(int roomtypeId, int year, int month)
        {
            var arrival = new DateTime(year, month, 1);
            var enddate = new DateTime(year, month, DateTime.DaysInMonth(year, month));

            var criteria = DetachedCriteria.For(typeof(RoomTypeInventory))
                .Add(Restrictions.Eq("RoomTypeId", roomtypeId))
                .CreateAlias("Date", "d")
                    .Add(Restrictions.Ge("d.Value", arrival.Date))
                    .Add(Restrictions.Le("d.Value", enddate.Date));

            return InventoryRepo.FindAll(criteria);
        }

        public IEnumerable<RoomTypeInventory> FindByIds(IEnumerable<int> ids, int year, int month)
        {
            var arrival = new DateTime(year, month, 1);
            var enddate = new DateTime(year, month, DateTime.DaysInMonth(year, month));

            var criteria = DetachedCriteria.For(typeof(RoomTypeInventory))
                .Add(Restrictions.In("RoomTypeId", ids.ToArray()))
                .CreateAlias("Date", "d")
                    .Add(Restrictions.Ge("d.Value", arrival.Date))
                    .Add(Restrictions.Le("d.Value", enddate.Date));

            return InventoryRepo.FindAll(criteria);
        }

        public IEnumerable<RoomTypeInventory> FindByDate(int year, int month)
        {
            var arrival = new DateTime(year, month, 1);
            var enddate = new DateTime(year, month, DateTime.DaysInMonth(year, month));

            var criteria = DetachedCriteria.For(typeof(RoomTypeInventory))
                .CreateAlias("Date", "d")
                    .Add(Restrictions.Ge("d.Value", arrival.Date))
                    .Add(Restrictions.Le("d.Value", enddate.Date));

            return InventoryRepo.FindAll(criteria);            
        }

        public void Save(RoomTypeInventory inventory)
        {
            if (inventory.IsValid())
            {
                InventoryRepo.Save(inventory);   
            }
        }
        
        public void Save(List<RoomTypeInventory> inventories)
        {
            inventories.ForEach(Save);
        }
    }

}
