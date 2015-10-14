using System;
using System.Collections.Generic;
using System.Linq;
using EcoHotels.Core.Common;
using EcoHotels.Core.Common.Enum;
using EcoHotels.Core.Domain.Models.Commerce;
using EcoHotels.Core.Domain.Models.Media;
using EcoHotels.Core.Domain.Models.Tags;
using EcoHotels.Core.Domain.Value_objects;
using EcoHotels.Core.Infrastructure;
using EcoHotels.Extensions;
using EcoHotels.Core.Domain.Models.Localization;

namespace EcoHotels.Core.Domain.Models.Property
{
    [Serializable]
    public class Hotel : EntityBase<Hotel>
    {
        protected Hotel()
        {
            Name = string.Empty;
            Address1 = string.Empty;
            Address2 = string.Empty;
            Zipcode = string.Empty;
            City = string.Empty;
            Country = string.Empty;
            Phone = string.Empty;
            Fax = string.Empty;
            Email = string.Empty;
            WWW = string.Empty;
            VatNo = string.Empty;
            IsActive = false;

            About = new MultiLanguageText();
            CancellationPolicy = new MultiLanguageText();
            PaymentPolicy = new MultiLanguageText();
            PageInformation = new PageInformation();

            CheckIn = 3;
            CheckInAMPM = "PM";
            CheckOut = 3;
            CheckOutAMPM = "PM";

            Assets = new List<Asset>();
            Addons = new List<Addon>();
            Descriptions = new List<HotelBulletPoint>();

            Enumerable.Range(0, 5)
                .Select(x => HotelBulletPoint.Create(this, HotelBulletPointEnum.WhatWeLove, x))
                .ToList()
                .ForEach(x => Descriptions.Add(x));

            Enumerable.Range(0, 5)
                .Select(x => HotelBulletPoint.Create(this, HotelBulletPointEnum.WhatYouNeedToKnow, x))
                .ToList()
                .ForEach(x => Descriptions.Add(x));
        }

        /// <summary>
        /// Factory Method
        /// </summary>
        /// <param name="name"></param>
        /// <param name="vatno"></param>
        /// <param name="organization"></param>
        /// <returns></returns>
        public static Hotel Create(string name, string vatno, Organization organization)
        {
            Check.Require(name.IsNotNullOrEmpty(), "Name can not be null or empty.");
            Check.Require(organization.IsNotNull(), "Organization can not be null.");

            var hotel = new Hotel
                            {
                                Name = name,
                                VatNo = vatno,
                                Organization = organization,
                                Created = DateTime.Now
                            };

            hotel.PageInformation.SetVirtualPagename(name);

            return hotel;
        }

        /// <summary>
        /// Owner of the hotel.
        /// </summary>
        public virtual Organization Organization { get; protected internal set; }

        #region - Property Information -

        public virtual string Name { get; set; }

        public virtual string Address1 { get; set; }

        public virtual string Address2 { get; set; }

        public virtual string Zipcode { get; set; }

        public virtual string City { get; set; }

        public virtual string Country { get; set; }

        public virtual string Phone { get; set; }

        public virtual string Fax { get; set; }

        public virtual string Email { get; set; }

        public virtual string WWW { get; set; }

        public virtual string VatNo { get; set; }

        #endregion

        /// <summary>
        /// The local currency used at this hotel.
        /// </summary>
        public virtual Currency Currency { get; set; }

        /// <summary>
        /// A description about the hotel.
        /// </summary>
        public virtual MultiLanguageText About { get; protected internal set; }

        public virtual HotelCategoryEnum CategoryOne { get; set; }
        
        public virtual HotelCategoryEnum CategoryTwo { get; set; }

        public virtual HotelCategoryEnum CategoryThree { get; set; }

        #region - Hotel Policies -

        public virtual int CheckIn { get; set; }

        public virtual string CheckInAMPM { get; set; }

        public virtual int CheckOut { get; set; }

        public virtual string CheckOutAMPM { get; set; }

        /// <summary>
        /// Minimum number of days a guest is required to stay at the hotel
        /// </summary>
        public virtual int MinimumStay { get; set; }

        /// <summary>
        /// Minimum age required for a guest to check-in
        /// </summary>
        public virtual int MinimumCheckInAge { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual MultiLanguageText CancellationPolicy { get; protected internal set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual MultiLanguageText PaymentPolicy { get; protected internal set; }

        #endregion


        /// <summary>
        /// 
        /// </summary>
        public virtual ICollection<Amenity> Amenities { get; protected internal set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual ICollection<RoomType> RoomTypes { get; protected internal set; }

        /// <summary>
        /// Short bullet points about the hotel.
        /// </summary>
        public virtual ICollection<HotelBulletPoint> Descriptions { get; protected internal set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual ICollection<Asset> Assets { get; protected internal set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual ICollection<Addon> Addons { get; protected internal set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual PageInformation PageInformation { get; protected internal set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual SearchTagCity SearchTagCity { get; set; }
        
        /// <summary>
        /// This flag indicates wheather or not the hotel has been verified by us.
        /// </summary>
        public virtual bool IsActive { get; set; }

        /// <summary>
        /// Time of creation.
        /// </summary>
        public virtual DateTime Created { get; protected internal set; }

        /// <summary>
        /// Last modified.
        /// </summary>
        public virtual DateTime? Modified { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual DateTime? Verified { get; set; }

        /// <summary>
        /// 
        /// </summary>
        protected override void Validate()
        {
            //Enumerable.Range(0, 6)
            //    .Select(x => HotelBulletPoint.Create(this, HotelBulletPointEnum.WhatWeLove, x))
            //    .ToList()
            //    .ForEach(x => Descriptions.Add(x));

            //Enumerable.Range(0, 6)
            //    .Select(x => HotelBulletPoint.Create(this, HotelBulletPointEnum.WhatYouNeedToKnow, x))
            //    .ToList()
            //    .ForEach(x => Descriptions.Add(x));
        }
    }
}
