using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EcoHotels.Core.Common.Enum;
using EcoHotels.Core.Domain.Models.Commerce;
using EcoHotels.Core.Domain.Models.Localization;
using EcoHotels.Core.Domain.Models.Media;
using EcoHotels.Core.Domain.Models.Property;
using EcoHotels.Core.Domain.Models.Security;

namespace EcoHotels.Web.UI.Areas.Admin.Models
{
    public class MenuItemModel
    {
        public MenuItemModel(int id, string name, bool isSelected)
        {
            Id = id;
            Name = name;
            IsSelected = isSelected;
        }

        [ReadOnly(true)]
        public int Id { get; set; }

        [ReadOnly(true)]
        public string Name { get; set; }

        [ReadOnly(true)]
        public bool IsSelected { get; set; }  
    }

    /// <summary>
    /// Top level navigation
    /// </summary>
    public class MainMenuModel
    {
        public MainMenuModel(string page, int selectedHotelId, IEnumerable<Hotel> hotels)
        {
            Page = page;

            //TODO: Check if user can switch hotels, if not show the name

            SelectedHotelId = selectedHotelId;
            Hotelname = (selectedHotelId != 0)? hotels.First(x => x.Id == selectedHotelId).Name : string.Empty;
            Hotels = hotels.Select(x => new MenuItemModel(x.Id, x.Name, false));
        }

        [ReadOnly(true)]
        public string Page { get; set; }

        [ReadOnly(true)]
        public string Hotelname { get; set; }

        [ReadOnly(true)]
        public int SelectedHotelId { get; set; }

        [ReadOnly(true)]
        public IEnumerable<MenuItemModel> Hotels { get; set; }
    }

    /// <summary>
    /// Dashboard section navigation.
    /// </summary>
    public class DashboardSectionModel
    {
        public DashboardSectionModel(string page)
        {
            Page = page;
        }

        [ReadOnly(true)]
        public string Page { get; set; }
    }

    /// <summary>
    /// Media section navigation.
    /// </summary>
    public class MediaSectionModel
    {
        public MediaSectionModel(int assetCategoryId, IEnumerable<AssetCategory> assetCategories, string page)
        {
            Page = page;
            Items = assetCategories.Select(x => new MenuItemModel(x.Id, x.Name, x.Id == assetCategoryId));
        }

        [ReadOnly(true)]
        public string Page { get; set; }

        [ReadOnly(true)]
        public IEnumerable<MenuItemModel> Items { get; set; }
    }

    /// <summary>
    /// Rate section navigation.
    /// </summary>
    public class RateSectionModel
    {
        public RateSectionModel(int rateCategoryId, IEnumerable<RateCategory> rateCategories, string page)
        {
            Page = page;
            Items = rateCategories.Select(x => new MenuItemModel(x.Id, x.Name, x.Id == rateCategoryId));    
        }

        [ReadOnly(true)]
        public string Page { get; set; }

        [ReadOnly(true)]
        public IEnumerable<MenuItemModel> Items { get; set; }
    }

    /// <summary>
    /// Rate section navigation.
    /// </summary>
    public class AddonSectionModel
    {
        public AddonSectionModel(int selectedAddonId, IEnumerable<Addon> addons, string page)
        {
            Page = page;
            Items = addons.Select(x => new MenuItemModel(x.Id, x.Name.GetText(LanguageTypeEnum.English), x.Id == selectedAddonId));
        }

        [ReadOnly(true)]
        public string Page { get; set; }

        [ReadOnly(true)]
        public IEnumerable<MenuItemModel> Items { get; set; }
    }

    /// <summary>
    /// Used for creating the sidebar navigation in the settings section.
    /// </summary>
    public class SettingSectionModel
    {
        public SettingSectionModel(int selectedUserId, IEnumerable<User> users, string page)
        {
            Page = page;
            Items = users.Select(x => new MenuItemModel(x.Id, x.GetFullname(), x.Id == selectedUserId));
        }

        [ReadOnly(true)]
        public string Page { get; set; }

        [ReadOnly(true)]
        public IEnumerable<MenuItemModel> Items { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class HotelSectionModel
    {
        public HotelSectionModel(int hotelId, int selectedRoomTypeId, IEnumerable<RoomType> roomTypes, string page)
        {
            HotelId = hotelId;
            
            Page = page;
            Items = roomTypes.Select(x => new MenuItemModel(x.Id, x.Name.GetText(LanguageTypeEnum.English), x.Id == selectedRoomTypeId));
        }

        [ReadOnly(true)]
        public int HotelId { get; set; }

        [ReadOnly(true)]
        public Guid RoomTypeId { get; set; }

        [ReadOnly(true)]
        public string Page { get; set; }

        [ReadOnly(true)]
        public IEnumerable<MenuItemModel> Items { get; set; }
    }

}
