using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Woopin.SGC.Model.Common
{

    public enum EstadoComprobante
    {
        Anulada = -1,
        Creada = 1,
        Pagada = 5,
        Cobrada = 6,
        Vencida = 11,
        Pendiente_Afip = 4,
        Imputado = 13
    }

    public enum EstadoComprobanteCancelacion
    {
        Anulada = -1,
        Creada = 1,
        Pagada = 5,
        Cobrada = 6,
    }


    public enum CuentaCorrienteFilter
    {
        Todos = 0,
        Pendientes = 1,
        Vencidos = 2,
        Anulados = 3,
        Pendiente_Afip = 4
    }

    // Filtro para ComprobantesAPagar y ComprobantesACobrar cuando imputa.
    public enum ComprobantesACancelarFilter
    {
        Todos = 0,
        Pendientes = 2,
        Cancelados = 1
    }

    public enum EstadosGenericoFilter
    { 
        Todos = 0,
        Cobrados = 1,
        Pagados = 2,
        Vencidos = 3,
        PorVencer = 4,
        Anulados = 5
    }

}
