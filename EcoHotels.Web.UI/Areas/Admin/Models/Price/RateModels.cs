using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using EcoHotels.Core.Common.Enum;
using EcoHotels.Core.Domain.Models.Commerce;
using EcoHotels.Core.Domain.Models.Localization;
using EcoHotels.Core.Domain.Models.Property;

namespace EcoHotels.Web.UI.Areas.Admin.Models.Price
{
    public class CreateRateCategoryModel
    {
        public CreateRateCategoryModel(){}

        public CreateRateCategoryModel(IEnumerable<RoomType> roomTypes)
        {
            Items = roomTypes.Select(x => new CreateRateCategoryItemModel(x)).ToList();
        }

        [Required]
        public string Name { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Description { get; set; }

        [StringLength(6)]
        public string Color { get; set; }

        public List<CreateRateCategoryItemModel> Items { get; set; }
    }

    public class CreateRateCategoryItemModel
    {
        public CreateRateCategoryItemModel(){}

        public CreateRateCategoryItemModel(RoomType roomType)
        {
            RoomTypeId = roomType.Id;
            Name = roomType.Name.GetText(LanguageTypeEnum.English);
            Value = 0.0m;
            IsActive = true;
        }

        [Required]
        public int RoomTypeId { get; set; }

        [ReadOnly(true)]
        public string Name { get; set; }

        [Required]
        public decimal Value { get; set; }

        [Required]
        public bool IsActive { get; set; }
    }

    /// <summary>
    /// Edit ViewModel
    /// </summary>
    public class EditRateCategoryModel
    {
        public EditRateCategoryModel() { }

        public EditRateCategoryModel(RateCategory rateCategory, IEnumerable<RoomType> roomTypes)
        {
            Id = rateCategory.Id;
            Name = rateCategory.Name;
            Description = rateCategory.Description;
            Color = rateCategory.Color;
            Items = rateCategory.Items.Select(x => new EditRateCategoryItemModel(x, roomTypes)).ToList();
        }

        [Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [StringLength(6)]
        public string Color { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Description { get; set; }

        public List<EditRateCategoryItemModel> Items { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class EditRateCategoryItemModel
    {
        public EditRateCategoryItemModel() { }

        public EditRateCategoryItemModel(RatePrice ratePrice, IEnumerable<RoomType> roomTypes)
        {
            Id = ratePrice.Id;
            RoomTypeId = ratePrice.RoomTypeId;
            Name = roomTypes.First(x => x.Id == ratePrice.RoomTypeId).Name.GetText(LanguageTypeEnum.English);
            Value = ratePrice.Value;
            IsActive = ratePrice.IsActive;
        }

        [Required]
        public int Id { get; set; }

        [Required]
        public int RoomTypeId { get; set; }

        [ReadOnly(true)]
        public string Name { get; set; }

        [Required]
        public decimal Value { get; set; }

        [Required]
        public bool IsActive { get; set; }
    }


    public class RackRateModel
    {
        public RackRateModel(){}

        public RackRateModel(IEnumerable<RoomType> roomTypes)
        {
            Items = roomTypes.Select(x => new RackRateItemModel(x)).ToList();
        }

        public List<RackRateItemModel> Items { get; set; }
    }

    public class RackRateItemModel
    {
        public RackRateItemModel(){}

        public RackRateItemModel(RoomType roomType)
        {
            Name = roomType.Name.GetText(LanguageTypeEnum.English);
            RoomTypeId = roomType.Id;
            Value = roomType.RackRate;
        }

        [ReadOnly(true)]
        public string Name { get; set; }

        [Required]
        public int RoomTypeId { get; set; }

        [Required]
        public decimal Value { get; set; }
    }
}