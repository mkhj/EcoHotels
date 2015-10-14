using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EcoHotels.Core.Common;
using EcoHotels.Core.Domain.Models.Property;
using EcoHotels.Extensions;

namespace EcoHotels.Core.Domain.Models.Tags
{
    [Serializable]
    public class SearchTagCity 
    {
        protected SearchTagCity()
        {
            Hotels = new List<Hotel>();
        }

        public static SearchTagCity Create(string name, Country country)
        {
            Check.Require(name.IsNotNullOrEmpty(), "Name can not be null or empty.");
            Check.Require(country.IsNotNull(), "Country can not be null.");

            return new SearchTagCity
                       {
                           Name = name,
                           Country = country
                       };
        }

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
        public virtual Country Country { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual ICollection<Hotel> Hotels { get; protected internal set; }
    }
}
