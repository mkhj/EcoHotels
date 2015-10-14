using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EcoHotels.Core.Domain.Models.Tags;

namespace EcoHotels.Web.UI.Models
{
    public class HomepageModel
    {
        public HomepageModel(IEnumerable<SearchTagCity> tagCities)
        {
            TagCities = tagCities.GroupBy(x => x.Country.Name.Trim())
                .ToDictionary(
                    item => item.Key, 
                    item => item.OrderBy(x => x.Name).Select(x => new SelectListItem {Text = x.Name, Value = x.Id.ToString()})
                );
        }

        public Dictionary<string, IEnumerable<SelectListItem>> TagCities { get; set; }
    }
}