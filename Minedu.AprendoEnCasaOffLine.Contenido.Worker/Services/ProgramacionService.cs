using MediatR;
using Microsoft.Extensions.Logging;
using Minedu.AprendoEnCasaOffLine.Contenido.Core;
using Release.Helper.Worker;
using System;

namespace Minedu.AprendoEnCasaOffLine.Contenido.Worker
{
    public class ProgramacionService : BaseBackgroundService
    {
        private readonly AppSettings _appSettings;
        private readonly ILogger<ProgramacionService> _logger;
        private readonly IMediator _mediator;
        public ProgramacionService(ILogger<ProgramacionService> logger, IMediator mediator, AppSettings appSettings) : base(logger)
        {
            _logger = logger;
            _mediator = mediator;
            _appSettings = appSettings;
        }
        protected override void InitVariables()
        {

            var programacionTodo = _appSettings.Settings.PROGRAMACION_TODO;

            var programacion = programacionTodo;
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
