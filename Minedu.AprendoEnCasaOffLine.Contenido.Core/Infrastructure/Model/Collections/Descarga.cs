using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Release.MongoDB.Repository;
using Release.MongoDB.Repository.Model;
using System;

namespace Minedu.AprendoEnCasaOffLine.Contenido.Core.Model
{
    [CollectionProperty("descarga")]
    [BsonIgnoreExtraElements]
    public class Descarga : EntityToLower<ObjectId>
    {
        public ObjectId idContenido { get; set; }
        public ObjectId idServidor { get; set; }
        public DateTime fechaProgramada { get; set; }
        public DateTime? fechaInicio { get; set; }
        public DateTime? fechaFin { get; set; }
        public EstadoDescarga estado { get; set; }
    }
}
