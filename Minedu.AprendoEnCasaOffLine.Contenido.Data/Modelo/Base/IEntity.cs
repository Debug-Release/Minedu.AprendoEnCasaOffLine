using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Minedu.AprendoEnCasaOffLine.Contenido.Data.Modelo
{
    public interface IEntity<TKey>
    {
        TKey id { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        DateTime fechaCreacion { get; set; }        

        bool esActivo { get; set; }
    }

    public interface IEntity : IEntity<ObjectId>
    {
    }
}
