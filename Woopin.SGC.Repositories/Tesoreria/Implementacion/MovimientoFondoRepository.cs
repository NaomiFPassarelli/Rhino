using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Common.HtmlModel;
using Woopin.SGC.Model.Common;
using Woopin.SGC.Model.Tesoreria;
using Woopin.SGC.Repositories.Helpers;

namespace Woopin.SGC.Repositories.Tesoreria
{
    public class MovimientoFondoRepository : BaseSecuredRepository<MovimientoFondo>, IMovimientoFondoRepository
    {
        public MovimientoFondoRepository(IHibernateSessionFactory hibernateSessionFactory)
            : base(hibernateSessionFactory)
        {
        }

        public override IList<MovimientoFondo> GetAll()
        {
            return this.GetSessionFactory().GetSession().QueryOver<MovimientoFondo>()
                                                        .GetFilterBySecurity()
                                                        .Fetch(item => item.Movimiento).Eager
                                                        .OrderBy(x => x.Fecha).Desc
                                                        .List();
        }

        public IList<MovimientoFondo> GetAllByDates(DateTime start, DateTime end)
        {
            IList<MovimientoFondo> list = this.GetSessionFactory().GetSession().QueryOver<MovimientoFondo>()
                                                        .Where(X => X.Fecha >= start && X.Fecha <= end)
                                                        .GetFilterBySecurity()
                                                        .Fetch(item => item.Movimiento).Eager
                                                        .Fetch(item => item.Caja).Eager
                                                        .Fetch(item => item.CuentaDestino).Eager
                                                        .OrderBy(x => x.Fecha).Desc
                                                        .List();

            foreach(var item in list)
            {
                item.Asiento = null;
                item.Usuario = null;
            }

            return list;
        }

        
    }
}
