using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Common.Validations;
using Woopin.SGC.Model.Common;

namespace Woopin.SGC.Model.Cooperativa
{
    public class Asociado : ISecuredEntity
    {
        public virtual int Id { get; set; }
        [Required(ErrorMessage = "Es Necesario un Numero de Asociado")]
        [DisplayName("Numero de Referencia")]
        public virtual int NumeroReferencia { get; set; }

        [Required(ErrorMessage = "Es Necesario un Nombre")]
        public virtual string Nombre { get; set; }

        [Required(ErrorMessage = "Es Necesario un Apellido")]
        public virtual string Apellido { get; set; }

        [DisplayName("Dirección")]
        public virtual string Direccion { get; set; }

        [DisplayName("Número")]
        public virtual string Numero { get; set; }
        public virtual string Piso { get; set; }
        public virtual string Departamento { get; set; }

        [DisplayName("Codigo Postal")]
        public virtual string CodigoPostal { get; set; }
        [DisplayName("Recomendado Por")]
        public virtual string RecomendadoPor { get; set; }
        [DoNotValidate]
        public virtual Localizacion Localizacion { get; set; }

        [DoNotValidate]
        public virtual ComboItem Nacionalidad { get; set; }
        [DisplayName("Numero de Carnet de Conductor")]
        public virtual string NroCarnetConductor { get; set; }
        [DisplayName("Categoria de Conductor")]
        public virtual string CategoriaConductor { get; set; }
        [DisplayName("Marca del Vehiculo")]
        public virtual string MarcaVehiculo { get; set; }
        [DisplayName("Modelo del Vehiculo")]
        public virtual string ModeloVehiculo { get; set; }
        [DisplayName("Numero de Chapa del Vehiculo")]
        public virtual string NroChapaVehiculo { get; set; }

        [DisplayName("Estado Civil")]
        [DoNotValidate]
        public virtual ComboItem EstadoCivil { get; set; }
        [RegularExpression(@"[0-9]{8,20}",
        ErrorMessage = "El telefono no es valido.")]
        public virtual string Telefono { get; set; }

        [RegularExpression(@"[0-9]{2}-[0-9]{8}-[0-9]{1}",
        ErrorMessage = "El CUIT no es valido.")]
        public virtual string CUIT { get; set; }

        [RegularExpression(@"[0-9]{8}", ErrorMessage = "El DNI no es valido.")]
        public virtual string DNI { get; set; }
        public virtual string CI { get; set; }
        public virtual string LE { get; set; }
        public virtual string LC { get; set; }

        public virtual bool Activo { get; set; }

        [DoNotValidate]
        public virtual Organizacion Organizacion { get; set; }
        public virtual DateTime FechaCreacion { get; set; }
        [DisplayName("Fecha de Notificacion")]
        public virtual DateTime? FechaNotificacion { get; set; }

        [DisplayName("Fecha de Ingreso")]
        [Required(ErrorMessage = "Es Necesario una Fecha de Ingreso")]
        public virtual DateTime FechaIngreso { get; set; }
        [DisplayName("Fecha de Egreso")]
        public virtual DateTime? FechaEgreso { get; set; }

        [DisplayName("Fecha de Acta Ingreso")]
        public virtual DateTime? FechaActaIngreso { get; set; }
        
        [DisplayName("Fecha de Nacimiento")]
        public virtual DateTime? FechaNacimiento { get; set; }
        [DisplayName("Lugar de Nacimiento")]
        public virtual string LugarNacimiento { get; set; }
        public virtual string Cargo { get; set; }
        //[DisplayName("Número de Acta del Alta")]
        //public virtual int? ActaAlta { get; set; } // nro
        //[DisplayName("Número de Acta de Baja")]
        //public virtual int? ActaBaja { get; set; } // nro

        [DoNotValidate]
        public virtual Acta ActaAlta { get; set; }
        [DoNotValidate]
        public virtual Acta ActaBaja { get; set; }

        //[DisplayName("Cantidad de Cuotas")]
        //[Required(ErrorMessage = "Es Necesario una Cantidad de Cuotas")]
        //public virtual int CantidadCuotas { get; set; }
        [DisplayName("Importe de cada pago")]
        [Required(ErrorMessage = "Es Necesario un Importe de cada pago")]
        public virtual decimal ImportePago { get; set; }
        //public virtual bool AbonoTotalmente { get; set; }
        public virtual int CantidadPagosAbonados { get; set; }


        [DisplayName("Cantidad de Abonos")]
        [Required(ErrorMessage = "Es Necesario una Cantidad de Abonos")]
        public virtual int CantidadAbonos { get; set; }
        [DisplayName("Importe de cada Abono")]
        [Required(ErrorMessage = "Es Necesario un Importe de cada Abono")]
        public virtual decimal ImporteAbono { get; set; }
        public virtual bool AbonoFinalizado { get; set; }
        public virtual int CantidadAbonosEfectivos { get; set; }
        
        
        public virtual string Padre { get; set; }
        public virtual string Madre { get; set; }
        public virtual string NroPolicia { get; set; }
        [DisplayName("Imagen")]
        public virtual string ImagePath { get; set; }
        public Asociado()
        {
            this.Activo = true;
            //this.AbonoTotalmente = false;
            this.FechaCreacion = DateTime.Now;
            this.CantidadAbonosEfectivos = 0;
            this.AbonoFinalizado = false;
        }
    }
}
