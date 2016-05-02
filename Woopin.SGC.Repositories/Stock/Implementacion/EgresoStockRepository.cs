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
    public class EgresoStockRepository : BaseSecuredRepository<EgresoStock>, IEgresoStockRepository
    {
        public EgresoStockRepository(IHibernateSessionFactory hibernateSessionFactory)
            : base(hibernateSessionFactory)
        {
        }

        public override IList<EgresoStock> GetAll()
        {
            return this.GetSessionFactory().GetSession().QueryOver<EgresoStock>().GetFilterBySecurity().List();
        }

        public IList<EgresoStock> GetAllByArticulo(int IdArticulo, DateTime _start, DateTime _end)
        {
            return this.GetSessionFactory().GetSession().QueryOver<EgresoStock>()
                                                        .GetFilterBySecurity()
                                                        .Where(c => c.FechaCreacion >= _start && c.FechaCreacion <= _end && (c.Articulo.Id == IdArticulo || IdArticulo == 0))
                                                        .OrderBy(x => x.FechaCreacion).Desc
                                                        .List();
        }


        //TODO PONER FILTRO DE FECHA
        public IList<EgresoStock> GetAllByFilter(SelectComboRequest req)
        {
            return this.GetSessionFactory().GetSession().QueryOver<EgresoStock>()
                                                        .And(Expression.Eq("Activo", true))
                                                        .GetFilterBySecurity()
                                                        .List();
        }
        //TODO PONER FILTRO DE FECHA
        public IList<EgresoStock> GetAllByFilter(PagingRequest req)
        {
            // Este puede tener informacion de paginado para traer solo algunos registros y ordenamiento

            return this.GetSessionFactory().GetSession().QueryOver<EgresoStock>()
                                                        .And(Expression.Eq("Activo", true))
                                                        .GetFilterBySecurity()
                                                        .List();
        }

    }
}
