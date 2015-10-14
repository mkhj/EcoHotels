using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using EcoHotels.Core.Common.Enum;
using EcoHotels.Core.Domain.Models.Property;
using EcoHotels.Core.Domain.Value_objects;

namespace EcoHotels.Web.UI.Models
{
    public class HotelModel
    {
        public HotelModel(SearchResultHotel hotel)
        {
            Name = hotel.Name;
            About = hotel.About;
            CheckIn = hotel.CheckIn;
            CheckOut = hotel.CheckOut;
        }

        [ReadOnly(true)]
        public string Name { get; set; }

        [ReadOnly(true)]
        public string About { get; set; }

        [ReadOnly(true)]
        public string CheckIn { get; set; }

        [ReadOnly(true)]
        public string CheckOut { get; set; }

        public List<HotelAvailableRoomsModel> AvailableRooms { get; set; }
    }

    public class HotelAvailableRoomsModel
    {
        
    }
}