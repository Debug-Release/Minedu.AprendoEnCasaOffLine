//using Minedu.AprendoEnCasaOffLine.Contenido.Core.Interfaces;
//using MediatR;
//using Microsoft.AspNetCore.Mvc;
//using Release.Helper;
//using Release.Helper.WebCoreMvc.Controllers;
//using Swashbuckle.AspNetCore.Annotations;
//using System.Threading.Tasks;
//using DTO = Minedu.AprendoEnCasaOffLine.Contenido.Entities;

//namespace Minedu.AprendoEnCasaOffLine.Contenido.Api.Controllers
//{
//    //[Authorize]    
//    [ApiExplorerSettings(IgnoreApi = false)]
//    [SwaggerTag("Operaciones de insercion, actualizacion y eliminacion de entregas")]
//    [Route("[controller]")]
//    public class CommandEntregaController : BaseController
//    {
//        private readonly IEntregaAplicacion _entregaAplicacion;
//        private readonly IMediator _mediator;

//        public CommandEntregaController(IEntregaAplicacion entregaAplicacion, IMediator mediator)
//        {
//            _entregaAplicacion = entregaAplicacion;
//            _mediator = mediator;
//        }

//        [HttpPost]
//        [Route("cargaMasiva")]
//        [SwaggerOperation(Summary = "Guardar lista de entregas con estado Pendiente",
//            Description = "Guardar lista de entregas con estado Pendiente")]
//        public async Task<CommandResponse> CargaMasiva([FromBody] DTO.CargaMasivaCommand command)
//        {
//            var result = await _mediator.Send(command);
//            return result;
//            /*
//            return TryCatch(() =>
//            {
//                var sr = _entregaAplicacion.CargaMasiva(entregas);
//                return _Response(sr);
//            });
//            */
//        }

//        [HttpPost]
//        [Route("registraMotivos")]
//        public CommandResponse RegistraMotivos([FromBody] DTO.Motivo[] motivos)
//        {
//            return TryCatch(() =>
//            {
//                var sr = _entregaAplicacion.RegistraMotivos(motivos);
//                return _Response(sr);
//            });
//        }

//        [HttpPost]
//        [Route("removeAllEntregas")]
//        public CommandResponse RemoveAllEntregas()
//        {
//            return TryCatch(() =>
//            {
//                var sr = _entregaAplicacion.RemoveAllEntregas();
//                return _Response(sr);
//            });
//        }

//        #region FluentValidation

//        [HttpPost]
//        [Route("registrarEntrega")]
//        [SwaggerOperation(Summary = "Registrar una entreg con estado pendiente",
//            Description = "Registrar una entreg con estado pendiente")]
//        public async Task<IActionResult> RegistrarEntrega([FromBody] DTO.Entrega request)
//        {

//            if (!ModelState.IsValid)
//            {
                
//            }
            
            
//            return Ok(await Task.FromResult(new CommandResponse()
//            {
//                StatusCode = 200,
//                Success = true
//            }));

//            /*
//            return TryCatch(() =>
//            {
//                var sr = _entregaAplicacion.CargaMasiva(entregas);
//                return _Response(sr);
//            });
//            */
//        }

//        #endregion
//    }
//}
