using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Model.Compras;

namespace Woopin.SGC.Repositories.Compras
{
    public class DetalleComprobanteCompraRepository : BaseRepository<DetalleComprobanteCompra>, IDetalleComprobanteCompraRepository
    {
        public DetalleComprobanteCompraRepository(IHibernateSessionFactory hibernateSessionFactory)
            : base(hibernateSessionFactory)
        {
        }
    }
}
