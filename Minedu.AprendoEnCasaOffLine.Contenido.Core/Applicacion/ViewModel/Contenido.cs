using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Minedu.AprendoEnCasaOffLine.Contenido.Core.ViewModel
{
    //[JsonObject(NamingStrategyType = typeof(DefaultNamingStrategy))]    
    public class Contenido : Base
    {
        //public string nombre { get; set; }
        //public string descripcion { get; set; }
        public string archivo { get; set; }
        public string pesoMb { get; set; }
        public EstadoContenido estado { get; set; }
    }
}
