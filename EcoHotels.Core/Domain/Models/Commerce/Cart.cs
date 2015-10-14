using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace EcoHotels.Core.Domain.Models.Commerce
{
    [Serializable]
    public enum CartItemType
    {
        Room,
        Package
    }

    [Serializable]
    public class Cart
    {
        public Cart(string ipaddress, Value_objects.Currency currency)
        {
            Id = Guid.NewGuid();
            BookingDetails = new CartBookingDetails();
            IpAddress = ipaddress;
            Items = new List<CartItem>();
            Currency = currency;
        }

        public Guid Id { get; private set; }

        public string IpAddress { get; private set; }

        public CartBookingDetails BookingDetails { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>IEnumerble is not Serializable</remarks>
        public IList<CartItem> Items { get; set; }

        /// <summary>
        /// The Hotel default currency
        /// </summary>
        public EcoHotels.Core.Domain.Value_objects.Currency Currency { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public decimal TotalPrice
        {
            get { return Items.Sum(x => x.TotalPrice); }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        public void AddItem(CartItem item)
        {
            if (item != null)
            {
                Items.Add(item);
            }            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cartItemId"></param>
        public void RemoveRoom(Guid cartItemId)
        {
            if(Items.Any(x => x.ItemTypeId == cartItemId))
            {
                var cartItem = Items.First(x => x.ItemTypeId == cartItemId);
                Items.Remove(cartItem);
            }
        }

        public void AddPackage()
        {
            throw new NotImplementedException();
        }

        public void RemovePackage()
        {
            throw new NotImplementedException();
        }

        public void AddAddon()
        {
            throw new NotImplementedException();
            //var cartItem = cart.Items.Select(x => x.Id == item.Id);
        }

        public void RemoveAddon()
        {
            throw new NotImplementedException();
        }

        public void SetDates(DateTime arrival, DateTime departure)
        {
            BookingDetails.Arrival = arrival;
            BookingDetails.Departure = departure;
        }
    }

    [Serializable]
    public class CartBookingDetails
    {
        public DateTime Arrival { get; set; }
        public DateTime Departure { get; set; }

        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Phonenumber { get; set; }
        public string Country { get; set; }
        public string Email { get; set; }
        public string EmailConfirm { get; set; }

        public string CreditCartNumber { get; set; }
        public string CreditCartDate { get; set; }
        public string CreditCartYear { get; set; }
        public string CreditCartCVC { get; set; }
    }

    [Serializable]
    public class CartItem
    {
        private CartItem()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; private set; }

        /// <summary>
        /// This can be either the Room.Id or the Package.Id
        /// </summary>
        public Guid ItemTypeId { get; private set; }

        /// <summary>
        /// Name for the Room or Package
        /// </summary>
        public string ItemType { get; set; }

        /// <summary>
        /// A description of the Room or Package
        /// </summary>
        public string Description { get; set; }

        public string NameOfPrimaryGuest { get; set; }

        public int Quantity { get; set; }

        public int Adults { get; set; }

        public int Children { get; set; }

        public int Babies { get; set; }

        public decimal TotalPrice { get; set; }

        public int MaxCapacity { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="itemId"></param>
        /// <param name="type"></param>
        /// <param name="capacity"></param>
        /// <returns></returns>
        public static CartItem Create(Guid itemId, int capacity, CartItemType type)
        {
            return new CartItem
                       {
                           ItemTypeId = itemId,
                           ItemType = type.ToString(),
                           MaxCapacity = capacity
                       };
        }
    }
}
