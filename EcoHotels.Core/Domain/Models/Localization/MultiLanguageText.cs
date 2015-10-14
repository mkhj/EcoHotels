using System;
using System.Collections.Generic;
using System.Linq;
using EcoHotels.Core.Common;
using EcoHotels.Core.Common.Enum;
using EcoHotels.Core.Infrastructure;

namespace EcoHotels.Core.Domain.Models.Localization
{
    [Serializable]
    public class MultiLanguageText : EntityBase<MultiLanguageText>
    {
        protected internal MultiLanguageText()
        {
            Texts = new List<Text>();
        }

        public virtual ICollection<Text> Texts
        {
            get;
            protected internal set;
        }

        public virtual string GetText(LanguageTypeEnum languageId)
        {
            var obj = Texts.FirstOrDefault(x => x.Language.Id == (int) languageId);
            return (obj == null)? string.Empty : obj.Value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <param name="language"></param>
        public virtual void AddLocalizedText(string text, Language language)
        {
            Check.Require(text != null, "Text can not be null.");
            Check.Require(language != null, "language can not be null.");

            var obj = Texts.ToList().Find(t => t.Language.Id == language.Id);
            if (obj != null)
            {
                obj.Value = text;
            }
            else
            {
                var multiLanguageTextLanguage = Text.Create(text, this, language);
                Texts.Add(multiLanguageTextLanguage);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="language"></param>
        public virtual void RemoveLocalizedText(Language language)
        {
            throw new NotImplementedException();
        }

        protected override void Validate()
        {
            
        }
    }
}
