using MediatR;
using Microsoft.Extensions.Logging;
using Minedu.AprendoEnCasaOffLine.Contenido.Core.Aplicacion;
using Release.Helper;
using Release.MongoDB.Repository;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Minedu.AprendoEnCasaOffLine.Contenido.Core.Commands
{
    public class RegistrarContenidoCommandHandler : IRequestHandler<RegistrarContenidoCommand, StatusResponse>
    {
        private readonly IBaseRepository<Model.Contenido> _contenidoRepository;
        private readonly ContenidoValidation _contenidoValidation;
        private readonly ILogger<RegistrarContenidoCommandHandler> _logger;

        public RegistrarContenidoCommandHandler(IBaseRepository<Model.Contenido> contenidoRepository, ContenidoValidation contenidoValidation, ILogger<RegistrarContenidoCommandHandler> logger)
        {
            _contenidoRepository = contenidoRepository;
            _contenidoValidation = contenidoValidation;
            _logger = logger;
        }

        public async Task<StatusResponse> Handle(RegistrarContenidoCommand request, CancellationToken cancellationToken)
        {
            var sr = await Execute.TryCatchAsync(() =>
            {
                return _contenidoValidation.ValidaRegistrar(request);
            },
            () =>
            {
                var r = _contenidoRepository.InsertOneAsync(new Model.Contenido
                {
                    nombre = null,
                    descripcion = null,
                    archivo = request.archivo,
                    pesoMb = request.pesoMb,
                    estado = EstadoContenido.Cargado, //Cargado = cuando el archivo ya esta cargado en directorio de descargas
                    esActivo = true,
                    fechaCreacion = DateTime.Now
                }).Result;

                return new StatusResponse(true, "El contenido se registro correctamente", data: r.id);

            }, _logger);

            return sr;
        }
    }
}
