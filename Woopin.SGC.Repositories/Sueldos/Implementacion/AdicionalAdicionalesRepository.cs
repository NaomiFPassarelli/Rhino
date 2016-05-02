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
using Woopin.SGC.Model.Sueldos;

namespace Woopin.SGC.Repositories.Sueldos
{
    //public class AdicionalAdicionalesRepository : BaseRepository<AdicionalAdicionales>, IAdicionalAdicionalesRepository
    public class AdicionalAdicionalesRepository : BaseSecuredRepository<AdicionalAdicionales>, IAdicionalAdicionalesRepository    
    {
        public AdicionalAdicionalesRepository(IHibernateSessionFactory hibernateSessionFactory)
            : base(hibernateSessionFactory)
        {
        }

        public override IList<AdicionalAdicionales> GetAll()
        {
            return this.GetSessionFactory().GetSession().QueryOver<AdicionalAdicionales>().GetFilterBySecurity().List();
            //return this.GetSessionFactory().GetSession().QueryOver<AdicionalAdicionales>().List();

        }

        public IList<AdicionalAdicionales> GetByAdicional(int IdAdicional, bool ImportaSoloDefault)
        {
            IList<AdicionalAdicionales> AAs = new List<AdicionalAdicionales>();
            AdicionalAdicionales aaAlias = new AdicionalAdicionales();
            Adicional aAlias = new Adicional();
            if (ImportaSoloDefault)
            {
                AAs = this.GetSessionFactory().GetSession().QueryOver<AdicionalAdicionales>()
                                            .GetFilterBySecurity()
                                            .Where(x => (x.Adicional.Id == IdAdicional || x.Adicional.Id == 0) && x.EsDefault)
                                            .Fetch(x => x.AdicionalSobre).Eager
                                            .List();
            }
            else {
                AAs = this.GetSessionFactory().GetSession().QueryOver<AdicionalAdicionales>()
                                            .GetFilterBySecurity()
                                            .Where(x => (x.Adicional.Id == IdAdicional || x.Adicional.Id == 0) )
                                            .Fetch(x => x.AdicionalSobre).Eager
                                            .List();
            }
            return AAs;
        
        }

        public IList<AdicionalAdicionales> GetSobreByAdicional(int IdAdicional)
        {
            IList<AdicionalAdicionales> AAs = new List<AdicionalAdicionales>();
            AAs = this.GetSessionFactory().GetSession().QueryOver<AdicionalAdicionales>()
                                        .GetFilterBySecurity().Where(x => x.Adicional.Id == IdAdicional && x.EsDefault)
                                        .Fetch(x => x.AdicionalSobre).Eager
                                        .List();
            return AAs;
        
        }

        //public void Delete(AdicionalAdicionales Adic)
        //{
        //    this.GetSessionFactory().GetSession().QueryOver<AdicionalAdicionales>().GetFilterBySecurity().Where(x => x.Id == Adic.Id);
        //}

    }
}
