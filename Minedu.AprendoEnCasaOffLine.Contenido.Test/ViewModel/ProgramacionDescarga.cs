using System;

namespace Minedu.AprendoEnCasaOffLine.Contenido.Core.ViewModel
{
    public class ProgramacionDescarga : Base
    {
        public Contenido contenido { get; set; }
        public string macServidor { get; set; }
        public DateTime fechaProgramada { get; set; }
        public DateTime? fechaInicio { get; set; }
        public DateTime? fechaFin { get; set; }
        public EstadoDescarga estado { get; set; }        
    }
}
