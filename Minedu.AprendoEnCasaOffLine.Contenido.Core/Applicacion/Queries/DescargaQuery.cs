using MediatR;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using Release.Helper;
using Release.MongoDB.Repository;
using System;
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
                    //Leer Archivo de disco
                    //Por ahora esta de esta forma...falta tunear
                    if (System.IO.File.Exists(q.contenido.archivo))
                    {
                        _logger.LogInformation($"Inicia Lectura de archivo:{q.contenido.archivo}");

                        //Marcar descarga Descargando

                        q.estado = EstadoDescarga.Descargando;
                        q.fechaModificacion = DateTime.Now;
                        q.fechaInicio = q.fechaModificacion;

                        await _descargaRepository.UpdateOneAsync(q.id, q);

                        _logger.LogInformation($"El archivo:{q.contenido.archivo} está en estado descargando...");

                        sr.FileBytes = await System.IO.File.ReadAllBytesAsync(q.contenido.archivo);
                        sr.FileName = System.IO.Path.GetFileName(q.contenido.archivo);
                        sr.ContentType = "application/gzip";
                        sr.Exists = true;

                        _logger.LogInformation($"Termina Lectura de archivo:{q.contenido.archivo}");
                    }
                    else
                    {
                        sr.Exists = false;
                        _logger.LogWarning($"El archivo:{q.contenido.archivo} no existe");
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