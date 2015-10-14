using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using EcoHotels.Core.Domain.Models.Localization;

namespace EcoHotels.Web.Core.Models
{
    public class LocalizedValueModel
    {
        /// <summary>
        /// Required.
        /// </summary>
        public LocalizedValueModel(){}

        public LocalizedValueModel(int languageId, string languageName, string value)
        {
            Id = languageId;
            LanguageName = languageName;
            Value = value;
        }

        public static List<LocalizedValueModel> Create(IEnumerable<Language> availableLanguages)
        {
            return availableLanguages.Select(x => new LocalizedValueModel(x.Id, x.Name, string.Empty)).ToList();
        }

        public static List<LocalizedValueModel> Create(IEnumerable<Language> availableLanguages, MultiLanguageText text)
        {
            return availableLanguages.Select(x => new LocalizedValueModel(x.Id, x.Name, text.GetText(x.Type))).ToList();
        }

        public int Id { get; set; }

        public string LanguageName { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Value { get; set; }
    }
}