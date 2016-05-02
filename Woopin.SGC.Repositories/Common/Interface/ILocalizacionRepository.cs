using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Model.Common;
using Woopin.SGC.Model.Contabilidad;

namespace Woopin.SGC.Repositories.Common
{
    public interface ILocalizacionRepository : IRepository<Localizacion>
    {
        void SetDefault(int Id);

        Localizacion GetByNombre(string nombre);
    }
}
