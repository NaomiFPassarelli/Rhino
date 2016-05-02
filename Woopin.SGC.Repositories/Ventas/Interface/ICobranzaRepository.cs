using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Model.Ventas;

namespace Woopin.SGC.Repositories.Ventas
{
    public interface ICobranzaRepository : IRepository<Cobranza>
    {
        string GetProximoRecibo(string talonario);

        int GetProximoIdCobranza();

        Cobranza GetCompleto(int Id);
    
        IList<Cobranza> GetAllByCliente(int IdCliente,DateTime _start,DateTime _end);}
}
