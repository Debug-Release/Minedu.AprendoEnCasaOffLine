using MediatR;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using Release.Helper;
using Release.MongoDB.Repository;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Minedu.AprendoEnCasaOffLine.Contenido.Core.Commands
{

    public class ActualizarDescargaCommandHandler : IRequestHandler<ActualizarDescargaCommand, StatusResponse>
    {
        private readonly ILogger<ActualizarDescargaCommandHandler> _logger;
        private readonly IBaseRepository<Model.Descarga> _descargaRepository;

        public ActualizarDescargaCommandHandler(
            IBaseRepository<Model.Descarga> descargaRepository,
            ILogger<ActualizarDescargaCommandHandler> logger)
        {
            _descargaRepository = descargaRepository;
            _logger = logger;
        }
        public async Task<StatusResponse> Handle(ActualizarDescargaCommand request, CancellationToken cancellationToken)
        {
            var sr = new StatusResponse { StatusCode = 200 };

            _logger.LogInformation($"Actualizando Descarga idDescarga:{request.idDescarga}");

            var q = _descargaRepository.FirstOrDefault(x =>
                    x.id == ObjectId.Parse(request.idDescarga) &&
                    x.esActivo == true &&
                    x.esEliminado == false &&
                    (x.estado == EstadoDescarga.Programado || x.estado == EstadoDescarga.Descargando)
                    );

            if (q != null)
            {
                q.estado = EstadoDescarga.Descargado;
                q.fechaModificacion = DateTime.Now;
                q.fechaFin = q.fechaModificacion;

                await _descargaRepository.UpdateOneAsync(q.id, q);

                sr.Success = true;                
                sr.Messages.Add($"La descarga del contenido [{q.contenido.nombre}] se actualizó correctamente a estado descargado");
            }
            else
            {
                sr.Success = false;
                sr.Messages.Add($"No existe descarga programada con id [{request.idDescarga}]");
                _logger.LogWarning($"No existe descarga programada con id [{request.idDescarga}]");
            }

            return await Task.FromResult(sr);
        }
    }

}