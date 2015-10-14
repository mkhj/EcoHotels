using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EcoHotels.Core.Domain.Value_objects;
using EcoHotels.Extensions;

namespace EcoHotels.Web.UI.Areas.Admin.Models
{
    public class EditCurrencyModel
    {
        public EditCurrencyModel(){}

        public EditCurrencyModel(IEnumerable<Currency> currencies)
        {
            currencies.OrderBy(x => x.Name);

            Items = currencies.Select(x => new EditCurrencyItemModel(
                        x.Id,
                        x.ISO_4217_Number,
                        x.Name,
                        x.ISOCurrencySymbol,
                        x.ExchangeRate
                    )
                );
        }

        public IEnumerable<EditCurrencyItemModel> Items { get; set; }
    }

    public class EditCurrencyItemModel
    {
        /// <summary>
        /// Required.
        /// </summary>
        public EditCurrencyItemModel() { }

        public EditCurrencyItemModel(int id, int isoCode, string name, string symbol, decimal conversionFactor)
        {
            Id = id;
            Name = name;
            IsoCode = isoCode;
            Symbol = symbol;
            ConversionFactor = conversionFactor;
        }

        public int Id { get; set; }

        [ReadOnly(true)]
        public int IsoCode { get; set; }

        [ReadOnly(true)]
        public string Name { get; set; }

        [ReadOnly(true)]
        public string Symbol { get; set; }

        public decimal ConversionFactor { get; set; }
    }

    public class RefreshCurrencyModel
    {
        public RefreshCurrencyModel() { }

        public RefreshCurrencyModel(IEnumerable<Currency> newCurrencies, IEnumerable<Currency> oldCurrencies)
        {
            newCurrencies.OrderBy(x => x.Name);

            var result = new List<RefreshCurrencyItemModel>();
            foreach (var newCurrency in newCurrencies)
            {
                var currency = oldCurrencies.FirstOrDefault(x => x.Id == newCurrency.Id);
                if(currency.IsNotNull())
                {
                    var item = new RefreshCurrencyItemModel(newCurrency.Id, newCurrency.ISO_4217_Number, newCurrency.Name, newCurrency.ISOCurrencySymbol, newCurrency.ExchangeRate, currency.ExchangeRate);
                    result.Add(item);
                }

                
            }

            Items = result;
        }

        public List<RefreshCurrencyItemModel> Items { get; set; }
    }

    public class RefreshCurrencyItemModel
    {
                /// <summary>
        /// Required.
        /// </summary>
        public RefreshCurrencyItemModel() { }

        public RefreshCurrencyItemModel(int id, int isoCode, string name, string symbol, decimal newConversionFactor, decimal oldConversionFactor)
        {
            Id = id;
            Name = name;
            IsoCode = isoCode;
            Symbol = symbol;
            NewConversionFactor = newConversionFactor;
            OldConversionFactor = oldConversionFactor;
        }

        public int Id { get; set; }

        [ReadOnly(true)]
        public int IsoCode { get; set; }

        [ReadOnly(true)]
        public string Name { get; set; }

        [ReadOnly(true)]
        public string Symbol { get; set; }

        public decimal NewConversionFactor { get; set; }

        [ReadOnly(true)]
        public decimal OldConversionFactor { get; set; }
    }

}