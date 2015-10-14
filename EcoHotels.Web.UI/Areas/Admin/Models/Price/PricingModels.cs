using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using EcoHotels.Core.Domain.Models.Commerce;
using EcoHotels.Extensions;
using Newtonsoft.Json;

namespace EcoHotels.Web.UI.Areas.Admin.Models.Price
{
    public class PricingModel
    {
        public PricingModel()
        {
            Months = new List<SelectListItem>();
            Years = new List<SelectListItem>();
            DateWithPrice = new List<PricingModelItem>();
        }

        public PricingModel(IEnumerable<RateCategory> rateCategories, IEnumerable<RoomTypeInventory> roomTypeInventories, int selectedYear, int selectedMonth) : this()
        {
            SelectedMonth = selectedMonth;


            SelectedMonth = selectedMonth;
            var months = new List<string> { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };
            months.ForEach(x => Months.Add(new SelectListItem { Selected = months.IndexOf(x) == selectedMonth, Text = x, Value = (months.IndexOf(x) + 1).ToString() }));

            SelectedYear = selectedYear;
            Years.AddRange(Enumerable.Range(DateTime.Now.Year, 4).Select(x => new SelectListItem { Selected = x == selectedYear, Text = x.ToString(), Value = x.ToString() }).ToList());

            var daysInMonth = DateTime.DaysInMonth(selectedYear, selectedMonth);
            DateWithPrice = BuildCalendar(daysInMonth, rateCategories, roomTypeInventories);


            RateCategoryIdsJSON = BuildRateCategoryIdsJson(rateCategories);
            RateCategoriesJSON = BuildMenuRateCategoryJson(rateCategories);
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

        [ReadOnly(true)]
        public string RateCategoryIdsJSON { get; set; }

        [ReadOnly(true)]
        public string RateCategoriesJSON { get; set; }

        public List<PricingModelItem> DateWithPrice { get; set; }

        private string BuildRateCategoryIdsJson(IEnumerable<RateCategory> rateCategories)
        {
            var data = rateCategories.Select(x => x.Id).ToList();
            data.Insert(0, 0);

            return data.ToJSON();
        }

        private string BuildMenuRateCategoryJson(IEnumerable<RateCategory> rateCategories)
        {
            var result = new StringBuilder();

            var data = rateCategories.Select(x => '"' + x.Name + '"' + ":" + '"'+ x.Color + '"');

            result.Append("{");

            result.Append("\"Rackrate\":\"#ffffff\",");

            result.Append(string.Join(",", data));

            result.Append("}");

            return result.ToString();
        }

        private List<PricingModelItem> BuildCalendar(int daysInMonth, IEnumerable<RateCategory> rateCategories, IEnumerable<RoomTypeInventory> roomTypeInventories)
        {
            var result = new List<PricingModelItem>();

            for (var i = 0; i < daysInMonth; i++)
            {
                var roomTypeInventory = roomTypeInventories.ElementAtOrDefault(i);
                if (roomTypeInventory.IsNull())
                {
                    result.Add(new PricingModelItem { Id = 0, Color = "ffffff" });
                }
                else
                {
                    if(roomTypeInventory.RateCategoryId.IsNull())
                    {
                        result.Add(new PricingModelItem { Id = 0, Color = "ffffff" });
                    }
                    else
                    {
                        var rateCategory = rateCategories.First(x => x.Id == roomTypeInventory.RateCategoryId);
                        result.Add(new PricingModelItem { Id = rateCategory.Id, Color = rateCategory.Color });
                    }
                }
            }

            return result;
        }
    }

    public class PricingModelItem
    {
        public int Id { get; set; }

        [ReadOnly(true)]
        public string Color { get; set; }
    }

}