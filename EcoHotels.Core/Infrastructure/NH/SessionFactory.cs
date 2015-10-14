using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Web;
using EcoHotels.Core.Infrastructure.Mappings;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Cache;
using NHibernate.Context;
using NHibernate.Stat;

namespace EcoHotels.Core.Infrastructure.NH
{
    public class SessionFactory
    {
        private static string ConnectionstringName = "ApplicationServices";

        private static ISessionFactory SessionFactoryInstance;

        static SessionFactory()
        {
            Trace.WriteLine("SessionScope Initializing...");
            Init();
        }

        private static void Init()
        {
            if(SessionFactoryInstance != null)
            {
                return;
            }

            if (HttpContext.Current == null)
            {
                // _nhSessionStorageContainer = new ThreadSessionStorageContainer();
                SessionFactoryInstance = Fluently.Configure()
                    //.Database(MsSqlConfiguration.MsSql2008.ConnectionString("Data Source=localhost;Initial Catalog=EcoHotels;Persist Security Info=True;Trusted_Connection=true;").ShowSql())
                    .Database(MsSqlConfiguration.MsSql2008.ConnectionString(x => x.FromConnectionStringWithKey(ConnectionstringName)).ShowSql())
                    .ExposeConfiguration(c => c.SetProperty("current_session_context_class", "thread_static"))
                    .ExposeConfiguration(c => c.SetProperty("generate_statistics", "true"))
                    //.ExposeConfiguration(c => c.SetProperty("use_reflection_optimizer", "true"))
                    .Mappings(m => m.FluentMappings.AddFromAssemblyOf<UserMap>())
                    .Mappings(m => m.FluentMappings.ExportTo("C:\\temp\\"))
                    .Cache(c => c.ProviderClass<NHibernate.Caches.SysCache.SysCacheProvider>().UseQueryCache())
                    .BuildSessionFactory();
            }
            else
            {
                //_nhSessionStorageContainer = new HttpSessionContainer();
                
                SessionFactoryInstance = Fluently.Configure()
                    .Database(MsSqlConfiguration.MsSql2008.ConnectionString(x => x.FromConnectionStringWithKey(ConnectionstringName)))
                    .ExposeConfiguration(c => c.SetProperty("current_session_context_class", "web"))
                    .ExposeConfiguration(c => c.SetProperty("generate_statistics", "true"))
                    .Mappings(m => m.FluentMappings.AddFromAssemblyOf<UserMap>())                    
                    .Cache(c => c.ProviderClass<NHibernate.Caches.SysCache.SysCacheProvider>().UseQueryCache())
                    .BuildSessionFactory();

            }
        }

        public static IStatistics Statistics
        {
            get { return SessionFactoryInstance.Statistics; }
        }

        public static void BeginTransaction()
        {
            Trace.WriteLine("SessionScope BeginTransaction");

            var session = SessionFactoryInstance.OpenSession();
            session.BeginTransaction();
            CurrentSessionContext.Bind(session);
        }

        public static void EndTransaction()
        {
            Trace.WriteLine("SessionScope EndTransaction");

            var session = CurrentSessionContext.Unbind(SessionFactoryInstance);
            if (session == null)
            {
                Trace.WriteLine("Session Unbind failed?");
                return;
            }

            try
            {
                session.Transaction.Commit();
            }
            catch (Exception ex)
            {
                session.Transaction.Rollback();
                Trace.WriteLine(ex.Message);
            }
            finally
            {
                session.Close();
                session.Dispose();
            }
        }

        public static ISession GetCurrentSession()
        {
            return SessionFactoryInstance.GetCurrentSession();
        }
    }
}
