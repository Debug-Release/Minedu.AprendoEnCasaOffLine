using MediatR;
using Release.Helper;

namespace Minedu.AprendoEnCasaOffLine.Contenido.Core.Commands
{
    public class RegistrarServidorCommand : IRequest<StatusResponse>
    {
        public string ip { get; set; }
        public string mac { get; set; }
        public string nombre { get; set; }
        public string fqdn { get; set; }

    }
}
