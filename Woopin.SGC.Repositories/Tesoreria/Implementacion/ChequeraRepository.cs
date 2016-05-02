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
    public class ChequeraRepository : BaseSecuredRepository<Chequera>, IChequeraRepository
    {
        public ChequeraRepository(IHibernateSessionFactory hibernateSessionFactory)
            : base(hibernateSessionFactory)
        {
        }

        public override IList<Chequera> GetAll()
        {
            return this.GetSessionFactory().GetSession().QueryOver<Chequera>().GetFilterBySecurity().List();
        }

        public IList<Chequera> FindChequera(int CuentaBancaria, int NumeroDesde, int NumeroHasta)
        {
            //listado porque puede ser que justo este agarrando los valores de dos chequeras 1 a 100 , 100 a 1000 y quiero insertar 50 a 150
            return this.GetSessionFactory().GetSession().QueryOver<Chequera>()
                                                        .Where(x => x.CuentaBancaria.Id == CuentaBancaria && ((x.NumeroHasta >= NumeroDesde && x.NumeroDesde <= NumeroDesde) || (x.NumeroDesde <= NumeroHasta && x.NumeroHasta >= NumeroHasta)))
                                                        .GetFilterBySecurity()
                                                        .OrderBy(x => x.Id).Asc
                                                        .List();
        }

        public Chequera ControlChequePropioChequera(int IdCuentaBancaria, int Numero)
        {
            return this.GetSessionFactory().GetSession().QueryOver<Chequera>()
                                                        .Where(x => x.CuentaBancaria.Id == IdCuentaBancaria && x.NumeroDesde <= Numero && x.NumeroHasta >= Numero)
                                                        .GetFilterBySecurity()
                                                        .SingleOrDefault();
        }
        

    }
}
