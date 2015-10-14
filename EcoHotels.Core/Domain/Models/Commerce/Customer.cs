using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EcoHotels.Core.Common.Enum;
using EcoHotels.Core.Domain.Models.Security;
using EcoHotels.Core.Helpers;
using EcoHotels.Core.Infrastructure;
using EcoHotels.Extensions;

namespace EcoHotels.Core.Domain.Models.Commerce
{
    [Serializable]
    public class Customer : EntityBase<Customer>
    {
        protected internal Customer()
        {
            Firstname = string.Empty;
            Lastname = string.Empty;
            PhoneNumber = string.Empty;
            Country = string.Empty;
            Email = string.Empty;
            Role = RolesEnum.Customer;
            Gender = GenderTypeEnum.None;
            ExternalId = string.Empty;
        }

        /// <summary>
        /// Factory method
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <param name="externalId"></param>
        /// <param name="accountTypeEnum"></param>
        /// <returns></returns>
        public static Customer Create(string email, string password, string externalId = "", AccountTypeEnum accountTypeEnum = AccountTypeEnum.Normal)
        {
            var customer = new Customer
                {
                    Email = email,
                    ExternalId = externalId,
                    AccountType = accountTypeEnum
                };

            customer.SetNewPassword(password);

            return customer;
        }

        public virtual string Firstname { get; set; }

        public virtual string Lastname { get; set; }

        public virtual GenderTypeEnum Gender { get; set; }

        public virtual DateTime? Birthday { get; set; }

        public virtual string PhoneNumber { get; set; }

        public virtual string Country { get; set; }

        public virtual string Email { get; set; }

        public virtual string Password { get; protected internal set; }

        public virtual RolesEnum Role { get; protected internal set; }

        public virtual AccountTypeEnum AccountType { get; protected internal set; }

        /// <summary>
        /// Used for connecting with other forms of Authentication services like Facebook, Twitter etc.
        /// </summary>
        public virtual string ExternalId { get; protected internal set; }

        public virtual bool IsBonusMember { get; set; }

        public virtual bool WantNewsletter { get; set; }        

        public virtual string GetFullname()
        {
            return Firstname + " " + Lastname;
        }

        public virtual bool SetNewPassword(string password)
        {
            var hashPassword = PasswordHelper.HashPassword(password);

            if (string.Compare(Password, hashPassword) != 0)
            {
                Password = hashPassword;
                return true;
            }

            return false;
        }


        protected override void Validate()
        {
            if(AccountType != AccountTypeEnum.Normal && ExternalId.IsNullOrEmpty())
            {
                throw new ApplicationException("External Id can not be null or empty.");
            }
        }
    }
}
