using System;
using EcoHotels.Core.Common;
using EcoHotels.Core.Domain.Models.Localization;
using EcoHotels.Core.Infrastructure;
using EcoHotels.Extensions;

namespace EcoHotels.Core.Domain.Models.Property
{
    /// <summary>
    /// Things like Hot Water Shower, Wireless, Laudry ect.
    /// </summary>
    [Serializable]
    public class Amenity : EntityBase<Amenity>
    {
        protected internal Amenity()
        {
            Name = string.Empty;
        }

        /// <summary>
        /// The given name for the amentity within the system only.
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Factory method
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static Amenity Create(string name)
        {
            Check.Require(name.IsNotNullOrEmpty(), "Name can not be null or empty.");

            return new Amenity{ Name = name };
        }

        protected override void Validate()
        {
            
        }
    }
}
