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
    public class DepositoRepository : BaseSecuredRepository<Deposito>, IDepositoRepository
    {
        public DepositoRepository(IHibernateSessionFactory hibernateSessionFactory)
            : base(hibernateSessionFactory)
        {
        }

        public int GetProximoNumeroReferencia()
        {
            Deposito ultimoDeposito = this.GetSessionFactory().GetSession().QueryOver<Deposito>()
                                                                            .GetFilterBySecurity()
                                                                            .OrderBy(x => x.Id).Desc
                                                                            .Take(1)
                                                                            .SingleOrDefault();

            return ultimoDeposito != null ? ultimoDeposito.Id + 1 : EntidadHelper.PrimerNumeroReferencia;
        }

        public Deposito GetDepositoCompleto(int Id)
        {
            Deposito d = this.GetSessionFactory().GetSession().QueryOver<Deposito>()
                                                        .Where(x => x.Id == Id)
                                                        .GetFilterBySecurity()
                                                        .Fetch(x => x.Cheques).Eager
                                                        .Fetch(x => x.Organizacion).Eager
                                                        .SingleOrDefault();
            d.Usuario = null;
            d.Asiento = null;
            foreach(DepositoItem di in d.Cheques)
            {
                di.Cheque.Usuario = null;
            }
            return d;
        }

        public IList<Deposito> GetDepositoFilter(int idCuentaBancaria, DateTime start, DateTime end)
        {
            return this.GetSessionFactory().GetSession().QueryOver<Deposito>()
                                    .Where(x => x.FechaCreacion >= start && x.FechaCreacion <= end && (x.Cuenta.Id == idCuentaBancaria || idCuentaBancaria == 0))
                                    .GetFilterBySecurity()
                                    .OrderBy(x => x.FechaCreacion)
                                    .Desc.List();
        }
    }
}
