using MediatR;
using Release.Helper;
using System;

namespace Minedu.AprendoEnCasaOffLine.Contenido.Core.Commands
{
    public class ActualizarDescargaCommand : IRequest<StatusResponse>
    {
        public string idDescarga { get; set; }
        public string mac { get; set; }

        //ACK
        public Guid guidempq { get; set; }
        public string nombrearchivo { get; set; }
        public bool extraido { get; set; }
        public bool procesado { get; set; }
        public bool script { get; set; }
        public DateTime fecha { get; set; }
    }
}
