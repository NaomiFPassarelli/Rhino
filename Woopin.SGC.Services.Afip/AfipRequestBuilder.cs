using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.CommonApp.Security;
using Woopin.SGC.CommonApp.Session;
using Woopin.SGC.Model.Common;
using Woopin.SGC.Model.Exceptions;
using Woopin.SGC.Model.Ventas;

namespace Woopin.SGC.Services.Afip
{
    public static class AfipRequestBuilder
    {
        public static Wsfe.FECAERequest GetSolicitarCAE_Request(ComprobanteVenta c)
        {
            Wsfe.FECAERequest req = new Wsfe.FECAERequest();

            // Información de Cabecera
            req.FeCabReq = new Wsfe.FECAECabRequest();
            req.FeCabReq.CantReg = 1;
            req.FeCabReq.CbteTipo = Convert.ToInt32(c.Tipo.AfipData);

            if (!c.Talonario.PuntoVenta.HasValue)
                throw new ValidationException("El talonario no posee punto de venta");

            req.FeCabReq.PtoVta = c.Talonario.PuntoVenta.Value;

            List<Wsfe.FECAEDetRequest> Detalle = new List<Wsfe.FECAEDetRequest>();
            var item = new Wsfe.FECAEDetRequest();
            item.CbteFch = c.Fecha.ToString("yyyyMMdd"); // Fecha del comprobante
            item.Concepto = Convert.ToInt32(c.Organizacion.Actividad.AfipData); // Concepto 01-Producto, 02- Servicio, 03- Prod y Serv
            item.DocTipo = 80; // Tipo de Documento del Cliente 80-CUIT
            item.DocNro = Convert.ToInt64(c.Cliente.CUIT.Replace("-","")); // Nro del cliente
            item.CbteDesde = Convert.ToInt64(c.Numero.Split('-')[1]); // Nro de Comprobante desde
            item.CbteHasta = Convert.ToInt64(c.Numero.Split('-')[1]); // Nro de Comprobante hasta, 
            
            if (Security.GetOrganizacion().Actividad.AfipData != "1")
            {
                DateTime mesPrestacionPrimerDia = new DateTime(Convert.ToInt32(c.MesPrestacion.Split('-')[1]), Convert.ToInt32(c.MesPrestacion.Split('-')[0]), 1);
                DateTime mesPrestacionUltimoDia = mesPrestacionPrimerDia.AddMonths(1).AddDays(-1);
                item.FchServDesde = mesPrestacionPrimerDia.ToString("yyyyMMdd");// Para servicios (2 o 3) se le asigna un inicio
                item.FchServHasta = mesPrestacionUltimoDia.ToString("yyyyMMdd");// Para servicios (2 o 3) se le asigna un fin
                item.FchVtoPago = c.FechaVencimiento.ToString("yyyyMMdd");// Para servicios (2 o 3) se le asigna un fin
            }

            item.MonCotiz = Convert.ToDouble(c.Cotizacion); // Cotizacion de la moneda, en caso de Pesos 1.
            item.MonId = c.Moneda.CodigoAfip;

            // Informacion de Importes
            item.ImpTrib = 0; // total tributos
            item.ImpIVA = Convert.ToDouble(c.IVA); // total de ivas
            item.ImpTotal = Convert.ToDouble(c.Total); // total de la factura
            item.ImpNeto = Convert.ToDouble(c.Subtotal); // importe neto gravado
            item.ImpOpEx = Convert.ToDouble(c.ImporteExento); // importe exento
            item.ImpTotConc = 0; // Importe total no gravado

            // Listado de IVAS
            List<Wsfe.AlicIva> listIVAs = new List<Wsfe.AlicIva>();
            if (c.IVA21 > 0)
            {
                var itemIva = new Wsfe.AlicIva();
                itemIva.BaseImp = Convert.ToDouble(c.Detalle.Where(x => x.TipoIva.AdditionalData == "21").Sum(x => x.Total));
                itemIva.Importe = Convert.ToDouble(c.IVA21);
                itemIva.Id = Convert.ToInt32(c.Detalle.Where(x => x.TipoIva.AdditionalData == "21").First().TipoIva.AfipData);
                listIVAs.Add(itemIva);
            }

            if(c.IVA > 0)
            {
                item.Iva = listIVAs.ToArray();
            }
            
            Detalle.Add(item);
            req.FeDetReq = Detalle.ToArray();
            return req;
        }
    }
}
