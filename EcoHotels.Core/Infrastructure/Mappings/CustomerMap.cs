using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EcoHotels.Core.Common.Enum;
using EcoHotels.Core.Domain.Models.Commerce;
using EcoHotels.Core.Domain.Models.Security;
using FluentNHibernate.Mapping;

namespace EcoHotels.Core.Infrastructure.Mappings
{
    public class CustomerMap : ClassMap<Customer>
    {
        public CustomerMap()
        {
            Table("Customers");
            Id(x => x.Id).GeneratedBy.Identity();

            Map(x => x.Firstname);
            Map(x => x.Lastname);
            Map(x => x.PhoneNumber);
            Map(x => x.Country);
            Map(x => x.Email);
            Map(x => x.Password);
            Map(x => x.Role).CustomType(typeof(RolesEnum));
            Map(x => x.Gender).CustomType(typeof(GenderTypeEnum));
            Map(x => x.AccountType).CustomType(typeof(AccountTypeEnum));
            Map(x => x.ExternalId);
            Map(x => x.Birthday);
            Map(x => x.IsBonusMember);
            Map(x => x.WantNewsletter);

        }
    }
}
