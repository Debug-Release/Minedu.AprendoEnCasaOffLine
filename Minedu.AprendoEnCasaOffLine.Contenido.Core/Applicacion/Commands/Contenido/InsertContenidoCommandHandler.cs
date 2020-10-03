using MediatR;
using Release.Helper;
using Release.MongoDB.Repository;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Minedu.AprendoEnCasaOffLine.Contenido.Core.Commands
{
    public class InsertContenidoCommandHandler : IRequestHandler<InsertContenidoCommand, CommandResponse>
    {
        private readonly IBaseRepository<Model.Contenido> _contenidoRepository;

        public InsertContenidoCommandHandler(IBaseRepository<Model.Contenido> contenidoRepository)
        {
            _contenidoRepository = contenidoRepository;
        }

        public async Task<CommandResponse> Handle(InsertContenidoCommand request, CancellationToken cancellationToken)
        {
            var cr = new CommandResponse
            {
                StatusCode = 200,
                Success = true,
                Msg = "El contenido se registro correctamente"
            };

            var q = _contenidoRepository.FirstOrDefault(x => x.nombre == request.nombre);
            if (q != null)
            {
                cr.Success = false;
                cr.Msg = "Ya existe un contenido registrado con el nombre [" + request.nombre + "]";
            }
            else
            {
                var r = await _contenidoRepository.InsertOneAsync(new Model.Contenido
                {
                    nombre = request.nombre,
                    descripcion = request.descripcion,
                    rutaOrigen = request.rutaOrigen,
                    pesoMb = request.pesoMb,
                    estado = EstadoContenido.Pendiente,
                    esActivo = true,
                    fechaCreacion = DateTime.Now
                });

                cr.Data = r.id;
            }

            return await Task.FromResult(cr);
        }
    }
}
