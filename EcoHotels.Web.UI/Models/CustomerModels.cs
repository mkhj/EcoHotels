using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using EcoHotels.Core.Common.Enum;
using EcoHotels.Core.Domain.Models;
using EcoHotels.Core.Domain.Models.Commerce;

namespace EcoHotels.Web.UI.Models
{
    public class EditCustomerInformationModel
    {
        public EditCustomerInformationModel() {}

        public EditCustomerInformationModel(Customer customer, IEnumerable<Country> countries)
        {
            Firstname = customer.Firstname;
            Lastname = customer.Lastname;
            Email = customer.Email;
            //Birthday = customer.Birthday ?? DateTime.MinValue;
            SelectedGenderType = (int) customer.Gender;

            AvailableCountries = countries.Select(x => new SelectListItem { Selected = x.Name.Trim() == customer.Country.Trim(), Text = x.Name.Trim(), Value = x.Name.Trim() }).ToList();
            AvailableCountries.Insert(0, new SelectListItem { Selected = false, Text = "I'd rather not say", Value = string.Empty });

            Genders = new List<SelectListItem>();
            Genders.Add(new SelectListItem { Selected = false, Text = "I'd rather not say", Value = "0" });
            Genders.Add(new SelectListItem { Selected = false, Text = GenderTypeEnum.Male.ToString(), Value = ((int)GenderTypeEnum.Male).ToString() });
            Genders.Add(new SelectListItem { Selected = false, Text = GenderTypeEnum.Female.ToString(), Value = ((int)GenderTypeEnum.Female).ToString() });

            SelectedBirthDate = (customer.Birthday.HasValue) ? customer.Birthday.Value.Day : 0;
            SelectedBirthMonth = (customer.Birthday.HasValue) ? customer.Birthday.Value.Month : 0;
            SelectedBirthYear = (customer.Birthday.HasValue) ? customer.Birthday.Value.Year : 1970;

            BirthDate = new List<SelectListItem>{ new SelectListItem { Selected = false, Text = "Day", Value = "0" } };
            BirthDate.AddRange(Enumerable.Range(1, 31).Select(x => new SelectListItem { Selected = x == SelectedBirthDate, Text = x.ToString(), Value = x.ToString() }).ToList());

            var months = new List<string> { "Month", "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };
            BirthMonth = new List<SelectListItem>();
            months.ForEach(x => BirthMonth.Add(new SelectListItem { Selected = months.IndexOf(x) == SelectedBirthMonth, Text = x, Value = months.IndexOf(x).ToString() }));
        }

        public bool HasValidBirthday()
        {
            var date = string.Format("{0}/{1}/{2}", SelectedBirthMonth, SelectedBirthDate, SelectedBirthYear);
            var birthdate = DateTime.MinValue;
            return DateTime.TryParse(date, CultureInfo.CreateSpecificCulture("en-US"), DateTimeStyles.None, out birthdate);
        }

        public DateTime GetBirthday()
        {
            var date = string.Format("{0}/{1}/{2}", SelectedBirthMonth, SelectedBirthDate, SelectedBirthYear);
            var birthdate = DateTime.MinValue;
            DateTime.TryParse(date, CultureInfo.CreateSpecificCulture("en-US"), DateTimeStyles.None, out birthdate);
            return birthdate;
        }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Firstname { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Lastname { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }


        #region - Birthday -

        public int SelectedBirthMonth { get; set; }

        [ReadOnly(true)]
        public List<SelectListItem> BirthMonth { get; set; }

        public int SelectedBirthDate { get; set; }

        [ReadOnly(true)]
        public List<SelectListItem> BirthDate { get; set; }

        public int SelectedBirthYear { get; set; }

        #endregion

        #region - Gender -

        public int SelectedGenderType { get; set; }

        [ReadOnly(true)]
        public List<SelectListItem> Genders { get; set; }

        #endregion
        
        #region - Country -

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string SelectedCountryName { get; set; }

        [ReadOnly(true)]
        public List<SelectListItem> AvailableCountries { get; set; }

        #endregion
    }

    public class EditCustomerPasswordModel
    {
        [Required]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("NewPassword")]
        public string ConfirmPassword { get; set; }
    }

    //public class EditCustomerBillingInformation
    //{
    //    public EditCustomerBillingInformation(){}

    //    public EditCustomerBillingInformation(object model)
    //    {
            
    //    }

    //    public string CreditCardHolder { get; set; }

    //    public string CreditCardNumber { get; set; }

    //    public string CreditCardExpirationMonth { get; set; }

    //    public string CreditCardExpirationYear { get; set; }

    //    public string CreditCardSecurityCode { get; set; }


    //    public string BillingCountry { get; set; }

    //    public string BillingAddress1 { get; set; }

    //    public string BillingAddress2 { get; set; }

    //    public string BillingCity { get; set; }

    //    public string BillingState { get; set; }

    //    public string BillingZIPCode { get; set; }
    //}

}