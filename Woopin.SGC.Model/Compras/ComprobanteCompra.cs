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

namespace Woopin.SGC.Model.Compras
{
    public class ComprobanteCompra : ISecuredEntity
    {
        public virtual int Id { get; set; }

        public virtual int NumeroReferencia { get; set; }

        [Required(ErrorMessage = "Es Necesario el Proveedor")]
        [DoNotValidateOnlyId]
        public virtual Proveedor Proveedor { get; set; }

        public virtual string Observacion { get; set; }

        [Required(ErrorMessage = "Es Necesario un Tipo de Comprobante")]
        [DoNotValidateOnlyId]
        public virtual ComboItem Tipo { get; set; }

        [Required(ErrorMessage = "La Fecha es requerida")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/mm/yyyy}", ApplyFormatInEditMode = true)]
        [DisplayName("Fecha")]
        public virtual DateTime Fecha { get; set; }

        [Required(ErrorMessage = "La Fecha es requerida")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/mm/yyyy}", ApplyFormatInEditMode = true)]
        [DisplayName("Fecha Contable")]
        public virtual DateTime FechaContable { get; set; }
        public virtual DateTime FechaVencimiento { get; set; }
        public virtual DateTime FechaCreacion { get; set; }

        [DisplayName("Condición de Compra")]
        [Required(ErrorMessage = "Es Necesario una condicion de compra")]
        [DoNotValidateOnlyId]
        public virtual ComboItem CondicionCompra { get; set; }

        [Required(ErrorMessage = "Es Necesario un Número de Comprobante")]
        [DisplayName("Número")]
        public virtual string Numero { get; set; }

        [Required(ErrorMessage = "Es Necesario una Letra")]
        public virtual string Letra { get; set; }

        [Required(ErrorMessage = "Es Necesario un Estado")]
        [DisplayName("Estado del Comprobante")]
        public virtual EstadoComprobante Estado { get; set; }

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
        public virtual decimal Subtotal { get; set; }
        public virtual decimal IVA { get; set; }
        public virtual decimal Total { get; set; }
        public virtual decimal TotalPagado { get; set; }
        public virtual decimal PercepcionesIIBB { get; set; }
        public virtual decimal PercepcionesIVA { get; set; }
        public virtual IList<DetalleComprobanteCompra> Detalle { get; set; }
        public virtual IList<ImputacionCompra> Imputacion { get; set; }

        [DoNotValidate]
        public virtual Asiento Asiento { get; set; }

        [DoNotValidate]
        public virtual Usuario Usuario { get; set; }

        [DoNotValidate]
        public virtual Organizacion Organizacion { get; set; }

        public ComprobanteCompra() 
        { 
        }

        /// <summary>
        /// Obtiene el nro del comprobante asociado concatenado con la letra del mismo.
        /// </summary>
        /// <returns>Letra y Numero del comprobante</returns>
        public virtual string GetLetraNumero()
        {
            return this.Letra + this.Numero;
        }


        /// <summary>
        /// Setea los campos Letra y Numeros dado un Numero de Comprobante Completo.
        /// </summary>
        /// <param name="LetraNumero">Numero completo del comprobante compra</param>
        public virtual void SplitLetraNumero(string LetraNumero)
        {
            this.Letra = LetraNumero.Substring(0, 1).ToUpper();
            this.Numero = LetraNumero.Substring(1);
        }


        public virtual void TryAnular()
        {
            if (this.TotalPagado != 0)
                throw new BusinessException("El Comprobante no se puede anular ya que hay una parte ya cancelada");
            if (this.Estado == EstadoComprobante.Anulada)
                throw new BusinessException("El Comprobante ya está anulado");
            if (this.Estado == EstadoComprobante.Pagada)
                throw new BusinessException("El Comprobante no se puede anular ya que ya fue pagada");
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

    }
}
