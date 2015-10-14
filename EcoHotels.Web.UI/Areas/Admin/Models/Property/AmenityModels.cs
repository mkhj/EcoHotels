using System;
using System.Collections.Generic;
using System.Linq;
using EcoHotels.Core.Common.Enum;
using EcoHotels.Core.Domain.Models.Localization;
using EcoHotels.Core.Domain.Models.Property;
using EcoHotels.Web.Core.Models;

namespace EcoHotels.Web.UI.Areas.Admin.Models.Property
{
    public class AmenityModel
    {
        /// <summary>
        /// Required.
        /// </summary>
        public AmenityModel(){}

        public AmenityModel(IEnumerable<Amenity> amenities)
        {
            MenuItems = amenities.Select(x => new AmenityMenuItemModel(x.Id, x.Name, false))
                .OrderBy(x => x.Name)
                .ToList();
        }

        public AmenityModel(int selectedId, IEnumerable<Amenity> amenities)
        {
            var selectedAmenity = amenities.First(x => x.Id == selectedId);

            Id = selectedId;
            Name = selectedAmenity.Name;

            MenuItems = amenities.Select(x => new AmenityMenuItemModel(x.Id, x.Name, x.Id == selectedId))
                .OrderBy(x => x.Name)
                .ToList();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public List<AmenityMenuItemModel> MenuItems { get; set; }
    }

    public class AmenityMenuItemModel
    {
        public AmenityMenuItemModel(int id, string name, bool isSelected)
        {
            Id = id;
            Name = name;
            IsSelected = isSelected;
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public bool IsSelected { get; set; }
    }

}