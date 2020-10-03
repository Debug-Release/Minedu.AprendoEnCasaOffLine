using MongoDB.Bson.Serialization.Attributes;

namespace Minedu.AprendoEnCasaOffLine.Contenido.Data.Modelo
{
    public class Estado
    {
        [BsonId]
        public int id { get; set; }
        public string descripcion { get; set; }
    }
}
