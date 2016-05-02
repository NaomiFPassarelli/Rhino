using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.CommonApp.Session;
using Woopin.SGC.Model.Common;

namespace Woopin.SGC.Repositories.Common
{
    public interface IGeneralRepository
    {
        IHibernateSessionFactory GetSessionFactory();
        Dashboard GetDashboard();
        SessionData CreateSessionData(Usuario user);
        SessionData CreateSessionData(JobHeader header);
    }
}
