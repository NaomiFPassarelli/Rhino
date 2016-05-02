namespace Woopin.SGC.Repositories
{
    using System.Collections.Generic;

    /// <summary>
    /// The Repository interface.
    /// </summary>
    /// <typeparam name="T">the domain class</typeparam>
    public interface IRepository<T> where T : class
    {
        /// <summary>
        /// The get.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The T.
        /// </returns>
        T Get(int id);

        /// <summary>
        /// The add.
        /// </summary>
        /// <param name="t">
        /// The t.
        /// </param>
        void Add(T t);

        /// <summary>
        /// The remove.
        /// </summary>
        /// <param name="t">
        /// The t.
        /// </param>
        void Delete(T t);


        /// <summary>
        /// The update.
        /// </summary>
        /// <param name="t">
        /// The t.
        /// </param>
        void Update(T t);


        /// <summary>
        /// The get all.
        /// </summary>
        /// <returns>
        /// The System.Collections.Generic.IList`1[T -&gt; T].
        /// </returns>
        IList<T> GetAll();

        /// <summary>
        /// The get session factory.
        /// </summary>
        /// <returns>
        /// The Repository.IHibernateSessionFactory.
        /// </returns>
        IHibernateSessionFactory GetSessionFactory();
    }
}
