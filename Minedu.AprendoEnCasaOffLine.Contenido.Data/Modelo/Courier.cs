using MongoDB.Bson.Serialization.Attributes;

namespace Minedu.AprendoEnCasaOffLine.Contenido.Data.Modelo
{
    public class Courier
    {
        [BsonId]
        public int id { get; set; }
        public string nombre { get; set; }
    }
}
