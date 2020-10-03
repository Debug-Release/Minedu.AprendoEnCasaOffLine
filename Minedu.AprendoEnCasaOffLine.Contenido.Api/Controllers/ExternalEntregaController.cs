//using Minedu.AprendoEnCasaOffLine.Contenido.Core.Interfaces;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using Release.Helper;
//using Release.Helper.Pagination;
//using Release.Helper.WebCoreMvc.Controllers;
//using Swashbuckle.AspNetCore.Annotations;
//using System.Collections.Generic;
//using DTO = Minedu.AprendoEnCasaOffLine.Contenido.Entities;

//namespace Minedu.AprendoEnCasaOffLine.Contenido.Api.Controllers
//{
//    [Authorize]
//    [SwaggerTag("Operaciones de actualizacion y consulta de entregas")]
//    [Route("[controller]")]
//    public class ExternalEntregaController : BaseController
//    {
//        private readonly IEntregaAplicacion _entregaAplicacion;

//        public ExternalEntregaController(IEntregaAplicacion entregaAplicacion)
//        {
//            _entregaAplicacion = entregaAplicacion;
//        }

//        [HttpPost]
//        [Route("actualizarEstado")]
//        [SwaggerOperation(Summary = "Registrar seguimiento del folio asignado a un courier, cambiar el estado y motivo, agregar ubicación e imagen", Description = "Registrar seguimiento del folio")]
//        public CommandResponse RegistrarBitacora([FromBody] DTO.Entrega_Bitacora request)
//        {
//            return TryCatch(() =>
//            {
//                var sr = _entregaAplicacion.RegistrarBitacora(request);
//                return _Response(sr);
//            });
//        }

//        [HttpPost]
//        [Route("busquedaPaginada")]
//        [SwaggerOperation(Summary = "Busqueda paginada", Description = "Busqueda paginada")]
//        public PagedResponse<DTO.Entrega> EntregasPaginada([FromBody]DTO.EntregaFiltro filtro)
//        {
//            filtro.incluirVisita = false;
//            return _entregaAplicacion.EntregasPaginada(filtro);
//        }

//        [HttpGet]
//        [Route("motivos")]
//        [SwaggerOperation(Summary = "Lista de motivos", Description = "Lista de motivos usados para actualizar el seguimiento de los folios")]
//        [SwaggerResponse(statusCode: (int)System.Net.HttpStatusCode.OK, type: typeof(DTO.Motivo[]))]
//        public JsonResult Motivos()
//        {
//            var r = _entregaAplicacion.Motivos(true);

//            return Json(r);
//        }
//        [HttpGet]
//        [Route("visitasByFolio/{folio}")]
//        [SwaggerOperation(Summary = "Obtener visitas por folio", Description = "Obtener visitas por folio")]
//        public DTO.Entrega_Bitacora[] VisitasByFolio(string folio)
//        {
//            return _entregaAplicacion.Bitacoras(new DTO.EntregaFiltro { folio = folio, Page = 1, PageSize = 1, incluirVisita = true }, false);
//        }
//    }
//}
