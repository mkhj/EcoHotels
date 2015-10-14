using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using EcoHotels.Core.Domain.Value_objects;

namespace EcoHotels.Core.Common
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class Money
    {
        private Money(decimal value, Currency currency)
        {
            Value = Math.Round(value, 3, MidpointRounding.AwayFromZero);
            Currency = currency;
        }

        #region - Factory Methods -

        public static Money Create(decimal value, Currency currency)
        {
            return new Money(value, currency);
        }

        public static Money Create(string value, Currency currency)
        {
            return new Money(decimal.Parse(value), currency);
        }

        #endregion

        public decimal Value
        {
            get; private set;
        }

        public Currency Currency
        {
            get; private set;
        }

        public override string ToString()
        {
            return string.Format(Currency.NumberFormatInfo, "{0:c}", Value);
        }

        public Money ConvertTo(Currency currency)
        {
            var baseValue = (Value * Currency.ExchangeRate) / 100;

            var newValue = (baseValue * 100) / currency.ExchangeRate;

            return Create(newValue, currency);
        }

        #region - operators -

        /// <summary>
        /// 
        /// </summary>
        /// <param name="m1"></param>
        /// <param name="m2"></param>
        /// <returns></returns>
        public static Money operator +(Money m1, Money m2)
        {
            Check.Require(m1.Currency.Id == m2.Currency.Id, "Currency is not allowed to defer for math operators");

            return Create(m1.Value + m2.Value, m1.Currency);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="m1"></param>
        /// <param name="m2"></param>
        /// <returns></returns>
        public static Money operator /(Money m1, Money m2)
        {
            Check.Require(m1.Currency.Id == m2.Currency.Id, "Currency is not allowed to defer for math operators");

            return Create(m1.Value / m2.Value, m1.Currency);
        }

        public static Money operator /(Money m1, int value)
        {
            return Create(m1.Value / value, m1.Currency);
        }

        public static Money operator /(int value, Money m2)
        {
            return Create(value / m2.Value, m2.Currency);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="m1"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Money operator *(Money m1, int value)
        {
            return Create(m1.Value * value, m1.Currency);
        }

        #endregion
    }
}
