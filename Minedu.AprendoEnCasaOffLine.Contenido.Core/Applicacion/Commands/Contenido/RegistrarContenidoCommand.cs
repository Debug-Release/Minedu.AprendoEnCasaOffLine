using MediatR;
using Release.Helper;

namespace Minedu.AprendoEnCasaOffLine.Contenido.Core.Commands
{
    public class RegistrarContenidoCommand : IRequest<StatusResponse>
    {
        //public string nombre { get; set; }
        //public string descripcion { get; set; }
        public string archivo { get; set; }
        public string pesoMb { get; set; }
    }
}
