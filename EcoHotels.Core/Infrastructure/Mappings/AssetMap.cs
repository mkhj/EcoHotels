using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EcoHotels.Core.Domain.Models.Media;
using FluentNHibernate.Mapping;
using EcoHotels.Core.Domain.Models;
using NHibernate.Type;

namespace EcoHotels.Core.Infrastructure.Mappings
{
    public class AssetMap : ClassMap<Asset>
    {
        public AssetMap()
        {
            Table("Assets");

            Id(x => x.Id).GeneratedBy.Identity();
            
            Map(x => x.Name);
            Map(x => x.Size);
            //Map(x => x.Data).CustomSqlType("varbinary(MAX)");
            Map(x => x.Data).CustomType(typeof (BinaryBlobType)); // without it anything passed 8k gets truncated
            Map(x => x.MimeType);
            Map(x => x.Created);
            Map(x => x.Modified);

            Map(x => x.TopX);
            Map(x => x.TopY);
            Map(x => x.BottomX);
            Map(x => x.BottomY);
            Map(x => x.CropXUnits);
            Map(x => x.CropYUnits);

            #region - BelongsTo -

            References(x => x.Category, "AssetCategoryId")
                .Cascade.None();

            #endregion

        }
    }
}
