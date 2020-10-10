using MediatR;
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
        private readonly IBaseRepository<Model.Descarga> _descargaRepository;
        private readonly IBaseRepository<Model.Servidor> _servidorRepository;
        private readonly ICollectionContext<Model.Contenido> _contenidoCollection;

        public ProgramarDescargaCommandHandler(
            IBaseRepository<Model.Descarga> descargaRepository,
            ICollectionContext<Model.Contenido> contenidoCollection,
            IBaseRepository<Model.Servidor> servidorRepository)
        {
            _descargaRepository = descargaRepository;
            _contenidoCollection = contenidoCollection;
            _servidorRepository = servidorRepository;
        }

        public async Task<StatusResponse> Handle(ProgramarDescargaCommand request, CancellationToken cancellationToken)
        {
            var sr = new StatusResponse
            {
                StatusCode = 200,
                Success = true,
            };


            var hdinicio = Functions.GetEnvironmentVariable("HORA_DESCARGA_INICIO");
            var hdintervalo = Functions.GetEnvironmentVariable("HORA_DESCARGA_INTERVALO");

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
            //ultimoContenido Cargado
            var filter = Builders<Model.Contenido>.Filter.Where(s => s.estado == EstadoContenido.Cargado &&
                                                                s.esEliminado == false &&
                                                                s.esActivo == true);
            var ucc = await _contenidoCollection.Context().Find<Model.Contenido>(filter)
                                                                      .Limit(1)
                                                                      .SortByDescending(f => f.fechaCreacion)
                                                                      .FirstOrDefaultAsync();

            if (ucc == null)
            {
                sr.Messages.Add("No se encuentra disponible contenido para descargar");
                sr.Success = false;
                //Return 2
                return await Task.FromResult(sr);
            }

            var qProgramcion = _descargaRepository.FirstOrDefault(x =>
                        x.ipServidor == request.ipServidor &&
                        x.esActivo == true &&
                        x.esEliminado == false &&
                        x.estado == EstadoDescarga.Programado &&
                        x.contenido.id == ucc.id
                        );

            if (qProgramcion != null)
            {
                sr.Messages.Add($"Hay una descarga programada con el contenido [{ucc.nombre}]");
                sr.Success = false;
                //Return 3
                return await Task.FromResult(sr);
            }

            if (dHoraInicio > DateTime.Now)
            {

            }

            var r = await _descargaRepository.InsertOneAsync(new Model.Descarga
            {
                contenido = new Model.ContenidoDescarga
                {
                    id = ucc.id,
                    nombre = ucc.nombre,
                    fechaCreacion = (ucc.fechaModificacion == null ? ucc.fechaCreacion : ucc.fechaModificacion.Value),
                    descripcion = ucc.descripcion,
                    archivo = ucc.archivo,
                    pesoMb = ucc.pesoMb
                },
                fechaProgramada = dHoraInicio,
                ipServidor = request.ipServidor,
                estado = EstadoDescarga.Programado,
                esActivo = true,
                fechaCreacion = DateTime.Now
            });

            sr.Data = r.id;
            sr.Messages.Add("La descarga ha sido programada correctamente");

            return await Task.FromResult(sr);
        }

        private List<string> Validacion(ProgramarDescargaCommand request)
        {
            var v = new List<string>();

            //validar servidor
            var servidor = _servidorRepository.FirstOrDefault(x => x.ip == request.ipServidor);
            if (servidor == null)
            {
                string m = $"La dirección ip del servidor [{request.ipServidor}] no está registrada";
                v.Add(m);
            }

            //validar si ya existe una descarga programada

            //var descarga = _descargaRepository.FirstOrDefault(x => x.ip == request.ipServidor);

            return v;
        }
    }
}
