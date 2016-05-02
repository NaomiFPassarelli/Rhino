using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Common.Validations;
using Woopin.SGC.Model.Compras;
using Woopin.SGC.Model.Contabilidad;
using Woopin.SGC.Model.Ventas;

namespace Woopin.SGC.Model.Tesoreria
{
    public class ComprobanteRetencionReporte
    {
        public virtual int Id { get; set; }
        public virtual string Numero { get; set; }
        public virtual decimal Total { get; set; }
        public virtual decimal Debe { get; set; }
        public virtual decimal Haber { get; set; }
        public virtual DateTime Fecha { get; set; }
        public virtual int RetencionId { get; set; }
        public virtual string RetencionAbreviatura { get; set; }
        public virtual string RetencionDescripcion { get; set; }
        public virtual int RetencionJuridiccionId { get; set; }
        public virtual int ClienteId { get; set; }
        public virtual string ClienteRazonSocial { get; set; }
        public virtual string ClienteCUIT { get; set; }
        public virtual int ProveedorId { get; set; }
        public virtual string ProveedorRazonSocial { get; set; }
        public virtual string ProveedorCUIT { get; set; }
        public virtual EstadoRetencion Estado { get; set; }
        public virtual string NumeroRetencion { get; set; } // esto es util solo para el reporte de ComprobantesRetenciones

        public ComprobanteRetencionReporte()
        {

        }
    }

}
