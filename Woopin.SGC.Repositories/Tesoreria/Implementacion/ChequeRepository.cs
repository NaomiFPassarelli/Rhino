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
    public class ChequeRepository : BaseSecuredRepository<Cheque>, IChequeRepository
    {
        public ChequeRepository(IHibernateSessionFactory hibernateSessionFactory)
            : base(hibernateSessionFactory)
        {
        }

        public override IList<Cheque> GetAll()
        {
            return this.GetSessionFactory().GetSession().QueryOver<Cheque>().Where(c => c.Estado != EstadoCheque.Borrador).GetFilterBySecurity().List();
        }

        public IList<Cheque> GetAllChequesEnCartera()
        {
            return this.GetSessionFactory().GetSession().QueryOver<Cheque>().Where(c => c.Estado == EstadoCheque.Cartera).GetFilterBySecurity().List();
        }

        public IList<Cheque> GetChequeFilter(int IdCliente, DateTime start, DateTime end, FilterCheque filter, int IdBanco)
        {
            List<EstadoCheque> estados = ValoresHelper.GetEstadosChequesByfilter(filter);
            Cheque chAlias = null;

            return this.GetSessionFactory().GetSession().QueryOver<Cheque>(() => chAlias)
                                    .Where(() => chAlias.FechaCreacion >= start && chAlias.FechaCreacion <= end && (chAlias.Cliente.Id == IdCliente || IdCliente == 0) && (chAlias.Banco.Id == IdBanco || IdBanco == 0))
                                    .WhereRestrictionOn(() => chAlias.Estado).IsIn(estados)
                                    .GetFilterBySecurity()
                                    .OrderBy(x => x.FechaCreacion)
                                    .Desc.List();
            
        }

    }
}
