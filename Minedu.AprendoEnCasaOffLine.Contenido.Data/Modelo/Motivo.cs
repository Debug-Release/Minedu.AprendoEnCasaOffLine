using MongoDB.Bson.Serialization.Attributes;

namespace Minedu.AprendoEnCasaOffLine.Contenido.Data.Modelo
{
    public class Motivo
    {
        [BsonId]
        public int id { get; set; }
        public int estado_id { get; set; }
        public string descripcion { get; set; }
    }
}
