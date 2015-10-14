using System;
using System.Globalization;
using EcoHotels.Core.Infrastructure;
using EcoHotels.Extensions;
using EcoHotels.Core.Common;

namespace EcoHotels.Core.Domain.Value_objects
{
    /// <summary>
    /// Currency
    /// http://msdn.microsoft.com/en-us/library/system.globalization.numberformatinfo.aspx
    /// 
    /// Exchange rates
    /// http://www.nationalbanken.dk/dndk/valuta.nsf/valuta.xml
    /// </summary>
    [Serializable]
    public class Currency : EntityBase<Currency>
    {
        protected internal Currency()
        {
            Name = string.Empty;
            ISOCurrencySymbol = string.Empty;
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual int ISO_4217_Number { get; protected internal set; }

        /// <summary>
        /// The string to use as the currency symbol.
        /// </summary>
        public virtual string ISOCurrencySymbol { get; protected internal set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual int LCID { get; protected internal set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual decimal ExchangeRate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual bool IsBaseCurrency
        {
            get { return string.Compare(ISOCurrencySymbol, "dkk", true) == 0; }
        }

        public virtual NumberFormatInfo NumberFormatInfo
        {
            get
            {
                var cultureInfo = CultureInfo.GetCultureInfo(LCID);

                return new NumberFormatInfo
                {
                    CurrencyNegativePattern = 2, // cultureInfo.NumberFormat.CurrencyNegativePattern,
                    CurrencyPositivePattern = 2, // cultureInfo.NumberFormat.CurrencyPositivePattern,
                    CurrencyGroupSizes = cultureInfo.NumberFormat.CurrencyGroupSizes,
                    CurrencyDecimalDigits = cultureInfo.NumberFormat.CurrencyDecimalDigits,
                    CurrencyDecimalSeparator = cultureInfo.NumberFormat.CurrencyDecimalSeparator,
                    CurrencyGroupSeparator = cultureInfo.NumberFormat.CurrencyGroupSeparator,
                    CurrencySymbol = ISOCurrencySymbol
                };
            }
        }


        public static Currency Create(string name, int isoNumber, string symbol, decimal exhangeRate)
        {
            Check.Require(name.IsNotNullOrEmpty(), "Currency name can not be null or empty");
            Check.Require(symbol.IsNotNullOrEmpty(), "Currency symbol can not be null or empty");

            var LCID = GetNumberFormatInfo(symbol);

            return new Currency
            {
                Name = name,
                ISO_4217_Number = isoNumber,
                ExchangeRate = exhangeRate,
                ISOCurrencySymbol = symbol,
                LCID = LCID
            };
        }

        private static int GetNumberFormatInfo(string isoSymbol)
        {
            foreach (var cultureInfo in CultureInfo.GetCultures(CultureTypes.SpecificCultures))
            {
                if (!cultureInfo.Equals(CultureInfo.InvariantCulture))
                {
                    var regionCulture = new RegionInfo(cultureInfo.LCID);

                    if (regionCulture.ISOCurrencySymbol == isoSymbol)
                    {
                        return cultureInfo.LCID;
                    }
                }
            }

            throw new CultureNotFoundException();

            //return CultureInfo.InvariantCulture.NumberFormat;
        }

        public override string ToString()
        {
            return string.Format("{0} ({1})", Name, ISOCurrencySymbol);
        }

        protected override void Validate()
        {
            
        }
    }
}
