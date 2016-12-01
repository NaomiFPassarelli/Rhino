using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Common.HtmlModel;
using Woopin.SGC.Model.Common;
using Woopin.SGC.Model.Cooperativa;

namespace Woopin.SGC.Repositories.Cooperativa
{
    public interface IAsociadoRepository : IRepository<Asociado>
    {
        IList<Asociado> GetAllByFilter(SelectComboRequest req);
        IList<Asociado> GetAllByFilter(PagingRequest req);
        bool ExistCUIT(string cuit, int? IdUpdate);
        Asociado GetCompleto(int IdAsociado);
        IList<Asociado> GetAsociados(IList<int> Ids);
        IList<Asociado> GetAsociadosMes(int Mes, int Año);
        IList<Asociado> GetAsociadosMesEgreso(int Mes, int Año);
        //Asociado LoadHeader();
        int GetProximoNumeroReferencia();

    }
}
