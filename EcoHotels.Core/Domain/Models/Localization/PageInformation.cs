using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EcoHotels.Core.Common;
using EcoHotels.Core.Infrastructure;
using EcoHotels.Extensions;

namespace EcoHotels.Core.Domain.Models.Localization
{
    /// <summary>
    /// This provides meta information for hotel pages.
    /// </summary>
    [Serializable]
    public class PageInformation : EntityBase<PageInformation>
    {
        protected internal PageInformation()
        {
            Filename = string.Empty;
            Title = new MultiLanguageText();
            Description = new MultiLanguageText();
            Keywords = new MultiLanguageText();

            LastModified = DateTime.Now;
        }


        /// <summary>
        /// 
        /// </summary>
        public virtual string Filename { get; protected internal set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual MultiLanguageText Title { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual MultiLanguageText Description { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual MultiLanguageText Keywords { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual DateTime LastModified { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public virtual bool SetVirtualPagename(string name)
        {
            var slug = name.GenerateSlug();
            if(string.Compare(Filename, slug, true) != 0)
            {
                Filename = slug;
                return true;
            }

            return false;
        }

        protected override void Validate()
        {
            if(Filename.IsNullOrEmpty())
            {
                throw new ArgumentNullException("Filename can not be null");
            }
        }
    }
}
