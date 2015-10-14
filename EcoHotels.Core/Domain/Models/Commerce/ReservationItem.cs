using System;
using System.Collections.Generic;
using System.Linq;
using EcoHotels.Core.Common;
using EcoHotels.Core.Infrastructure;
using EcoHotels.Extensions;

namespace EcoHotels.Core.Domain.Models.Commerce
{
    public enum ReservationItemType
    {
        Room,

        Package
    }

    [Serializable]
    public class ReservationItem : EntityBase<ReservationItem>
    {
        protected internal ReservationItem()
        {
            PricePerDate = new List<ReservationPrice>();
            Addons = new List<ReservationAddon>();
        }

        public static ReservationItem Create(Reservation reservation, string description, ReservationItemType type)
        {
            Check.Require(reservation.IsNotNull(), "Reservation can not be empty.");                     

            return new ReservationItem
                       {
                           Reservation = reservation,
                           Description = description,
                           Type = type
                       };
        }

        public virtual Reservation Reservation { get; protected internal set; }

        public virtual int RoomTypeId { get; set; }

        public virtual string Description { get; protected internal set; }

        public virtual string NameOfPrimaryGuest { get; set; }

        public virtual int Adults { get; set; }

        public virtual int Children { get; set; }

        public virtual int Babies { get; set; }

        public virtual ICollection<ReservationPrice> PricePerDate { get; protected internal set; }

        public virtual ICollection<ReservationAddon> Addons { get; protected internal set; }

        public virtual ReservationItemType Type { get; protected internal set; }


        public virtual decimal TotalPriceWithoutDiscount
        {
            get { return PricePerDate.Sum(x => x.Price) + Addons.Sum(x => x.Price); }
        }

        public virtual int TotalNumberOfGuests
        {
            get { return Adults + Children + Babies; }
        }

        public virtual string AddonsSummary
        {
            get
            {
                var addonsSummary = Addons.Select(x => x.Name + " DKK " + x.CalculatePrice(Adults, Children, Babies, PricePerDate.Count));
                return string.Join(",", addonsSummary);
            }
        }

        public virtual void SetAddons(ICollection<ReservationAddon> addons)
        {
            Addons = addons;
        }

        public virtual void SetPriceRates(ICollection<ReservationPrice> prices)
        {
            PricePerDate = prices;
        }
        
        protected override void Validate()
        {
            Check.Require(PricePerDate.Count > 0, "Collection ReservationPrice can not be empty."); 
        }
    }
}
