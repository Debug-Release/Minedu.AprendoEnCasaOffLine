using MediatR;
using Microsoft.Extensions.Logging;
using Minedu.AprendoEnCasaOffLine.Contenido.Core.Commands;
using Release.Helper;
using Release.Helper.Worker;
using System;

namespace Minedu.AprendoEnCasaOffLine.Contenido.Worker
{
    public class ProgramacionService : BaseBackgroundService
    {
        private readonly ILogger<ProgramacionService> _logger;
        private readonly IMediator _mediator;
        public ProgramacionService(ILogger<ProgramacionService> logger, IMediator mediator) : base(logger)
        {
            _logger = logger;
            _mediator = mediator;
        }
        protected override void InitVariables()
        {
            
            var programacionTodo = Functions.GetEnvironmentVariable("PROGRAMACION_TODO");

            var programacion = TimeSpan.Parse(programacionTodo);
            this.Delay = programacion;
            this.ServiceName = "Programacion Automatica";
        }

        protected override void RunOperation()
        {
            string date = DateTime.Now.ToString("yyyy-MM-dd hhmmss");
            //var request = new ProgramarTodoDescargaCommand { idContenido = "" };
            //var rrr = _mediator.Send(request).Result;

            _logger.LogInformation(date);
            //Console.WriteLine(date);
        }
    }
}
