using NHibernate;
using NHibernate.Transform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Model.Compras;
using Woopin.SGC.Model.Negocio;
using Woopin.SGC.Repositories.Compras.Helpers;
using Woopin.SGC.Repositories.Helpers;

namespace Woopin.SGC.Repositories.Compras
{
    public class OtroEgresoRepository : BaseSecuredRepository<OtroEgreso>, IOtroEgresoRepository
    {
        public OtroEgresoRepository(IHibernateSessionFactory hibernateSessionFactory)
            : base(hibernateSessionFactory)
        {
        }

        public IList<OtroEgreso> GetAllByProveedor(int IdProveedor, DateTime start, DateTime end)
        {
            return this.GetSessionFactory().GetSession().QueryOver<OtroEgreso>()
                                                        .Where(c => c.Fecha >= start && c.Fecha <= end && (c.Proveedor.Id == IdProveedor || IdProveedor == 0))
                                                        .GetFilterBySecurity()
                                                        .GetByPermissions()
                                                        .OrderBy(x => x.Fecha).Desc.List();
        }


        public int GetProximoNumeroReferencia()
        {
            OtroEgreso ultimaComprobante = this.GetSessionFactory().GetSession().QueryOver<OtroEgreso>()
                                                                                .GetFilterBySecurity()
                                                                                .OrderBy(x => x.Id).Desc
                                                                                .Take(1).SingleOrDefault();

            return ultimaComprobante != null ? ultimaComprobante.Id + 1 : EntidadHelper.PrimerNumeroReferencia;
        }

        public OtroEgreso GetCompleto(int Id)
        {
            OtroEgreso oe = this.GetSessionFactory().GetSession().QueryOver<OtroEgreso>()
                                                        .Where(x => x.Id == Id)
                                                        .GetFilterBySecurity()
                                                        .Fetch(x => x.Detalle).Eager
                                                        .TransformUsing(Transformers.DistinctRootEntity)
                                                        .Fetch(x => x.Proveedor).Eager
                                                        .Fetch(x => x.Organizacion).Eager
                                                        .SingleOrDefault();
            if (oe == null) return null;


            // Limpio variables innecesarias para no tener conflictos de Lazyload en JSON
            foreach (var Pago in oe.Pagos)
            {
                Pago.Valor.CuentaContable = null;
            }
            NHibernateUtil.Initialize(oe.Pagos);

            return oe;
        }
    }
}
