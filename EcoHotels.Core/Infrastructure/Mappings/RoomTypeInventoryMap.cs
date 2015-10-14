using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EcoHotels.Core.Domain.Models.Commerce;
using FluentNHibernate.Mapping;

namespace EcoHotels.Core.Infrastructure.Mappings
{
    public class RoomTypeInventoryMap: ClassMap<RoomTypeInventory>
    {
        public RoomTypeInventoryMap()
        {
            Table("RoomTypeInventory");
            Id(x => x.Id).GeneratedBy.Identity();

            Map(x => x.Quantity);
            Map(x => x.RateCategoryId);
            Map(x => x.RoomTypeId);
            
            #region - BelongsTo -

            References(x => x.Date, "CalendarId")
                .Cascade.None();

            #endregion
        }
    }
}
