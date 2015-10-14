using System.Collections.Generic;
using System.Xml.Linq;
using EcoHotels.Core.Domain.Value_objects;

namespace EcoHotels.Core.Infrastructure.Services
{
    public interface IImportCurrenciesService
    {
        XDocument ReadData();

        IEnumerable<Currency> ProcessData();
    }
}
