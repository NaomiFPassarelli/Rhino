using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Woopin.SGC.Repositories
{
    using System;

    using NHibernate;

    /// <summary>
    /// The HibernateSessionFactory interface.
    /// </summary>
    public interface IHibernateSessionFactory : IDisposable
    {
        /// <summary>
        /// The get session.
        /// </summary>
        /// <returns>
        /// The NHibernate.ISession.
        /// </returns>
        ISession GetSession();

        /// <summary>
        /// Defines boundaries for a session and a transaction
        /// </summary>
        /// <param name="action">the action</param>
        void TransactionalInterceptor(Action action);

        /// <summary>
        /// Defines boundaries for a session
        /// </summary>
        /// <param name="action">the action</param>
        void SessionInterceptor(Action action);
    }
}
