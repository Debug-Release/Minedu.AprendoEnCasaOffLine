using Minedu.AprendoEnCasaOffLine.Contenido.Data.Modelo;
using Release.MongoDB.Repository;
using Release.MongoDB.Repository.Base;

namespace Minedu.AprendoEnCasaOffLine.Contenido.Data.Repositorios
{
    public class EstadoRepositorio : CustomBaseRepository<Estado>, IEstadoRepositorio
    {
        public EstadoRepositorio(IDataContext context) : base(context)
        {
        }
    }
}
