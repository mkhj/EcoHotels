using System;
using System.Diagnostics;
using EcoHotels.Core.Infrastructure.Mappings;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Context;

namespace EcoHotels.Core.Tests
{
    public class NHibernateStarter
    {        
        private static ISessionFactory SessionFactory;

        public static bool IsInitialized { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// A static constructor will only be executed once in the lifetime of the application.
        /// </remarks>
        static NHibernateStarter()
        {
            IsInitialized = true;
            CreateSessionFactory();
        }

        public static void BeginTransaction()
        {

            var session = SessionFactory.OpenSession();
            session.BeginTransaction();
            CurrentSessionContext.Bind(session);
        }

        public static ISession GetCurrentSession()
        {
            return SessionFactory.GetCurrentSession();
        }

        public static void EndTransaction()
        {
            Trace.WriteLine("SessionScope End");

            var session = CurrentSessionContext.Unbind(SessionFactory);
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

        private static void CreateSessionFactory()
        {
            Trace.WriteLine("CreateSessionFactory");

            SessionFactory = Fluently.Configure()
                .Database(MsSqlConfiguration.MsSql2008.ConnectionString("Data Source=localhost;Initial Catalog=EcoHotels;Persist Security Info=True;Trusted_Connection=true;").ShowSql())
                .ExposeConfiguration(c => c.SetProperty("current_session_context_class", "thread_static"))
                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<UserMap>())
                .BuildSessionFactory();
        }
    }
}
