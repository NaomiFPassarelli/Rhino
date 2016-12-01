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
using Woopin.SGC.Model.Cooperativa;

namespace Woopin.SGC.Repositories.Cooperativa
{
    public class AdicionalPagoRepository : BaseSecuredRepository<AdicionalPago>, IAdicionalPagoRepository
    {
        public AdicionalPagoRepository(IHibernateSessionFactory hibernateSessionFactory)
            : base(hibernateSessionFactory)
        {
        }

        public override IList<AdicionalPago> GetAll()
        {
            return this.GetSessionFactory().GetSession().QueryOver<AdicionalPago>().GetFilterBySecurity().List();
        }

        public IList<AdicionalPago> GetAllByFilter(SelectComboRequest req)
        {
            return this.GetSessionFactory().GetSession().QueryOver<AdicionalPago>()
                                                        //.Where((Restrictions.On<AdicionalPago>(x => x.Descripcion).IsLike('%' + req.where + '%')))
                                                        .GetFilterBySecurity()
                                                        .List();
        }

        public IList<AdicionalPago> GetAllByFilter(PagingRequest req)
        {
            // Este puede tener informacion de paginado para traer solo algunos registros y ordenamiento

            return this.GetSessionFactory().GetSession().QueryOver<AdicionalPago>()
                                                        //.Where((Restrictions.On<AdicionalPago>(x => x.Descripcion).IsLike('%' + req.where + '%')))
                                                        .GetFilterBySecurity()
                                                        .List();
        }

        public IList<AdicionalPago> GetAllAdicionalesByPago(int PagoId) 
        {
            return this.GetSessionFactory().GetSession().QueryOver<AdicionalPago>()
                                .Where(c => c.Pago.Id == PagoId).GetFilterBySecurity().List();
        }

        //public IList<AdicionalPago> GetAdicionalesDelPeriodoByEmpleado(DateTime Periodo, int IdAsociado)
        //{
        //    AdicionalPago ar = null;
        //    Pago r = null;
        //    //TODO SQL
        //    //obra social = 1007, cuotasindical = 1010 
        //    int[] IdssConceptosBuscados = { 1007, 1010, 3007, 3008, 3009, 3010, 3011, 3012 };
        //    return this.GetSessionFactory().GetSession().QueryOver<AdicionalPago>(() => ar)
        //                                                                    .JoinAlias(() => ar.Pago, () => r)
        //                                                                    .Where(() => r.FechaPeriodo == Periodo && r.Asociado.Id == IdAsociado && ar.Concepto.Id.IsIn(IdssConceptosBuscados))
        //                                                                    .List();
        //}

    }
}
