using MediatR;
using Release.Helper;
using Release.MongoDB.Repository;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Minedu.AprendoEnCasaOffLine.Contenido.Core.Commands
{
    public class RegistrarServidorCommandHandler : IRequestHandler<RegistrarServidorCommand, StatusResponse>
    {
        private readonly IBaseRepository<Model.Servidor> _servidorRepository;

        public RegistrarServidorCommandHandler(IBaseRepository<Model.Servidor> servidorRepository)
        {
            _servidorRepository = servidorRepository;
        }

        public async Task<StatusResponse> Handle(RegistrarServidorCommand request, CancellationToken cancellationToken)
        {
            var cr = new StatusResponse
            {
                StatusCode = 200,
                Success = true,                
            };

            var q = _servidorRepository.FirstOrDefault(x => x.ip == request.ip);
            if (q != null)
            {
                cr.Success = false;
                cr.Messages.Add("Ya existe un servidor registrado con la ip [" + request.ip + "]");
            }
            else
            {
                var r = await _servidorRepository.InsertOneAsync(new Model.Servidor
                {
                    ip = request.ip,
                    mac = request.mac,
                    nombre = request.nombre,
                    fqdn = request.fqdn,
                    esActivo = true,
                    fechaCreacion = DateTime.Now
                });

                cr.Data = r.id;
                cr.Messages.Add("El servidor se registro correctamente");
            }

            return await Task.FromResult(cr);
        }
    }
}
