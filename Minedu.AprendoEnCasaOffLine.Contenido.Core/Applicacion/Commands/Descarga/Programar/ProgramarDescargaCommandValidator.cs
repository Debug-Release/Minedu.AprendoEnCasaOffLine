using FluentValidation;

namespace Minedu.AprendoEnCasaOffLine.Contenido.Core.Commands
{
    public class ProgramarDescargaCommandValidator : AbstractValidator<ProgramarDescargaCommand>
    {
        public ProgramarDescargaCommandValidator()
        {
            RuleFor(x => x)
                .NotNull().WithMessage("El request no debe ser nulo")
                .DependentRules(() =>
                {
                    //idContenido             
                    When(x => !string.IsNullOrEmpty(x.idContenido), () =>
                    {
                        RuleFor(x => x.idContenido).Matches(@"[0-9a-fA-F]{24}").WithMessage("El formato del id del contenido no es válido.");
                    });

                    //idServidor
                    RuleFor(x => x.ipServidor).NotEmpty().WithMessage("La dirección ip del servidor es requerida");
                    
                    When(x => !string.IsNullOrEmpty(x.ipServidor), () =>
                    {
                        RuleFor(x => x.ipServidor).Matches(@"^(([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])\.){3}([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])$").WithMessage("La dirección ip del servidor no es válido.");
                    });
                    
                });
        }
    }
}
