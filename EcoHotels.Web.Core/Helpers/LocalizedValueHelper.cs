using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EcoHotels.Core.Domain.Models.Localization;
using EcoHotels.Web.Core.Models;

namespace EcoHotels.Web.Core.Helpers
{
    public class LocalizedValueHelper
    {
        public static void MapToMultiLanguageText(List<LocalizedValueModel> values, MultiLanguageText text, IEnumerable<Language> languages)
        {
            foreach (var language in languages)
            {
                var value = values.FirstOrDefault(x => x.Id == language.Id);
                text.AddLocalizedText(value.Value, language);
            }
        }
    }
}
