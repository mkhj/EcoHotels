using System.Collections.Generic;
using EcoHotels.Core.Infrastructure.Cache;
using EcoHotels.Core.Domain.Value_objects;
using EcoHotels.Core.Infrastructure.NH;
using NHibernate.Criterion;

namespace EcoHotels.Core.Infrastructure.Services.Impl.Price
{
    public class CurrencyService : ICurrencyService
    {
        private ICacheStorage CacheStorage;

        private Repository<Currency> CurrencyRepo;

        private List<Currency> AvailableSystemCurrencies { get; set; }

        public CurrencyService(ICacheStorage cacheStorage)
        {            
            CacheStorage = cacheStorage;

            CurrencyRepo = new Repository<Currency>();
        }

        public Currency FindById(int id)
        {
            return CurrencyRepo.FindBy(id);
        }

        public Currency FindByISOSymbol(string symbol)
        {
            var criteria = DetachedCriteria.For(typeof(Currency))
                .Add(Restrictions.Eq("ISOCurrencySymbol", symbol));

            return CurrencyRepo.FindOne(criteria);
        }

        public IEnumerable<Currency> FindAll()
        {
            return CurrencyRepo.FindAll();
        }

        public void Save(Currency currency)
        {
            if(currency.IsValid())
            {
                CurrencyRepo.Save(currency);
            }
        }

        public void Save(IEnumerable<Currency> currencies)
        {
            foreach (var currency in currencies)
            {
                Save(currency);
            }
        }

        public void Delete(Currency currency)
        {
            CurrencyRepo.Remove(currency);
        }

        private void InitSystemCurrencies()
        {
            // http://en.wikipedia.org/wiki/ISO_4217

            AvailableSystemCurrencies = new List<Currency>();

            //AvailableSystemCurrencies.Add(Currency.Create("United Arab Emirates dirham", 784, "AED", 0, ".", ","));
            //AvailableSystemCurrencies.Add(Currency.Create("Afghan afghani", 971, "AFN", 0, ".", ","));
            //AvailableSystemCurrencies.Add(Currency.Create("Albanian lek", 008, "ALL", 0, ".", ","));
            //AvailableSystemCurrencies.Add(Currency.Create("Armenian dram", 051, "AMD", 0, ".", ","));
            //AvailableSystemCurrencies.Add(Currency.Create("Netherlands Antillean guilder", 532, "ANG", 0, ".", ","));
            //AvailableSystemCurrencies.Add(Currency.Create("Angolan kwanza", 973, "AOA", 0, ".", ","));
            //AvailableSystemCurrencies.Add(Currency.Create("Argentine peso", 032, "ARS", 0, ".", ","));
            AvailableSystemCurrencies.Add(Currency.Create("Australian dollar", 036, "AUD", 0));
            //AvailableSystemCurrencies.Add(Currency.Create("Aruban florin", 533, "AWG", 0, ".", ","));
            //AvailableSystemCurrencies.Add(Currency.Create("Azerbaijani manat", 944, "AZN", 0, ".", ","));
            //AvailableSystemCurrencies.Add(Currency.Create("Bosnia and Herzegovina convertible mark", 977, "BAM", 0, ".", ","));
            //AvailableSystemCurrencies.Add(Currency.Create("Barbados dollar", 052, "BDD", 0, ".", ","));
            //AvailableSystemCurrencies.Add(Currency.Create("Bangladeshi taka", 050, "BDT", 0, ".", ","));
            //AvailableSystemCurrencies.Add(Currency.Create("Bulgarian lev", 975, "BGN", 0, ".", ","));
            //AvailableSystemCurrencies.Add(Currency.Create("Bahraini dinar", 048, "BHD", 0, ".", ","));
            //AvailableSystemCurrencies.Add(Currency.Create("Burundian franc", 108, "BIF", 0, ".", ","));
            //AvailableSystemCurrencies.Add(Currency.Create("Bermudian dollar", 060, "BMD", 0, ".", ","));
            //AvailableSystemCurrencies.Add(Currency.Create("Brunei dollar", 096, "BND", 0, ".", ","));
            //AvailableSystemCurrencies.Add(Currency.Create("Boliviano", 068, "BOB", 0, ".", ","));
            //AvailableSystemCurrencies.Add(Currency.Create("Brazilian real", 986, "BRL", 0, ".", ","));
            //AvailableSystemCurrencies.Add(Currency.Create("Bahamian dollar", 044, "BSD", 0, ".", ","));
            //AvailableSystemCurrencies.Add(Currency.Create("Bhutanese ngultrum", 064, "BTN", 0, ".", ","));
            //AvailableSystemCurrencies.Add(Currency.Create("Botswana pula", 072, "BWP", 0, ".", ","));
            //AvailableSystemCurrencies.Add(Currency.Create("Belarusian ruble", 974, "BYR", 0, ".", ","));
            //AvailableSystemCurrencies.Add(Currency.Create("Belize dollar", 084, "BZD", 0, ".", ","));
            //AvailableSystemCurrencies.Add(Currency.Create("Canadian dollar", 124, "CAD", 0, ".", ","));
            //AvailableSystemCurrencies.Add(Currency.Create("Congolese franc", 976, "CDF", 0, ".", ","));
            AvailableSystemCurrencies.Add(Currency.Create("Swiss franc", 756, "CHF", 0));
            //AvailableSystemCurrencies.Add(Currency.Create("Chilean peso", 152, "CLP", 0, ".", ","));
            AvailableSystemCurrencies.Add(Currency.Create("Chinese yuan", 156, "CNY", 0));
            //AvailableSystemCurrencies.Add(Currency.Create("Colombian peso", 170, "COP", 0, ".", ","));
            //AvailableSystemCurrencies.Add(Currency.Create("Costa Rican colon", 188, "CRC", 0, ".", ","));
            //AvailableSystemCurrencies.Add(Currency.Create("Costa Rican colon", 192, "CUP", 0, ".", ","));
            //AvailableSystemCurrencies.Add(Currency.Create("Cape Verde escudo", 132, "CVE", 0, ".", ","));
            //AvailableSystemCurrencies.Add(Currency.Create("Czech koruna", 203, "CZK", 0, ".", ","));
            //AvailableSystemCurrencies.Add(Currency.Create("Djiboutian franc", 262, "DJF", 0, ".", ","));
            AvailableSystemCurrencies.Add(Currency.Create("Danish krone", 208, "DKK", 0));
            //AvailableSystemCurrencies.Add(Currency.Create("Dominican peso", 214, "DOP", 0, ".", ","));
            //AvailableSystemCurrencies.Add(Currency.Create("Algerian dinar", 012, "DZD", 0, ".", ","));
            //AvailableSystemCurrencies.Add(Currency.Create("Egyptian pound", 818, "EGP", 0, ".", ","));
            //AvailableSystemCurrencies.Add(Currency.Create("Eritrean nakfa", 232, "ERN", 0, ".", ","));
            //AvailableSystemCurrencies.Add(Currency.Create("Ethiopian birr", 230, "ETB", 0, ".", ","));
            AvailableSystemCurrencies.Add(Currency.Create("Euro", 978, "EUR", 0));
            //AvailableSystemCurrencies.Add(Currency.Create("Fiji dollar", 242, "FJD", 0, ".", ","));
            //AvailableSystemCurrencies.Add(Currency.Create("Falkland Islands pound", 238, "FKP", 0, ".", ","));
            AvailableSystemCurrencies.Add(Currency.Create("Pound sterling", 826, "GBP", 0));
            //AvailableSystemCurrencies.Add(Currency.Create("Georgian lari", 981, "GEL", 0, ".", ","));
            //AvailableSystemCurrencies.Add(Currency.Create("Ghanaian cedi", 936, "GHS", 0, ".", ","));
            //AvailableSystemCurrencies.Add(Currency.Create("Gibraltar pound", 292, "GIP", 0, ".", ","));
            //AvailableSystemCurrencies.Add(Currency.Create("Gambian dalasi", 270, "GMD", 0, ".", ","));
            //AvailableSystemCurrencies.Add(Currency.Create("Guinean franc", 324, "GNF", 0, ".", ","));
            //AvailableSystemCurrencies.Add(Currency.Create("Guatemalan quetzal", 320, "GTQ", 0, ".", ","));
            //AvailableSystemCurrencies.Add(Currency.Create("Guyanese dollar", 328, "GYD", 0, ".", ","));
            AvailableSystemCurrencies.Add(Currency.Create("Hong Kong dollar", 344, "HKD", 0));
            //AvailableSystemCurrencies.Add(Currency.Create("Honduran lempira", 340, "HNL", 0, ".", ","));
            //AvailableSystemCurrencies.Add(Currency.Create("Croatian kuna", 191, "HRK", 0, ".", ","));
            //AvailableSystemCurrencies.Add(Currency.Create("Haitian gourde", 332, "HTG", 0, ".", ","));
            //AvailableSystemCurrencies.Add(Currency.Create("Hungarian forint", 348, "HUF", 0, ".", ","));
            //AvailableSystemCurrencies.Add(Currency.Create("Indonesian rupiah", 360, "IDR", 0, ".", ","));
            //AvailableSystemCurrencies.Add(Currency.Create("Israeli new sheqel", 376, "ILS", 0, ".", ","));
            //AvailableSystemCurrencies.Add(Currency.Create("Indian rupee", 356, "INR", 0, ".", ","));
            //AvailableSystemCurrencies.Add(Currency.Create("Iraqi dinar", 368, "IQD", 0, ".", ","));
            //AvailableSystemCurrencies.Add(Currency.Create("Iranian rial", 364, "IRR", 0, ".", ","));
            //AvailableSystemCurrencies.Add(Currency.Create("Icelandic króna", 352, "ISK", 0, ".", ","));
            //AvailableSystemCurrencies.Add(Currency.Create("Jamaican dollar", 388, "JMD", 0, ".", ","));
            //AvailableSystemCurrencies.Add(Currency.Create("Jordanian dinar", 400, "JOD", 0, ".", ","));
            AvailableSystemCurrencies.Add(Currency.Create("Japanese yen", 392, "JPY", 0));
            //AvailableSystemCurrencies.Add(Currency.Create("Kenyan shilling", 404, "KES", 0, ".", ","));
            //AvailableSystemCurrencies.Add(Currency.Create("Kyrgyzstani som", 417, "KGS", 0, ".", ","));
            //AvailableSystemCurrencies.Add(Currency.Create("Cambodian riel", 116, "KHR", 0, ".", ","));
            //AvailableSystemCurrencies.Add(Currency.Create("Comoro franc", 174, "KMF", 0, ".", ","));
            //AvailableSystemCurrencies.Add(Currency.Create("North Korean won", 408, "KPW", 0, ".", ","));
            //AvailableSystemCurrencies.Add(Currency.Create("South Korean won", 410, "KRW", 0, ".", ","));
            //AvailableSystemCurrencies.Add(Currency.Create("Kuwaiti dinar", 414, "KWD", 0, ".", ","));
            //AvailableSystemCurrencies.Add(Currency.Create("Cayman Islands dollar", 136, "KYD", 0, ".", ","));
            //AvailableSystemCurrencies.Add(Currency.Create("Kazakhstani tenge", 398, "KZT", 0, ".", ","));
            //AvailableSystemCurrencies.Add(Currency.Create("Lao kip", 418, "LAK", 0, ".", ","));
            //AvailableSystemCurrencies.Add(Currency.Create("Lebanese pound", 422, "LBP", 0, ".", ","));
            //AvailableSystemCurrencies.Add(Currency.Create("Sri Lankan rupee", 144, "LKR", 0, ".", ","));
            //AvailableSystemCurrencies.Add(Currency.Create("Liberian dollar", 430, "LRD", 0, ".", ","));
            //AvailableSystemCurrencies.Add(Currency.Create("Lesotho loti", 426, "LSL", 0, ".", ","));
            //AvailableSystemCurrencies.Add(Currency.Create("Lithuanian litas", 440, "LTL", 0, ".", ","));
            //AvailableSystemCurrencies.Add(Currency.Create("Latvian lats", 428, "LVL", 0, ".", ","));
            //AvailableSystemCurrencies.Add(Currency.Create("Libyan dinar", 434, "LYD", 0, ".", ","));
            //AvailableSystemCurrencies.Add(Currency.Create("Moroccan dirham", 504, "MAD", 0, ".", ","));
            //AvailableSystemCurrencies.Add(Currency.Create("Moldovan leu", 498, "MDL", 0, ".", ","));
            //AvailableSystemCurrencies.Add(Currency.Create("Malagasy ariary", 969, "MGA", 0, ".", ","));
            //AvailableSystemCurrencies.Add(Currency.Create("Macedonian denar", 807, "MKD", 0, ".", ","));
            //AvailableSystemCurrencies.Add(Currency.Create("Myanma kyat", 104, "MMK", 0, ".", ","));
            //AvailableSystemCurrencies.Add(Currency.Create("Mongolian tugrik", 496, "MNT", 0, ".", ","));
            //AvailableSystemCurrencies.Add(Currency.Create("Macanese pataca", 446, "MOP", 0, ".", ","));
            //AvailableSystemCurrencies.Add(Currency.Create("Mauritanian ouguiya", 478, "MRO", 0, ".", ","));
            //AvailableSystemCurrencies.Add(Currency.Create("Mauritian rupee", 480, "MUR", 0, ".", ","));
            //AvailableSystemCurrencies.Add(Currency.Create("Maldivian rufiyaa", 462, "MVR", 0, ".", ","));
            //AvailableSystemCurrencies.Add(Currency.Create("Malawian kwacha", 454, "MWK", 0, ".", ","));
            //AvailableSystemCurrencies.Add(Currency.Create("Mexican peso", 484, "MXN", 0, ".", ","));
            //AvailableSystemCurrencies.Add(Currency.Create("Malaysian ringgit", 458, "MYR", 0, ".", ","));
            //AvailableSystemCurrencies.Add(Currency.Create("Mozambican metical", 943, "MZN", 0, ".", ","));
            //AvailableSystemCurrencies.Add(Currency.Create("Namibian dollar", 516, "NAD", 0, ".", ","));
            //AvailableSystemCurrencies.Add(Currency.Create("Nigerian naira", 566, "NGN", 0, ".", ","));
            //AvailableSystemCurrencies.Add(Currency.Create("Nicaraguan córdoba", 558, "NIO", 0, ".", ","));
            AvailableSystemCurrencies.Add(Currency.Create("Norwegian krone", 578, "NOK", 0));
            //AvailableSystemCurrencies.Add(Currency.Create("Nepalese rupee", 524, "NPR", 0, ".", ","));
            //AvailableSystemCurrencies.Add(Currency.Create("New Zealand dollar", 554, "NZD", 0, ".", ","));
            //AvailableSystemCurrencies.Add(Currency.Create("Omani rial", 512, "OMR", 0, ".", ","));
            //AvailableSystemCurrencies.Add(Currency.Create("Panamanian balboa", 590, "PAB", 0, ".", ","));
            //AvailableSystemCurrencies.Add(Currency.Create("Peruvian nuevo sol", 604, "PEN", 0, ".", ","));
            //AvailableSystemCurrencies.Add(Currency.Create("Papua New Guinean kina", 598, "PGK", 0, ".", ","));
            //AvailableSystemCurrencies.Add(Currency.Create("Philippine peso", 608, "PHP", 0, ".", ","));
            //AvailableSystemCurrencies.Add(Currency.Create("Pakistani rupee", 586, "PKR", 0, ".", ","));
            //AvailableSystemCurrencies.Add(Currency.Create("Polish złoty", 985, "PLN", 0, ".", ","));
            //AvailableSystemCurrencies.Add(Currency.Create("Paraguayan guaraní", 600, "PYG", 0, ".", ","));
            //AvailableSystemCurrencies.Add(Currency.Create("Qatari rial", 634, "QAR", 0, ".", ","));
            //AvailableSystemCurrencies.Add(Currency.Create("Romanian new leu", 946, "RON", 0, ".", ","));
            //AvailableSystemCurrencies.Add(Currency.Create("Serbian dinar", 941, "RSD", 0, ".", ","));
            AvailableSystemCurrencies.Add(Currency.Create("Russian rouble", 643, "RUB", 0));
            //AvailableSystemCurrencies.Add(Currency.Create("Rwandan franc", 646, "RWF", 0, ".", ","));
            //AvailableSystemCurrencies.Add(Currency.Create("Saudi riyal", 682, "SAR", 0, ".", ","));
            //AvailableSystemCurrencies.Add(Currency.Create("Solomon Islands dollar", 090, "SBD", 0, ".", ","));
            //AvailableSystemCurrencies.Add(Currency.Create("Seychelles rupee", 690, "SCR", 0, ".", ","));
            //AvailableSystemCurrencies.Add(Currency.Create("Sudanese pound", 938, "SDG", 0, ".", ","));
            AvailableSystemCurrencies.Add(Currency.Create("Swedish krona/kronor", 752, "SEK", 0));
            //AvailableSystemCurrencies.Add(Currency.Create("Singapore dollar", 702, "SGD", 0, ".", ","));
            //AvailableSystemCurrencies.Add(Currency.Create("Saint Helena pound", 654, "SHP", 0, ".", ","));
            //AvailableSystemCurrencies.Add(Currency.Create("Sierra Leonean leone", 694, "SLL", 0, ".", ","));
            //AvailableSystemCurrencies.Add(Currency.Create("Somali shilling", 706, "SOS", 0, ".", ","));
            //AvailableSystemCurrencies.Add(Currency.Create("Surinamese dollar", 968, "SRD", 0, ".", ","));
            //AvailableSystemCurrencies.Add(Currency.Create("South Sudanese pound", 728, "SSP", 0, ".", ","));
            //AvailableSystemCurrencies.Add(Currency.Create("São Tomé and Príncipe dobra", 678, "STD", 0, ".", ","));
            //AvailableSystemCurrencies.Add(Currency.Create("Syrian pound", 760, "SYP", 0, ".", ","));
            //AvailableSystemCurrencies.Add(Currency.Create("Swazi lilangeni", 748, "SZL", 0, ".", ","));
            //AvailableSystemCurrencies.Add(Currency.Create("Thai baht", 764, "THB", 0, ".", ","));
            //AvailableSystemCurrencies.Add(Currency.Create("Tajikistani somoni", 972, "TJS", 0, ".", ","));
            //AvailableSystemCurrencies.Add(Currency.Create("Turkmenistani manat", 934, "TMT", 0, ".", ","));
            //AvailableSystemCurrencies.Add(Currency.Create("Tunisian dinar", 788, "TND", 0, ".", ","));
            //AvailableSystemCurrencies.Add(Currency.Create("Tongan paʻanga", 776, "TOP", 0, ".", ","));
            //AvailableSystemCurrencies.Add(Currency.Create("Turkish lira", 949, "TRY", 0, ".", ","));
            //AvailableSystemCurrencies.Add(Currency.Create("Trinidad and Tobago dollar", 780, "TTD", 0, ".", ","));
            //AvailableSystemCurrencies.Add(Currency.Create("New Taiwan dollar", 901, "TWD", 0, ".", ","));
            //AvailableSystemCurrencies.Add(Currency.Create("Tanzanian shilling", 834, "TZS", 0, ".", ","));
            //AvailableSystemCurrencies.Add(Currency.Create("Ukrainian hryvnia", 980, "UAH", 0, ".", ","));
            //AvailableSystemCurrencies.Add(Currency.Create("Ugandan shilling", 800, "UGX", 0, ".", ","));
            AvailableSystemCurrencies.Add(Currency.Create("United States dollar", 840, "USD", 0));
            //AvailableSystemCurrencies.Add(Currency.Create("Uruguayan peso", 858, "UYU", 0, ".", ","));
            //AvailableSystemCurrencies.Add(Currency.Create("Uzbekistan som", 860, "UZS", 0, ".", ","));
            //AvailableSystemCurrencies.Add(Currency.Create("Venezuelan bolívar fuerte", 937, "VEF", 0, ".", ","));
            //AvailableSystemCurrencies.Add(Currency.Create("Vietnamese đồng", 704, "VND", 0, ".", ","));
            //AvailableSystemCurrencies.Add(Currency.Create("Vanuatu vatu", 548, "VUV", 0, ".", ","));
            //AvailableSystemCurrencies.Add(Currency.Create("Samoan tala", 882, "WST", 0, ".", ","));
            //AvailableSystemCurrencies.Add(Currency.Create("CFA franc BEAC", 950, "XAF", 0, ".", ","));
            //AvailableSystemCurrencies.Add(Currency.Create("East Caribbean dollar", 951, "XCD", 0, ".", ","));
            //AvailableSystemCurrencies.Add(Currency.Create("CFA Franc BCEAO", 952, "XOF", 0, ".", ","));
            //AvailableSystemCurrencies.Add(Currency.Create("CFP franc", 953, "XPF", 0, ".", ","));
            //AvailableSystemCurrencies.Add(Currency.Create("Yemeni rial", 886, "YER", 0, ".", ","));
            //AvailableSystemCurrencies.Add(Currency.Create("South African rand", 710, "ZAR", 0, ".", ","));
            //AvailableSystemCurrencies.Add(Currency.Create("Zambian kwacha", 894, "ZMK", 0, ".", ","));
            //AvailableSystemCurrencies.Add(Currency.Create("Zimbabwe dollar", 932, "XWL", 0, ".", ","));
        }
    }
}
