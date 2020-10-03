using Minedu.AprendoEnCasaOffLine.Contenido.Data.Modelo;
using Release.MongoDB.Repository;
using Release.MongoDB.Repository.Base;

namespace Minedu.AprendoEnCasaOffLine.Contenido.Data.Repositorios
{
    public class EntregaRepositorio : CustomBaseRepository<Entrega>, IEntregaRepositorio
    {
        public EntregaRepositorio(IDataContext context) : base(context)
        {
        }
    }
}
