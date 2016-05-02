using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Model.Ventas;
using Woopin.SGC.Model.Common;
using Woopin.SGC.Common.HtmlModel;
using NHibernate.Criterion;
using Woopin.SGC.Repositories.Helpers;


namespace Woopin.SGC.Repositories.Ventas
{
    public class ImputacionVentaRepository : BaseSecuredRepository<ImputacionVenta>, IImputacionVentaRepository
    {
        public ImputacionVentaRepository(IHibernateSessionFactory hibernateSessionFactory)
            : base(hibernateSessionFactory)
        {
        }

        public override IList<ImputacionVenta> GetAll()
        {
            return this.GetSessionFactory().GetSession().QueryOver<ImputacionVenta>()
                                                        .GetFilterBySecurity()
                                                        .OrderBy(x => x.Fecha).Desc.List();
        }

        public ImputacionVenta GetCompleto(int Id)
        {
            ImputacionVenta imputacion = this.GetSessionFactory().GetSession().QueryOver<ImputacionVenta>()
                                                        .Where(x => x.Id == Id)
                                                        .GetFilterBySecurity()
                                                        .Fetch(x => x.ComprobanteADescontar).Eager
                                                        .Fetch(x => x.NotaCredito).Eager
                                                        .SingleOrDefault();


            imputacion.ComprobanteADescontar.Organizacion = null;

            return imputacion;
        }

        public IList<ImputacionVenta> GetAllByCliente(int IdCliente, DateTime start, DateTime end)
        {
            IList<ImputacionVenta> imputaciones = this.GetSessionFactory().GetSession().QueryOver<ImputacionVenta>()
                .Where(i => i.Fecha >= start && i.Fecha <= end)
                .GetFilterBySecurity()
                .OrderBy(i => i.Fecha).Desc
                .Fetch(i => i.NotaCredito).Eager
                .JoinQueryOver(i => i.NotaCredito.Cliente)
                .Where(p => p.Id == IdCliente || IdCliente == 0)
                .List();

            foreach (var item in imputaciones)
            {
                item.ComprobanteADescontar.Organizacion = null;
                item.NotaCredito.Organizacion = null;
            }

            return imputaciones;
        }

        public IList<ImputacionVenta> GetAllByComprobante(int IdComprobante)
        {
            // TODO - SEGURIZAR - Segirozar!
            return this.GetSessionFactory().GetSession().QueryOver<ImputacionVenta>()
                .Where(i => i.ComprobanteADescontar.Id == IdComprobante)
                .GetFilterBySecurity()
                .List();
        }
    }
}
