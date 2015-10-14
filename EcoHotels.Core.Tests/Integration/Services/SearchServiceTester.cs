using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using EcoHotels.Core.Domain.Models.Commerce;
using EcoHotels.Core.Infrastructure.Services;
using EcoHotels.Core.Infrastructure.Services.Impl;
using EcoHotels.Core.Infrastructure.Services.Impl.Price;
using EcoHotels.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EcoHotels.Core.Tests.Integration.Services
{
    [TestClass]
    public class SearchServiceTester : UnittestContext
    {
        [TestMethod]
        public void TestMethod2()
        {

            var roomTypeId = 1;
            var year = 2013;
            var month = 6;

            var dates = new CalendarService().FindAllDaysInMonthBy(year, month);

            var rateCategory = new RateService(null).FindByHotel(1).FirstOrDefault();

            var inventoryService = new InventoryService(null, null);
            var inventories = inventoryService.FindBy(roomTypeId, year, month);

            var result = new List<RoomTypeInventory>();
            foreach (var date in dates)
            {
                var inventory = inventories.FirstOrDefault(x => x.Date == date);
                if(inventory.IsNull())
                {
                    var newInventory = RoomTypeInventory.Create(roomTypeId, date, rateCategory.Id);
                    newInventory.Quantity = 10;
                    result.Add(newInventory);
                }
                else
                {
                    //NOTE: Here is where we should update any changes to and existing Inventory
                }
            }

            inventoryService.Save(result);
        }


        [TestMethod]
        public void TestMethod3()
        {
            var reservation = Reservation.Create(
                DateTime.Now,
                DateTime.Now.AddDays(6),
                "DKK",
                ReservationType.Person,
                "10.0.0.1"
            );

            reservation.HotelId = 1;
            //reservation.Customer = new CustomerService().FindById(0); // (User.Identity.Name.ToGuid());

            reservation.Firstname = "Lise";
            reservation.Lastname = "Elle";
            reservation.Email = "my@email.com";
            reservation.PhoneNumber = "45000000";
            reservation.Country = "Denmark";

            var reservationItem = ReservationItem.Create(reservation, "Penthouse", ReservationItemType.Room);

            reservationItem.NameOfPrimaryGuest = "Lise Elle";
            reservationItem.Adults = 2;
            reservationItem.RoomTypeId = 1;

            reservationItem.SetAddons(new List<ReservationAddon>());

            var prices = new List<ReservationPrice>
                        {
                            ReservationPrice.Create(reservationItem, DateTime.Now, 380.00m),
                            ReservationPrice.Create(reservationItem, DateTime.Now.AddDays(1), 380.00m),
                            ReservationPrice.Create(reservationItem, DateTime.Now.AddDays(2), 380.00m),
                            ReservationPrice.Create(reservationItem, DateTime.Now.AddDays(3), 380.00m),
                            ReservationPrice.Create(reservationItem, DateTime.Now.AddDays(4), 360.00m),
                        };

            reservationItem.SetPriceRates(prices);
            
            reservation.Items.Add(reservationItem);

            new ReservationService(null).Save(reservation);
        }

        [TestMethod]
        public void TestMethod1()
        {
            var service = new SearchService();

            var result = service.FindByCity(1, DateTime.Now, DateTime.Now.AddDays(3));

            Assert.IsNotNull(result);

            Assert.IsTrue(result.Hotels.Count() > 0);
        }
    }
}
