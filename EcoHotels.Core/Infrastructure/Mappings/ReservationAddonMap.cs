using FluentNHibernate.Mapping;
using EcoHotels.Core.Common;
using EcoHotels.Core.Domain.Models.Commerce;

namespace EcoHotels.Core.Infrastructure.Mappings
{
    public class ReservationAddonMap : ClassMap<ReservationAddon>
    {
        public ReservationAddonMap()
        {
            Table("ReservationAddons");

            Id(x => x.Id).GeneratedBy.Identity();
            Map(x => x.Name);
            Map(x => x.Description);
            Map(x => x.Price);
            Map(x => x.CalculationRule, "CalculationRuleId").CustomType(typeof(CalculationRule));
            Map(x => x.PostingRhythm, "PostingRhythmId").CustomType(typeof(PostingRhythm));

            References(x => x.ReservationItem, "ReservationItemId")
                .Cascade.None();
        }
    }
}
