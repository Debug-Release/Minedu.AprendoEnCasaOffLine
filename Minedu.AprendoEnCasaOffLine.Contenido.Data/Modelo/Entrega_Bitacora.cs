using MongoDB.Bson;
using System;

namespace Minedu.AprendoEnCasaOffLine.Contenido.Data.Modelo
{
    public class Entrega_Bitacora : IEntity
    {
        public bool esActivo { get; set; }
        public DateTime fechaCreacion { get; set; }
        public DateTime? fechaModificacion { get; set; }
        public ObjectId id { get; set; }

        //Fields        
        public ObjectId entrega_id { get; set; }        
        public string folio { get; set; } 
        public int motivo_id { get; set; }
        public string ubicacion { get; set; }
        public byte[] imagen { get; set; }
    }
}
