using MediatR;
using Microsoft.AspNetCore.Mvc;
using Minedu.AprendoEnCasaOffLine.Contenido.Core.Commands;
using Minedu.AprendoEnCasaOffLine.Contenido.Core.Queries;
using Minedu.AprendoEnCasaOffLine.Contenido.Core.ViewModel;
using Release.Helper;
using Release.Helper.Pagination;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using VM = Minedu.AprendoEnCasaOffLine.Contenido.Core.ViewModel;

namespace Minedu.AprendoEnCasaOffLine.Contenido.Api.Controllers.V1
{
#if DEBUG

#else
    //[Authorize]
#endif

    //[ApiExplorerSettings(IgnoreApi = true)]
    //[SwaggerTag("Operaciones para gestionar descargas de contenido")]
    //[ApiExplorerSettings(GroupName = "v1")]
    [ApiVersion("1")]    
    //[Route("[controller]")]
    [Route("api/v1/[controller]")]
    public class ContenidoController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ContenidoController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [MapToApiVersion("1")]
        [Route("obtenerServidores")]
        //[ApiExplorerSettings(IgnoreApi = true)]
        [SwaggerResponse(statusCode: HttpStatusCodes.Status200OK, type: typeof(Servidor[]))]
        public async Task<IActionResult> ObtenerServidores()
        {
            var query = new ServidorQuery();
            var r = await _mediator.Send(query);

            return Ok(r);
        }
        [HttpGet]
        [MapToApiVersion("1")]
        [Route("obtenerContenido")]
        //[ApiExplorerSettings(IgnoreApi = true)]
        //[SwaggerOperation(Summary = "Listar contenido", Description = "Listar contenido paginado")]
        [SwaggerResponse(statusCode: HttpStatusCodes.Status200OK, type: typeof(PagedResponse<Core.ViewModel.Contenido>))]
        public async Task<IActionResult> ObtenerContenido(ContenidoQuery query)
        {
            var r = await _mediator.Send(query);

            return Ok(r);
        }
        [HttpGet]
        [MapToApiVersion("1")]
        [Route("obtenerProgramacion")]
        [SwaggerResponse(statusCode: HttpStatusCodes.Status200OK, type: typeof(StatusResponse<ProgramacionDescarga>))]
        public async Task<IActionResult> ObtenerProgramacion(ProgramacionDescargaQuery query)
        {
            var r = await _mediator.Send(query);

            return Ok(r);
        }

        [HttpGet]
        [MapToApiVersion("1")]
        [Route("obtenerProgramacionTest")]
        [SwaggerResponse(statusCode: HttpStatusCodes.Status200OK, type: typeof(StatusResponse<ProgramacionDescarga>))]
        public IActionResult ObtenerProgramacionTest()
        {
            var sr = new StatusResponse<ProgramacionDescarga>() { Data = new ProgramacionDescarga() };

            return Ok(sr);
        }

        [HttpPost]
        [MapToApiVersion("1")]
        [Route("registrarServidor")]
        //[SwaggerOperation(Summary = "Registrar servidor", Description = "Registrar servidor")]
        [SwaggerResponse(statusCode: HttpStatusCodes.Status200OK, type: typeof(StatusResponse))]
        public async Task<IActionResult> RegistrarServidor([FromBody] RegistrarServidorCommand command)
        {
            var r = await _mediator.Send(command);

            return Ok(r);
        }

        [HttpPost]
        [MapToApiVersion("1")]
        [Route("agregarContenido")]
        //[SwaggerOperation(Summary = "Registrar contenido a descargar", Description = "Registrar contenido a descargar")]
        [SwaggerResponse(statusCode: HttpStatusCodes.Status200OK, type: typeof(StatusResponse))]
        public async Task<IActionResult> AgregarContenido([FromBody] RegistrarContenidoCommand command)
        {
            var r = await _mediator.Send(command);

            return Ok(r);
        }

        [HttpPost]
        [MapToApiVersion("1")]
        [Route("programarDescarga")]
        //[SwaggerOperation(Summary = "Programar descarga por servidor", Description = "Programar descarga por servidor")]
        [SwaggerResponse(statusCode: HttpStatusCodes.Status200OK, type: typeof(StatusResponse))]
        public async Task<IActionResult> ProgramarDescarga([FromBody] ProgramarDescargaCommand command)
        {
            var r = await _mediator.Send(command);

            return Ok(r);
        }
        [HttpPost]
        [MapToApiVersion("1")]
        [Route("programarTodoDescarga")]
        [ApiExplorerSettings(IgnoreApi = true)]
        //[SwaggerOperation(Summary = "Programar descarga para todos los servidores activos", Description = "Programar descarga para todos los servidores activos")]
        [SwaggerResponse(statusCode: HttpStatusCodes.Status200OK, type: typeof(StatusResponse))]
        public async Task<IActionResult> ProgramarTodoDescarga([FromBody] ProgramarTodoDescargaCommand command)
        {
            var r = await _mediator.Send(command);

            return Ok(r);
        }

        [HttpGet("descargar")]
        [MapToApiVersion("1")]
        [ApiExplorerSettings(IgnoreApi = true)]
        //[SwaggerOperation(Summary = "Descargar contenido programado", Description = "Descargar contenido programado")]
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
        [HttpPost]
        [MapToApiVersion("1")]
        [Route("recibirACK")]
        //[SwaggerOperation(Summary = "Recibir ACK  de descarga", Description = "Recibir ACK  de descarga")]
        [SwaggerResponse(statusCode: HttpStatusCodes.Status200OK, type: typeof(StatusResponse))]
        public async Task<IActionResult> ActualizarDescargar([FromBody] ActualizarDescargaCommand command)
        {
            var r = await _mediator.Send(command);

            return Ok(r);
        }
        [HttpPost]
        [MapToApiVersion("1")]
        [Route("enviarTrazabilidad")]
        [SwaggerResponse(statusCode: HttpStatusCodes.Status200OK, type: typeof(StatusResponse))]
        public async Task<IActionResult> EnviarTrazabilidad([FromBody] TrazabilidadCommand command)
        {
            var r = await _mediator.Send(command);

            return Ok(r);
        }

        [HttpPost]
        [MapToApiVersion("1")]
        [Route("enviarContinuidadOperativa")]
        [SwaggerResponse(statusCode: HttpStatusCodes.Status200OK, type: typeof(StatusResponse))]
        public async Task<IActionResult> EnviarContinuidadOperativa([FromBody] ContinuidadOperativaCommand command)
        {
            var r = await _mediator.Send(command);

            return Ok(r);
        }        
    }
}
