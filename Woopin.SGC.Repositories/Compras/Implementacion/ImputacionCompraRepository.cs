using NHibernate;
using NHibernate.Criterion;
using NHibernate.Dialect.Function;
using NHibernate.Transform;
using NHibernate.Type;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Common.Helpers;
using Woopin.SGC.Common.LinqHelpers;
using Woopin.SGC.Model.Common;
using Woopin.SGC.Model.Compras;
using Woopin.SGC.Model.Negocio;
using Woopin.SGC.Repositories.Compras.Helpers;
using Woopin.SGC.Repositories.Helpers;

namespace Woopin.SGC.Repositories.Compras
{
    public class ImputacionCompraRepository : BaseSecuredRepository<ImputacionCompra>, IImputacionCompraRepository
    {
        public ImputacionCompraRepository(IHibernateSessionFactory hibernateSessionFactory)
            : base(hibernateSessionFactory)
        {
        }

        public override IList<ImputacionCompra> GetAll()
        {
            return this.GetSessionFactory().GetSession().QueryOver<ImputacionCompra>()
                                                        .GetFilterBySecurity()
                                                        .OrderBy(x => x.Fecha).Desc.List();
        }

        public ImputacionCompra GetCompleto(int Id)
        {
            ImputacionCompra imputacion = this.GetSessionFactory().GetSession().QueryOver<ImputacionCompra>()
                                                        .Where(x => x.Id == Id)
                                                        .GetFilterBySecurity()
                                                        .Fetch(x => x.ComprobanteADescontar).Eager
                                                        .Fetch(x => x.NotaCredito).Eager
                                                        .SingleOrDefault();


            return imputacion;
        }

        public IList<ImputacionCompra> GetAllByProveedor(int IdProveedor, DateTime start, DateTime end)
        {
            return this.GetSessionFactory().GetSession().QueryOver<ImputacionCompra>()
                .Where(i => i.Fecha >= start && i.Fecha <= end)
                .GetFilterBySecurity()
                .OrderBy(i => i.Fecha).Desc
                .Fetch(i => i.NotaCredito).Eager
                .JoinQueryOver(i => i.NotaCredito.Proveedor)                                        
                .Where(p => p.Id == IdProveedor || IdProveedor == 0)
                .List();
        }

        public IList<ImputacionCompra> GetAllByComprobante(int IdComprobante)
        {
            // TODO - SEGURIZAR - Segirozar!
            return this.GetSessionFactory().GetSession().QueryOver<ImputacionCompra>()
                .Where(i => i.ComprobanteADescontar.Id == IdComprobante)
                .GetFilterBySecurity()
                .List();
        }
        
    }
}
