using System;
using System.Globalization;

namespace EcoHotels.Core.Domain.Value_objects
{
    //internal class SystemCurrency : EntityGuidBase<SystemCurrency>
    //{
    //    private static readonly SystemCurrency instance = new SystemCurrency();

    //    public static SystemCurrency Instance
    //    {
    //        get { return instance; }
    //    }

    //    public SystemCurrency()
    //    {

    //        NegativePattern = 1;
    //        PositivePattern = 1;
    //    }

    //    public static void Create()
    //    {
            
    //    }


    //    #region - Currency information -

    //    /// <summary>
    //    /// The number of decimal places to use in currency values. 
    //    /// </summary>
    //    /// <remarks>
    //    /// The default for InvariantInfo is 2.
    //    /// 1000.00
    //    /// 1000.0000
    //    /// </remarks>
    //    private int DecimalDigits
    //    {
    //        get { return CultureInfo.InvariantCulture.NumberFormat.CurrencyDecimalDigits; }
    //    }

    //    /// <summary>
    //    /// The string to use as the decimal separator in currency values.
    //    /// </summary>
    //    /// <remarks>
    //    /// The default for InvariantInfo is ".".
    //    /// $123,456,789.00
    //    /// $123,456,789 00
    //    /// </remarks>
    //    public string DecimalSeparator { get; set; }

    //    /// <summary>
    //    /// The string that separates groups of digits to the left of the decimal in currency values.
    //    /// </summary>
    //    /// <remarks>
    //    /// The default for InvariantInfo is ",".
    //    /// $123,456,789.00
    //    /// $123 456 789.00
    //    /// </remarks>
    //    public string GroupSeparator { get; set; }

    //    /// <summary>
    //    /// The number of digits in each group to the left of the decimal in currency values. 
    //    /// </summary>
    //    /// <remarks>
    //    /// The default for InvariantInfo is a one-dimensional array with only one element, which is set to 3.
    //    /// </remarks>
    //    private int[] GroupSizes
    //    {
    //        get { return CultureInfo.InvariantCulture.NumberFormat.CurrencyGroupSizes; }
    //    }

    //    /// <summary>
    //    /// The format pattern for negative currency values.
    //    /// </summary>
    //    /// <remarks>
    //    /// The default for InvariantInfo is 0, which represents "($n)", where "$" is the CurrencySymbol and n is a number.
    //    /// </remarks>
    //    public int NegativePattern { get; set; }

    //    /// <summary>
    //    /// The format pattern for positive currency values.
    //    /// </summary>
    //    /// <remarks>
    //    /// The default for InvariantInfo is 0, which represents "$n", where "$" is the CurrencySymbol and n is a number.
    //    /// </remarks>
    //    public int PositivePattern { get; set; }

    //    #endregion

    //    public NumberFormatInfo NumberFormatInfo
    //    {
    //        get
    //        {
    //            return new NumberFormatInfo
    //            {
    //                CurrencyNegativePattern = NegativePattern,
    //                CurrencyPositivePattern = PositivePattern,
    //                CurrencyGroupSizes = GroupSizes,
    //                CurrencyDecimalDigits = DecimalDigits,
    //                CurrencyDecimalSeparator = DecimalSeparator,
    //                CurrencyGroupSeparator = GroupSeparator,
    //                CurrencySymbol = ""
    //            };
    //        }
    //    }

    //    protected override void Validate()
    //    {
    //        throw new NotImplementedException();
    //    }
    //}
}
