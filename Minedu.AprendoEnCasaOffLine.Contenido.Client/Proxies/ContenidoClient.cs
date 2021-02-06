using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace Minedu.AprendoEnCasaOffLine.Contenido.Client.ContenidoClient
{
    public partial class Client
    {
        public Client(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("MsContenido");
            _settings = new System.Lazy<Newtonsoft.Json.JsonSerializerSettings>(CreateSerializerSettings);
        }
    }
}
