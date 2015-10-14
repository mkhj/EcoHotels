using System;
using System.Collections.Generic;
using EcoHotels.Core.Common;
using EcoHotels.Core.Infrastructure;
using EcoHotels.Extensions;

namespace EcoHotels.Core.Domain.Models.Media
{
    [Serializable]
    public class AssetCategory : EntityBase<AssetCategory>
    {
        protected AssetCategory()
        {
            Name = string.Empty;
            Items = new List<Asset>();
        }

        public virtual int HotelId { get; set; }

        public virtual string Name { get; set; }

        public virtual ICollection<Asset> Items { get; protected internal set; }

        public static AssetCategory Create(int hotelId, string name)
        {
            Check.Require(hotelId != 0, "OrganizationId can not be empty.");
            Check.Require(name.IsNotNullOrEmpty(), "Name can not be null or empty.");

            return new AssetCategory
            {
                HotelId = hotelId,
                Name = name
            };
        }

        protected override void Validate()
        {
            
        }
    }
}
