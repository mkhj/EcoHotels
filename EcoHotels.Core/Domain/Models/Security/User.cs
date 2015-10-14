using System;
using System.Collections.Generic;
using System.Linq;
using EcoHotels.Core.Domain.Models.Property;
using EcoHotels.Core.Helpers;
using EcoHotels.Core.Infrastructure;

namespace EcoHotels.Core.Domain.Models.Security
{
    public enum RolesEnum
    {
        Backend = 10,
        Supporter = 20,

        Organization = 110,
        GeneralManager = 120,
        MarketingManager = 130,

        Operations = 160,

        Customer = 200,
        Memeber = 210,

        None = 0
    }

    [Serializable]
    public class User : EntityBase<User> 
    {
        protected User()
        {
            Email = string.Empty;
            Password = string.Empty;
            Firstname = string.Empty;
            Lastname = string.Empty;
            Role = RolesEnum.None;
            IsActive = false;
        }

        /// <summary>
        /// Factory method.
        /// </summary>
        /// <param name="organization"></param>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        public static User Create(Organization organization, string email, string password, RolesEnum role)
        {
            var user = new User
            {
                Organization = organization,
                Email = email,
                Role = role,
            };

            user.SetNewPassword(password);
            return user;
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual Organization Organization { get; protected internal set; }

        /// <summary>
        /// Login ID
        /// </summary>
        public virtual string Email { get; set; }

        public virtual string Password { get; protected internal set; }

        public virtual string Firstname { get; set; }

        public virtual string Lastname { get; set; }

        /// <summary>
        /// Root,
        /// Supporter,
        /// HotelManager,
        /// HotelAssistant,
        /// Customer,
        /// Anonymous,
        /// </summary>
        public virtual RolesEnum Role { get; set; }

        public virtual bool IsActive { get; set; }


        public virtual string GetFullname()
        {
            return Firstname + " " + Lastname;
        }

        public virtual bool SetNewPassword(string password)
        {
            var hashPassword = PasswordHelper.HashPassword(password);

            //var encrypted = CryptoHelper.Encrypt(password);
            if (string.Compare(Password, hashPassword) != 0)
            {
                Password = hashPassword;
                return true;
            }

            return false;
        }


        protected override void Validate()
        {
            
        }
    }
}
