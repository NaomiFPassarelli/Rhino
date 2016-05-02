using NHibernate.Transform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Model.Common;
using Woopin.SGC.Model.Tesoreria;
using Woopin.SGC.Repositories.Helpers;

namespace Woopin.SGC.Repositories.Tesoreria
{
    public class ValorIngresadoRepository : BaseSecuredRepository<ValorIngresado>, IValorIngresadoRepository
    {
        public ValorIngresadoRepository(IHibernateSessionFactory hibernateSessionFactory)
            : base(hibernateSessionFactory)
        {
        }


        public IList<ReporteTesoreria> GetIngresosByDates(DateTime start, DateTime end)
        {
            ValorIngresado vi = null;
            Valor va = null;
            ReporteTesoreria rt = null;

            return this.GetSessionFactory().GetSession().QueryOver<ValorIngresado>(() => vi)
                                                        .JoinAlias(() => vi.Valor, () => va)
                                                        .Where(() => vi.Fecha >= start && vi.Fecha <= end && vi.TipoIngreso == TipoIngreso.Ingreso)
                                                        .GetFilterBySecurity()
                                                        .SelectList(list =>
                                                            list.SelectGroup(() => va.Nombre).WithAlias(() => rt.Valor)
                                                                .SelectGroup(() => vi.Descripcion).WithAlias(() => rt.Descripcion)
                                                                .SelectSum( () => vi.Importe).WithAlias( () => rt.Monto)
                                                        )
                                                        .OrderBy(() => va.Nombre).Asc
                                                        .TransformUsing(Transformers.AliasToBean<ReporteTesoreria>())
                                                        .List<ReporteTesoreria>();
        }

        public IList<ReporteTesoreria> GetEgresosByDates(DateTime start, DateTime end)
        {
            ValorIngresado vi = null;
            Valor va = null;
            ReporteTesoreria rt = null;

            return this.GetSessionFactory().GetSession().QueryOver<ValorIngresado>(() => vi)
                                                        .JoinAlias(() => vi.Valor, () => va)
                                                        .Where(() => vi.Fecha >= start && vi.Fecha <= end && vi.TipoIngreso == TipoIngreso.Egreso)
                                                        .GetFilterBySecurity()
                                                        .SelectList(list =>
                                                            list.SelectGroup(() => va.Nombre).WithAlias(() => rt.Valor)
                                                                .SelectGroup(() => vi.Descripcion).WithAlias(() => rt.Descripcion)
                                                                .SelectSum(() => vi.Importe).WithAlias(() => rt.Monto)
                                                        )
                                                        .OrderBy(() => va.Nombre).Asc
                                                        .TransformUsing(Transformers.AliasToBean<ReporteTesoreria>())
                                                        .List<ReporteTesoreria>();
        }

        public ValorIngresado GetByTipo(int NroReferencia, string TipoValor)
        {
            Valor vAlias = null;
            ComboItem ciAlias = null;

            return this.GetSessionFactory().GetSession().QueryOver<ValorIngresado>()
                                                        .JoinAlias(x => x.Valor, () => vAlias)
                                                        .JoinAlias(() => vAlias.TipoValor, () => ciAlias)
                                                        .Where(x => x.NumeroReferencia == NroReferencia && ciAlias.Data == TipoValor)
                                                        .GetFilterBySecurity()
                                                        .SingleOrDefault();
        }
    }
}
