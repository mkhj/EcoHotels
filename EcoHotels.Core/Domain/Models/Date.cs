using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EcoHotels.Core.Domain.Models
{
    [Serializable]
    public class Date
    {
        protected Date() { }

        protected Date(DateTime value)
        {
            Value = value.Date;
        }

        public static Date Create(DateTime date)
        {
            return new Date(date);
        }

        /// <summary>
        /// This the Surrogate key
        /// </summary>
        public virtual int Id
        {
            get;
            protected internal set;
        }

        public virtual DateTime Value
        {
            get;
            protected internal set;
        }
    }
}
