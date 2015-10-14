using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EcoHotels.Core.Domain.Models;
using EcoHotels.Core.Domain.Models.Property;
using EcoHotels.Core.Domain.Value_objects;

namespace EcoHotels.Web.UI.Areas.Admin.Models
{
    public class EditOrganizationModel
    {
        public EditOrganizationModel(){}   

        public EditOrganizationModel(Organization organization, IEnumerable<Country> countries)
        {
            Name = organization.Name;
            Address1 = organization.Address1;
            Address2 = organization.Address2;
            Zipcode = organization.Zipcode;
            City = organization.City;
            Phone = organization.Phone;
            Email = organization.Email;
            VatNo = organization.VatNo;
            SelectedCountryName = organization.Country;

            AvailableCountries = countries.Select(x => new SelectListItem { Selected = x.Name == organization.Country, Text = x.Name, Value = x.Name }).ToList();
        }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Address1 { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Address2 { get; set; }

        [Required]
        public string Zipcode { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string Phone { get; set; }

        [Required]
        public string Email { get; set; }


        [Required]
        public string VatNo { get; set; }

        [Required]
        public string SelectedCountryName { get; set; }

        [ReadOnly(true)]
        public List<SelectListItem> AvailableCountries { get; set; }
    }
}