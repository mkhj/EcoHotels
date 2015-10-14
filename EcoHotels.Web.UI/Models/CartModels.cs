using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EcoHotels.Web.UI.Models
{
    public class CartModel
    {
        public List<CartItemModel> Items { get; set; }
    }

    public class CartItemModel
    {
        public int RoomTypeId { get; set; }

        public int Quantity { get; set; }
    }
}