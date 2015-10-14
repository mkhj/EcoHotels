using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using EcoHotels.Core.Domain.Models.Property;

namespace EcoHotels.Core.Infrastructure.Mappings
{
    public class OrganizationMap : ClassMap<Organization>
    {
        public OrganizationMap()
        {
            Table("Organizations");

            #region - Properties -

            Id(x => x.Id).GeneratedBy.Identity();
            Map(x => x.Name);
            Map(x => x.VatNo);
            Map(x => x.Phone);
            Map(x => x.Email);
            Map(x => x.Address1);
            Map(x => x.Address2);
            Map(x => x.City);
            Map(x => x.Zipcode);
            Map(x => x.Country);
            Map(x => x.Created);

            #endregion

            #region - HasMany -

            HasMany(x => x.Users)
                .Table("Users")
                .KeyColumn("OrganizationId")
                .LazyLoad()
                .Inverse()
                .AsSet()
                .Cascade.All();

            HasMany(x => x.Hotels)
                .Table("Hotels")
                .KeyColumn("OrganizationId")
                .LazyLoad()
                .Inverse()
                .AsSet()
                .Cascade.All();

            #endregion


            #region - HasAndBelongsToMany -

            //HasManyToMany(x => x.AvailableLanguages)
            //    .Table("Organizations_Languages").ParentKeyColumn("OrganizationId").ChildKeyColumn("LanguageId")
            //    .LazyLoad()
            //    .Cascade.None();

 
            #endregion

        }
    }
}
