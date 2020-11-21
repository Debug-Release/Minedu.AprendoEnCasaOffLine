using MediatR;
using MongoDB.Driver;
using Release.Helper.Pagination;
using Release.MongoDB.Repository.Base;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using VM = Minedu.AprendoEnCasaOffLine.Contenido.Core.ViewModel;

namespace Minedu.AprendoEnCasaOffLine.Contenido.Core.Queries
{
    public class ContenidoQuery : PagedRequest, IRequest<PagedResponse<VM.Contenido>>
    {

        #region Handler

        public class ContenidoQueryHandler : IRequestHandler<ContenidoQuery, PagedResponse<VM.Contenido>>
        {
            private readonly ICustomBaseRepository<Model.Contenido> _contenidoCustomRepository;


            public ContenidoQueryHandler(ICustomBaseRepository<Model.Contenido> contenidoCustomRepository)
            {
                _contenidoCustomRepository = contenidoCustomRepository;
            }
            public async Task<PagedResponse<VM.Contenido>> Handle(ContenidoQuery request, CancellationToken cancellationToken)
            {
                var pr = new PagedResponse<VM.Contenido> { Data = new List<VM.Contenido>() };
                request.Page = request.Page == 0 ? 1 : request.Page;
                request.PageSize = request.PageSize == 0 ? 10 : request.PageSize;

                var sort = new SortDefinitionBuilder<Model.Contenido>().Descending(x => x.fechaCreacion);
                var filters = new List<Expression<Func<Model.Contenido, bool>>>
                {
                    e => e.esEliminado == false
                };

                var r = _contenidoCustomRepository.FindPaged(request, filters, sort);

                foreach (var item in r.Data)
                {
                    pr.Data.Add(new VM.Contenido
                    {
                        id = item.id.ToString(),
                        fechaCreacion = item.fechaCreacion,
                        esActivo = item.esActivo,
                        //nombre = item.nombre,
                        //descripcion = item.descripcion,
                        archivo = item.archivo,
                        estado = item.estado,
                        pesoMb = item.pesoMb,
                        fechaModificacion = item.fechaModificacion
                    });
                }
                pr.TotalPages = r.TotalPages;
                pr.TotalRows = r.TotalRows;

                return await Task.FromResult(pr);
            }
        }

        #endregion
    }
}