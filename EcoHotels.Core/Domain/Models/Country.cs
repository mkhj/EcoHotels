using System;
using EcoHotels.Core.Domain.Models.Localization;
using EcoHotels.Core.Domain.Value_objects;

namespace EcoHotels.Core.Domain.Models
{
    [Serializable]
    public class Country
    {
        /// <summary>
        /// This the Surrogate key
        /// </summary>
        public virtual int Id
        {
            get;
            protected internal set;
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual string Alpha2Code { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual Currency Currency { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual Language Language { get; set; }
    }
}
