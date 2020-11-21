using MediatR;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using Release.Helper;
using Release.MongoDB.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Minedu.AprendoEnCasaOffLine.Contenido.Core.Commands
{
    public class ProgramarDescargaCommandHandler : IRequestHandler<ProgramarDescargaCommand, StatusResponse>
    {
        private readonly ILogger<ProgramarDescargaCommandHandler> _logger;

        private readonly IBaseRepository<Model.Descarga> _descargaRepository;
        private readonly IBaseRepository<Model.Servidor> _servidorRepository;
        private readonly ICollectionContext<Model.Contenido> _contenidoCollection;

        public ProgramarDescargaCommandHandler(
            IBaseRepository<Model.Descarga> descargaRepository,
            ICollectionContext<Model.Contenido> contenidoCollection,
            IBaseRepository<Model.Servidor> servidorRepository,
            ILogger<ProgramarDescargaCommandHandler> logger)
        {
            _descargaRepository = descargaRepository;
            _contenidoCollection = contenidoCollection;
            _servidorRepository = servidorRepository;
            _logger = logger;
        }

        public async Task<StatusResponse> Handle(ProgramarDescargaCommand request, CancellationToken cancellationToken)
        {
            var sr = new StatusResponse
            {
                StatusCode = 200,
                Success = true,
            };

            Model.Contenido contenido = null;
            var hdinicio = Functions.GetEnvironmentVariable("HORA_DESCARGA_INICIO");
            var hdintervalo = Functions.GetEnvironmentVariable("HORA_DESCARGA_INTERVALO");
            int sintervalo = int.Parse(Functions.GetEnvironmentVariable("SERVIDOR_INTERVALO"));

            DateTime dHoraInicio = DateTime.ParseExact(hdinicio, "hh:mm:ss", null);
            TimeSpan tsHoraIntervalo = TimeSpan.Parse(hdintervalo);

            var valid = Validacion(request);
            if (valid.Any())
            {
                sr.Messages.AddRange(valid);
                sr.Success = false;
                //Return 1
                return await Task.FromResult(sr);
            }
            /*
            if (!string.IsNullOrWhiteSpace(request.idContenido))
            {
                var filter = Builders<Model.Contenido>.Filter.Where(s => s.id == ObjectId.Parse(request.idContenido) &&
                                                                         s.estado == EstadoContenido.Cargado &&
                                                                         s.esEliminado == false &&
                                                                         s.esActivo == true);
                contenido = await _contenidoCollection.Context().Find<Model.Contenido>(filter)
                                                                          .FirstOrDefaultAsync();
            }
            else
            */
            //{
            //ultimo Contenido Cargado
            var filter = Builders<Model.Contenido>.Filter.Where(s => s.estado == EstadoContenido.Cargado &&
                                                                            s.esEliminado == false &&
                                                                            s.esActivo == true);
            contenido = await _contenidoCollection.Context().Find<Model.Contenido>(filter)
                                                                      .Limit(1)
                                                                      .SortByDescending(f => f.fechaCreacion)
                                                                      .FirstOrDefaultAsync();
            //}

            if (contenido == null)
            {
                sr.Messages.Add("No se encuentra disponible contenido para descargar");
                sr.Success = false;
                //Return 2
                return await Task.FromResult(sr);
            }

            var validDescargado = _descargaRepository.FirstOrDefault(x =>
                        x.macServidor.ToUpper() == request.macServidor.ToUpper() &&
                        x.esActivo == true &&
                        x.esEliminado == false &&
                        (x.estado == EstadoDescarga.Descargado) &&
                        x.contenido.id == contenido.id
                        );
            if (validDescargado != null)
            {
                sr.Messages.Add($"Ya se realizó una descarga para el servidor [{request.macServidor}] con el archivo [{contenido.archivo}]");
                sr.Success = false;
                //Return 3
                return await Task.FromResult(sr);
            }

            var validDescarga = _descargaRepository.FirstOrDefault(x =>
                        x.macServidor.ToUpper() == request.macServidor.ToUpper() &&
                        x.esActivo == true &&
                        x.esEliminado == false &&
                        (x.estado == EstadoDescarga.Programado || x.estado == EstadoDescarga.Descargando) &&
                        x.contenido.id == contenido.id
                        );

            if (validDescarga != null)
            {
                sr.Messages.Add($"Hay una descarga programada con el contenido [{contenido.archivo}]");
                sr.Success = false;
                //sr.StatusCode = 100;
                //Return 3
                return await Task.FromResult(sr);
            }

            var pNow = DateTime.Now;
            var currentTime = new TimeSpan(pNow.Hour, pNow.Minute, pNow.Second);
            var startTime = new TimeSpan(dHoraInicio.Hour, dHoraInicio.Minute, dHoraInicio.Second);

            var dProgramadas = _descargaRepository.Query(x =>
                 //x.ipServidor == request.ipServidor &&
                 x.esActivo == true &&
                 x.esEliminado == false &&
                 (x.estado == EstadoDescarga.Programado || x.estado == EstadoDescarga.Descargando) &&
                 x.contenido.id == contenido.id
             );

            if (currentTime >= startTime)
            {
                dHoraInicio = dHoraInicio.AddDays(1);
            }

            if (dProgramadas.Any())
            {
                var maxFechaProgramada = dProgramadas.Max(p => p.fechaProgramada);
                var countDescargasProgramadas = dProgramadas.Count(c => c.fechaProgramada == maxFechaProgramada) + 1;
                if (countDescargasProgramadas <= sintervalo)
                {
                    dHoraInicio = maxFechaProgramada;
                }
                else
                {
                    dHoraInicio = maxFechaProgramada.Add(tsHoraIntervalo);
                }
            }

            var r = await _descargaRepository.InsertOneAsync(new Model.Descarga
            {
                contenido = new Model.ContenidoDescarga
                {
                    id = contenido.id,
                    nombre = contenido.nombre,
                    fechaCreacion = (contenido.fechaModificacion == null ? contenido.fechaCreacion : contenido.fechaModificacion.Value),
                    descripcion = contenido.descripcion,
                    archivo = contenido.archivo,
                    pesoMb = contenido.pesoMb
                },
                fechaProgramada = dHoraInicio,
                macServidor = request.macServidor,
                estado = EstadoDescarga.Programado,
                esActivo = true,
                fechaCreacion = DateTime.Now
            });

            sr.Messages.Add($"La descarga ha sido programada correctamente para servidor [mac:{request.macServidor}]");

            return await Task.FromResult(sr);
        }

        private List<string> Validacion(ProgramarDescargaCommand request)
        {
            var v = new List<string>();

            //validar servidor
            var servidor = _servidorRepository.FirstOrDefault(x => x.mac.ToUpper() == request.macServidor.ToUpper());
            if (servidor == null)
            {
                string m = $"La dirección mac del servidor [mac:{request.macServidor}] no está registrada";
                v.Add(m);
            }

            //validar si ya existe una descarga programada

            //var descarga = _descargaRepository.FirstOrDefault(x => x.ip == request.ipServidor);

            return v;
        }
    }
}
