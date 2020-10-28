using MediatR;
using Release.Helper;
using System.Collections.Generic;
using System.Text;

namespace Minedu.AprendoEnCasaOffLine.Contenido.Core.Commands
{
    public class TrazabilidadCommand : IRequest<StatusResponse>
    {
        public byte[] archivo { get; set; }
    }
}
