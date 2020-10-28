using MediatR;
using Release.Helper;

namespace Minedu.AprendoEnCasaOffLine.Contenido.Core.Commands
{
    public class ContinuidadOperativaCommand : IRequest<StatusResponse>
    {
        public ContinuidadOperativaCommand()
        {
            aplicaciones = new ViewModel.Aplicacion[] { };
        }
        public string memoria { get; set; }
        public string procesador { get; set; }
        public string disco { get; set; }
        public ViewModel.Aplicacion[] aplicaciones { get; set; }
    }

}
