using MediatR;
using Release.Helper;
using System;

namespace Minedu.AprendoEnCasaOffLine.Contenido.Core.Commands
{
    public class ActualizarDescargaCommand : IRequest<StatusResponse>
    {
        public string idDescarga { get; set; }
    }
}
