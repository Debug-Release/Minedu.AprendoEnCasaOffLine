﻿using MediatR;
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
    public class ProgramarTodoDescargaCommandHandler : IRequestHandler<ProgramarTodoDescargaCommand, StatusResponse>
    {
        private readonly ILogger<ProgramarDescargaCommandHandler> _logger;
        private readonly AppSettings _appSettings;
        private readonly IBaseRepository<Model.Descarga> _descargaRepository;
        private readonly IBaseRepository<Model.Servidor> _servidorRepository;
        private readonly ICollectionContext<Model.Contenido> _contenidoCollection;

        public ProgramarTodoDescargaCommandHandler(
            IBaseRepository<Model.Descarga> descargaRepository,
            ICollectionContext<Model.Contenido> contenidoCollection,
            IBaseRepository<Model.Servidor> servidorRepository,
            ILogger<ProgramarDescargaCommandHandler> logger, AppSettings appSettings)
        {
            _descargaRepository = descargaRepository;
            _contenidoCollection = contenidoCollection;
            _servidorRepository = servidorRepository;
            _logger = logger;
            _appSettings = appSettings;
        }

        public async Task<StatusResponse> Handle(ProgramarTodoDescargaCommand request, CancellationToken cancellationToken)
        {
            var sr = new StatusResponse
            {
                StatusCode = 200,
                Success = true,
            };

            Model.Contenido contenido = null;

            var hdinicio = _appSettings.Settings.HORA_DESCARGA_INICIO;
            var hdintervalo = _appSettings.Settings.HORA_DESCARGA_INTERVALO;
            int sintervalo = _appSettings.Settings.SERVIDOR_INTERVALO;

            DateTime dHoraInicio = DateTime.ParseExact(hdinicio, "hh:mm:ss", null);
            TimeSpan tsHoraIntervalo = hdintervalo;


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
            {
                //ultimo Contenido Cargado
                var filter = Builders<Model.Contenido>.Filter.Where(s => s.estado == EstadoContenido.Cargado &&
                                                                                s.esEliminado == false &&
                                                                                s.esActivo == true);
                contenido = await _contenidoCollection.Context().Find<Model.Contenido>(filter)
                                                                          .Limit(1)
                                                                          .SortByDescending(f => f.fechaCreacion)
                                                                          .FirstOrDefaultAsync();
            }


            if (contenido == null)
            {
                sr.Messages.Add("No se encuentra disponible contenido para descargar");
                sr.Success = false;
                //Return 2
                return await Task.FromResult(sr);
            }

            var pNow = DateTime.Now;
            var currentTime = new TimeSpan(pNow.Hour, pNow.Minute, pNow.Second);
            var startTime = new TimeSpan(dHoraInicio.Hour, dHoraInicio.Minute, dHoraInicio.Second);

            if (currentTime >= startTime)
            {
                dHoraInicio = dHoraInicio.AddDays(1);
            }

            var serversActivos = _servidorRepository.Query(x => x.esActivo == true).ToList();

            int interval = sintervalo;
            int count = serversActivos.Count();

            var macs = serversActivos.Select(g => g.mac);

            var validDescargas = _descargaRepository.Query(x =>
                    macs.Contains(x.macServidor) &&
                    x.esActivo == true &&
                    x.esEliminado == false &&
                    (x.estado == EstadoDescarga.Programado || x.estado == EstadoDescarga.Descargando) &&
                    x.contenido.id == contenido.id).ToList();

            for (int i = 0; i < count; i = i + interval)
            {
                if (i + interval > count)
                {
                    interval = count - i;
                }
                var rangeServers = serversActivos.GetRange(i, interval);

                var nDescargas = new List<Model.Descarga>();

                foreach (var server in rangeServers)
                {
                    var qProgramcion = validDescargas.FirstOrDefault(h => h.macServidor == server.mac);

                    if (qProgramcion != null)
                    {
                        sr.Messages.Add($"Hay una descarga programada para el servidor [mac:{server.mac}] con el contenido [{contenido.nombre}]");
                    }
                    else
                    {
                        nDescargas.Add(new Model.Descarga
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
                            macServidor = server.mac,
                            estado = EstadoDescarga.Programado,
                            esActivo = true,
                            fechaCreacion = DateTime.Now
                        });
                        //sr.Messages.Add($"La descarga ha sido programada correctamente para servidor [{server.ip}]");
                    }
                }
                if (nDescargas.Any())
                {
                    await _descargaRepository.InsertManyAsync(nDescargas, true);
                    dHoraInicio = dHoraInicio.Add(tsHoraIntervalo);
                }
            }
            sr.Success = true;

            return await Task.FromResult(sr);
        }

    }
}
