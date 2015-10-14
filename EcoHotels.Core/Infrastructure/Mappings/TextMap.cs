using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using EcoHotels.Core.Domain.Models.Localization;

namespace EcoHotels.Core.Infrastructure.Mappings
{
    public class TextMap : ClassMap<Text>
    {
        public TextMap()
        {
            Table("Texts");
            Id(x => x.Id).GeneratedBy.Identity();
            Map(x => x.Value);

            References(x => x.MultiLanguageText, "MultiLanguageTextId")
                .Cascade.None();

            References(x => x.Language, "LanguageId")
                .Cascade.None();

        }
    }
}
