using MediatR;
using Release.Helper;
using Release.MongoDB.Repository;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Minedu.AprendoEnCasaOffLine.Contenido.Core.Commands
{
    public class RegistrarContenidoCommandHandler : IRequestHandler<RegistrarContenidoCommand, StatusResponse>
    {
        private readonly IBaseRepository<Model.Contenido> _contenidoRepository;

        public RegistrarContenidoCommandHandler(IBaseRepository<Model.Contenido> contenidoRepository)
        {
            _contenidoRepository = contenidoRepository;
        }

        public async Task<StatusResponse> Handle(RegistrarContenidoCommand request, CancellationToken cancellationToken)
        {
            var cr = new StatusResponse
            {
                StatusCode = 200,
                Success = true,
            };

            var q = _contenidoRepository.FirstOrDefault(x => x.nombre == request.archivo);
            if (q != null)
            {
                cr.Success = false;
                cr.Messages.Add("Ya existe un archivo registrado con el nombre [" + request.archivo + "]");
            }
            else
            {
                var r = await _contenidoRepository.InsertOneAsync(new Model.Contenido
                {
                    nombre = null,
                    descripcion = null,
                    archivo = request.archivo,
                    pesoMb = request.pesoMb,
                    estado = EstadoContenido.Cargado, //Cargado = cuando el archivo ya esta cargado en directorio de descargas
                    esActivo = true,
                    fechaCreacion = DateTime.Now
                });

                cr.Data = r.id;
                cr.Messages.Add("El contenido se registro correctamente");
            }

            return await Task.FromResult(cr);
        }
    }
}
