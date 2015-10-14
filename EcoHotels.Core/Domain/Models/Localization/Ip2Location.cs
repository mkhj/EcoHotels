using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EcoHotels.Core.Domain.Models.Localization
{
    public class Ip2Location
    {
        private Ip2Location(){}

        public virtual string CountryShortName { get; set; }

        public virtual int IpFrom { get; set; }

        public virtual int IpTo { get; set; }
    }
}
