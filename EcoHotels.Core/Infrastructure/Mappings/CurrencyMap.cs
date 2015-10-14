using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;

namespace EcoHotels.Core.Infrastructure.Mappings
{
    public class CurrencyMap : ClassMap<Domain.Value_objects.Currency>
    {
        public CurrencyMap()
        {
            Table("Currencies");

            Id(x => x.Id).GeneratedBy.Identity();
            Map(x => x.Name);
            Map(x => x.ISO_4217_Number, "ISO_4217_Number");
            Map(x => x.ISOCurrencySymbol, "ISOCurrencySymbol");
            Map(x => x.ExchangeRate, "ExchangeRate");
            Map(x => x.LCID, "LCID");
        }
    }
}
