using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EcoHotels.Core.Common;
using EcoHotels.Core.Common.Enum;
using EcoHotels.Core.Domain.Models.Localization;
using EcoHotels.Core.Infrastructure;
using EcoHotels.Extensions;

namespace EcoHotels.Core.Domain.Models.Property
{
    [Serializable]
    public class HotelBulletPoint : EntityBase<HotelBulletPoint>
    {
        protected internal HotelBulletPoint()
        {
            Text = new MultiLanguageText();
        }

        public static HotelBulletPoint Create(Hotel hotel, HotelBulletPointEnum type, int orderId)
        {
            Check.Require(hotel.IsNotNull(), "Name can not be null or empty.");

            return new HotelBulletPoint
                       {
                           OrderId = orderId,
                           Hotel = hotel,
                           Type = type
                       };
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual Hotel Hotel { get; protected internal set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual MultiLanguageText Text { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual HotelBulletPointEnum Type { get; protected internal set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual int OrderId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        protected override void Validate()
        {
        }
    }
}
