using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json.Converters;
using Release.MongoDB.Repository;
using Release.MongoDB.Repository.Model;
using System;
using System.Text.Json.Serialization;

namespace Minedu.AprendoEnCasaOffLine.Contenido.Core.Model
{
    [CollectionProperty("descarga")]
    [BsonIgnoreExtraElements]
    public class Descarga : EntityToLower<ObjectId>
    {       
        public ContenidoDescarga contenido { get; set; }      
        public string ipServidor { get; set; }
        public DateTime fechaProgramada { get; set; }
        public DateTime? fechaInicio { get; set; }
        public DateTime? fechaFin { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        [BsonRepresentation(BsonType.String)]
        public EstadoDescarga estado { get; set; }
    }
    public class ContenidoDescarga
    {
        public ObjectId id { get; set; }
        public string nombre { get; set; }
        public DateTime fechaCreacion { get; set; }
        public string descripcion { get; set; }
        public string archivo { get; set; }
        public string pesoMb { get; set; }        
    }
}
