using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Model.Tesoreria;
using Woopin.SGC.Repositories.Helpers;

namespace Woopin.SGC.Repositories.Tesoreria
{
    public class TransferenciaRepository : BaseSecuredRepository<Transferencia>, ITransferenciaRepository
    {
        public TransferenciaRepository(IHibernateSessionFactory hibernateSessionFactory)
            : base(hibernateSessionFactory)
        {
        }

        public override IList<Transferencia> GetAll()
        {
            return this.GetSessionFactory().GetSession().QueryOver<Transferencia>().Where(c => c.Estado == EstadoTransferencia.Creado).GetFilterBySecurity().List();
        }

        public IList<Transferencia> GetTransferenciaFilter(int IdCuentaBancaria, int IdProveedor, int IdCliente, DateTime start, DateTime end)
        {
            return this.GetSessionFactory().GetSession().QueryOver<Transferencia>()
                                    .Where(x => x.FechaCreacion >= start && x.FechaCreacion <= end && (x.CuentaBancaria.Id == IdCuentaBancaria || IdCuentaBancaria == 0) && (x.Cliente.Id == IdCliente || IdCliente == 0) && (x.Proveedor.Id == IdProveedor || IdProveedor == 0) && x.Estado == EstadoTransferencia.Creado)
                                    .GetFilterBySecurity()
                                    .OrderBy(x => x.FechaCreacion)
                                    .Desc.List();
            
        }
    }
}
