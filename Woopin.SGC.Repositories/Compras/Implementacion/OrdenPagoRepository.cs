using NHibernate;
using NHibernate.Criterion;
using NHibernate.Dialect.Function;
using NHibernate.Transform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Common.LinqHelpers;
using Woopin.SGC.Model.Common;
using Woopin.SGC.Model.Compras;
using Woopin.SGC.Model.Negocio;
using Woopin.SGC.Repositories.Compras.Helpers;
using Woopin.SGC.Repositories.Helpers;

namespace Woopin.SGC.Repositories.Compras
{
    public class OrdenPagoRepository : BaseSecuredRepository<OrdenPago>, IOrdenPagoRepository
    {
        public OrdenPagoRepository(IHibernateSessionFactory hibernateSessionFactory)
            : base(hibernateSessionFactory)
        {
        }

        public IList<OrdenPago> GetAllByProveedor(int IdProveedor, DateTime start, DateTime end)
        {
            return this.GetSessionFactory().GetSession().QueryOver<OrdenPago>()
                                                        .Where(c => c.Fecha >= start && c.Fecha <= end && (c.Proveedor.Id == IdProveedor || IdProveedor == 0))
                                                        .GetByPermissions()
                                                        .GetFilterBySecurity()
                                                        .OrderBy(x => x.Fecha).Desc
                                                        .List();
        }

        public string GetProximoComprobante()
        {
            string ProximoNumeroComprobante = "";

            OrdenPago ultimaComprobante = this.GetSessionFactory().GetSession().QueryOver<OrdenPago>()
                                                                                      .GetFilterBySecurity()
                                                                                      .OrderBy(x => x.Numero).Desc
                                                                                      .Take(1).SingleOrDefault();
            // No tiene Comprobante
            if (ultimaComprobante == null)
            {
                ProximoNumeroComprobante = ComprobanteHelper.PrimerComprobante;
            }
            else
            {
                ProximoNumeroComprobante = ComprobanteHelper.GetProximoComprobante(ultimaComprobante.Numero);
            }

            return ProximoNumeroComprobante;
        }

        public int GetProximoNumeroReferencia()
        {
            OrdenPago ultimaComprobante = this.GetSessionFactory().GetSession().QueryOver<OrdenPago>()
                                                                                .GetFilterBySecurity()                                                               
                                                                                .OrderBy(x => x.Id).Desc
                                                                               .Take(1)
                                                                               .SingleOrDefault();

            return ultimaComprobante != null ? ultimaComprobante.Id + 1 : EntidadHelper.PrimerNumeroReferencia;
        }

        public OrdenPago GetCompleto(int Id)
        {
            OrdenPago op = this.GetSessionFactory().GetSession().QueryOver<OrdenPago>()
                                                        .Where(x => x.Id == Id)
                                                        .GetFilterBySecurity()
                                                        .Fetch(x => x.Comprobantes).Default
                                                        .TransformUsing(Transformers.DistinctRootEntity)
                                                        .Fetch(x => x.Proveedor).Eager
                                                        .Fetch(x => x.Pagos).Eager
                                                        .Fetch(x => x.Organizacion).Eager
                                                        .SingleOrDefault();
            if (op == null) return null;

            // Limpio variables innecesarias para no tener conflictos de Lazyload en JSON
            foreach(var Comprobante in op.Comprobantes)
            {
                Comprobante.ComprobanteCompra.Detalle = null;
                Comprobante.ComprobanteCompra.Asiento = null;
            }
            foreach(var Pago in op.Pagos)
            {
                Pago.Valor.CuentaContable = null;
            }

            op.Usuario = null;
            foreach (OrdenPagoComprobanteItem cc in op.Comprobantes)
            {
                cc.ComprobanteCompra.Usuario = null;
            }

            // Fix temporal
            op.Comprobantes = op.Comprobantes.Distinct().ToList();

            return op;
        }
       

    }
}
