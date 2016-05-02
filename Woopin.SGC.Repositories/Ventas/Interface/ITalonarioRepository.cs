using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Model.Ventas;

namespace Woopin.SGC.Repositories.Ventas
{
    public interface ITalonarioRepository : IRepository<Talonario>
    {
        /// <summary>
        /// Buscar el talonario por Prefijo del mismo
        /// </summary>
        /// <param name="prefijo">Numeracion del talonario, ej: 0001</param>
        /// <returns>Talonario</returns>
        Talonario GetByPrefijo(string prefijo);
    }
}
