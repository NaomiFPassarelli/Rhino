using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Woopin.SGC.Model.Common
{
    public class Dashboard
    {
        public int Clientes { get; set; }
        public int Proveedores { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public decimal VentasMensual { get; set; }

        /// <summary>
        /// Total facturado de ventas - notas de credito del mes en curso.
        /// </summary>
        public decimal VentasMes { get; set; }


        public decimal ComprasMensual { get; set; }


        /// <summary>
        /// Total facturado de compras - notas de credito del mes en curso.
        /// </summary>
        public decimal ComprasMes { get; set; }

        /// <summary>
        /// La sumatoria de las disponibilidades tanto en Caja como en Bancos.
        /// No incluye a los valores a depositar.
        /// </summary>
        public decimal Disponibilidades { get; set; }

        /// <summary>
        /// Monto de cheques en cartera no depositados
        /// </summary>
        public decimal ValoresADepositar { get; set; }


        public decimal ComprasVencidas { get; set; }
        public decimal ComprasVencidasMes { get; set; }


        public decimal ComprasPorVencerSemana { get; set; }
        public decimal VentasVencidas { get; set; }
        public decimal VentasVencidasMes { get; set; }
        public decimal VentasPorVencerSemana { get; set; }

        /// <summary>
        /// Monto de deudas a bancos por tarjetas de credito
        /// </summary>
        public decimal DeudasBancarias { get; set; }

        /// <summary>
        /// Cantidad de facturas electronicas pendientes de confirmacion.
        /// </summary>
        public int CantFEPendientes { get; set; }

        /// <summary>
        /// Cantidad de cheques propios pendientes de confirmación de pago
        /// </summary>
        public decimal ChequesADebitar { get; set; }
    }
}
