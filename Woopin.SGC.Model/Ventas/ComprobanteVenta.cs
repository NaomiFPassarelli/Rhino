using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Common.Validations;
using Woopin.SGC.Model.Common;
using Woopin.SGC.Model.Contabilidad;
using Woopin.SGC.Model.Exceptions;
using Woopin.SGC.Model.Negocio;

namespace Woopin.SGC.Model.Ventas
{
    public class ComprobanteVenta : ISecuredEntity
    {
        public virtual int Id { get; set; }

        [Required(ErrorMessage = "Es Necesario el Cliente")]
        [DoNotValidateOnlyId]
        public virtual Cliente Cliente { get; set; }

        [Required(ErrorMessage = "Es Necesario un Tipo de Comprobante")]
        [DoNotValidateOnlyId]
        public virtual ComboItem Tipo { get; set; }

        [Required(ErrorMessage = "La Fecha es requerida")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/mm/yyyy}", ApplyFormatInEditMode = true)]
        [DisplayName("Fecha")]
        public virtual DateTime Fecha { get; set; }
        public virtual DateTime FechaVencimiento { get; set; }
        public virtual DateTime FechaCreacion { get; set; }

        [DisplayName("Mes - Año de prestación")]
        [RegularExpression(@"[0-9]{2}-[0-9]{4}",
        ErrorMessage = "El Mes Año no es valido.")]
        public virtual string MesPrestacion { get; set; }

        [Required(ErrorMessage = "Es Necesario un Número de Comprobante")]
        [DisplayName("Número")]
        public virtual string Numero { get; set; }

        [DisplayName("Condición de Venta")]
        [Required(ErrorMessage = "Es Necesario una Condicion de Compra")]
        [DoNotValidateOnlyId]
        public virtual ComboItem CondicionVenta { get; set; }

        [RegularExpression(@"(([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)(\s*;\s*|\s*$))*(([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)(\s*;\s*|\s*$))*",
        ErrorMessage = "Los emails introducidos no son validos.")]
        [DisplayName("Email de Cobro")]
        public virtual string MailCobro { get; set; }

        [DisplayName("Contacto de Cobro")]
        public virtual string NombreCobro { get; set; }

        [Required(ErrorMessage = "Es Necesario una Letra")]
        public virtual string Letra { get; set; }

        [Required(ErrorMessage = "Es Necesario un Estado")]
        [DisplayName("Estado del Comprobante")]
        public virtual EstadoComprobante Estado { get; set; }
        public virtual decimal Subtotal { get; set; }
        public virtual decimal IVA { get; set; }
        [DisplayName("IVA 21%")]
        public virtual decimal IVA21 { get; set; }

        [DisplayName("IVA 10,5%")]
        public virtual decimal IVA105 { get; set; }

        [DisplayName("IVA 27%")]
        public virtual decimal IVA27 { get; set; }

        [DisplayName("Importe Exento")]
        public virtual decimal ImporteExento { get; set; }

        [DisplayName("Importe No Gravado")]
        public virtual decimal ImporteNoGravado { get; set; }
        public virtual decimal Total { get; set; }
        public virtual decimal TotalCobrado { get; set; }
        public virtual decimal Descuento { get; set; }
        public virtual Moneda Moneda { get; set; }
        public virtual decimal Cotizacion { get; set; }

        [DoNotValidate]
        public virtual Asiento Asiento { get; set; }

        [DoNotValidate]
        public virtual Usuario Usuario { get; set; }

        public virtual IList<DetalleComprobanteVenta> Detalle { get; set; }
        public virtual IList<ObservComprobanteVenta> Observaciones { get; set; }
        [DisplayName("Observación")]
        public virtual string Observacion { get; set; }

        public virtual IList<ImputacionVenta> Imputacion { get; set; }

        [DoNotValidate]
        public virtual Organizacion Organizacion { get; set; }

        [DoNotValidateOnlyId]
        public virtual Talonario Talonario { get; set; }


        public virtual string CAE { get; set; }
        public virtual DateTime? VencimientoCAE { get; set; }
        public virtual DateTime? EnviadoMail { get; set; }

        public ComprobanteVenta() 
        {

        }


        public virtual string GetLetraNumero()
        {
            return this.Letra + this.Numero;
        }

        public virtual void SplitLetraNumero(string LetraNumero)
        {
            this.Letra = LetraNumero.Substring(0, 1).ToUpper();
            this.Numero = LetraNumero.Substring(1);
        }

        public virtual bool CanAnular()
        {
            try
            {
                this.TryAnular();
                return true;
            }
            catch
            {
                return false;
            }
            
        }

        public virtual void TryAnular()
        {
            if (this.TotalCobrado != 0) 
                throw new BusinessException("El Comprobante no se puede anular ya que hay una parte ya cancelada");
            if (this.CAE != null) 
                throw new BusinessException("El Comprobante no se puede anular ya que hay una parte ya cancelada");
            if (this.Estado == EstadoComprobante.Pendiente_Afip) 
                throw new BusinessException("El Comprobante no se puede anular, ya que todavia no fue confirmado por la AFIP.");
            if (this.Estado == EstadoComprobante.Anulada) 
                throw new BusinessException("El Comprobante ya está anulado");
            if (this.Estado == EstadoComprobante.Cobrada) 
                throw new BusinessException("El Comprobante no se puede anular ya que ya fue cobrada");
        }
    }


}
