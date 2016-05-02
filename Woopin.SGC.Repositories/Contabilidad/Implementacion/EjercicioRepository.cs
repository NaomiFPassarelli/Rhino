using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Model.Contabilidad;
using Woopin.SGC.Common.HtmlModel;
using Woopin.SGC.Model.Exceptions;
using Woopin.SGC.Repositories.Helpers;
using Woopin.SGC.CommonApp.Session;

namespace Woopin.SGC.Repositories.Contabilidad
{
    public class EjercicioRepository : BaseSecuredRepository<Ejercicio>, IEjercicioRepository
    {
        public EjercicioRepository(IHibernateSessionFactory hibernateSessionFactory)
            : base(hibernateSessionFactory)
        {
        }

        public override void Add(Ejercicio e)
        {
            IList<Ejercicio> list = this.GetSessionFactory().GetSession().QueryOver<Ejercicio>()
                                                                        .Where(x => ((x.Inicio <= e.Inicio && x.Final >= e.Inicio) || 
                                                                                        (x.Inicio <= e.Final && x.Final >= e.Final)) && x.Activo)
                                                                                        .GetFilterBySecurity()
                                                                        .List();
            if (list.Count > 0)
            {
                throw new ValidationException("Ya se encuentra un ejercicio contable creado en ese periodo");
            }
            e.Organizacion = SessionDataManager.GetOrganizacion();
            this.GetSessionFactory().GetSession().Save(e);
        }

        public override IList<Ejercicio> GetAll()
        {
            IList<Ejercicio> list = this.GetSessionFactory().GetSession().QueryOver<Ejercicio>().Where(c => c.Activo).GetFilterBySecurity().List();


            for (int i = 0; i < list.Count; i++)
            {
                list[i].Bloqueos = null;
            }
            return list;
        }

        public IList<Ejercicio> GetAllAvailable()
        {
            IList<Ejercicio> list = this.GetSessionFactory().GetSession().QueryOver<Ejercicio>().Where(c => c.Activo && !c.Cerrado).GetFilterBySecurity().List();


            for (int i = 0; i < list.Count; i++)
            {
                list[i].Bloqueos = null;
            }
            return list;
        }
        public void ControlarIngreso(DateTime fechaContable)
        {
            Ejercicio ejercicio = this.GetSessionFactory().GetSession().QueryOver<Ejercicio>()
                                            .Where(x => x.Inicio <= fechaContable && x.Final >= fechaContable && x.Activo)
                                            .GetFilterBySecurity()
                                            .Fetch(x => x.Bloqueos).Eager
                                            .SingleOrDefault();
            if (ejercicio == null)
            {
                throw new ValidationException("No existen ejercicios contables para la fecha seleccionada. Por favor, cree el ejercicio en caso de ser necesario.");
            }

            if (ejercicio.Cerrado)
            {
                throw new ValidationException("El ejercicio contable se encuentra cerrado, y no se pueden realizar modificaciones.");
            }

            BloqueoContable b = ejercicio.Bloqueos.Where(x => x.Inicio <= fechaContable && x.Final >= fechaContable).SingleOrDefault();

            if (b != null && b.Activo)
            {
                throw new ValidationException("Se encuentra un bloqueo contable activo para la fecha seleccionada.");
            }
        }

        public Ejercicio GetCompleto(int Id)
        {
            Ejercicio e = this.GetSessionFactory().GetSession().QueryOver<Ejercicio>()
                                                        .Where(x => x.Id == Id)
                                                        .GetFilterBySecurity()
                                                        .Fetch(x => x.Bloqueos).Eager
                                                        .SingleOrDefault();
            for (int i = 0; i < e.Bloqueos.Count; i++)
            {
                e.Bloqueos[i].Ejercicio = null;
            }

            return e;
        }

        public Ejercicio GetByDate(DateTime fecha)
        {
            return this.GetSessionFactory().GetSession().QueryOver<Ejercicio>()
                                                        .Where(x => x.Inicio <= fecha && x.Final >= fecha && x.Activo)
                                                        .GetFilterBySecurity()
                                                        .SingleOrDefault();
        }

    }
}
