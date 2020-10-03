using Minedu.AprendoEnCasaOffLine.Contenido.Data.Modelo;
using Release.MongoDB.Repository;
using Release.MongoDB.Repository.Base;

namespace Minedu.AprendoEnCasaOffLine.Contenido.Data.Repositorios
{
    public class CourierRepositorio : CustomBaseRepository<Courier>, ICourierRepositorio
    {
        public CourierRepositorio(IDataContext context) : base(context)
        {
        }
    }
}
