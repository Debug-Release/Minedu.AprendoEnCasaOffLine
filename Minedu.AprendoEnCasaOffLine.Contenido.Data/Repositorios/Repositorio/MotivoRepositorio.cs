using Minedu.AprendoEnCasaOffLine.Contenido.Data.Modelo;
using Release.MongoDB.Repository;
using Release.MongoDB.Repository.Base;

namespace Minedu.AprendoEnCasaOffLine.Contenido.Data.Repositorios
{
    public class MotivoRepositorio : CustomBaseRepository<Motivo>, IMotivoRepositorio
    {
        public MotivoRepositorio(IDataContext context) : base(context)
        {
        }
    }
}
