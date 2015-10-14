using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EcoHotels.Web.UI.Models
{
    public class SearchModel
    {
        [Required]
        public int CityId { get; set; }

        //[Required]
        public DateTime Arrival { get; set; }

        //[Required]
        public DateTime Departure { get; set; }
    }
}