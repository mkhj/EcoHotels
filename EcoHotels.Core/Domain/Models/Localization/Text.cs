using System;
using EcoHotels.Core.Common;
using EcoHotels.Core.Infrastructure;

namespace EcoHotels.Core.Domain.Models.Localization
{
    [Serializable]
    public class Text : EntityBase<Text>
    {
        protected Text()
        {
        }

        public virtual string Value { get; set; }

        public virtual Language Language { get; protected internal set; }

        public virtual MultiLanguageText MultiLanguageText { get; protected internal set; }

        /// <summary>
        /// Factory Method
        /// </summary>
        /// <param name="value"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public static Text Create(string value, MultiLanguageText multiLanguageText, Language language)
        {
            Check.Require(value != null, "Text can not be null.");
            Check.Require(language != null, "language can not be null.");

            return new Text
            {
                Value = value,
                Language = language,
                MultiLanguageText = multiLanguageText
            };
        }

        protected override void Validate()
        {
            
        }
    }
}
