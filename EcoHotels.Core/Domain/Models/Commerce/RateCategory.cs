using System;
using System.Collections.Generic;
using EcoHotels.Core.Common;
using EcoHotels.Core.Domain.Models.Property;
using EcoHotels.Core.Infrastructure;
using EcoHotels.Extensions;

namespace EcoHotels.Core.Domain.Models.Commerce
{
    [Serializable]
    public class RateCategory : EntityBase<RateCategory>
    {
        protected RateCategory()
        {
            Items = new List<RatePrice>();
        }

        public static RateCategory Create(Hotel hotel, string name, string description)
        {
            Check.Require(hotel.IsNotNull(), "Hotel can not be null.");
            Check.Require(name.IsNotNull(), "Name can not be null or empty.");

            return new RateCategory
                       {
                           Hotel = hotel,
                           Name = name,
                           Description = description,
                           Color = "ff0000"
                       };
        }

        public virtual Hotel Hotel { get; set; } 

        public virtual string Name { get; set; }

        public virtual string Description { get; set; }

        /// <summary>
        /// Color is given in HEX
        /// </summary>
        public virtual string Color { get; set; }

        public virtual ICollection<RatePrice> Items { get; protected internal set; }

        protected override void Validate()
        {
        }
    }
}
