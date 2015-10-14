using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EcoHotels.Core.Domain.Models.Localization;
using FluentNHibernate.Mapping;

namespace EcoHotels.Core.Infrastructure.Mappings
{
    public class PageInformationMap : ClassMap<PageInformation>
    {
        public PageInformationMap()
        {
            Table("PageInformations");

            Id(x => x.Id).GeneratedBy.Identity();
            Map(x => x.Filename);
            Map(x => x.LastModified);

            #region - BelongsTo -

            References(x => x.Title, "TitleMultiLanguageId")
                .Cascade.All();

            References(x => x.Description, "DescriptionMultiLanguageId")
                .Cascade.All();

            References(x => x.Keywords, "KeywordsMultiLanguageId")
                .Cascade.All();

            #endregion
        }
    }
}
