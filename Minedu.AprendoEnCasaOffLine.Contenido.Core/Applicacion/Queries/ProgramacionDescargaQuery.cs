using MediatR;
using Release.Helper;
using Release.MongoDB.Repository;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VM = Minedu.AprendoEnCasaOffLine.Contenido.Core.ViewModel;

namespace Minedu.AprendoEnCasaOffLine.Contenido.Core.Queries
{
    public class ProgramacionDescargaQuery : IRequest<StatusResponse<VM.ProgramacionDescarga>>
    {
        public string ipServidor { get; set; }

        #region Handler

        public class ProgramacionDescargaQueryHandler : IRequestHandler<ProgramacionDescargaQuery, StatusResponse<VM.ProgramacionDescarga>>
        {
            private readonly IBaseRepository<Model.Descarga> _descargaRepository;
            public ProgramacionDescargaQueryHandler(IBaseRepository<Model.Descarga> descargaRepository)
            {
                _descargaRepository = descargaRepository;
            }
            public async Task<StatusResponse<VM.ProgramacionDescarga>> Handle(ProgramacionDescargaQuery request, CancellationToken cancellationToken)
            {
                var sr = new StatusResponse<VM.ProgramacionDescarga>();

                var q = _descargaRepository.FirstOrDefault(x =>
                        x.ipServidor == request.ipServidor &&
                        x.esActivo == true &&
                        x.esEliminado == false &&
                        x.estado == EstadoDescarga.Programado
                        );

                if (q != null)
                {
                    sr.Success = true;
                    char[] delimiters = new char[] { '/', '\\' };

                    sr.Data = new VM.ProgramacionDescarga
                    {
                        id = q.id.ToString(),
                        esActivo = q.esActivo,
                        fechaCreacion = q.fechaCreacion,
                        fechaModificacion = q.fechaModificacion,
                        ipServidor = q.ipServidor,
                        contenido = new VM.Contenido
                        {
                            id = q.contenido.id.ToString(),
                            nombre = q.contenido.nombre,
                            descripcion = q.contenido.descripcion,
                            archivo = q.contenido.archivo.Split(delimiters, StringSplitOptions.RemoveEmptyEntries).Last(),
                            pesoMb = q.contenido.pesoMb,
                            fechaCreacion = q.contenido.fechaCreacion,
                            estado = EstadoContenido.Cargado,
                            esActivo = true
                        },
                        estado = q.estado,
                        fechaProgramada = q.fechaProgramada,
                        fechaInicio = q.fechaInicio,
                        fechaFin = q.fechaFin,

                    };
                }
                else
                {
                    sr.Success = false;
                    sr.StatusCode = 200;
                    sr.Messages.Add($"No existe descargas programadas para el servidor [{request.ipServidor}]");
                }

                return await Task.FromResult(sr);
            }
        }

        #endregion
    }
}