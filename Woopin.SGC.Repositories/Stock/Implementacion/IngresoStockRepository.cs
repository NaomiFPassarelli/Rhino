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
using Woopin.SGC.Model.Stock;

namespace Woopin.SGC.Repositories.Stock
{
    public class IngresoStockRepository : BaseSecuredRepository<IngresoStock>, IIngresoStockRepository
    {
        public IngresoStockRepository(IHibernateSessionFactory hibernateSessionFactory)
            : base(hibernateSessionFactory)
        {
        }

        public override IList<IngresoStock> GetAll()
        {
            return this.GetSessionFactory().GetSession().QueryOver<IngresoStock>().GetFilterBySecurity().OrderBy(c => c.FechaCreacion).Desc().List();
        }

        public IList<IngresoStock> GetAllByArticulo(int IdArticulo, DateTime _start, DateTime _end)
        {
            return this.GetSessionFactory().GetSession().QueryOver<IngresoStock>()
                                                        .GetFilterBySecurity()
                                                        .Where(c => c.FechaCreacion >= _start && c.FechaCreacion <= _end && (c.Articulo.Id == IdArticulo || IdArticulo == 0))
                                                        .OrderBy(x => x.FechaCreacion).Desc
                                                        .List();
        }

        //TODO PONER FILTRO DE FECHA
        public IList<IngresoStock> GetAllByFilter(SelectComboRequest req)
        {
            return this.GetSessionFactory().GetSession().QueryOver<IngresoStock>()
                                                        .And(Expression.Eq("Activo", true))
                                                        .GetFilterBySecurity()
                                                        .List();
        }
        //TODO PONER FILTRO DE FECHA
        public IList<IngresoStock> GetAllByFilter(PagingRequest req)
        {
            // Este puede tener informacion de paginado para traer solo algunos registros y ordenamiento

            return this.GetSessionFactory().GetSession().QueryOver<IngresoStock>()
                                                        .And(Expression.Eq("Activo", true))
                                                        .GetFilterBySecurity()
                                                        .List();
        }

    }
}
