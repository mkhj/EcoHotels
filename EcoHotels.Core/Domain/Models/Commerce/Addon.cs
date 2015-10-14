using System;
using EcoHotels.Core.Common;
using EcoHotels.Core.Domain.Models.Localization;
using EcoHotels.Core.Domain.Models.Property;
using EcoHotels.Core.Infrastructure;
using EcoHotels.Extensions;

namespace EcoHotels.Core.Domain.Models.Commerce
{
    [Serializable]
    public class Addon : EntityBase<Addon>
    {
        protected Addon()
        {
            Name = new MultiLanguageText();
            Description = new MultiLanguageText();
            Price = 0.0m;
        }


        public virtual Hotel Hotel { get; protected internal set; }

        /// <summary>
        /// The name of the addon.
        /// </summary>
        public virtual MultiLanguageText Name { get; protected internal set; }

        /// <summary>
        /// A description of the addon.
        /// </summary>
        public virtual MultiLanguageText Description { get; protected internal set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual decimal Price { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual CalculationRule CalculationRule { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual PostingRhythm PostingRhythm { get; set; }

        public static Addon Create(Hotel hotel, decimal price)
        {
            Check.Require(hotel.IsNotNull(), "Hotel can not be null.");

            return new Addon
                       {
                           Hotel = hotel,
                           Price = price
                       };
        }

        public virtual double CalculatePrice(DateTime arrival, DateTime depature)
        {
            var days = (arrival - depature).Days;

            return 0;
        }

        protected override void Validate()
        {
            
        }
    }
}
