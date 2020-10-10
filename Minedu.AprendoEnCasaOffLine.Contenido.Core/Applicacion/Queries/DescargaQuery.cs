using MediatR;
using MongoDB.Bson;
using Release.Helper;
using Release.MongoDB.Repository;
using System.Threading;
using System.Threading.Tasks;

namespace Minedu.AprendoEnCasaOffLine.Contenido.Core.Queries
{
    public class DescargaQuery : IRequest<FileResponse>
    {
        public string idDescarga { get; set; }

        #region Handler

        public class DescargaQueryHandler : IRequestHandler<DescargaQuery, FileResponse>
        {
            private readonly IBaseRepository<Model.Descarga> _descargaRepository;
            public DescargaQueryHandler(IBaseRepository<Model.Descarga> descargaRepository)
            {
                _descargaRepository = descargaRepository;
            }
            public async Task<FileResponse> Handle(DescargaQuery request, CancellationToken cancellationToken)
            {
                var sr = new FileResponse();

                var q = _descargaRepository.FirstOrDefault(x =>
                        x.id == ObjectId.Parse(request.idDescarga) &&
                        x.esActivo == true &&
                        x.esEliminado == false &&
                        x.estado == EstadoDescarga.Programado
                        );

                if (q != null)
                {
                    //Leer Archivo de disco
                    sr.Exists = true;
                }
                else
                {
                    sr.Exists = false;
                }

                return await Task.FromResult(sr);
            }
        }

        #endregion
    }
}