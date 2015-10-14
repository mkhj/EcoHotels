using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using EcoHotels.Core.Domain.Models.Property;

namespace EcoHotels.Core.Infrastructure.Mappings
{
    public class RoomTypeMap : ClassMap<RoomType>
    {
        public RoomTypeMap()
        {
            Table("RoomTypes");

            Id(x => x.Id).GeneratedBy.Identity();
            Map(x => x.Beds, "BedTypes");
            Map(x => x.RoomView);
            Map(x => x.Capacity);
            Map(x => x.PhysicalRooms);
            Map(x => x.Size, "RoomSize");
            Map(x => x.Quantity);
            Map(x => x.RackRate, "Price");
            Map(x => x.IsSmoking, "Smoking");


            #region - BelongsTo -

            References(x => x.Hotel, "HotelId")
                .Cascade.None();

            References(x => x.Name, "NameMultiLanguageId")
                .Cascade.All();

            References(x => x.Description, "DescriptionMultiLanguageId")
                .Cascade.All();

            #endregion


            #region - HasAndBelongsToMany -

            HasManyToMany(x => x.Amenities)
                .Table("RoomTypes_Amenities").ParentKeyColumn("RoomTypeId").ChildKeyColumn("AmenityId")
                .LazyLoad()
                .Cascade.SaveUpdate();

            HasManyToMany(x => x.Addons)
                .Table("RoomTypes_Addons").ParentKeyColumn("RoomTypeId").ChildKeyColumn("AddonId")
                .LazyLoad()
                .Cascade.SaveUpdate();

            HasManyToMany(x => x.Assets)
                .Table("RoomTypes_Assets").ParentKeyColumn("RoomTypeId").ChildKeyColumn("AssetId")
                .LazyLoad()
                .Cascade.SaveUpdate();

            #endregion
        }
    }
}
