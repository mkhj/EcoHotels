using EcoHotels.Core.Common.Enum;
using FluentNHibernate.Mapping;
using EcoHotels.Core.Domain.Models.Property;

namespace EcoHotels.Core.Infrastructure.Mappings
{
    public class HotelMap : ClassMap<Hotel>
    {
        public HotelMap()
        {
            Table("Hotels");

            #region - Properties -

            Id(x => x.Id).GeneratedBy.Identity();
            Map(x => x.Name);
            Map(x => x.Address1);
            Map(x => x.Address2);
            Map(x => x.Zipcode);
            Map(x => x.City);
            Map(x => x.Country);
            Map(x => x.Phone);
            Map(x => x.Fax);
            Map(x => x.Email);
            Map(x => x.WWW);
            Map(x => x.VatNo);

            Map(x => x.CategoryOne, "CategoryOneId").CustomType(typeof(HotelCategoryEnum));
            Map(x => x.CategoryTwo, "CategoryTwoId").CustomType(typeof(HotelCategoryEnum)); ;
            Map(x => x.CategoryThree, "CategoryThreeId").CustomType(typeof(HotelCategoryEnum)); ;

            Map(x => x.IsActive);

            Map(x => x.CheckIn);
            Map(x => x.CheckInAMPM);
            Map(x => x.CheckOut);
            Map(x => x.CheckOutAMPM);

            Map(x => x.MinimumStay);
            Map(x => x.MinimumCheckInAge);

            Map(x => x.Created);
            Map(x => x.Modified);
            Map(x => x.Verified);


            #endregion

            #region - BelongsTo -

            References(x => x.About, "AboutMultiLanguageId")
                .Cascade.All();

            References(x => x.Organization, "OrganizationId")
                .Cascade.None();

            References(x => x.Currency, "LocalCurrencyId")
                .Cascade.None();

            References(x => x.CancellationPolicy, "CancellationPolicyMultiLanguageId")
                .Cascade.All();

            References(x => x.PaymentPolicy, "PaymentPolicyMultiLanguageId")
                .Cascade.All();

            References(x => x.PageInformation, "PageInformationId")
                .Cascade.All();

            References(x => x.SearchTagCity, "SearchTagCityId")
                .Cascade.None();

            #endregion

            #region - HasMany -

            HasMany(x => x.Addons)
                .Table("Addons")
                .KeyColumn("HotelId")
                .LazyLoad()
                .Inverse()
                .AsSet()
                .Cascade.AllDeleteOrphan();

            HasMany(x => x.RoomTypes)
                .Table("RoomTypes")
                .KeyColumn("HotelId")
                //.LazyLoad()
                .Inverse()
                .AsSet()
                .Cascade.AllDeleteOrphan();

            HasMany(x => x.Descriptions)
                .Table("HotelBulletPoints")
                .KeyColumn("HotelId")
                .OrderBy("OrderId")
                .LazyLoad()
                .Inverse()
                .AsSet()
                .Cascade.AllDeleteOrphan();

            #endregion


            #region - HasAndBelongsToMany -

            HasManyToMany(x => x.Amenities)
                .Table("Hotels_Amenities").ParentKeyColumn("HotelId").ChildKeyColumn("AmenityId")
                .LazyLoad()
                .Cascade.None();

            HasManyToMany(x => x.Assets)
                .Table("Hotels_Assets").ParentKeyColumn("HotelId").ChildKeyColumn("AssetId")
                .LazyLoad()
                .Cascade.SaveUpdate();

            #endregion
        }
    }
}
