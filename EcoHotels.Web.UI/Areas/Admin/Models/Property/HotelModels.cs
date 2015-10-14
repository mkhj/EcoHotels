using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using EcoHotels.Core.Common.Enum;
using EcoHotels.Core.Domain.Models;
using EcoHotels.Core.Domain.Models.Localization;
using EcoHotels.Core.Domain.Models.Media;
using EcoHotels.Core.Domain.Models.Property;
using EcoHotels.Core.Domain.Value_objects;
using EcoHotels.Web.Core.Models;

namespace EcoHotels.Web.UI.Areas.Admin.Models.Property
{
public class CreateHotelModel
{
    public CreateHotelModel(){}

    public CreateHotelModel(IEnumerable<Country> countries, IEnumerable<Currency> currencies)
    {
        AvailableCountries = countries.Select(x => new SelectListItem { Selected = false, Text = x.Name, Value = x.Name }).ToList();
        AvailableCountries.Insert(0, new SelectListItem { Selected = false, Text = string.Empty, Value = string.Empty });

        currencies.OrderBy(x => x.Name);

        AvailableCurrencies = currencies.Select(x => new SelectListItem { Selected = SelectedCurrencyId == x.Id, Text = x.Name, Value = x.Id.ToString() }).ToList();
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

    [DisplayFormat(ConvertEmptyStringToNull = false)]
    public string Fax { get; set; }

    [Required]
    public string Email { get; set; }

    [DisplayFormat(ConvertEmptyStringToNull = false)]
    public string WWW { get; set; }

    [Required]
    public string VatNo { get; set; }

    [Required]
    public string SelectedCountryName { get; set; }

    [ReadOnly(true)]
    public List<SelectListItem> AvailableCountries { get; set; }

    [Required]
    public int SelectedCurrencyId { get; set; }

    [ReadOnly(true)]
    public List<SelectListItem> AvailableCurrencies { get; set; }
}

    /// <summary>
    /// 
    /// </summary>
    public class EditHotelInfoModel
    {
        public EditHotelInfoModel() { }

        public EditHotelInfoModel(Hotel hotel, IEnumerable<Country> countries)
        {
            Address1 = hotel.Address1;
            Address2 = hotel.Address2;
            City = hotel.City;
            SelectedCountryName = hotel.Country;
            Email = hotel.Email;
            Fax = hotel.Fax;
            Name = hotel.Name;
            Phone = hotel.Phone;
            VatNo = hotel.VatNo;
            WWW = hotel.WWW;
            Zipcode = hotel.Zipcode;

            AvailableCountries = countries.Select(x => new SelectListItem { Selected = x.Name == hotel.Country, Text = x.Name, Value = x.Name }).ToList();
            AvailableCountries.Insert(0, new SelectListItem { Selected = false, Text = string.Empty, Value = string.Empty });
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

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Fax { get; set; }

        [Required]
        public string Email { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string WWW { get; set; }

        [Required]
        public string VatNo { get; set; }

        [Required]
        public string SelectedCountryName { get; set; }

        [ReadOnly(true)]
        public List<SelectListItem> AvailableCountries { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class EditHotelPhotoModel
    {
        public EditHotelPhotoModel(){}

        public EditHotelPhotoModel(IEnumerable<Asset> selectedAssets, IEnumerable<Asset> allAssets)
        {
            Assets = selectedAssets.Select(x => new RoomTypeSelectedAsset(x)).ToList();
            MediaAssets = allAssets.Select(x => new MediaSelectorAsset(x.Id, x.Name)).ToList();
        }

        public List<RoomTypeSelectedAsset> Assets { get; set; }

        [ReadOnly(true)]
        public List<MediaSelectorAsset> MediaAssets { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class EditHotelDescriptionModel
    {
        public EditHotelDescriptionModel(){}

        public EditHotelDescriptionModel(Hotel hotel, IEnumerable<Amenity> amenities, IEnumerable<Language> languages)
        {
            SelectedLanguageId = LanguageTypeEnum.English;

            CheckIn = hotel.CheckIn;
            CheckInAMPM = hotel.CheckInAMPM;
            CheckOut = hotel.CheckOut;
            CheckOutAMPM = hotel.CheckOutAMPM;

            MinimumStay = hotel.MinimumStay;
            MinimumCheckInAge = hotel.MinimumCheckInAge;

            Hours = Enumerable.Range(1, 12).Select(x => new SelectListItem { Text = x.ToString(), Value = x.ToString() }).ToList();
            AMPM = new List<SelectListItem>
                        {
                            new SelectListItem{ Text = "AM", Value = "AM" }, 
                            new SelectListItem{ Text = "PM", Value = "PM" }
                        };

            About = LocalizedValueModel.Create(languages, hotel.About).ToList();

            Amenities = amenities.Select(x => new EditHotelAmenityItem(x, hotel.Amenities.Any(a => a.Id == x.Id))).ToList();
            
            #region - Categories -

            var categories = HotelCategories.GetList();

            SelectedCategoryOne = (int) hotel.CategoryOne;
            SelectedCategoryThree = (int)hotel.CategoryThree;
            SelectedCategoryTwo = (int)hotel.CategoryTwo;

            Categories = categories.Select(x => new SelectListItem { Selected = SelectedCategoryOne == x.Key, Text = x.Value, Value = x.Key.ToString() }).ToList();
            Categories.Insert(0, new SelectListItem { Selected = false, Text = "Choose..", Value = "" });          

            #endregion

            #region - Bullet Points -

            WhatWeLove = hotel.Descriptions.Where(x => x.Type == HotelBulletPointEnum.WhatWeLove)
                .OrderBy(x => x.OrderId)
                .Select(x => new EditHotelDescriptionBulletPoint(x, languages)).ToList();

            WhatYouNeedToKnow = hotel.Descriptions.Where(x => x.Type == HotelBulletPointEnum.WhatYouNeedToKnow)
                .OrderBy(x => x.OrderId)
                .Select(x => new EditHotelDescriptionBulletPoint(x, languages)).ToList();

            #endregion
        }

        [ReadOnly(true)]
        public LanguageTypeEnum SelectedLanguageId { get; set; }
        
        #region - Check In & Check Out Details -

        [Required, Range(1, 12, ErrorMessage = "Check In range is not valid")]
        public int CheckIn { get; set; }

        [Required]
        public string CheckInAMPM { get; set; }

        [Required, Range(1, 12, ErrorMessage = "Check Out range is not valid")]
        public int CheckOut { get; set; }

        [ReadOnly(true)]
        public List<SelectListItem> Hours { get; set; }

        [ReadOnly(true)]
        public List<SelectListItem> AMPM { get; set; }

        [Required]
        public string CheckOutAMPM { get; set; }

        #endregion

        [Required, Range(1, 356, ErrorMessage = "Minimum stay range is not valid. It should be between 1 and 356")]
        public int MinimumStay { get; set; }

        [Required, Range(1, 100, ErrorMessage = "Minimum stay range is not valid. It should be between 1 and 100")]
        public int MinimumCheckInAge { get; set; }

        #region - Descriptions -

        public List<LocalizedValueModel> About { get; set; }

        public List<EditHotelDescriptionBulletPoint> WhatWeLove { get; set; }

        public List<EditHotelDescriptionBulletPoint> WhatYouNeedToKnow { get; set; }

        #endregion
        
        public int SelectedCategoryOne { get; set; }

        public int SelectedCategoryTwo { get; set; }

        public int SelectedCategoryThree { get; set; }

        public List<EditHotelAmenityItem> Amenities { get; set; }

        [ReadOnly(true)]
        public List<SelectListItem> Categories { get; set; }
    }

    public class EditHotelAmenityItem
    {
        public EditHotelAmenityItem(){}

        public EditHotelAmenityItem(Amenity amenity, bool isSelected)
        {
            Id = amenity.Id;
            Name = amenity.Name;
            IsSelected = isSelected;
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public bool IsSelected { get; set; }
    }

    public class EditHotelDescriptionBulletPoint
    {
        public EditHotelDescriptionBulletPoint(){}

        public EditHotelDescriptionBulletPoint(HotelBulletPoint bulletPoint, IEnumerable<Language> languages)
        {
            Id = bulletPoint.Id;
            Text = LocalizedValueModel.Create(languages, bulletPoint.Text).ToList();
        }

        public int Id { get; set; }

        public List<LocalizedValueModel> Text { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class EditHotelPolicyModel
    {
        public EditHotelPolicyModel() {}

        public EditHotelPolicyModel(Hotel hotel, IEnumerable<Language> languages)
        {
            SelectedLanguageId = LanguageTypeEnum.English;



            Cancellation = LocalizedValueModel.Create(languages, hotel.CancellationPolicy).ToList();
            Payment = LocalizedValueModel.Create(languages, hotel.PaymentPolicy).ToList();
        }

        [ReadOnly(true)]
        public LanguageTypeEnum SelectedLanguageId { get; set; }

        public List<LocalizedValueModel> Cancellation { get; set; }

        public List<LocalizedValueModel> Payment { get; set; }

    }

    /// <summary>
    /// 
    /// </summary>
    public class EditHotelCurrenciesModel
    {
        public EditHotelCurrenciesModel() {}

        public EditHotelCurrenciesModel(IEnumerable<Hotel> hotels, IEnumerable<Currency> currencies)
        {
            Items = hotels.Select(x => new EditHotelCurrencyItemModel(x, currencies)).ToList();

            
        }

        public List<EditHotelCurrencyItemModel> Items { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class EditHotelCurrencyItemModel
    {
        public EditHotelCurrencyItemModel(){}

        public EditHotelCurrencyItemModel(Hotel hotel, IEnumerable<Currency> currencies)
        {
            HotelId = hotel.Id;
            HotelName = hotel.Name;
         
            SelectedCurrencyId = hotel.Currency.Id;
            AvailableCurrencies = currencies.Select(x => new SelectListItem { Selected = SelectedCurrencyId == x.Id, Text = x.Name, Value = x.Id.ToString() }).ToList();
        }

        [Required]
        public int HotelId { get; set; }

        [ReadOnly(true)]
        public string HotelName { get; set; }

        [Required]
        public int SelectedCurrencyId { get; set; }

        [ReadOnly(true)]
        public List<SelectListItem> AvailableCurrencies { get; set; }
    }

}