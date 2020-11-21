using MediatR;
using MongoDB.Driver;
using Release.Helper;
using Release.MongoDB.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Minedu.AprendoEnCasaOffLine.Contenido.Core.Commands
{
    public class RegistrarServidorCommandHandler : IRequestHandler<RegistrarServidorCommand, StatusResponse>
    {
        private readonly IBaseRepository<Model.Servidor> _servidorRepository;
        private readonly ICollectionContext<Model.Servidor> _servidorCollection;

        public RegistrarServidorCommandHandler(IBaseRepository<Model.Servidor> servidorRepository, ICollectionContext<Model.Servidor> servidorCollection)
        {
            _servidorRepository = servidorRepository;
            _servidorCollection = servidorCollection;
        }

        public async Task<StatusResponse> Handle(RegistrarServidorCommand request, CancellationToken cancellationToken)
        {
            var cr = new StatusResponse
            {
                StatusCode = 200,
                Success = true,
            };                      

            var filters = new List<Expression<Func<Model.Servidor, bool>>> { x => x.esEliminado == false && x.esActivo == true };
            var macs = request.mac.ToUpper().Split(new char[] { ';', ',' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var m in macs)
            {
                filters.Add(x => x.mac1.Any(v => v == m));
            }
            var q = _servidorRepository.Query(filters).ToList();
            if (q.Count > 0)
            {
                cr.Success = false;
                cr.Messages.Add("Ya existe un servidor registrado con la dirección mac [" + request.mac + "]");
            }
            else
            {
                /*
                var bIp = new BsonArray();
                foreach (var item in request.ip.Split(new char[] { ';', ',' }))
                {
                    if (!string.IsNullOrWhiteSpace(item))
                    {
                        bIp.Add(item);
                    }
                }
                */
                /*
                var bMac = new BsonArray();
                foreach (var item in request.mac.Split(new char[] { ';', ',' }))
                {
                    if (!string.IsNullOrWhiteSpace(item))
                    {
                        bMac.Add(item);
                    }
                }
                */
                var r = await _servidorRepository.InsertOneAsync(new Model.Servidor
                {
                    ip = request.ip,
                    mac = request.mac,
                    mac1 = macs,
                    nombre = request.nombre,
                    //fqdn = request.fqdn,
                    esActivo = true,
                    fechaCreacion = DateTime.Now
                }); ;

                cr.Data = r.id;
                cr.Messages.Add($"El servidor [{request.nombre}] se registro correctamente");
            }

            return await Task.FromResult(cr);
        }
    }
}
