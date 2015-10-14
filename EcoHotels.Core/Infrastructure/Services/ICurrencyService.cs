using System.Collections.Generic;
using EcoHotels.Core.Domain.Value_objects;

namespace EcoHotels.Core.Infrastructure.Services
{
    public interface ICurrencyService
    {
        Currency FindById(int id);

        Currency FindByISOSymbol(string symbol);

        IEnumerable<Currency> FindAll();

        void Save(Currency currency);

        void Save(IEnumerable<Currency> currencies);

        void Delete(Currency currency);
    }
}
