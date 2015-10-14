using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EcoHotels.Core.Domain;
using EcoHotels.Core.Domain.Models.Commerce;
using EcoHotels.Core.Infrastructure.NH;
using FluentNHibernate.Mapping;
using EcoHotels.Core.Common;
using NHibernate;
using NHibernate.Criterion;

namespace EcoHotels.Core.Infrastructure.Mappings
{
    public class AddonMap : ClassMap<Addon>
    {
        public AddonMap()
        {
            Table("Addons");

            Id(x => x.Id).GeneratedBy.Identity();
            Map(x => x.Price);
            Map(x => x.CalculationRule, "CalculationRuleId").CustomType(typeof(CalculationRule));
            Map(x => x.PostingRhythm, "PostingRhythmId").CustomType(typeof(PostingRhythm));

            #region - BelongsTo -

            References(x => x.Hotel, "HotelId")
                .Cascade.None();

            References(x => x.Name, "NameMultiLanguageId")
                .Cascade.All();

            References(x => x.Description, "DescriptionMultiLanguageId")
                .Cascade.All();

            #endregion
        }
    }
}
