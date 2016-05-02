using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Model.Negocio;
using Woopin.SGC.Model.Tesoreria;
using Woopin.SGC.Repositories.Helpers;

namespace Woopin.SGC.Repositories.Tesoreria
{
    public class ChequePropioRepository : BaseSecuredRepository<ChequePropio>, IChequePropioRepository
    {
        public ChequePropioRepository(IHibernateSessionFactory hibernateSessionFactory)
            : base(hibernateSessionFactory)
        {
        }

        public override IList<ChequePropio> GetAll()
        {
            return this.GetSessionFactory().GetSession().QueryOver<ChequePropio>().Where(c => c.Estado != EstadoCheque.Borrador).GetFilterBySecurity().List();
        }

        public IList<ChequePropio> GetAllByFilter(int IdProveedor, int IdCuenta, DateTime start, DateTime end,FilterCheque filter)
        {
            List<EstadoCheque> estados = ValoresHelper.GetEstadosChequesByfilter(filter);
            ChequePropio chAlias = null;


            return this.GetSessionFactory().GetSession().QueryOver<ChequePropio>(() => chAlias)
                                    .Where(() => chAlias.FechaCreacion >= start && chAlias.FechaCreacion <= end && (chAlias.Proveedor.Id == IdProveedor || IdProveedor == 0) && (chAlias.CuentaBancaria.Id == IdCuenta || IdCuenta == 0))
                                    .WhereRestrictionOn(() => chAlias.Estado).IsIn(estados)
                                    .GetFilterBySecurity()
                                    .OrderBy(x => x.FechaCreacion)
                                    .Desc.List();
        }

        public IList<ChequePropio> GetAllInChequera(int IdCuentaBancaria, int NumeroDesde, int NumeroHasta)
        {
            IList<ChequePropio> ChequesPropios = this.GetSessionFactory().GetSession().QueryOver<ChequePropio>()
                                                .Where(x => x.CuentaBancaria.Id == IdCuentaBancaria && x.Numero >= NumeroDesde && x.Numero <= NumeroHasta && x.Estado != EstadoCheque.Borrador)
                                                .GetFilterBySecurity()
                                                    .OrderBy(x => x.Numero).Asc
                                                    .List();
            foreach (ChequePropio cp in ChequesPropios)
            {
                cp.Usuario = null;
            }
            return ChequesPropios;
        }

        public ChequePropio GetByFilter(int IdCuentaBancaria, int Numero)
        {
            return this.GetSessionFactory().GetSession().QueryOver<ChequePropio>()
                .Where(x => x.CuentaBancaria.Id == IdCuentaBancaria && x.Numero == Numero && x.Estado != EstadoCheque.Borrador)
                .GetFilterBySecurity()
                .SingleOrDefault();
        }


    }
}
