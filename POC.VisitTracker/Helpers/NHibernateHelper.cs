    using FluentNHibernate.Cfg;
    using FluentNHibernate.Cfg.Db;
    using NHibernate;
using System.Reflection;

using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;

    namespace POC.VisitTracker.Helpers
    {
        public static class NHibernateHelper
        {
            private static ISessionFactory _sessionFactory;

            public static ISessionFactory SessionFactory
            {
                get
                {
                    if (_sessionFactory == null)
                        InitializeSessionFactory();
                    return _sessionFactory;
                }
            }

            private static void InitializeSessionFactory()
            {
                _sessionFactory = Fluently.Configure()
                    .Database(
                        MsSqlConfiguration.MsSql2012
                        .ConnectionString(c => c.FromConnectionStringWithKey("POCVisits"))
                    )
                    .Mappings(m => m.FluentMappings.AddFromAssembly(Assembly.GetExecutingAssembly()))
                    .BuildSessionFactory();
            }

            public static IStatelessSession OpenStatelessSession()
            {
                return SessionFactory.OpenStatelessSession();
            }
        }

    }