using Release.Helper;
using Release.Helper.Proxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using DTO = Minedu.AprendoEnCasaOffLine.Contenido.Entities;

namespace Minedu.AprendoEnCasaOffLine.Contenido.Console.Proxy
{
    public class DeliveryApiProxy : BaseProxy
    {
        private string _url;
        public DeliveryApiProxy(string url)
        {
            this._url = url.TrimEnd('/');

            //Validate certificate
            //ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
        }

        public DTO.Courier[] Couriers()
        {
            return this.CallWebApi<DTO.Courier[]>(HttpMethod.Get, this._url + "/QueryEntrega/couriers", null);
        }

        public CommandResponse CargaMasiva(List<DTO.Entrega> entregas)
        {
            var request = new DTO.CargaMasivaCommand { Entregas = entregas.ToArray() };

            //var reqJson = Newtonsoft.Json.JsonConvert.SerializeObject(entregas.ToArray());
            var reqJson = this.GetJsonParameters(request);
            var cr = this.CallWebApi<CommandResponse>(HttpMethod.Post, this._url + "/CommandEntrega/cargaMasiva", reqJson, timeout: 500);

            return cr;
        }

        public CommandResponse RegistraMotivos(List<DTO.Motivo> motivos)
        {
            var reqJson = Newtonsoft.Json.JsonConvert.SerializeObject(motivos.ToArray());
            var cr = this.CallWebApi<CommandResponse>(HttpMethod.Post, this._url + "/CommandEntrega/registraMotivos", reqJson, timeout: 500);

            return cr;
        }
    }
}
