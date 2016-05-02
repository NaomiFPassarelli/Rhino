namespace Woopin.SGC.Repositories
{
    using System.Collections.Generic;
    using Woopin.SGC.CommonApp.Security;
    using Woopin.SGC.CommonApp.Session;
    using Woopin.SGC.Model.Common;
    using Woopin.SGC.Model.Exceptions;
    using Woopin.SGC.Repositories.Helpers;
    /// <summary>
    /// The base repository.
    /// </summary>
    /// <typeparam name="T">The domain class</typeparam>
    public abstract class BaseSecuredRepository<T> where T : class, ISecuredEntity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseRepository{T}"/> class.
        /// </summary>
        /// <param name="hibernateSessionFactory">
        /// The hibernate Session Factory.
        /// </param>
        protected BaseSecuredRepository(IHibernateSessionFactory hibernateSessionFactory)
        {
            this.hibernateSessionFactory = hibernateSessionFactory;
        }

        /// <summary>
        /// The get.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The T.
        /// </returns>
        public T Get(int id)
        {
            T model = (T)this.GetSessionFactory().GetSession().Get(typeof(T), id);
            if (model == null || model.Organizacion.Id != Security.GetOrganizacion().Id)
            {
                return null;
            }
            return model;
        }


        /// <summary>
        /// The get all.
        /// </summary>
        /// <returns>
        /// The System.Collections.Generic.IList`1[T -&gt; T].
        /// </returns>
        public virtual IList<T> GetAll()
        {
            return this.GetSessionFactory().GetSession().QueryOver<T>()
                                                        .GetFilterBySecurity()
                                                        .List();
        }

        /// <summary>
        /// The add.
        /// </summary>
        /// <param name="o">
        /// The o.
        /// </param>
        public virtual void Add(T o)
        {
            o.Organizacion = Security.GetOrganizacion();
            this.GetSessionFactory().GetSession().Save(o);
        }

        /// <summary>
        /// The remove.
        /// </summary>
        /// <param name="o">
        /// The o.
        /// </param>
        public void Delete(T o)
        {
            if (o.Organizacion.Id != Security.GetOrganizacion().Id)
            {
                throw new SecurityException("Not Allowed!");
            }
            this.GetSessionFactory().GetSession().Delete(o);
        }

        /// <summary>
        /// The update.
        /// </summary>
        /// <param name="o">
        /// The o.
        /// </param>
        public void Update(T o)
        {
            if (o.Organizacion.Id != Security.GetOrganizacion().Id)
            {
                throw new SecurityException("Not Allowed!");
            }
            this.GetSessionFactory().GetSession().Update(o);
        }

        /// <summary>
        /// The hibernate session factory.
        /// </summary>
        private IHibernateSessionFactory hibernateSessionFactory;

        /// <summary>
        /// The get session factory.
        /// </summary>
        /// <returns>
        /// The Repository.IHibernateSessionFactory.
        /// </returns>
        public IHibernateSessionFactory GetSessionFactory()
        {
            return this.hibernateSessionFactory;
        }
    }
}
