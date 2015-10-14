using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using EcoHotels.Core.Domain.Models.Security;

namespace EcoHotels.Core.Infrastructure.Mappings
{
    public class UserMap : ClassMap<User>
    {
        public UserMap()
        {
            Table("Users");
            Id(x => x.Id).GeneratedBy.Identity();
            Map(x => x.Firstname);
            Map(x => x.Lastname);
            Map(x => x.Email);
            Map(x => x.Password);
            Map(x => x.Role).CustomType(typeof(RolesEnum));
            Map(x => x.IsActive);


            #region - BelongsTo -

            References(x => x.Organization, "OrganizationId")
                .Cascade.None();

            #endregion
        }
    }
}
