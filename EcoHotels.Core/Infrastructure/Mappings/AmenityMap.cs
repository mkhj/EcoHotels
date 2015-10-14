using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using EcoHotels.Core.Domain.Models.Property;

namespace EcoHotels.Core.Infrastructure.Mappings
{
    public class AmenityMap : ClassMap<Amenity>
    {    
        public AmenityMap()
        {
            Table("Amenities");
            Id(x => x.Id).GeneratedBy.Identity();

            Map(x => x.Name);
        }
    }
}
