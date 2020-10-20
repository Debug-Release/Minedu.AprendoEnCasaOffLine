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
                if (request.extraido && request.procesado)
                {
                    q.estado = EstadoDescarga.Descargado;
                }
                else
                {
                    q.estado = EstadoDescarga.Error;
                }

                q.fechaModificacion = DateTime.Now;
                q.fechaFin = q.fechaModificacion;

                await _descargaRepository.UpdateOneAsync(q.id, q);

                sr.Success = true;
                sr.Messages.Add($"La descarga del contenido [{q.contenido.nombre}] se actualizó correctamente");

                //Persiste ack json
                try
                {
                    var rutaACK = Functions.GetEnvironmentVariable("RUTA_ACK");
                    string ackname = $"aeco_paq_ack-{request.fecha.ToString("yyyy-MM-dd_hhmmss")}.json";
                    string pathJson = System.IO.Path.Combine(rutaACK, ackname);
                    string jsonData = Newtonsoft.Json.JsonConvert.SerializeObject(request);
                    await System.IO.File.WriteAllTextAsync(pathJson, jsonData);

                    _logger.LogInformation($"El ack para {request.nombrearchivo} se guardo correctamente");
                }
                catch (Exception ex)
                {

                    _logger.LogError(ex, $"Error al escribir el ack {request.nombrearchivo}");
                }

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