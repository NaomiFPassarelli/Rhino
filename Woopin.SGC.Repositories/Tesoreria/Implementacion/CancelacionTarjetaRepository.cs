using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Model.Tesoreria;

namespace Woopin.SGC.Repositories.Tesoreria
{
    public class CancelacionTarjetaRepository : BaseRepository<CancelacionTarjeta>, ICancelacionTarjetaRepository
    {
        public CancelacionTarjetaRepository(IHibernateSessionFactory hibernateSessionFactory)
            : base(hibernateSessionFactory)
        {
        }

    }
}
