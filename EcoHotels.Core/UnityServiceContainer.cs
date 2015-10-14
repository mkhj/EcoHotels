using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using EcoHotels.Core.Infrastructure.Cache;
using EcoHotels.Core.Infrastructure.Services;
using EcoHotels.Core.Infrastructure.Services.Impl;
using EcoHotels.Core.Infrastructure.Services.Impl.Media;
using EcoHotels.Core.Infrastructure.Services.Impl.Price;
using EcoHotels.Core.Infrastructure.Services.Impl.Property;
using Microsoft.Practices.Unity;
using NHibernate;

namespace EcoHotels.Core
{
    public class UnityServiceContainer
    {
        public UnityContainer Container { get; private set; }

        public UnityServiceContainer()
        {
            Container = new UnityContainer();

            // Repositories
            //container.RegisterType<IAccountService, AccountService>();

            // Services            

            Container.RegisterType<IIP2LocationService, IP2LocationService>();
            Container.RegisterType<ICountryService, CountryService>();
            

            Container.RegisterType<ILanguageService, LanguageService>();
            Container.RegisterType<ICurrencyService, CurrencyService>();
            Container.RegisterType<IPageInformationService, PageInformationService>();

            Container.RegisterType<IOrganizationService, OrganizationService>();
            Container.RegisterType<IHotelService, HotelService>();
            Container.RegisterType<IRoomTypeService, RoomTypeService>();
            Container.RegisterType<IAmenityService, AmenityService>();

            // Commerce
            Container.RegisterType<IAddonService, AddonService>();
            Container.RegisterType<IRateService, RateService>();

            Container.RegisterType<IUserService, UserService>();

            Container.RegisterType<ICustomerService, CustomerService>();
            Container.RegisterType<IReservationService, ReservationService>();
            Container.RegisterType<IInventoryService, InventoryService>();
            Container.RegisterType<ICalendarService, CalendarService>();
            Container.RegisterType<ISearchService, SearchService>();
            

            Container.RegisterType<IAssetService, AssetService>();
            Container.RegisterType<IAssetCategoryService, AssetCategoryService>();

            // other forms of services

            Container.RegisterType<IImportCurrenciesService, ImportCurrenciesService>();

            // Caching Strategies
            Container.RegisterType<ICacheStorage, HttpContextCacheAdapter>();
        }

        public object GetService(Type serviceType)
        {
            //  Unity "Resolve" method throws exception so we need to wrap it up with non throwing method (like MVC expects)
            return TryGetService(serviceType);
        }

        public object TryGetService(Type serviceType)
        {
            try
            {
                return Container.Resolve(serviceType);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return null;
            }
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            //  Unity "Resolve" method throws exception so we need to wrap it up with non throwing method (like MVC expects)
            return TryGetServices(serviceType);
        }

        public IEnumerable<object> TryGetServices(Type serviceType)
        {
            try
            {
                return Container.ResolveAll(serviceType);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return null;
            }
        }
    }
}
