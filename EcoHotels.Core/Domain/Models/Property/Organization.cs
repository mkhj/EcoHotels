using System;
using System.Collections.Generic;
using EcoHotels.Core.Domain.Models.Localization;
using EcoHotels.Core.Domain.Models.Security;
using EcoHotels.Core.Domain.Value_objects;
using EcoHotels.Core.Infrastructure;

namespace EcoHotels.Core.Domain.Models.Property
{
    [Serializable]
    public class Organization : EntityBase<Organization>
    {
        public static Guid SystemOrganizationId = new Guid("9d4e7582-36e7-471a-bfe2-a06900c8293b");

        protected Organization()
        {
            Name = string.Empty;
            VatNo = string.Empty;
            Phone = string.Empty;
            Email = string.Empty;
            Address1 = string.Empty;
            Address2 = string.Empty;
            City = string.Empty;
            Zipcode = string.Empty;
            Country = string.Empty;

            Created = DateTime.Now;

            Hotels = new List<Hotel>();
            Users = new List<User>();

            //AvailableLanguages = new List<Language>();
            //AvailableCurrencies = new List<Currency>();
        }

        /// <summary>
        /// Factory Method
        /// </summary>
        /// <returns></returns>
        public static Organization Create(string name, string vatNo)
        {
            return new Organization
                       {
                           Name = name,
                           VatNo = vatNo
                       };
        }

        #region - Address -

        /// <summary>
        /// 
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual string VatNo { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual string Phone { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual string Email { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual string Address1 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual string Address2 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual string City { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual string Zipcode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual string Country { get; set; }

        #endregion

        public virtual DateTime Created { get; protected internal set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual ICollection<Hotel> Hotels { get; protected internal set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual ICollection<User> Users { get; protected internal set; }


        //public virtual Language DefaultLanguage { get; set; }

        //public virtual Currency DefaultCurrency { get; set; }

        ///// <summary>
        ///// 
        ///// </summary>
        //public virtual ICollection<Language> AvailableLanguages { get; set; }

        ///// <summary>
        ///// 
        ///// </summary>
        //public virtual ICollection<Currency> AvailableCurrencies { get; set; }


        protected override void Validate()
        {

        }
    }
}
