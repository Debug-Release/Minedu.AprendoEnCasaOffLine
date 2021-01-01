using MediatR;
using Microsoft.Extensions.Logging;
using Release.Helper;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Minedu.AprendoEnCasaOffLine.Contenido.Core.Commands
{
    public class TrazabilidadCommandHandler : IRequestHandler<TrazabilidadCommand, StatusResponse>
    {
        private readonly ILogger<TrazabilidadCommandHandler> _logger;
        private readonly AppSettings _appSettings;

        public TrazabilidadCommandHandler(ILogger<TrazabilidadCommandHandler> logger, AppSettings appSettings)
        {
            _logger = logger;
            _appSettings = appSettings;
        }
        public async Task<StatusResponse> Handle(TrazabilidadCommand request, CancellationToken cancellationToken)
        {
            var sr = new StatusResponse
            {
                StatusCode = 200
            };
            _logger.LogInformation($"Recibiendo trazabilidad de archivo");

            //Persiste
            try
            {
                var ruta = _appSettings.Settings.RUTA_TRAZABILIDAD;
                string name = $"trazabilidad-{DateTime.Now.ToString("yyyy-MM-dd_hhmmss")}.tar";
                string pathTar = System.IO.Path.Combine(ruta, name);
                await System.IO.File.WriteAllBytesAsync(pathTar, request.archivo);

                _logger.LogInformation($"El archivo de trazabilidad {name} se guardo correctamente");
                sr.Success = true;
                sr.Messages.Add($"El archivo de trazabilidad {name} se guardo correctamente");
            }
            catch (Exception ex)
            {
                sr.Messages.Add(ex.Message);
                _logger.LogError(ex, $"Error al guardar el archivo de trazabilidad");
            }

            return sr;
        }
    }
}
