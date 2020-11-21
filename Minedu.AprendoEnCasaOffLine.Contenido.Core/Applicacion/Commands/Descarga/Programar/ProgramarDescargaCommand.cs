using MediatR;
using Release.Helper;
using System;

namespace Minedu.AprendoEnCasaOffLine.Contenido.Core.Commands
{
    public class ProgramarDescargaCommand : IRequest<StatusResponse>
    {        
        public string macServidor { get; set; }       
    }
}
