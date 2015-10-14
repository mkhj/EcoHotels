using System;
using EcoHotels.Core.Common;
using EcoHotels.Core.Infrastructure;
using EcoHotels.Extensions;

namespace EcoHotels.Core.Domain.Models.Media
{
    [Serializable]
    public class Asset : EntityBase<Asset>
    {
        protected Asset()
        {
            Name = string.Empty;
            Size = 0;
            Data = new byte[0];
            MimeType = "";
            Created = DateTime.Now;
            Modified = DateTime.Now;

            TopX = 0;
            TopY = 0;
            BottomX = 0;
            BottomY = 0;
            CropXUnits = 0;
            CropYUnits = 0;
        }

        public static Asset Create(AssetCategory category, string name, long size, byte[] data, string mimetype)
        {
            Check.Require(category.IsNotNull(), "Category can not be null.");
            Check.Require(name.IsNotNullOrEmpty(), "Name can not be null or empty.");
            Check.Require(mimetype.IsNotNullOrEmpty(), "Mimetype can not be null or empty.");

            return new Asset
                       {
                           Category = category,
                           Name = name,
                           Size = size,
                           Data = data,
                           MimeType = mimetype
                       };
        }

        public virtual AssetCategory Category { get; set; }

        #region - File Information -

        public virtual string Name { get; set; }

        public virtual long Size { get; set; }

        public virtual byte[] Data { get; set; }

        public virtual string MimeType { get; set; }

        #endregion

        #region - Cropping Information -

        public virtual int TopX { get; set; }

        public virtual int TopY { get; set; }

        public virtual int BottomX { get; set; }

        public virtual int BottomY { get; set; }

        public virtual int CropXUnits { get; set; }

        public virtual int CropYUnits { get; set; }

        #endregion
        
        #region - Dates -

        public virtual DateTime Created { get; protected internal set; }

        public virtual DateTime Modified { get; set; }

        #endregion

        public virtual void SetCroppingInformation(int topX, int topY, int bottomX, int bottomY, int cropXUnits, int cropYUnits)
        {
            TopX = topX;
            TopY = topY;
            BottomX = bottomX;
            BottomY = bottomY;
            CropXUnits = cropXUnits;
            CropYUnits = cropYUnits;

            Modified = DateTime.Now;
        }

        public virtual string GenerateCroppingQuery()
        {
            return string.Format("?crop=({0},{1},{2},{3})&cropxunits={4}&cropyunits={5}", TopX, TopY, BottomX, BottomY, CropXUnits, CropYUnits);
        }

        public virtual string GenerateUrl(int width, int height)
        {
            return string.Format("/img/{0}/{1}x{2}/{3}", Id, width, height, Name);
        }

        protected override void Validate()
        {
            
        }
    }
}
