using System;
using EcoHotels.Core.Common;
using EcoHotels.Core.Common.Enum;
using EcoHotels.Core.Infrastructure;
using EcoHotels.Extensions;
using EcoHotels.Core.Domain.Value_objects;

namespace EcoHotels.Core.Domain.Models.Localization
{
    public class LanguageType
    {
        public static Guid English = new Guid("3873B730-57D4-4022-835C-9F31011587E3");
        public static Guid Danish = new Guid("bf865065-6179-4cb9-92ac-9fe700b8a4f4");
        public static Guid Swedish = new Guid("15dfad75-be68-44ed-8401-9fe700b8a4f9");
        public static Guid Norway = new Guid("82512b8e-fa31-4506-86e0-9fe700b8a4fc");
        public static Guid Italien = new Guid("aa200cdd-6f13-437a-a313-9fe700b8a4fc");
    }

    [Serializable]
    public class Language : EntityBase<Language>
    {
        protected internal Language()
        {
            Name = string.Empty;
            Shortname = string.Empty;
            IsActive = false;
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual string Shortname { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual Currency Currency { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual bool IsActive { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual LanguageTypeEnum Type { get { return Id.ConvertToEnum<LanguageTypeEnum>(); } }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="shortname"></param>
        /// <returns></returns>
        public static Language Create(string name, string shortname)
        {
            Check.Require(name.IsNotNullOrEmpty(), "Name can not be null or empty.");
            Check.Require(shortname.IsNotNullOrEmpty(), "Shortname can not be null or empty.");

            return new Language { Name = name, Shortname = shortname };
        }

        protected override void Validate()
        {
            
        }
    }
}
