using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json.Converters;
using Release.MongoDB.Repository;
using Release.MongoDB.Repository.Model;
using System.Text.Json.Serialization;

namespace Minedu.AprendoEnCasaOffLine.Contenido.Core.Model
{
    [CollectionProperty("contenido")]
    [BsonIgnoreExtraElements]
    public class Contenido : EntityToLower<ObjectId>
    {
        public string nombre { get; set; }
        public string descripcion { get; set; }
        public string rutaOrigen { get; set; }
        public string pesoMb { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        [BsonRepresentation(BsonType.String)]
        public EstadoContenido estado { get; set; }
    }
}
