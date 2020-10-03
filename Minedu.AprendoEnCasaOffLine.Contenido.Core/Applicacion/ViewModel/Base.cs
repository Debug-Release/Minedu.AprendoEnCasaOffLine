using System;

namespace Minedu.AprendoEnCasaOffLine.Contenido.Core.ViewModel
{
    public class Base
    {
        public string id { get; set; }
        public DateTime fechaCreacion { get; set; }
        public DateTime? fechaModificacion { get; set; }
        public bool esActivo { get; set; }
    }
}
