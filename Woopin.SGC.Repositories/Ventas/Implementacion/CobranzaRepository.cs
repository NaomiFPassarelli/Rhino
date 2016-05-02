using NHibernate.Criterion;
using NHibernate.Transform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Model.Negocio;
using Woopin.SGC.Model.Ventas;
using Woopin.SGC.Repositories.Helpers;

namespace Woopin.SGC.Repositories.Ventas
{
    public class CobranzaRepository : BaseSecuredRepository<Cobranza>, ICobranzaRepository
    {
        public CobranzaRepository(IHibernateSessionFactory hibernateSessionFactory)
            : base(hibernateSessionFactory)
        {
        }

        public string GetProximoRecibo(string talonario)
        {
            string ProximoRecibo = "";

            Cobranza ultimaCobranza = this.GetSessionFactory().GetSession().QueryOver<Cobranza>()
                                                                            .OrderBy(x => x.Numero).Desc
                                                                            .Where(Restrictions.On<Cobranza>(x => x.Numero).IsLike(talonario + '%'))
                                                                            .GetFilterBySecurity().Take(1).SingleOrDefault();
            
            if (ultimaCobranza == null) // Es la primera
            {
                ProximoRecibo = talonario + "-00000001";
            }
            else
            {
                ProximoRecibo = ComprobanteHelper.GetProximoComprobante(ultimaCobranza.Numero);
            }

            return ProximoRecibo;
        }

        public int GetProximoIdCobranza()
        {
            int ProximoId = EntidadHelper.PrimerNumeroReferencia;
            Cobranza ultimaCobranza = this.GetSessionFactory().GetSession().QueryOver<Cobranza>()
                                                                         .OrderBy(x => x.Id).Desc
                                                                         .GetFilterBySecurity()
                                                                         .Take(1)
                                                                         .SingleOrDefault();
            if (ultimaCobranza != null)
            {
                ProximoId = ultimaCobranza.Id + 1;
            }
               

            return ProximoId;
        }

        public Cobranza GetCompleto(int Id)
        {
            Cobranza cobranza = this.GetSessionFactory().GetSession().QueryOver<Cobranza>()
                                                        .Where(x => x.Id == Id)
                                                        .GetFilterBySecurity()
                                                        .Fetch(x => x.Comprobantes).Default
                                                        .Fetch(x => x.Organizacion).Eager
                                                        .TransformUsing(Transformers.DistinctRootEntity)
                                                        .Fetch(x => x.Cliente).Eager
                                                        .Fetch(x => x.Valores).Eager
                                                        .SingleOrDefault();

            if (cobranza == null) return null;

            // Limpio variables innecesarias para no tener conflictos de Lazyload en JSON
            foreach (var Comprobante in cobranza.Comprobantes)
            {
                Comprobante.ComprobanteVenta.Detalle = null;
                Comprobante.ComprobanteVenta.Asiento = null;
                Comprobante.ComprobanteVenta.Observaciones = null;
                Comprobante.ComprobanteVenta.Usuario = null;
                Comprobante.ComprobanteVenta.Organizacion = null;
            }
            foreach (var Cobro in cobranza.Valores)
            {
                Cobro.Valor.CuentaContable = null;
            }

            // Fix temporal
            cobranza.Comprobantes = cobranza.Comprobantes.Distinct().ToList();

            return cobranza;
        }

        public IList<Cobranza> GetAllByCliente(int IdCliente, DateTime start, DateTime end)
        {
            return this.GetSessionFactory().GetSession().QueryOver<Cobranza>()
                                                        .Where(c => c.Fecha >= start && c.Fecha <= end 
                                                                            && (c.Cliente.Id == IdCliente || IdCliente == 0))
                                                        .GetFilterBySecurity()
                                                        .OrderBy(x => x.Fecha).Desc.List();
        }
    }
}
