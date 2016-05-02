using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Model.Ventas;
using Woopin.SGC.Services.Afip.Model;

namespace Woopin.SGC.Services.Afip
{
    public interface IAfipService
    {
        LoginTicket DoLogin(string service,string certificate);
        Task<long> ConsultarUltimoComprobante(int PuntoVenta, int TipoComprobante);
        Task<string> ConsultarComprobante(long NroComprobante, int PuntoVenta, int TipoComprobante);
        Task<List<string>> GetAllMonedas();
        Task<List<string>> GetAllTiposComprobantes();
        Task<List<string>> GetAllPaises();
        Task<List<string>> GetAllPuntosVentas();
        Task<List<string>> GetAllTiposIvas();
        Task<List<string>> GetAllTiposConceptos();
        Task<List<string>> GetAllTiposDocumentos();
        Task<List<string>> GetAllTributos();
        Task<List<string>> GetAllTiposOpcional();
        Task<string> SolicitarCAEAnticipado(DateTime Date);
        Task<RespuestaCAE> SolicitarCAE(ComprobanteVenta c);
        
    }
}
