using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Model.Ventas;

namespace Woopin.SGC.Repositories.Ventas
{
    public class DetalleComprobanteVentaRepository : BaseRepository<DetalleComprobanteVenta>, IDetalleComprobanteVentaRepository
    {
        public DetalleComprobanteVentaRepository(IHibernateSessionFactory hibernateSessionFactory)
            : base(hibernateSessionFactory)
        {
        }

    }
}
