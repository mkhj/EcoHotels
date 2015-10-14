using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using EcoHotels.Core.Common;
using EcoHotels.Core.Common.Enum;
using EcoHotels.Core.Domain.Models.Commerce;
using EcoHotels.Core.Domain.Models.Localization;
using EcoHotels.Web.Core.Models;

namespace EcoHotels.Web.UI.Areas.Admin.Models.Price
{
    public class AddonModel
    {
        /// <summary>
        /// Required.
        /// </summary>
        public AddonModel(){}

        /// <summary>
        /// Used for CREATE
        /// </summary>
        /// <param name="addons"></param>
        /// <param name="languages"></param>
        /// <param name="selectedLanguageId"></param>
        public AddonModel(IEnumerable<Addon> addons, IEnumerable<Language> languages, LanguageTypeEnum selectedLanguageId, string currency)
        {
            Name = LocalizedValueModel.Create(languages).ToList();
            Description = LocalizedValueModel.Create(languages).ToList();
            Currency = currency;

            SelectedCalculationRule = 0;
            SelectedPostingRhythm = 0;

            SelectedLanguageId = selectedLanguageId;
            AvailableLanguages = languages.Select(x => new LocalizedValueModel(x.Id, x.Name, x.Name)).ToList();

            MenuItems = addons.Select(x => new MenuItemModel(x.Id, x.Name.GetText(selectedLanguageId), false))
                .OrderBy(x => x.Name)
                .ToList();

            InitDropDownLists();
        }

        /// <summary>
        /// Used for Edit
        /// </summary>
        /// <param name="selectedId"></param>
        /// <param name="addons"></param>
        /// <param name="languages"></param>
        /// <param name="selectedLanguageId"></param>
        /// <param name="currency"></param>
        public AddonModel(int selectedId, IEnumerable<Addon> addons, IEnumerable<Language> languages, LanguageTypeEnum selectedLanguageId, string currency)
        {
            var selectedAddon = addons.First(x => x.Id == selectedId);

            Id = selectedAddon.Id;
            Name = LocalizedValueModel.Create(languages, selectedAddon.Name).ToList();
            Description = LocalizedValueModel.Create(languages, selectedAddon.Description).ToList();
            Price = selectedAddon.Price;
            Currency = currency;

            SelectedCalculationRule = selectedAddon.CalculationRule;
            SelectedPostingRhythm = selectedAddon.PostingRhythm;

            SelectedLanguageId = selectedLanguageId;
            AvailableLanguages = languages.Select(x => new LocalizedValueModel(x.Id, x.Name, x.Name)).ToList();

            MenuItems = addons.Select(x => new MenuItemModel(x.Id, x.Name.GetText(selectedLanguageId), x.Id == selectedId))
                .OrderBy(x => x.Name)
                .ToList();

            InitDropDownLists();
        }

        private void InitDropDownLists()
        {
            CalculationRules = new List<SelectListItem>();
            CalculationRules.Add(new SelectListItem { Selected = false, Text = "Select Calculation Rule..", Value = string.Empty });
            CalculationRules.Add(new SelectListItem { Selected = false, Text = "Per Person", Value = CalculationRule.PerPerson.ToString() });
            CalculationRules.Add(new SelectListItem { Selected = false, Text = "Per Adult", Value = CalculationRule.PerAdult.ToString() });
            CalculationRules.Add(new SelectListItem { Selected = false, Text = "Per Child", Value = CalculationRule.PerChild.ToString() });
            CalculationRules.Add(new SelectListItem { Selected = false, Text = "Per Baby", Value = CalculationRule.PerBaby.ToString() });
            CalculationRules.Add(new SelectListItem { Selected = false, Text = "Per Room", Value = CalculationRule.PerRoom.ToString() });

            PostingRhythms = new List<SelectListItem>();
            PostingRhythms.Add(new SelectListItem { Selected = false, Text = "Select Posting Rhythm..", Value = string.Empty });
            PostingRhythms.Add(new SelectListItem { Selected = false, Text = "Per Day", Value = PostingRhythm.Everyday.ToString() });
            PostingRhythms.Add(new SelectListItem { Selected = false, Text = "Charge Once", Value = PostingRhythm.ChargeOnce.ToString() });   
        }

        public int Id { get; set; }

        public List<LocalizedValueModel> Name { get; set; }

        public List<LocalizedValueModel> Description { get; set; }

        public decimal Price { get; set; }

        public string Currency { get; set; }

        public List<SelectListItem> CalculationRules { get; set; }

        public CalculationRule SelectedCalculationRule { get; set; }

        public List<SelectListItem> PostingRhythms { get; set; }

        public PostingRhythm SelectedPostingRhythm { get; set; }

        public LanguageTypeEnum SelectedLanguageId { get; set; }

        public List<LocalizedValueModel> AvailableLanguages { get; set; }

        public List<MenuItemModel> MenuItems { get; set; }
    }

}