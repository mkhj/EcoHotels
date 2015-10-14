using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EcoHotels.Core.Domain.Models;
using FluentNHibernate.Mapping;

namespace EcoHotels.Core.Infrastructure.Mappings
{
    public class DateMap : ClassMap<Date>
    {
        public DateMap()
        {
            Table("Calendar");
            Id(x => x.Id).GeneratedBy.Identity();
            Map(x => x.Value);
        }
    }
}
