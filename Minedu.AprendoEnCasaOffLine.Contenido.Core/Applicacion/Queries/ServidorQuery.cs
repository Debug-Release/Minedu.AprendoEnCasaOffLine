using MediatR;
using Release.MongoDB.Repository;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VM = Minedu.AprendoEnCasaOffLine.Contenido.Core.ViewModel;

namespace Minedu.AprendoEnCasaOffLine.Contenido.Core.Queries
{
    public class ServidorQuery : IRequest<List<VM.Servidor>>
    {


        #region Handler

        public class ServidorQueryHandler : IRequestHandler<ServidorQuery, List<VM.Servidor>>
        {
            private readonly IBaseRepository<Model.Servidor> _servidorRepository;
            public ServidorQueryHandler(IBaseRepository<Model.Servidor> servidorRepository)
            {
                _servidorRepository = servidorRepository;
            }
            public async Task<List<VM.Servidor>> Handle(ServidorQuery request, CancellationToken cancellationToken)
            {
                var query = await _servidorRepository.FindAsync(x => x.esEliminado == false);

                var items = query.Select(v => new VM.Servidor
                {
                    id = v.id.ToString(),
                    fechaCreacion = v.fechaCreacion,
                    esActivo = v.esActivo,
                    ip = v.ip,
                    mac = v.mac,
                    nombre = v.nombre,
                    fqdn = v.fqdn
                }).ToList();
                return items;
            }
        }

        #endregion
    }
}