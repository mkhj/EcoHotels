using System;
using EcoHotels.Core.Common;
using EcoHotels.Core.Common.Enum;
using EcoHotels.Core.Domain.Helpers;
using EcoHotels.Core.Infrastructure;
using EcoHotels.Extensions;

namespace EcoHotels.Core.Domain.Models.Commerce
{
    [Serializable]
    public class ReservationAddon : EntityBase<ReservationAddon>
    {
        protected internal ReservationAddon() { }

        protected internal ReservationAddon(ReservationItem reservationItem, Addon addon)
        {
            ReservationItem = reservationItem;
            Name = addon.Name.GetText(LanguageTypeEnum.English);
            Description = addon.Description.GetText(LanguageTypeEnum.English);
            Price = addon.Price;
            CalculationRule = addon.CalculationRule;
            PostingRhythm = addon.PostingRhythm;
        }

        public static ReservationAddon Create(ReservationItem reservationItem, Addon addon)
        {
            Check.Require(reservationItem.IsNotNull(), "ReservationItem can not be null");
            Check.Require(addon.IsNotNull(), "Addon can not be null");

            return new ReservationAddon(reservationItem, addon);
        }

        public virtual ReservationItem ReservationItem { get; protected internal set; }

        public virtual string Name { get; protected internal set; }

        public virtual string Description { get; protected internal set; }

        public virtual decimal Price { get; protected internal set; }

        protected internal virtual CalculationRule CalculationRule { get; set; }

        protected internal virtual PostingRhythm PostingRhythm { get; set; }

        public virtual decimal CalculatePrice(int adults, int children, int babies, int days)
        {
            return AddonCalculator.CalculateAddonPrice(Price, CalculationRule, PostingRhythm, adults, children, babies, days);
        }


        protected override void Validate()
        {
            
        }
    }
}
