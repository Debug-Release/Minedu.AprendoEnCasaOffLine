using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Release.MongoDB.Repository;
using Release.MongoDB.Repository.Model;
using System;

namespace Minedu.AprendoEnCasaOffLine.Contenido.Data.Collections
{
    [CollectionProperty("servidor")]
    [BsonIgnoreExtraElements]
    public class Servidor : EntityToLower<ObjectId>
    {
        public string ip { get; set; }
        public string mac { get; set; }
        public string nombre { get; set; }
        public string fqdn { get; set; }
    }
}
