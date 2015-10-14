using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using EcoHotels.Core.Domain.Models.Security;

namespace EcoHotels.Web.UI.Areas.Admin.Models
{
    public class UserModel
    {     
        /// <summary>
        /// Required.
        /// </summary>
        public UserModel(){}

        /// <summary>
        /// Used for Create
        /// </summary>
        /// <param name="users"></param>
        public UserModel(IEnumerable<User> users)
        {
            //MenuItems = users.Select(x => new MenuItemModel(x.Id, x.Firstname + " " + x.Lastname, false))
            //    .OrderBy(x => x.Name)
            //    .ToList();

        }

        /// <summary>
        /// Used for Edit
        /// </summary>
        /// <param name="selectedUserId"></param>
        /// <param name="users"></param>
        public UserModel(int selectedUserId, IEnumerable<User> users)
        {
            MenuItems = users.Select(x => new MenuItemModel(x.Id, x.Firstname + " " + x.Lastname, x.Id == selectedUserId))
                .OrderBy(x => x.Name)
                .ToList();

            var selectedUser = users.Where(x => x.Id == selectedUserId).First();
            Id = selectedUser.Id;
            Firstname = selectedUser.Firstname;
            Lastname = selectedUser.Lastname;
            Email = selectedUser.Email;
            Password = "";

            IsActive = selectedUser.IsActive;
        }

        [ReadOnly(true)]
        public List<MenuItemModel> MenuItems { get; set; }

        [Required]
        public int Id { get; set; }

        [Required]
        public string Firstname { get; set; }

        [Required]
        public string Lastname { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        //[Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        public bool IsActive { get; set; }
    }


}