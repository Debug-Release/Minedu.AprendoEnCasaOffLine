using MediatR;
using Release.Helper;
using Release.MongoDB.Repository;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Minedu.AprendoEnCasaOffLine.Contenido.Core.Commands
{
    public class InsertServidorCommandHandler : IRequestHandler<InsertServidorCommand, CommandResponse>
    {
        private readonly IBaseRepository<Model.Servidor> _servidorRepository;

        public InsertServidorCommandHandler(IBaseRepository<Model.Servidor> servidorRepository)
        {
            _servidorRepository = servidorRepository;
        }

        public async Task<CommandResponse> Handle(InsertServidorCommand request, CancellationToken cancellationToken)
        {
            var cr = new CommandResponse
            {
                StatusCode = 200,
                Success = true,
                Msg = "El servidor se registro correctamente"
            };

            var q = _servidorRepository.FirstOrDefault(x => x.ip == request.ip);
            if (q != null)
            {
                cr.Success = false;
                cr.Msg = "Ya existe un servidor registrado con la ip [" + request.ip + "]";
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
            }

            return await Task.FromResult(cr);
        }
    }
}
