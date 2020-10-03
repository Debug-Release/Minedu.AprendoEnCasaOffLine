using Release.Helper;
using Release.Helper.Pagination;
using Release.Helper.Proxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using DTO = Minedu.AprendoEnCasaOffLine.Contenido.Entities;

namespace Minedu.AprendoEnCasaOffLine.Contenido.Test.Proxy
{
    public class DeliveryApiProxy : BaseProxy
    {
        private string _url;
        public DTO.TokenResponse Token { get; set; }

        public DeliveryApiProxy(string url)
        {
            this._url = url.TrimEnd('/');

            //Validate certificate
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
        }
        public DTO.TokenResponse GetToken(string userName, string password)
        {
            string authorizationMethod = "Basic";
            string authorizationToken = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{userName}:{password}"));

            var token = this.CallWebApi<DTO.TokenResponse>(HttpMethod.Post, this._url + "/ExternalAccount/token", authorizationMethod: authorizationMethod, authorizationToken: authorizationToken);
            return token;

        }

        public DTO.Motivo[] Motivos()
        {
            return this.CallWebApi<DTO.Motivo[]>(HttpMethod.Get, this._url + "/ExternalEntrega/motivos", null,
                authorizationToken: Token.access_token,
                authorizationMethod: Token.token_type);
        }

        public PagedResponse<DTO.Entrega> BusquedaPaginada(DTO.EntregaFiltro filtro)
        {
            string parameters = this.GetJsonParameters(filtro);
            return this.CallWebApi<PagedResponse<DTO.Entrega>>(HttpMethod.Post, this._url + "/ExternalEntrega/busquedaPaginada", parameters,
                authorizationToken: Token.access_token,
                authorizationMethod: Token.token_type);
        }
        public DTO.Entrega_Bitacora[] VisitasByFolio(string folio)
        {

            return this.CallWebApi<DTO.Entrega_Bitacora[]>(HttpMethod.Get, this._url + "/ExternalEntrega/visitasByFolio/" + folio, null,
                authorizationToken: Token.access_token,
                authorizationMethod: Token.token_type);
        }
        public CommandResponse ActualizarEstado(DTO.Entrega_Bitacora request)
        {
            string parameters = this.GetJsonParameters(request);
            return this.CallWebApi<CommandResponse>(HttpMethod.Post, this._url + "/ExternalEntrega/actualizarEstado", parameters,
                authorizationToken: Token.access_token,
                authorizationMethod: Token.token_type);
        }

    }
}
