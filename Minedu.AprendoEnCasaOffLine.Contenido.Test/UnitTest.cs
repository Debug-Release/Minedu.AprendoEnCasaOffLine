using Minedu.AprendoEnCasaOffLine.Contenido.Test.Proxy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using DTO = Minedu.AprendoEnCasaOffLine.Contenido.Entities;


namespace Minedu.AprendoEnCasaOffLine.Contenido.Test
{
    [TestClass]
    public class UnitTest
    {
        private const string urlPortalSaCApi = "https://falabellape-portalsacapi.azurewebsites.net";
      
        private DeliveryApiProxy _deliveryApiProxy;
        private DTO.TokenResponse _token = null;

        [TestInitialize]
        public void TestInitialize()
        {
            _deliveryApiProxy = new DeliveryApiProxy(urlPortalSaCApi);

            _token = _deliveryApiProxy.GetToken("INTELOGIS", "Iid97747543B5C138422C33FEF431!");
            _deliveryApiProxy.Token = _token;
        }

        [TestMethod]
        public void Motivos()
        {

            //Leyendo Couriers permitidos            
            var couriers = _deliveryApiProxy.Motivos();

            Assert.AreEqual(true, couriers.Any());

        }
        [TestMethod]
        public void BusquedaPaginada()
        {
            var request = new DTO.EntregaFiltro
            {
                Page = 1,
                PageSize = 10,
                estado_id = 0, //PENDIENTE
                courier_id = 1, //INTELOGIS
                //fechaCreacion = new System.DateTime(2019, 11, 22)
            };

            var folios = _deliveryApiProxy.BusquedaPaginada(request);

            Assert.AreEqual(true, folios.Data.Any());

        }
        [TestMethod]
        public void VisitasByFolio()
        {

            //Leyendo Couriers permitidos
            var visitas = _deliveryApiProxy.VisitasByFolio("12782513531");
            var result = visitas.Any();
            Assert.AreEqual(result, visitas.Any());

        }

        [TestMethod]
        public void ActualizarEstado()
        {            
            DTO.Entrega_Bitacora request = new DTO.Entrega_Bitacora
            {
                folio = "12579983394",
                courier = new DTO.Courier { id = 1 }, //INTELOGIS
                fechaCreacion = DateTime.Now,
                motivo = new DTO.Motivo { id = 17 }, 
                ubicacion = "12456545,-12845454545"

            };

            //Leyendo Couriers permitidos
            var actualizarEstado = _deliveryApiProxy.ActualizarEstado(request);

            var result = actualizarEstado.Success;
            Assert.AreEqual(result, actualizarEstado.Success);

        }
    }
}
