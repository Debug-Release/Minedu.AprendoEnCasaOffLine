using MediatR;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using Release.Helper;
using Release.MongoDB.Repository;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Minedu.AprendoEnCasaOffLine.Contenido.Core.Queries
{
    public class DescargaQuery : IRequest<FileResponse>
    {
        public string idDescarga { get; set; }

        #region Handler

        public class DescargaQueryHandler : IRequestHandler<DescargaQuery, FileResponse>
        {
            private readonly ILogger<DescargaQueryHandler> _logger;
            private readonly IBaseRepository<Model.Descarga> _descargaRepository;
            public DescargaQueryHandler(
                IBaseRepository<Model.Descarga> descargaRepository,
                ILogger<DescargaQueryHandler> logger)
            {
                _descargaRepository = descargaRepository;
                _logger = logger;
            }
            public async Task<FileResponse> Handle(DescargaQuery request, CancellationToken cancellationToken)
            {
                var sr = new FileResponse();

                _logger.LogInformation($"Inicia Descarga idDescarga:{request.idDescarga}");

                var q = _descargaRepository.FirstOrDefault(x =>
                        x.id == ObjectId.Parse(request.idDescarga) &&
                        x.esActivo == true &&
                        x.esEliminado == false &&
                        (x.estado == EstadoDescarga.Programado || x.estado == EstadoDescarga.Descargando)
                        );

                if (q != null)
                {
                    //validar la hora de la consulta y la hora programada
                    if (DateTime.Now >= q.fechaProgramada)
                    {
                        //Leer Archivo de disco

                        string pathArchivos = Functions.GetEnvironmentVariable("RUTA_ARCHIVOS");
                        pathArchivos = Path.Combine(pathArchivos, q.contenido.archivo);

                        if (File.Exists(pathArchivos))
                        {
                            _logger.LogInformation($"Inicia Lectura de archivo:{pathArchivos}");

                            //Marcar descarga Descargando

                            q.estado = EstadoDescarga.Descargando;
                            q.fechaModificacion = DateTime.Now;
                            q.fechaInicio = q.fechaModificacion;

                            await _descargaRepository.UpdateOneAsync(q.id, q);

                            _logger.LogInformation($"El archivo:{pathArchivos} está en estado descargando...");

                            sr.FileBytes = await System.IO.File.ReadAllBytesAsync(pathArchivos);
                            sr.FileName = System.IO.Path.GetFileName(q.contenido.archivo);
                            sr.ContentType = "application/gzip";
                            sr.Exists = true;

                            _logger.LogInformation($"Termina Lectura de archivo:{pathArchivos}");
                        }
                        else
                        {
                            sr.Exists = false;
                            _logger.LogWarning($"El archivo:{pathArchivos} no existe");
                        }
                    }
                    else
                    {
                        sr.Exists = false;
                        
                        _logger.LogWarning($"La hora de consulta de descarga aun no supera la hora programada");
                    }

                }
                else
                {
                    sr.Exists = false;
                    _logger.LogWarning($"No existe descarga programada con id [{request.idDescarga}]");
                }

                return await Task.FromResult(sr);
            }
        }

        #endregion
    }
}