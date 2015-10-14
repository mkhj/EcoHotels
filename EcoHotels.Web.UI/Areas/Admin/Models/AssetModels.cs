using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using EcoHotels.Core.Domain.Models.Media;

namespace EcoHotels.Web.UI.Areas.Admin.Models
{
    public class AssetCategoryModel
    {
        /// <summary>
        /// Required.
        /// </summary>
        public AssetCategoryModel() { }

        /// <summary>
        /// Used for ALL.
        /// </summary>
        /// <param name="categories"></param>
        public AssetCategoryModel(IEnumerable<AssetCategory> categories, IEnumerable<Asset> assets)
        {            
            Items = assets.Select(x => new AssetModel(x));

            MenuItems = categories.Select(x => new MenuItemModel(x.Id, x.Name, false))
                            .OrderBy(x => x.Name)
                            .ToList();


        }

        /// <summary>
        /// Used for Create.
        /// </summary>
        /// <param name="categories"></param>
        public AssetCategoryModel(IEnumerable<AssetCategory> categories)
        {
            MenuItems = categories.Select(x => new MenuItemModel(x.Id, x.Name, false))
                            .OrderBy(x => x.Name)
                            .ToList();
        }

        /// <summary>
        /// Used for Edit.
        /// </summary>
        /// <param name="categories"></param>
        public AssetCategoryModel(int selectedId, IEnumerable<AssetCategory> categories)
        {
            var selectedCategory = categories.First(x => x.Id == selectedId);

            Id = selectedCategory.Id;
            Name = selectedCategory.Name;

            MenuItems = categories.Select(x => new MenuItemModel(x.Id, x.Name, x.Id == selectedId))
                            .OrderBy(x => x.Name)
                            .ToList();


        }

        /// <summary>
        /// Used for Content showing.
        /// </summary>
        /// <param name="selectedId"></param>
        /// <param name="categories"></param>
        /// <param name="assets"></param>
        public AssetCategoryModel(int selectedId, IEnumerable<AssetCategory> categories, IEnumerable<Asset> assets)
        {
            var selectedCategory = categories.First(x => x.Id == selectedId);

            Id = selectedCategory.Id;
            Name = selectedCategory.Name;

            Items = assets.Select(x => new AssetModel(x));

            MenuItems = categories.Select(x => new MenuItemModel(x.Id, x.Name, x.Id == selectedId))
                            .OrderBy(x => x.Name)
                            .ToList();


        }

        public int Id { get; set; }

        [Required, StringLength(50)]
        public string Name { get; set; }

        [ReadOnly(true)]
        public List<MenuItemModel> MenuItems { get; set; }

        [ReadOnly(true)]
        public IEnumerable<AssetModel> Items { get; set; }
    }

    public class AssetModel
    {
        public AssetModel(Asset asset)
        {
            Id = asset.Id;
            Name = asset.Name;
        }

        [ReadOnly(true)]
        public int Id { get; set; }

        [ReadOnly(true)]
        public string Name { get; set; }
    }
}