using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using EcoHotels.Core.Common.Enum;
using EcoHotels.Core.Domain.Models;
using EcoHotels.Core.Domain.Models.Localization;
using EcoHotels.Core.Domain.Models.Media;
using EcoHotels.Core.Domain.Models.Property;
using EcoHotels.Web.Core.Models;

namespace EcoHotels.Web.UI.Areas.Admin.Models.Property
{
    public class CreateRoomTypelModel
    {
        public CreateRoomTypelModel() {}

        public CreateRoomTypelModel(IEnumerable<Language> languages, IEnumerable<Asset> assets, string currencySymbol)
        {
            SelectedLanguageId = LanguageTypeEnum.English;

            Name = LocalizedValueModel.Create(languages);
            Description = LocalizedValueModel.Create(languages);
            CurrencySymbol = currencySymbol;

            Assets = new List<RoomTypeSelectedAsset>();
            MediaAssets = assets.Select(x => new MediaSelectorAsset(x.Id, x.Name)).ToList();
        }

        public LanguageTypeEnum SelectedLanguageId { get; set; }

        public List<LocalizedValueModel> Name { get; set; }

        public List<LocalizedValueModel> Description { get; set; }

        public int Capacity { get; set; }

        public int Size { get; set; }

        public int PhysicalRooms { get; set; }

        public bool Smoking { get; set; }

        public decimal RackRate { get; set; }

        public string CurrencySymbol { get; set; }

        public List<RoomTypeSelectedAsset> Assets { get; set; }

        [ReadOnly(true)]
        public IEnumerable<MediaSelectorAsset> MediaAssets { get; set; }
    }

    public class EditRoomTypeModel
    {
        public EditRoomTypeModel(){}

        public EditRoomTypeModel(RoomType roomType, IEnumerable<Language> languages, IEnumerable<Asset> assets, string currencySymbol)
        {
            RoomTypeId = roomType.Id;
            SelectedLanguageId = LanguageTypeEnum.English;

            Name = LocalizedValueModel.Create(languages, roomType.Name).ToList();
            Description = LocalizedValueModel.Create(languages, roomType.Description).ToList();
            Capacity = roomType.Capacity;
            Size = roomType.Size;
            PhysicalRooms = roomType.PhysicalRooms;
            Smoking = roomType.IsSmoking;
            RackRate = roomType.RackRate;
            CurrencySymbol = currencySymbol;

            Assets = roomType.Assets.Select(x => new RoomTypeSelectedAsset(x)).ToList();
            MediaAssets = assets.Select(x => new MediaSelectorAsset(x.Id, x.Name)).ToList();
        }

        public int RoomTypeId { get; set; }

        public LanguageTypeEnum SelectedLanguageId { get; set; }

        public List<LocalizedValueModel> Name { get; set; }

        public List<LocalizedValueModel> Description { get; set; }

        public int Capacity { get; set; }

        public int Size { get; set; }

        public int PhysicalRooms { get; set; }

        public bool Smoking { get; set; }

        public decimal RackRate { get; set; }

        public string CurrencySymbol { get; set; }


        public List<RoomTypeSelectedAsset> Assets { get; set; }

        [ReadOnly(true)]
        public IEnumerable<MediaSelectorAsset> MediaAssets { get; set; }
    }




    public class RoomTypeModel
    {
        /// <summary>
        /// Required.
        /// </summary>
        public RoomTypeModel() { }

        /// <summary>
        /// Used for CREATE
        /// </summary>
        /// <param name="roomTypes"></param>
        /// <param name="languages"></param>
        /// <param name="selectedLanguageId"></param>
        public RoomTypeModel(int hotelId, IEnumerable<RoomType> roomTypes, IEnumerable<Language> languages, LanguageTypeEnum selectedLanguageId, string currencySymbol, IEnumerable<Asset> assets)
        {
            HotelId = hotelId;

            SelectedLanguageId = selectedLanguageId;
            AvailableLanguages = languages.Select(x => new LocalizedValueModel(x.Id, x.Name, x.Name)).ToList();

            Name = LocalizedValueModel.Create(languages).ToList();
            Description = LocalizedValueModel.Create(languages).ToList();
            CurrencySymbol = currencySymbol;

            Assets = new List<RoomTypeSelectedAsset>();
            MediaAssets = assets.Select(x => new MediaSelectorAsset(x.Id, x.Name)).ToList();

            MenuItems = roomTypes.Select(x => new MenuItemModel(x.Id, x.Name.GetText(selectedLanguageId), false))
                .OrderBy(x => x.Name)
                .ToList();
        }

        /// <summary>
        /// Used for EDIT
        /// </summary>
        /// <param name="selectedId"></param>
        /// <param name="roomTypes"></param>
        /// <param name="languages"></param>
        /// <param name="selectedLanguageId"></param>
        public RoomTypeModel(int hotelId, int selectedId, IEnumerable<RoomType> roomTypes, IEnumerable<Language> languages, LanguageTypeEnum selectedLanguageId, string currencySymbol, IEnumerable<Asset> assets)
        {
            var selectedRoomType = roomTypes.First(x => x.Id == selectedId);

            HotelId = selectedId;
            
            SelectedLanguageId = selectedLanguageId;
            AvailableLanguages = languages.Select(x => new LocalizedValueModel(x.Id, x.Name, x.Name)).ToList();

            Name = LocalizedValueModel.Create(languages, selectedRoomType.Name).ToList();
            Description = LocalizedValueModel.Create(languages, selectedRoomType.Description).ToList(); 

            Capacity = selectedRoomType.Capacity;
            Size = selectedRoomType.Size;
            PhysicalRooms = selectedRoomType.PhysicalRooms;
            Smoking = selectedRoomType.IsSmoking;
            RackRate = selectedRoomType.RackRate;
            CurrencySymbol = currencySymbol;

            Assets = selectedRoomType.Assets.Select(x => new RoomTypeSelectedAsset(x)).ToList();
            MediaAssets = assets.Select(x => new MediaSelectorAsset(x.Id, x.Name)).ToList();

            MenuItems = roomTypes.Select(x => new MenuItemModel(x.Id, x.Name.GetText(selectedLanguageId), x.Id == selectedId))
                .OrderBy(x => x.Name)
                .ToList();
        }

        public int HotelId { get; set; }

        public List<LocalizedValueModel> Name { get; set; }

        public List<LocalizedValueModel> Description { get; set; }

        public int Capacity { get; set; }

        public int Size { get; set; }

        public int PhysicalRooms { get; set; }

        public bool Smoking { get; set; }

        public decimal RackRate { get; set; }

        public string CurrencySymbol { get; set; }


        public List<RoomTypeSelectedAsset> Assets { get; set; }

        public List<MediaSelectorAsset> MediaAssets { get; set; }


        public LanguageTypeEnum SelectedLanguageId { get; set; }

        public List<LocalizedValueModel> AvailableLanguages { get; set; }

        public List<MenuItemModel> MenuItems { get; set; }
    }

    public class RoomTypeAmenityModel
    {
        /// <summary>
        /// Required.
        /// </summary>
        public RoomTypeAmenityModel(){}

        public RoomTypeAmenityModel(int id, string name, bool isSelected)
        {
            Id = id;
            Name = name;
            IsSelected = isSelected;
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public bool IsSelected { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class RoomTypeSelectedAsset
    {
        /// <summary>
        /// Required.
        /// </summary>
        public RoomTypeSelectedAsset(){}

        public RoomTypeSelectedAsset(Asset asset)
        {
            Id = asset.Id;
            Name = asset.Name;
            TopX = asset.TopX;
            TopY = asset.TopY;
            BottomX = asset.BottomX;
            BottomY = asset.BottomY;
            CropXUnits = asset.CropXUnits;
            CropYUnits = asset.CropYUnits;
            CroppingQuery = asset.GenerateCroppingQuery();
        }

        [Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public int TopX { get; set; }

        [Required]
        public int TopY { get; set; }

        [Required]
        public int BottomX { get; set; }

        [Required]
        public int BottomY { get; set; }

        [Required]
        public int CropXUnits { get; set; }

        [Required]
        public int CropYUnits { get; set; }

        [ReadOnly(true)]
        public string CroppingQuery { get; set; }
    }

    public class MediaSelectorAsset
    {
        /// <summary>
        /// Required.
        /// </summary>
        public MediaSelectorAsset(){}

        public MediaSelectorAsset(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public int Id { get; set; }

        public string Name { get; set; }
    }

}