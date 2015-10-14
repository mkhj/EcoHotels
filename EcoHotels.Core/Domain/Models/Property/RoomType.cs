using System;
using System.Collections.Generic;
using System.Linq;
using EcoHotels.Core.Common;
using EcoHotels.Core.Domain.Models.Commerce;
using EcoHotels.Core.Domain.Models.Localization;
using EcoHotels.Core.Domain.Models.Media;
using EcoHotels.Core.Infrastructure;
using EcoHotels.Extensions;

namespace EcoHotels.Core.Domain.Models.Property
{
    /// <summary>
    /// Represents a given Room Type.
    /// </summary>
    [Serializable]
    public class RoomType : EntityBase<RoomType>
    {
        protected RoomType()
        {
            Beds = string.Empty;
            RoomView = string.Empty;

            Name = new MultiLanguageText();
            Description = new MultiLanguageText();

            Assets = new List<Asset>();
            Amenities = new List<Amenity>();
        }

        /// <summary>
        /// Factory method
        /// </summary>
        /// <param name="name"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public static RoomType Create(Hotel hotel, string name, Language language)
        {
            Check.Require(hotel.IsNotNull(), "hotel can not be null.");
            Check.Require(name.IsNotNullOrEmpty(), "Name can not be null or empty.");
            Check.Require(language != null, "Language can not be null.");

            var roomType = new RoomType
                               {
                                   Hotel = hotel
                               };
            roomType.Name.AddLocalizedText(name, language);

            return roomType;
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual Hotel Hotel { get; protected internal set; }

        /// <summary>
        /// The name of the room.
        /// </summary>
        public virtual MultiLanguageText Name { get; set; }

        /// <summary>
        /// A description of what the room is like.
        /// </summary>
        public virtual MultiLanguageText Description { get; set; }

        /// <summary>
        /// Type of beds.
        /// </summary>
        public virtual string Beds { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual string RoomView { get; set; }

        /// <summary>
        /// Number of persons allowed in the room.
        /// </summary>
        public virtual int Capacity { get; set; }

        /// <summary>
        /// Number of rooms in the Room/Apartment
        /// </summary>
        public virtual int PhysicalRooms { get; set; }

        /// <summary>
        /// Room size in square meters / ft.
        /// </summary>
        public virtual int Size { get; set; }

        /// <summary>
        /// Total Number of rooms of the current type within the hotel.
        /// </summary>
        public virtual int Quantity { get; set; }

        /// <summary>
        /// Full price for the room
        /// </summary>
        public virtual decimal RackRate { get; set; }

        /// <summary>
        /// Is smoking allowed.
        /// </summary>
        public virtual bool IsSmoking { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public virtual ICollection<Amenity> Amenities { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual ICollection<Addon> Addons { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual ICollection<Asset> Assets { get; set; }

        //public int FindAvailableQuantity(IEnumerable<RoomTypeInventory> inventories, IEnumerable<ReservationItem> reservationItems, DateTime arrival, DateTime departure)
        //{
        //    var duration = (departure - arrival).Days;
        //    var reservationPrices = reservationItems.SelectMany(x => x.PricePerDate);

        //    for(var i = 0; i <= duration; i++)
        //    {
        //        var date = arrival.AddDays(i).Date.Date;

        //        var inventory = inventories.FirstOrDefault(x => x.Date.Value.Date == date);
                
        //        // if no days a found qty is zero else qty = number of days
        //        var quantityUsedForGivenDate = reservationPrices.Where(x => x.Date.Date == date).Count();

        //        if(inventory.IsNotNull())
        //        {
        //            availableQuantity = (inventory.Quantity - quantityUsedForGivenDate);
        //        }
        //        else
        //        {
        //            availableQuantity = (Quantity - quantityUsedForGivenDate);
        //        }
        //    }

        //    foreach (var inventory in inventories)
        //    {
        //        inventory.Quantity
        //    }
        //}



        protected override void Validate()
        {
            
        }
    }
}
