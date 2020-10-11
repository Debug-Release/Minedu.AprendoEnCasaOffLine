using MediatR;
using Release.Helper;
using System;

namespace Minedu.AprendoEnCasaOffLine.Contenido.Core.Commands
{
    public class ProgramarDescargaCommand : IRequest<StatusResponse>
    {
        //public string idContenido { get; set; }
        public string ipServidor { get; set; }
        //public string macServidor { get; set; }
        //public DateTime fechaProgramada { get; set; }
        //public DateTime? fechaInicio { get; set; }
        //public DateTime? fechaFin { get; set; }
        //public EstadoDescarga estado { get; set; }
    }
}
