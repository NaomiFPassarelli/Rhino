using NHibernate;
using NHibernate.Criterion;
using NHibernate.Dialect.Function;
using NHibernate.Transform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Model.Compras;
using Woopin.SGC.Model.Contabilidad;
using Woopin.SGC.Model.Tesoreria;
using Woopin.SGC.Model.Ventas;
using Woopin.SGC.Repositories.Helpers;

namespace Woopin.SGC.Repositories.Tesoreria
{
    public class ComprobanteRetencionRepository : BaseSecuredRepository<ComprobanteRetencion>, IComprobanteRetencionRepository
    {
        public ComprobanteRetencionRepository(IHibernateSessionFactory hibernateSessionFactory)
            : base(hibernateSessionFactory)
        {
        }

        public override IList<ComprobanteRetencion> GetAll()
        {
            return this.GetSessionFactory().GetSession().QueryOver<ComprobanteRetencion>()
                                                        .Where(c => c.Estado != EstadoRetencion.Borrador && c.Estado != EstadoRetencion.Anulada)
                                                        .GetFilterBySecurity()
                                                        .List();
        }

        public IList<ComprobanteRetencion> GetRetencionFilter(int TipoRetencion, int IdProveedor, int IdCliente, DateTime start, DateTime end)
        {
            IList<ComprobanteRetencion> crs = this.GetSessionFactory().GetSession().QueryOver<ComprobanteRetencion>()
                                    .Where(x => x.FechaCreacion >= start && x.FechaCreacion <= end && (x.Retencion.Id == TipoRetencion || TipoRetencion == 0) && (x.Cliente.Id == IdCliente || IdCliente == 0) && (x.Proveedor.Id == IdProveedor || IdProveedor == 0))
                                    .GetFilterBySecurity()
                                    .OrderBy(x => x.FechaCreacion)
                                    .Desc.List();

            foreach (ComprobanteRetencion cr in crs)
            {
                cr.Usuario = null;
            }
            return crs;

        }

        public IList<ComprobanteRetencionReporte> GetRetencionFilterReporte(int TipoRetencion, int IdProveedor, int IdCliente, DateTime start, DateTime end)
        {
            ComprobanteRetencion c = null;
            ComprobanteRetencionReporte cr = null;
            Retencion rAlias = null;
            Proveedor pAlias = null;
            Cliente cAlias = null;
            return this.GetSessionFactory().GetSession().QueryOver<ComprobanteRetencion>(() => c)
                                .Left.JoinAlias(() => c.Cliente, () => cAlias)
                                .Left.JoinAlias(() => c.Proveedor, () => pAlias)
                                .JoinAlias(() => c.Retencion, () => rAlias)
                                .Where(() => c.FechaCreacion >= start && c.FechaCreacion <= end && (c.Retencion.Id == TipoRetencion || TipoRetencion == 0) && (c.Cliente.Id == IdCliente || IdCliente == 0) && (c.Proveedor.Id == IdProveedor || IdProveedor == 0))
                                .GetFilterBySecurity()
                                    .SelectList(list => list
                                        .Select(() => c.Id).WithAlias(() => cr.Id)
                                        .Select(() => c.Numero).WithAlias(() => cr.Numero)
                                        .Select(() => c.NumeroRetencion).WithAlias(() => cr.NumeroRetencion)
                                        .Select(() => pAlias.Id).WithAlias(() => cr.ProveedorId)
                                        .Select(() => pAlias.RazonSocial).WithAlias(() => cr.ProveedorRazonSocial)
                                        .Select(() => pAlias.CUIT).WithAlias(() => cr.ProveedorCUIT)
                                        .Select(() => rAlias.Id).WithAlias(() => cr.RetencionId)
                                        .Select(() => rAlias.Descripcion).WithAlias(() => cr.RetencionDescripcion)
                                        .Select(() => rAlias.Abreviatura).WithAlias(() => cr.RetencionAbreviatura)
                                        .Select(() => rAlias.Juridiccion.Id).WithAlias(() => cr.RetencionJuridiccionId)
                                        .Select(() => c.Total).WithAlias(() => cr.Total)
                                        .Select(() => cAlias.Id).WithAlias(() => cr.ClienteId)
                                        .Select(() => cAlias.RazonSocial).WithAlias(() => cr.ClienteRazonSocial)
                                        .Select(() => cAlias.CUIT).WithAlias(() => cr.ClienteCUIT)
                                        .Select(() => c.Estado).WithAlias(() => cr.Estado)
                                        .Select(() => c.Fecha).WithAlias(() => cr.Fecha)
                                        .Select(Projections.Alias(
                                                Projections.Conditional(Restrictions.IsNull("Cliente"),
                                                    Projections.Property("Total"),
                                                    Projections.Constant(0, NHibernateUtil.Decimal)), "Debe"
                                            ))
                                        .Select(Projections.Alias(
                                                Projections.Conditional(Restrictions.IsNull("Proveedor"),
                                                    Projections.Property("Total"),
                                                    Projections.Constant(0, NHibernateUtil.Decimal)), "Haber"
                                            ))
                                    )
                                    .OrderBy(x => x.Fecha)
                                    .Desc
                                    .TransformUsing(Transformers.AliasToBean<ComprobanteRetencionReporte>())
                                    .List<ComprobanteRetencionReporte>();

        }
    }
}
