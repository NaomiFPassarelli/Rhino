using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Common.Helpers;
using Woopin.SGC.CommonApp.Security;
using Woopin.SGC.Model.Exceptions;
using Woopin.SGC.Model.Ventas;
using Woopin.SGC.Services.Afip.Model;

namespace Woopin.SGC.Services.Afip
{
    public class AfipService : IAfipService
    {
        public string CertPath { get; set; }
        public Afip.Wsfe.ServiceSoapClient WSFE { get; set; }
        public LoginTicket loginTicket { get; set; }

        protected static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        public AfipService()
        {
            this.CertPath = System.Web.HttpContext.Current.Server.MapPath("~/");
            this.WSFE = new Wsfe.ServiceSoapClient();
            this.WSFE.Endpoint.Address = new System.ServiceModel.EndpointAddress(new Uri(Convert.ToString(ConfigurationManager.AppSettings["AfipWSFE"])));
        }



        public async Task<long> ConsultarUltimoComprobante(int PuntoVenta, int TipoComprobante)
        {
            var response = await this.WSFE.FECompUltimoAutorizadoAsync(this.GetAuthRequest(), PuntoVenta,TipoComprobante);
            if (response != null)
            {
                return response.Body.FECompUltimoAutorizadoResult.CbteNro;
            }
            else
            {
                throw new AfipServiceException("Error AFIP: Hubo un problema al consultar con el WSFE");
            }
        }
        public async Task<string> ConsultarComprobante(long NroComprobante, int PuntoVenta, int TipoComprobante)
        {
            Wsfe.FECompConsultaReq req = new Wsfe.FECompConsultaReq();
            req.CbteNro = NroComprobante;
            req.CbteTipo = TipoComprobante;
            req.PtoVta = PuntoVenta;
            var response = await this.WSFE.FECompConsultarAsync(this.GetAuthRequest(),req);
            if (response != null)
            {
                return JSONHelper.Serialize(response.Body.FECompConsultarResult.ResultGet);
            }
            else
            {
                throw new AfipServiceException("Error AFIP: Hubo un problema al consultar con el WSFE");
            }
        }

        public async Task<List<string>> GetAllMonedas()
        {
            var response = await this.WSFE.FEParamGetTiposMonedasAsync(this.GetAuthRequest());
            if (response != null)
            {
                return response.Body.FEParamGetTiposMonedasResult.ResultGet.Select(x => x.Id.ToString() + "-" + x.Desc).ToList();
            }
            else
            {
                throw new AfipServiceException("Error AFIP: Hubo un problema al consultar con el WSFE");
            }
        }

        public async Task<List<string>> GetAllTiposComprobantes()
        {
            var response = await this.WSFE.FEParamGetTiposCbteAsync(this.GetAuthRequest());
            if (response != null)
            {
                return response.Body.FEParamGetTiposCbteResult.ResultGet.Select(x => x.Id.ToString() + '-' + x.Desc).ToList();
            }
            else
            {
                throw new AfipServiceException("Error AFIP: Hubo un problema al consultar con el WSFE");
            }
        }

        public async Task<List<string>> GetAllPaises()
        {
            var response = await this.WSFE.FEParamGetTiposPaisesAsync(this.GetAuthRequest());
            if (response != null)
            {
                return response.Body.FEParamGetTiposPaisesResult.ResultGet.Select(x => x.Id.ToString() + '-' + x.Desc).ToList();
            }
            else
            {
                throw new AfipServiceException("Error AFIP: Hubo un problema al consultar con el WSFE");
            }
        }

        public async Task<List<string>> GetAllPuntosVentas()
        {
            var response = await this.WSFE.FEParamGetPtosVentaAsync(this.GetAuthRequest());
            if (response != null)
            {
                if (response.Body.FEParamGetPtosVentaResult.ResultGet != null)
                    return response.Body.FEParamGetPtosVentaResult.ResultGet.Select(x => x.Nro.ToString()).ToList();
                else
                    return null;
            }
            else
            {
                throw new AfipServiceException("Error AFIP: Hubo un problema al consultar con el WSFE");
            }
        }

        public async Task<List<string>> GetAllTiposIvas()
        {
            var response = await this.WSFE.FEParamGetTiposIvaAsync(this.GetAuthRequest());
            if (response != null)
            {
                return response.Body.FEParamGetTiposIvaResult.ResultGet.Select(x => x.Id.ToString() + '-' + x.Desc).ToList();
            }
            else
            {
                throw new AfipServiceException("Error AFIP: Hubo un problema al consultar con el WSFE");
            }
        }

        public async Task<List<string>> GetAllTiposConceptos()
        {
            var response = await this.WSFE.FEParamGetTiposConceptoAsync(this.GetAuthRequest());
            if (response != null)
            {
                return response.Body.FEParamGetTiposConceptoResult.ResultGet.Select(x => x.Id.ToString() + '-' + x.Desc).ToList();
            }
            else
            {
                throw new AfipServiceException("Error AFIP: Hubo un problema al consultar con el WSFE");
            }
        }

        public async Task<List<string>> GetAllTiposDocumentos()
        {
            var response = await this.WSFE.FEParamGetTiposDocAsync(this.GetAuthRequest());
            if (response != null)
            {
                return response.Body.FEParamGetTiposDocResult.ResultGet.Select(x => x.Id.ToString() + '-' + x.Desc).ToList();
            }
            else
            {
                throw new AfipServiceException("Error AFIP: Hubo un problema al consultar con el WSFE");
            }
        }

        public async Task<List<string>> GetAllTributos()
        {
            var response = await this.WSFE.FEParamGetTiposTributosAsync(this.GetAuthRequest());
            if (response != null)
            {
                return response.Body.FEParamGetTiposTributosResult.ResultGet.Select(x => x.Id.ToString() + '-' + x.Desc).ToList();
            }
            else
            {
                throw new AfipServiceException("Error AFIP: Hubo un problema al consultar con el WSFE");
            }
        }

        public async Task<List<string>> GetAllTiposOpcional()
        {
            var response = await this.WSFE.FEParamGetTiposOpcionalAsync(this.GetAuthRequest());
            if (response != null)
            {
                return response.Body.FEParamGetTiposOpcionalResult.ResultGet.Select(x => x.Id.ToString() + '-' + x.Desc).ToList();
            }
            else
            {
                throw new AfipServiceException("Error AFIP: Hubo un problema al consultar con el WSFE");
            }
        }

        public async Task<string> SolicitarCAEAnticipado(DateTime fecha)
        {
            short Orden = Convert.ToInt16(fecha.Day <= 15 ? 1 : 2);
            int Periodo = Convert.ToInt32(fecha.ToString("yyyyMM"));
            var response = await this.WSFE.FECAEASolicitarAsync(this.GetAuthRequest(),Periodo,Orden);
            if (response.Body.FECAEASolicitarResult.ResultGet != null)
            {
                return response.Body.FECAEASolicitarResult.ResultGet.CAEA;
            }
            else
            {

                throw new AfipServiceException("Error AFIP: Hubo un problema al consultar con el WSFE - " + response.Body.FECAEASolicitarResult.Errors[0]);
            }
        }

        public async Task<RespuestaCAE> SolicitarCAE(ComprobanteVenta c)
        {
            Wsfe.FECAERequest req = AfipRequestBuilder.GetSolicitarCAE_Request(c);
            log.Debug(JSONHelper.Serialize(req));
            var response = await this.WSFE.FECAESolicitarAsync(this.GetAuthRequest(c.Talonario.CertificadoPath),req);
            log.Debug(JSONHelper.Serialize(response));
            if (response.Body.FECAESolicitarResult.FeCabResp.Resultado == "A")
            {
                log.Debug("Respuesta correcta de CAE: " + response.Body.FECAESolicitarResult.FeDetResp[0].CAE);
                RespuestaCAE r = new RespuestaCAE()
                {
                    CAE = response.Body.FECAESolicitarResult.FeDetResp[0].CAE,
                    Vencimiento = response.Body.FECAESolicitarResult.FeDetResp[0].CAEFchVto
                };
                return r;
            }
            else
            {
                log.Debug("No aprobado");
                foreach (var obs in response.Body.FECAESolicitarResult.FeDetResp.First().Observaciones)
                {
                    log.Error("Solicitar CAE - N°" + c.GetLetraNumero() + " observacion:  " + obs.Msg);
                }
                throw new AfipServiceException("Error AFIP: Hubo un problema al consultar con el WSFE - " + response.Body.FECAESolicitarResult.Errors[0]);
            }
        }

        public void ConsultarCuit(string cuit)
        {
            //var response = await this.WSCN3.getAsync(cuit, this.loginTicket.Token, this.loginTicket.Sign);
            //if (response != null)
            //{
            //    return response.getReturn;
            //}
            //else
            //{
            //    throw new AfipServiceException("Error AFIP: Hubo un problema al consultar con el WSFE");
            //}
        }

        protected Wsfe.FEAuthRequest GetAuthRequest()
        {
            return this.GetAuthRequest(null);
        }
        protected Wsfe.FEAuthRequest GetAuthRequest(string certPath)
        {
            if(this.loginTicket == null || (this.loginTicket != null && this.loginTicket.ExpirationTime < DateTime.Now))
                this.loginTicket = this.DoLogin(WServices.WSFE, certPath);
            Wsfe.FEAuthRequest req = new Wsfe.FEAuthRequest();
            req.Cuit = Convert.ToInt64(Security.GetOrganizacion().CUIT.Replace("-",""));
            req.Sign = this.loginTicket.Sign;
            req.Token = this.loginTicket.Token;
            return req;
        }

        public LoginTicket DoLogin(string service, string certPath)
        {
            string certificadoPath = null;
            if (certPath != null)
            {
                certificadoPath = this.CertPath + certPath;
            }
            else
            {
                // TODO - Necesitariamos que cada organizacion en la entidad tenga un ceritificado de cualquiera para consultar.
                certificadoPath = this.CertPath + "Certificados/" + ConfigurationManager.AppSettings["CertificadoAFIP"];
            }

            LoginTicket objTicketRespuesta = new LoginTicket();
            bool IsDebugEnable = Convert.ToBoolean(ConfigurationManager.AppSettings["IsDebugEnable"]);
            string WSAA_Url = Convert.ToString(ConfigurationManager.AppSettings["AfipWSAA"]);
            string strTicketRespuesta = objTicketRespuesta.ObtenerLoginTicketResponse(service, WSAA_Url, certificadoPath, IsDebugEnable);
            return objTicketRespuesta;
        }
    }
}
