using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using EcoHotels.Core;
using EcoHotels.Core.Email;
using EcoHotels.Web.Core.Services;
using Microsoft.Practices.Unity;


namespace EcoHotels.Web.Core
{
    public class UnityDependencyResolver : IDependencyResolver
    {
        private UnityServiceContainer Provider;

        public UnityDependencyResolver()
        {
            Provider = new UnityServiceContainer();

            //
            //Provider.Container.RegisterInstance<ISession>(SessionScopeWebModule.GetCurrentSession(), new TransientLifetimeManager());

            // Services            
            Provider.Container.RegisterType<IAppService, AppService>();
            Provider.Container.RegisterType<ILocationService, LocationService>();
            Provider.Container.RegisterType<IFormsAuthenticationService, FormsAuthenticationService>();
            Provider.Container.RegisterType<IEmailService, SendGridEmailService>();
            Provider.Container.RegisterType<ICartService, CartService>();
            
            //  Register all controller type found in current assembly to the Unity container will be able to resolve them
            foreach (Type controllerType in (from t in Assembly.GetExecutingAssembly().GetTypes() where typeof(IController).IsAssignableFrom(t) select t))
            {
                this.Provider.Container.RegisterType(controllerType);
            }
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
                return Provider.TryGetService(serviceType);
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
                return this.Provider.TryGetServices(serviceType);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return null;
            } 
        }
    }
}