using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EcoHotels.Core.Domain.Models;
using EcoHotels.Core.Infrastructure.Services;
using EcoHotels.Extensions;

namespace EcoHotels.Web.Core.Services
{
    public interface ILocationService
    {
        Country FindLocationByIp(string ip);
    }

    public class LocationService : ILocationService
    {
        private IIP2LocationService Ip2LocationService;
        private ICountryService CountryService;

        public LocationService(IIP2LocationService ip2LocationService, ICountryService countryService)
        {
            Ip2LocationService = ip2LocationService;
            CountryService = countryService;
        }

        public Country FindLocationByIp(string ip)
        {
            var location = Ip2LocationService.GetLocation(ip);
            if(location == "-")
            {
                return CountryService.FindByISOCode("en");
            }

            var originCountry = CountryService.FindByISOCode(location);

            return (originCountry.IsNotNull()) ? originCountry : CountryService.FindByISOCode("en");
        }
    }
}
