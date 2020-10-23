using Microsoft.VisualStudio.TestTools.UnitTesting;
using Minedu.AprendoEnCasaOffLine.Contenido.Test.Proxy;
using System.Linq;
using DTO = Minedu.AprendoEnCasaOffLine.Contenido.Core.ViewModel;


namespace Minedu.AprendoEnCasaOffLine.Contenido.Test
{
    [TestClass]
    public class UnitTest
    {
        private const string urlProxy = "https://localhost:44388";

        private ContenidoProxy _apiProxy;
        private DTO.TokenResponse _token = null;

        [TestInitialize]
        public void TestInitialize()
        {
            _apiProxy = new ContenidoProxy(urlProxy);

            _token = _apiProxy.GetToken("DOWNLOADOFFLINE_DEV", "rYWXlDIqFSsjrpFEehaqYFkcofOsdFLdhsZIsViF3a6WxHZq0CFDdg==");
            _apiProxy.Token = _token;
        }

        [TestMethod]
        public void obtenerServidores()
        {

            //Leyendo         
            var items = _apiProxy.obtenerServidores().Result;

            Assert.AreEqual(true, items.Any());

        }
    }
}
