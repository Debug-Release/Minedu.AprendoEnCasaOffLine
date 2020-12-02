using MediatR;
using Release.Helper;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Minedu.AprendoEnCasaOffLine.Contenido.Core.Commands
{
    public class RegistrarSaludoCommandHandler : IRequestHandler<RegistrarSaludoCommand, StatusResponse>
    {
        public RegistrarSaludoCommandHandler()
        {

        }
        public async Task<StatusResponse> Handle(RegistrarSaludoCommand request, CancellationToken cancellationToken)
        {
            var sr = new StatusResponse();

            sr.Success = true;
            sr.Messages.Add($"Hola {request.mensaje} ");

            return await Task.FromResult(sr);
        }
    }
}
