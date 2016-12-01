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
using Woopin.SGC.CommonApp.Session;

namespace Woopin.SGC.Repositories.Cooperativa
{
    public class ActaPuntoExtraRepository : BaseRepository<ActaPuntoExtra>, IActaPuntoExtraRepository
    {
        public ActaPuntoExtraRepository(IHibernateSessionFactory hibernateSessionFactory)
            : base(hibernateSessionFactory)
        {
        }

        public IList<ActaPuntoExtra> GetActaPuntoExtraByActa(int ActaId)
        {
            Acta A = this.GetSessionFactory().GetSession().QueryOver<Acta>()
                .Fetch(c => c.OtrosPuntos).Eager
                .And(c => c.Organizacion.Id == SessionDataManager.GetOrganizacion().Id)
                .Where(c => c.Id == ActaId).SingleOrDefault();

            return A.OtrosPuntos;

            //return this.GetSessionFactory().GetSession().QueryOver<ActaPuntoExtra>()
            //    .Where(c => c.Acta.Id == ActaId)
            //    .And(c => c.Acta.Organizacion.Id == SessionDataManager.GetOrganizacion().Id)
            //    .Fetch(c => c.Acta).Eager
            //    .List();
        }

            
    }
}