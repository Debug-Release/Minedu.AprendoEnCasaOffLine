using MediatR;
using Release.Helper;
using System;

namespace Minedu.AprendoEnCasaOffLine.Contenido.Core.Commands
{
    public class ProgramarTodoDescargaCommand : IRequest<StatusResponse>
    {
        public string idContenido { get; set; }       
    }
}
