using System.Collections.Generic;
using EcoHotels.Core.Domain.Models;

namespace EcoHotels.Core.Infrastructure.Services
{
    public interface ICountryService
    {
        Country FindByISOCode(string code);

        IEnumerable<Country> FindAll();
    }
}
