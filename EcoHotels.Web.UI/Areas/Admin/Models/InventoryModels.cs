using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EcoHotels.Core.Common.Enum;
using EcoHotels.Core.Domain.Models.Commerce;
using EcoHotels.Core.Domain.Models.Localization;
using EcoHotels.Core.Domain.Models.Property;
using EcoHotels.Extensions;

namespace EcoHotels.Web.UI.Areas.Admin.Models
{
    public class InventoryModel
    {
        public InventoryModel(){}

        public InventoryModel(Hotel selectedHotel, IEnumerable<RoomTypeInventory> inventories, IEnumerable<Reservation> reservations, int selectedMonth, int selectedYear)
        {
            SelectedMonth = selectedMonth;            
           
            Months = new List<SelectListItem>();            
            var months = new List<string> { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };            
            months.ForEach(x => Months.Add(new SelectListItem { Selected = months.IndexOf(x) == selectedMonth, Text = x, Value = (months.IndexOf(x) + 1).ToString() }));

            SelectedYear = selectedYear;
            Years = new List<SelectListItem>();
            Years.AddRange(Enumerable.Range(DateTime.Now.Year, 4).Select(x => new SelectListItem { Selected = x == selectedYear, Text = x.ToString(), Value = x.ToString() }).ToList());

            DaysInMonth = BuildDaysInMonth(selectedYear, selectedMonth);

            Rooms = BuildRoomTypeInventoryList(selectedHotel.RoomTypes, inventories, reservations, new DateTime(selectedYear, selectedMonth, 1));
        }

        #region - Selected Month -

        [Required, Range(1, 12, ErrorMessage = "Selected Month is out of range.")]
        public int SelectedMonth { get; set; }

        [ReadOnly(true)]
        public List<SelectListItem> Months { get; set; }

        #endregion

        #region - Selected Year -

        [Required, Range(2000, 2100, ErrorMessage = "Selected Year is out of range.")]
        public int SelectedYear { get; set; }

        [ReadOnly(true)]
        public List<SelectListItem> Years { get; set; }

        #endregion

        public Dictionary<int, string> DaysInMonth { get; set; }

        public List<InventoryItemModel> Rooms { get; set; }

        #region - View model methods -

        private Dictionary<int, string> BuildDaysInMonth(int selectedYear, int selectedMonth)
        {
            var numberOfDays = DateTime.DaysInMonth(selectedYear, selectedMonth);
            var startDate = new DateTime(selectedYear, selectedMonth, 1);

            var result = new Dictionary<int, string>();
            for(var i = 0; i < numberOfDays; i++)
            {
                var date = startDate.AddDays(i);
                result.Add(date.Day, date.ToString("ddd").Substring(0, 2));
            }

            return result;
        }

        private List<InventoryItemModel> BuildRoomTypeInventoryList(IEnumerable<RoomType> roomTypes, IEnumerable<RoomTypeInventory> inventories, IEnumerable<Reservation> reservations, DateTime firstDayInMonth)
        {
            var daysInMonth = DateTime.DaysInMonth(firstDayInMonth.Year, firstDayInMonth.Month);

            var reservationItems = reservations.SelectMany(x => x.Items);

            var result = new List<InventoryItemModel>();
            foreach (var roomType in roomTypes)
            {
                var inventoryItemModel = new InventoryItemModel(roomType);

                var inventoriesForRoomtype = inventories.Where(x => x.RoomTypeId == roomType.Id);
                var reservationsItemsForRoomtype = reservationItems.Where(x => x.RoomTypeId == roomType.Id);

                for (var i = 0; i < daysInMonth; i++)
                {
                    var date = firstDayInMonth.AddDays(i);
                    var inventory = inventoriesForRoomtype.FirstOrDefault(x => x.Date.Value == date);

                    var numberOfReservations = reservationsItemsForRoomtype.SelectMany(x => x.PricePerDate).Count(x => x.Date == date);

                    if(inventory.IsNull())
                    {
                        var availableQuantity = roomType.Quantity - numberOfReservations;
                        inventoryItemModel.Quantities.Add(new InventoryItemQtyModel { Date = date, Value = availableQuantity });
                    }
                    else
                    {
                        var availableQuantity = inventory.Quantity - numberOfReservations;
                        inventoryItemModel.Quantities.Add(new InventoryItemQtyModel { Date = inventory.Date.Value, Value = availableQuantity });
                    }
                }

                result.Add(inventoryItemModel);
            }

            return result;
        }

        #endregion

    }

    public class InventoryItemModel
    {
        public InventoryItemModel(){}

        public InventoryItemModel(RoomType roomType)
        {
            RoomTypeId = roomType.Id;
            RoomType = roomType.Name.GetText(LanguageTypeEnum.English);
            Quantities = new List<InventoryItemQtyModel>();
        }

        [Required]
        public int RoomTypeId { get; set; }

        [ReadOnly(true)]
        public string RoomType { get; set; }

        //TODO: what to do with Dates and Qty
        public List<InventoryItemQtyModel> Quantities { get; set; }
    }

    public class InventoryItemQtyModel
    {
        public InventoryItemQtyModel()
        {
            
        }

        public DateTime Date { get; set; }

        public int Value { get; set; }
    }
}