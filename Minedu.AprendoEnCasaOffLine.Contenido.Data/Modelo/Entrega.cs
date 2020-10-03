using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Minedu.AprendoEnCasaOffLine.Contenido.Data.Modelo
{
    public class Entrega : IEntity
    {
        public bool esActivo { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime fechaCreacion { get; set; }
        public DateTime? fechaModificacion { get; set; }
        public ObjectId id { get; set; }

        //Fields
        public int courier_id { get; set; }
        public int estado_id { get; set; } //Estado Actual del Folio
        public string ordenCompra { get; set; }
        public string subOrdenCompra { get; set; }
        public string folio { get; set; }
        public string dni { get; set; }
        public string cliente { get; set; }
        public string direccion { get; set; }
        public string provincia { get; set; }
        public string distrito { get; set; }
        public string fechaPactada { get; set; }
        public List<Producto> productos { get; set; }
    }
    public class Producto
    {
        [BsonId]
        public string id { get; set; }
        public string sku { get; set; }
        public string descripcion { get; set; }
        public string cantidad { get; set; }
    }
}
