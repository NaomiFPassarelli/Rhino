using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Common.Validations;
using Woopin.SGC.Model.Common;

namespace Woopin.SGC.Model.Contabilidad
{
    public class Asiento : ISecuredEntity
    {
        public virtual int Id { get; set; }
        
        [Required(ErrorMessage = "La Fecha es requerida")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/mm/yyyy}", ApplyFormatInEditMode = true)]
        public virtual DateTime Fecha { get; set; }

        [Required(ErrorMessage = "Es Necesario una Fecha")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/mm/yyyy}", ApplyFormatInEditMode = true)]
        public virtual DateTime FechaCreacion { get; set; }
        [Required(ErrorMessage = "Es Necesario una Leyenda")]
        public virtual string Leyenda { get; set; }
        
        //quien lo genero: compra venta tesoreria contabilidad
        public virtual string Modulo { get; set; }
        
        public virtual IList<AsientoItem> Items { get; set; }

        public virtual Ejercicio Ejercicio { get; set; }

        [Required(ErrorMessage = "Es Necesario un Número de Referencia")]
        [DisplayName("N° de Referencia")]
        public virtual int NumeroReferencia { get; set; }
        [DoNotValidate]
        public virtual Usuario Usuario { get; set; }

        public virtual bool Manualizado { get; set; }

        public virtual TipoOperacion TipoOperacion { get; set; }
        public virtual int ComprobanteAsociado { get; set; }
        [DoNotValidate]
        public virtual Organizacion Organizacion { get; set; }
    }

    public class ModulosSistema
    {
        public const string SISTEMA_GESTION = "Sistema de Gestión";
    }

    public enum TipoOperacion
    {
        FV, // Factura de ventas
        CZ, // Cobranza
        OC, // Orden de compra
        OE, // Otro egreso
        OP, // Orden de Pago
        MOVF, // Movimiento de fondos
        DEP, // Deposito
        CTC, //Cancelacion Tarjeta Credito
        RS //Recibo de Sueldo
    }
}
