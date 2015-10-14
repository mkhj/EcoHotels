using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EcoHotels.Core.Common;
using EcoHotels.Core.Infrastructure;
using EcoHotels.Extensions;

namespace EcoHotels.Core.Domain.Models.Commerce
{
    [Serializable]
    public class RatePrice : EntityBase<RatePrice>
    {
        protected RatePrice()
        {
            
        }

        public static RatePrice Create(RateCategory category, int roomTypeId, decimal value, bool isActive)
        {
            Check.Require(category.IsNotNull(), "Rate Category can not be null.");
            Check.Require(roomTypeId != 0, "RoomTypeId can not be empty.");

            return new RatePrice
            {
                Category = category,
                RoomTypeId = roomTypeId,
                Value = value,
                IsActive = isActive
            };
        }

        public virtual int RoomTypeId { get; protected set; }

        public virtual RateCategory Category { get; set; }

        public virtual decimal Value { get; set; }

        public virtual bool IsActive { get; set; }

        

        protected override void Validate()
        {
            
        }
    }
}
