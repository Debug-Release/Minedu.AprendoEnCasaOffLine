using Microsoft.VisualStudio.TestTools.UnitTesting;
using Minedu.AprendoEnCasaOffLine.Contenido.Test.Proxy;
using System.Linq;
using System.Threading.Tasks;
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
        public async Task TestInitialize()
        {
            _apiProxy = new ContenidoProxy(urlProxy);
            var config = new DTO.AccessConfig
            {
                userName = "DOWNLOADOFFLINE_DEV",
                password = "rYWXlDIqFSsjrpFEehaqYFkcofOsdFLdhsZIsViF3a6WxHZq0CFDdg==",
                client_id = "download-offline_dev"
            };

            _token = await _apiProxy.GetToken(config);
            _apiProxy.Token = _token;
        }

        [TestMethod]
        public async Task obtenerServidores()
        {

            //Leyendo         
            var items = await _apiProxy.obtenerServidores();

            Assert.AreEqual(true, items.Any());

        }
    }
}
