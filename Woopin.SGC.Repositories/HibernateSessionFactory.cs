using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.NHMapping.Helpers;

namespace Woopin.SGC.Repositories
{
    using System;
    using System.Configuration;

    using NHMapping;

    using FluentNHibernate.Cfg;
    using FluentNHibernate.Cfg.Db;

    using NHibernate;
    using NHibernate.Proxy.DynamicProxy;
    using NHibernate.Tool.hbm2ddl;

    using Configuration = NHibernate.Cfg.Configuration;
    using Model;

    /// <summary>
    /// The hibernate session factory.
    /// </summary>
    public class HibernateSessionFactory : IHibernateSessionFactory
    {
        /// <summary>
        /// The session factory.
        /// </summary>
        private ISessionFactory sessionFactory;

        /// <summary>
        /// The session.
        /// </summary>
        private ISession session;

        /// <summary>
        /// The get hibernate session factory.
        /// </summary>
        /// <returns>
        /// The Repository.HibernateSessionFactory.
        /// </returns>
        public static HibernateSessionFactory GetHibernateSessionFactory()
        {
            return null;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HibernateSessionFactory"/> class.
        /// </summary>
        public HibernateSessionFactory()
        {
            var connString = ConfigurationManager.ConnectionStrings["Production"].ConnectionString;
            this.sessionFactory = Fluently.Configure()
                                .Database(MsSqlConfiguration.MsSql2008.ConnectionString(connString).ShowSql())
                                .Mappings(m =>
                                {
                                    m.FluentMappings.AddFromAssemblyOf<UsuarioMap>();
                                    m.FluentMappings.AddFromAssemblyOf<CuentaMap>();
                                    m.FluentMappings.AddFromAssemblyOf<SucursalMap>();
                                    m.FluentMappings.AddFromAssemblyOf<MonedaMap>();
                                    m.FluentMappings.AddFromAssemblyOf<LocalizacionMap>();
                                })
                                .ExposeConfiguration(BuildSchema)
                                .BuildSessionFactory();
        }

        /// <summary>
        /// The get session.
        /// </summary>
        /// <returns>
        /// The NHibernate.ISession.
        /// </returns>
        public ISession GetSession()
        {
            return this.session;
        }

        /// <summary>
        /// Defines boundaries for a session and a transaction
        /// </summary>
        /// <param name="action">The action</param>
        public void TransactionalInterceptor(Action action)
        {
            using (this.session = this.sessionFactory.OpenSession())
            {
                using (var transaction = this.session.BeginTransaction())
                {
                    try
                    {
                        action();
                        transaction.Commit();
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        throw e;
                    }
                    
                }
            }
        }

        /// <summary>
        /// Defines boundaries for a session
        /// </summary>
        /// <param name="action">The action</param>
        public void SessionInterceptor(Action action)
        {
            using (this.session = this.sessionFactory.OpenSession())
            {
                action();
            }
        }

        /// <summary>
        /// The dispose.
        /// </summary>
        public void Dispose()
        {
            //hibernateSessionFactory = null;
        }

        /// <summary>
        /// The build schema.
        /// </summary>
        /// <param name="config">
        /// The config.
        /// </param>
        private static void BuildSchema(Configuration config)
        {
            // Added FKs Index support.
            config.CreateIndexesForForeignKeys();

            var createSchema = ConfigurationManager.AppSettings["CreateDBSchema"];
            var generateSchema = !string.IsNullOrEmpty(createSchema) && bool.Parse(createSchema);

            var updateSchema = ConfigurationManager.AppSettings["UpdateSchema"];
            var doUpdate = !string.IsNullOrEmpty(createSchema) && bool.Parse(updateSchema);

            // This NHibernate tool takes a configuration (with mapping info in) and exports a database schema from it
            var schemaExport = new SchemaExport(config);


            if (doUpdate)
            {
                var schemaUpdate = new SchemaUpdate(config);
                schemaUpdate.Execute(true, true);
            }
            schemaExport.Drop(false, generateSchema);
            schemaExport.Create(false, generateSchema);
        }

        /// <summary>
        /// No remove this method
        /// </summary>
        private static void ReferByteCode()
        {
            //Just to make sure the ByteCodeCastle is loaded
            ProxyFactory fake = new ProxyFactory();
        }


    }
}
