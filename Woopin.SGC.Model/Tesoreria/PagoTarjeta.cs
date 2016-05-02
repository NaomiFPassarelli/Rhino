using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Common.Validations;
using Woopin.SGC.Model.Common;

namespace Woopin.SGC.Model.Tesoreria
{
    public class PagoTarjeta : ISecuredEntity
    {
        public virtual int Id { get; set; }

        [Required(ErrorMessage = "Es Necesario una Tarjeta")]
        [DoNotValidateOnlyId]
        public virtual TarjetaCredito Tarjeta { get; set; }

        public virtual string Numero { get; set; }

        [Required(ErrorMessage = "Es Necesario un Importe")]
        [DisplayName("Monto")]
        public virtual decimal Total { get; set; }
        public virtual decimal TotalCancelado { get; set; }

        [Required(ErrorMessage = "Es Necesario las Cuotas")]
        [DisplayName("Cuotas")]
        public virtual int Cuotas { get; set; }

        public virtual int CuotasCanceladas { get; set; }

        [DisplayName("Fecha Emisión")]
        public virtual DateTime Fecha { get; set; }

        [DisplayName("Vencimiento")]
        public virtual DateTime? FechaCancelado { get; set; }
        public virtual DateTime FechaCreacion { get; set; }
        public virtual EstadoPagoTarjeta Estado { get; set; }
        public virtual string Detalle { get; set; }

        public virtual IList<CancelacionTarjeta> Cancelaciones { get; set; }
        [DoNotValidate]
        public virtual Usuario Usuario { get; set; }
        [DoNotValidate]
        public virtual Organizacion Organizacion { get; set; }
        public PagoTarjeta()
        {

        }

    }

    public enum EstadoPagoTarjeta
    {
        //Anulada = -2,
        //Borrador = -1,
        //Emitida = 1,
        //Cancelando = 2,
        //Cancelada = 3
        Anulada = -1,
        Borrador = 0,
        Emitida = 3,
        Cancelando = 4,
        Cancelada = 10
    }

    public enum PagoTarjetaFilter
    {
        Todos = 0,
        Pendientes = 1,
        Cancelados = 2
    }
}
