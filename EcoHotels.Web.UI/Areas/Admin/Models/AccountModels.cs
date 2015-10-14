using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using EcoHotels.Core.Domain.Models.Security;

namespace EcoHotels.Web.UI.Areas.Admin.Models
{
    public class AccountModel
    {
        /// <summary>
        /// Requied.
        /// </summary>
        public AccountModel(){}

        public AccountModel(List<User> users)
        {
            MenuItems = users.ConvertAll(x => new AccountMenuItemModel(x.Firstname, false));
        }

        public AccountModel(int selectedId, List<User> users)
        {
            var selectedUser = users.Find(x => x.Id == selectedId);

            Id = selectedUser.Id;
            Firstname = selectedUser.Firstname;
            Lastname = selectedUser.Lastname;
            Email = selectedUser.Email;
            MenuItems = users.ConvertAll(x => new AccountMenuItemModel(x.Firstname, x.Id == selectedId));
        }

        public int Id { get; set; }

        [Required]
        public string Firstname { get; set; }

        [Required]
        public string Lastname { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        public string Password { get; set; }

        public string RetypedPassword { get; set; }

        public bool IsActive { get; set; }

        public List<AccountMenuItemModel> MenuItems { get; set; }

        public bool HasChangedPassword()
        {
            // Defensive programming

            if(Password.Trim() == string.Empty || RetypedPassword.Trim() == string.Empty)
            {
                return false;
            }

            return true;
        }

        public bool IsValidPassword()
        {
            if(HasChangedPassword() == false)
            {
                throw new ApplicationException();
            }

            return (string.Compare(Password.Trim(), RetypedPassword.Trim(), true) == 0);
        }
    }

    public class AccountMenuItemModel
    {
        public AccountMenuItemModel(string name, bool isSelected)
        {
            Name = name;
            IsSelected = isSelected;
        }


        public string Name { get; set; }

        public bool IsSelected { get; set; }
    }


    public class LogOnModel
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }

    public class ResetAccountModel
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
    }

    public class VerifyResetModel
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        public string Token { get; set; }

        [Required]
        public long Time { get; set; }

        public bool IsTimeValid()
        {
            return new DateTime(Time).AddMinutes(10) > DateTime.Now;
        }
    }
}