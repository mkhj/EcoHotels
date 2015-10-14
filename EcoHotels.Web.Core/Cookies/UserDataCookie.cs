using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EcoHotels.Web.Core.Cookies
{
    public class UserDataCookie
    {
        public Guid LanguageId { get; set; }

        public Guid CurrencyId { get; set; }
    }
}
