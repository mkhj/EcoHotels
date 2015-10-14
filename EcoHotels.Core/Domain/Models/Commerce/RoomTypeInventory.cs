using System;
using EcoHotels.Core.Infrastructure;

namespace EcoHotels.Core.Domain.Models.Commerce
{
    [Serializable]
    public class RoomTypeInventory : EntityIdentityBase<RoomTypeInventory>
    {
        protected RoomTypeInventory(){}

        public static RoomTypeInventory Create(int roomTypeId, Date date)
        {
            return new RoomTypeInventory
                       {
                           RoomTypeId = roomTypeId,
                           Date = date
                       };
        }

        public static RoomTypeInventory Create(int roomTypeId, Date date, int rateCategoryId)
        {
            return new RoomTypeInventory
            {
                RateCategoryId = rateCategoryId,
                RoomTypeId = roomTypeId,
                Date = date
            };
        }

        public virtual Date Date { get; protected set; }

        public virtual int RoomTypeId { get; protected set; }

        public virtual int? RateCategoryId { get; set; }

        public virtual int Quantity { get; set; }

        protected override void Validate()
        {
            
        }
    }
}
