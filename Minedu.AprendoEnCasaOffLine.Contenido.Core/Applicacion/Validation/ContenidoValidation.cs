using Minedu.AprendoEnCasaOffLine.Contenido.Core.Commands;
using Release.MongoDB.Repository;
using System.Collections.Generic;

namespace Minedu.AprendoEnCasaOffLine.Contenido.Core.Aplicacion
{
    public class ContenidoValidation
    {
        private List<string> _msg;
        private readonly IBaseRepository<Model.Contenido> _contenidoRepository;

        public ContenidoValidation(IBaseRepository<Model.Contenido> contenidoRepository)
        {
            _msg = new List<string>();
            _contenidoRepository = contenidoRepository;
        }

        public List<string> ValidaRegistrar(RegistrarContenidoCommand request)
        {
            var q = _contenidoRepository.FirstOrDefault(x => x.archivo == request.archivo);
            if (q != null)
            {
                _msg.Add("Ya existe un archivo registrado con el nombre [" + request.archivo + "]");
            }
            return _msg;
        }        
    }
}
