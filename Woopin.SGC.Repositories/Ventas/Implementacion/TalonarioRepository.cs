using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Model.Ventas;
using Woopin.SGC.Repositories.Helpers;

namespace Woopin.SGC.Repositories.Ventas
{
    public class TalonarioRepository : BaseSecuredRepository<Talonario>, ITalonarioRepository
    {
        public TalonarioRepository(IHibernateSessionFactory hibernateSessionFactory)
            : base(hibernateSessionFactory)
        {
        }

        /// <summary>
        /// Busca todos los talonarios activos
        /// </summary>
        /// <returns>Listado de talonarios</returns>
        public override IList<Talonario> GetAll()
        {
            return this.GetSessionFactory().GetSession().QueryOver<Talonario>()
                                                        .GetFilterBySecurity()
                                                        .Where(x => x.Activo)
                                                        .List();
        }

        /// <summary>
        /// Buscar el talonario por Prefijo del mismo
        /// </summary>
        /// <param name="prefijo">Numeracion del talonario, ej: 0001</param>
        /// <returns>Talonario</returns>
        public Talonario GetByPrefijo(string prefijo)
        {
            return this.GetSessionFactory().GetSession().QueryOver<Talonario>()
                                                        .GetFilterBySecurity()
                                                        .Where(x => x.Prefijo == prefijo && x.Activo)
                                                        .SingleOrDefault();
        }

    }
}
