using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Model.Contabilidad;
using Woopin.SGC.Common.HtmlModel;
using NHibernate.Criterion;
using Woopin.SGC.Repositories.Helpers;
using Woopin.SGC.Model.Negocio;

namespace Woopin.SGC.Repositories.Contabilidad
{
    public class CuentaRepository : BaseSecuredRepository<Cuenta>, ICuentaRepository
    {
        public CuentaRepository(IHibernateSessionFactory hibernateSessionFactory)
            : base(hibernateSessionFactory)
        {
        }

        public override IList<Cuenta> GetAll()
        {
            return this.GetSessionFactory().GetSession().QueryOver<Cuenta>().GetFilterBySecurity().List();
        }

        public IList<Cuenta> GetRubros()
        {
           return this.GetSessionFactory().GetSession().QueryOver<Cuenta>()
                                                       .Where(c => c.Corriente == 0 && c.Numero == 0 && c.SubRubro == 0)
                                                       .GetFilterBySecurity()
                                                       .OrderBy(c => c.Rubro).Asc
                                                       .List();
        }

        public IList<Cuenta> GetCorriente(int Rubro)
        {
            return this.GetSessionFactory().GetSession().QueryOver<Cuenta>()
                                                        .Where(c => c.SubRubro == 0 && c.Rubro == Rubro && c.Corriente != 0 && c.Numero == 0)
                                                        .GetFilterBySecurity()
                                                        .OrderBy(c => c.Rubro).Asc
                                                        .List();
        }

        public IList<Cuenta> GetSubRubros(int Rubro,int Corriente)
        {
            return this.GetSessionFactory().GetSession().QueryOver<Cuenta>()
                                                        .Where(c => c.Numero == 0 && c.Rubro == Rubro && c.Corriente == Corriente 
                                                            && c.SubRubro != 0)
                                                            .GetFilterBySecurity()
                                                        .OrderBy(c => c.Rubro).Asc
                                                        .List();
        }

        public void Create(Cuenta Cuenta)
        {
            if(Cuenta.Codigo == null)
            {
                // Se genero desde cero, sin codigo y sin numero
                Cuenta ultimaCuenta = this.GetSessionFactory().GetSession().QueryOver<Cuenta>()
                                                                            .Where(c => c.Rubro == Cuenta.Rubro && 
                                                                                        c.SubRubro == Cuenta.SubRubro && 
                                                                                        c.Corriente == Cuenta.Corriente)
                                                                                        .GetFilterBySecurity()
                                                                            .OrderBy(c => c.Numero).Desc
                                                                            .Take(1)
                                                                            .SingleOrDefault();
                
                Cuenta.Numero = ultimaCuenta != null ? 1 + ultimaCuenta.Numero : 1;
                Cuenta.CalcularCodigo();
            }
            else
            {
                // Se genero desde codigo.
                Cuenta.ParseCodigo();
            }
            this.Add(Cuenta);
        }

        public IList<Cuenta> GetCuentasdeIngresos()
        {
            return this.GetSessionFactory().GetSession()
                                           .QueryOver<Cuenta>()
                                           .Where(x => x.Rubro == 4 && x.Numero != 0)
                                           .GetFilterBySecurity()
                                           .OrderBy(x => x.SubRubro).Desc
                                           .List();
        }

        public IList<Cuenta> GetCuentasdeEgresos()
        {
            return this.GetSessionFactory().GetSession()
                                           .QueryOver<Cuenta>()
                                           .Where(x => x.Rubro == 5 &&  x.Numero != 0)
                                           .GetFilterBySecurity()
                                           .OrderBy(x => x.SubRubro).Desc
                                           .List();
        }

        public IList<Cuenta> GetSubRubrosEgresos()
        {
            /* TODO 09/09/2014 
             * Para piroska la cuenta de egresos son 4.2.Y.X
             * En cambio en general se usa que todo el 5 sea de ingresos osea 5.X.Y.Z
             * En este caso trae todos los subrubros de gastos para inputar un rubro de compra
             */

            return this.GetSessionFactory().GetSession()
                                           .QueryOver<Cuenta>()
                                           .Where(x => x.Rubro == 5 && x.Numero == 0 && (x.SubRubro != 0 || x.Corriente != 2))
                                           .GetFilterBySecurity()
                                           .OrderBy(x => x.SubRubro).Asc
                                           .List();
        }

        public Cuenta GetCuentaByCodigo(string Codigo)
        {
            return this.GetSessionFactory().GetSession().QueryOver<Cuenta>()
                                                        .Where(x => x.Codigo == Codigo)
                                                        .GetFilterBySecurity()
                                                        .SingleOrDefault();
        }

        public IList<Cuenta> GetAllByFilter(SelectComboRequest req)
        {
            return this.GetSessionFactory().GetSession().QueryOver<Cuenta>()
                                                        .Where((Restrictions.On<Cuenta>(x => x.Nombre).IsLike('%' + req.where + '%') ||
                                                                Restrictions.On<Cuenta>(x => x.Codigo).IsLike('%' + req.where + '%')))
                                                        .And(x => x.Rubro != 0 && x.Corriente != 0 && x.SubRubro != 0 && x.Numero > 0)
                                                        .GetFilterBySecurity()
                                                        .List();
        }
  
    }
}
