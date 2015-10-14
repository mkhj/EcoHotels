using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Xml.Linq;
using EcoHotels.Core.Domain.Value_objects;
using EcoHotels.Extensions;

namespace EcoHotels.Core.Infrastructure.Services.Impl
{
    public class ImportCurrenciesService : IImportCurrenciesService
    {
        private ICurrencyService CurrencyService;

        public ImportCurrenciesService(ICurrencyService currencyService)
        {
            CurrencyService = currencyService;
        }

        public XDocument ReadData()
        {
            var req = WebRequest.Create("http://www.nationalbanken.dk/dndk/valuta.nsf/valuta.xml");                     
            var resp = req.GetResponse();
            var sr = new System.IO.StreamReader(resp.GetResponseStream());
            return XDocument.Load(sr);
        }

        public IEnumerable<Currency> ProcessData()
        {
            var document = ReadData();

            var result = new Collection<Currency>();

            var xElements = document.Descendants("currency");
            foreach (var xElement in xElements)
            {
                var code = xElement.Attribute("code");
                var rate = xElement.Attribute("rate");

                var currency = CurrencyService.FindByISOSymbol(code.Value);
                if(currency.IsNotNull())
                {
                    var clone = currency.Clone();
                    clone.ExchangeRate = rate.Value.ToDecimal();

                    result.Add(clone);
                }
            }

            return result;
        }
    }
}
