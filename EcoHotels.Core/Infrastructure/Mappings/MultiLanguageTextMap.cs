using FluentNHibernate.Mapping;
using EcoHotels.Core.Domain.Models.Localization;

namespace EcoHotels.Core.Infrastructure.Mappings
{
    public class MultiLanguageTextMap : ClassMap<MultiLanguageText>
    {
        public MultiLanguageTextMap()
        {
            Table("MultiLanguageText");
            Id(x => x.Id).GeneratedBy.Identity();

            HasMany(x => x.Texts)
                .KeyColumn("MultiLanguageTextId")
                .Inverse()
                .AsSet()                
                .Cascade.AllDeleteOrphan();
                //.Not.LazyLoad()
                //.Fetch.Join();


        }
    }
}
