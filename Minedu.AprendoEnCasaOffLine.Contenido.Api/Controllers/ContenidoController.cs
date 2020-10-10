﻿using MediatR;
using Microsoft.AspNetCore.Mvc;
using Minedu.AprendoEnCasaOffLine.Contenido.Core.Commands;
using Minedu.AprendoEnCasaOffLine.Contenido.Core.Queries;
using Minedu.AprendoEnCasaOffLine.Contenido.Core.ViewModel;
using Release.Helper;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;

namespace Minedu.AprendoEnCasaOffLine.Contenido.Api.Controllers
{
    //[Authorize]
    //[ApiExplorerSettings(IgnoreApi = true)]
    [SwaggerTag("Operaciones para gestionar descargas de contenido")]
    [Route("[controller]")]
    public class ContenidoController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ContenidoController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("obtenerServidores")]
        [SwaggerResponse(statusCode: (int)System.Net.HttpStatusCode.OK, type: typeof(Servidor[]))]
        public async Task<IActionResult> ObtenerServidores()
        {
            var query = new ServidorQuery();
            var r = await _mediator.Send(query);

            return Ok(r);
        }
        [HttpGet]
        [Route("obtenerProgramacion")]
        [SwaggerResponse(statusCode: (int)System.Net.HttpStatusCode.OK, type: typeof(ProgramacionDescarga))]
        public async Task<IActionResult> ObtenerProgramacion(ProgramacionDescargaQuery query)
        {
            var r = await _mediator.Send(query);

            return Ok(r);
        }

        [HttpPost]
        [Route("registrarServidor")]
        [SwaggerOperation(Summary = "Registrar servidor", Description = "Registrar servidor")]
        [SwaggerResponse(statusCode: (int)System.Net.HttpStatusCode.OK, type: typeof(CommandResponse))]
        public async Task<IActionResult> RegistrarServidor([FromBody] RegistrarServidorCommand command)
        {
            var r = await _mediator.Send(command);

            return Ok(r);
        }

        [HttpPost]
        [Route("agregarContenido")]
        [SwaggerOperation(Summary = "Registrar contenido a descargar", Description = "Registrar contenido a descargar")]
        [SwaggerResponse(statusCode: (int)System.Net.HttpStatusCode.OK, type: typeof(CommandResponse))]
        public async Task<IActionResult> AgregarContenido([FromBody] RegistrarContenidoCommand command)
        {
            var r = await _mediator.Send(command);

            return Ok(r);
        }

        [HttpPost]
        [Route("programarDescarga")]
        [SwaggerOperation(Summary = "Programar descarga por servidor", Description = "Programar descarga por servidor")]
        [SwaggerResponse(statusCode: (int)System.Net.HttpStatusCode.OK, type: typeof(CommandResponse))]
        public async Task<IActionResult> ProgramarDescarga([FromBody] ProgramarDescargaCommand command)
        {
            var r = await _mediator.Send(command);

            return Ok(r);
        }
        /*
        [HttpPost]
        [Route("descargar")]
        [SwaggerOperation(Summary = "Descargar contenido programado", Description = "Descargar contenido programado")]
        [SwaggerResponse(statusCode: (int)System.Net.HttpStatusCode.OK, type: typeof(StatusResponse<FileResponse>))]
        public async Task<IActionResult> Descargar([FromBody] DescargaQuery query)
        {
            var r = await _mediator.Send(query);


            return Ok(r);
        }
        */
        [HttpGet("descargar")]
        [SwaggerOperation(Summary = "Descargar contenido programado", Description = "Descargar contenido programado")]
        public async Task<ActionResult> DownloadAsync(string id)
        {
            var file = await _mediator.Send(new DescargaQuery { idDescarga = id });

            if (!file.Exists)
            {
                return NotFound();
            }

            return new FileContentResult(file.FileBytes, file.ContentType)
            {
                FileDownloadName = file.FileName
            };
        }
        /*
        [HttpGet]
        [Route("entregaByFolio/{folio}")]
        [SwaggerOperation(Summary = "Obtener entrega por folio", Description = "Obtener entrega por folio")]
        public async Task<DTO.Entrega> EntregaByFolio(string folio)
        {
            var query = new EntregasPaginadaQuery(new DTO.EntregaFiltro { folio = folio, Page = 1, PageSize = 1, incluirVisita = true });
            var r = await _mediator.Send(query);
            return r.Data.FirstOrDefault();
        }

        [HttpGet]
        [Route("entregaById/{id}")]
        [SwaggerOperation(Summary = "Obtener entrega por id", Description = "Obtener entrega por id")]
        public async Task<DTO.Entrega> EntregaById(string id)
        {
            var query = new EntregasPaginadaQuery(new DTO.EntregaFiltro { id = id, Page = 1, PageSize = 1, incluirVisita = true });
            var r = await _mediator.Send(query);
            return r.Data.FirstOrDefault();
        }

        [HttpPost]
        [Route("busquedaPaginada")]
        [SwaggerOperation(Summary = "Busqueda paginada", Description = "Busqueda paginada")]
        public async Task<PagedResponse<DTO.Entrega>> EntregasPaginada([FromBody] DTO.EntregaFiltro filtro)
        {
            var query = new EntregasPaginadaQuery(filtro);
            var r = await _mediator.Send(query);

            return r;
        }

        [HttpGet]
        [Route("estados")]
        [SwaggerOperation(Summary = "Estados del folio", Description = "Lista de estados por los que que pasará un folio")]
        [SwaggerResponse(statusCode: (int)System.Net.HttpStatusCode.OK, type: typeof(DTO.Estado[]))]
        public async Task<IActionResult> Estados()
        {
            var query = new EstadosQuery();
            var r = await _mediator.Send(query);

            return Ok(r);
        }

        [HttpGet]
        [Route("motivos")]
        [SwaggerOperation(Summary = "Lista de motivos", Description = "Lista de motivos usados para actualizar el seguimiento de los folios")]
        [SwaggerResponse(statusCode: (int)System.Net.HttpStatusCode.OK, type: typeof(DTO.Motivo[]))]
        public async Task<IActionResult> Motivos()
        {
            var query = new MotivosQuery();
            var r = await _mediator.Send(query);

            return Ok(r);
        }

        [HttpGet]
        [Route("reporteEntregas")]
        public async Task<IActionResult> ReporteEntregas([FromBody] DTO.EntregaFiltro filtro)
        {
            var query = new ReporteEntregasQuery(filtro);
            var fr = await _mediator.Send(query);

            return File(fr.FileBytes, fr.ContentType, fileDownloadName: fr.FileName);
        }

        [HttpGet]
        [Route("visitasByFolio/{folio}")]
        [SwaggerOperation(Summary = "Obtener visitas por folio", Description = "Obtener visitas por folio")]
        public async Task<DTO.Entrega_Bitacora[]> VisitasByFolio(string folio)
        {
            var query = new VisitasByQuery(new DTO.EntregaFiltro { folio = folio, Page = 1, PageSize = 1, incluirVisita = true });
            var r = await _mediator.Send(query);

            return r;
        }

        [HttpGet]
        [Route("visitasById/{id}")]
        [SwaggerOperation(Summary = "Obtener visitas por id de entrega", Description = "Obtener visitas por id de entrega")]
        public async Task<DTO.Entrega_Bitacora[]> VisitasById(string id)
        {
            var query = new VisitasByQuery(new DTO.EntregaFiltro { id = id, Page = 1, PageSize = 1, incluirVisita = true });
            var r = await _mediator.Send(query);

            return r;
        }
        */
    }
}
