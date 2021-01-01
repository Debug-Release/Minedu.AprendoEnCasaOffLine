using MediatR;
using Microsoft.Extensions.Logging;
using Release.Helper;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Minedu.AprendoEnCasaOffLine.Contenido.Core.Commands
{
    public class ContinuidadOperativaCommandHandler : IRequestHandler<ContinuidadOperativaCommand, StatusResponse>
    {
        private readonly ILogger<ContinuidadOperativaCommandHandler> _logger;
        private readonly AppSettings _appSettings;

        public ContinuidadOperativaCommandHandler(ILogger<ContinuidadOperativaCommandHandler> logger, AppSettings appSettings)
        {
            _logger = logger;
            _appSettings = appSettings;
        }
        public async Task<StatusResponse> Handle(ContinuidadOperativaCommand request, CancellationToken cancellationToken)
        {
            var sr = new StatusResponse
            {
                StatusCode = 200
            };
            _logger.LogInformation($"Recibiendo informacion de continuidad operativa");
            if (request == null)
            {
                sr.Messages.Add("El request no debe ser nulo");
                return sr;
            }

            //Persiste json
            try
            {
                var ruta = _appSettings.Settings.RUTA_CONTINUIDAD;
                string name = $"continuidad-operativa-{DateTime.Now.ToString("yyyy-MM-dd_hhmmss")}.json";

                string pathJson = System.IO.Path.Combine(ruta, name);
                string jsonData = Newtonsoft.Json.JsonConvert.SerializeObject(request);
                await System.IO.File.WriteAllTextAsync(pathJson, jsonData);

                _logger.LogInformation($"El archivo de continuidad operativa {name} se genero correctamente");
                sr.Success = true;
                sr.Messages.Add($"El archivo de continuidad operativa {name} se genero correctamente");
            }
            catch (Exception ex)
            {
                sr.Messages.Add(ex.Message);
                _logger.LogError(ex, $"Error al escribir el archivo de continuidad operativa");
            }

            return sr;
        }
    }

}
