using Release.Helper.Proxy;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using DTO = Minedu.AprendoEnCasaOffLine.Contenido.Core.ViewModel;

namespace Minedu.AprendoEnCasaOffLine.Contenido.Test.Proxy
{
    public class ContenidoProxy : BaseProxy
    {
        private string _url;
        public DTO.TokenResponse Token { get; set; }

        public ContenidoProxy(string url)
        {
            this._url = url.TrimEnd('/');

            //Validate certificate
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
        }
        public DTO.TokenResponse GetToken(string userName, string password)
        {
            string authorizationMethod = "Basic";
            string authorizationToken = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{userName}:{password}"));

            var headers = new Dictionary<string, string>()
            {
                { "client_id","download-offline_dev" }
            };

            var token = this.CallWebApi<DTO.TokenResponse>(HttpMethod.Post, this._url + "/security/token", authorizationMethod: authorizationMethod, authorizationToken: authorizationToken, headers: headers);
            return token;

        }

        public DTO.Servidor[] obtenerServidores()
        {
            return this.CallWebApi<DTO.Servidor[]>(HttpMethod.Get, this._url + "/Contenido/obtenerServidores", null,
                authorizationToken: Token.access_token,
                authorizationMethod: Token.token_type);
        }



    }
}
