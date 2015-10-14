using System;
using EcoHotels.Core.Domain.Value_objects;

namespace EcoHotels.Core.Infrastructure.Services
{
    public interface ISearchService
    {
        SearchResultHotel FindByHotel(int hotelId);

        SearchResultHotel FindByHotel(int hotelId, DateTime arrival, DateTime depature);

        SearchResultList FindByCity(int searchTagCityId, DateTime arrival, DateTime depature);

    }
}
