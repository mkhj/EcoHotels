using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using EcoHotels.Core.Domain.Models;

namespace EcoHotels.Core.Infrastructure.Mappings
{
    public class CountryMap : ClassMap<Country>
    {
        public CountryMap()
        {
            Table("Countries");

            Cache.Region("foo")
                .ReadWrite();

            Id(x => x.Id).GeneratedBy.Identity();
            Map(x => x.Name);
            Map(x => x.Alpha2Code, "[Alpha-2 code]");

            #region - BelongsTo -

            References(x => x.Currency, "CurrencyId")
                .LazyLoad()
                .Cascade.None();

            References(x => x.Language, "LanguageId")
                .LazyLoad()
                .Cascade.None();

            #endregion

        }
    }
}
