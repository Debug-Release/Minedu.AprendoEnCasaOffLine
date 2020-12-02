using MediatR;
using Release.Helper;
using System.Collections.Generic;
using System.Text;

namespace Minedu.AprendoEnCasaOffLine.Contenido.Core.Commands
{
    public class RegistrarSaludoCommand : IRequest<StatusResponse>
    {        
        public string mensaje { get; set; }
    }
}
