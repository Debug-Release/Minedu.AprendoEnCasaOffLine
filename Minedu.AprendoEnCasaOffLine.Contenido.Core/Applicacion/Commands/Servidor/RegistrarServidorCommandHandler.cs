using MediatR;
using Microsoft.Extensions.Logging;
using Release.Helper;
using Release.MongoDB.Repository;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Minedu.AprendoEnCasaOffLine.Contenido.Core.Commands
{
    public class RegistrarServidorCommandHandler : IRequestHandler<RegistrarServidorCommand, StatusResponse>
    {
        private readonly ILogger<RegistrarServidorCommandHandler> _logger;
        private readonly IBaseRepository<Model.Servidor> _servidorRepository;
        private readonly ICollectionContext<Model.Servidor> _servidorCollection;
        private readonly IMediator _mediator;

        public RegistrarServidorCommandHandler(IBaseRepository<Model.Servidor> servidorRepository, ICollectionContext<Model.Servidor> servidorCollection, IMediator mediator, ILogger<RegistrarServidorCommandHandler> logger)
        {
            _servidorRepository = servidorRepository;
            _servidorCollection = servidorCollection;
            _mediator = mediator;
            _logger = logger;
        }

        public async Task<StatusResponse> Handle(RegistrarServidorCommand request, CancellationToken cancellationToken)
        {

            return await Execute.TryCatchAsync(() =>
              {
                  var sr = new StatusResponse();

                  var r = _servidorRepository.InsertOneAsync(new Model.Servidor
                  {
                      ip = null,//request.ip,
                      mac = request.mac,
                      //mac1 = macs,
                      nombre = request.nombre,
                      //fqdn = request.fqdn,
                      esActivo = true,
                      fechaCreacion = DateTime.Now
                  }).Result;


                  sr.Success = true;
                  return sr;
              }, _logger);

        }
    }
}
