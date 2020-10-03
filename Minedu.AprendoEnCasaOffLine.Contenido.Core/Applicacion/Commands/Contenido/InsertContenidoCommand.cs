using MediatR;
using Release.Helper;

namespace Minedu.AprendoEnCasaOffLine.Contenido.Core.Commands
{
    public class InsertContenidoCommand : IRequest<CommandResponse>
    {
        public string nombre { get; set; }
        public string descripcion { get; set; }
        public string rutaOrigen { get; set; }
        public string pesoMb { get; set; }
    }
}
