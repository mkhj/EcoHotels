using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using EcoHotels.Core.Domain.Models.Localization;

namespace EcoHotels.Core.Infrastructure.Mappings
{
    public class LanguageMap : ClassMap<Language>
    {
        public LanguageMap()
        {
            Table("Languages");
            Id(x => x.Id).GeneratedBy.Identity();

            Map(x => x.Name);
            Map(x => x.Shortname);
            Map(x => x.IsActive);
        }
    }
}
