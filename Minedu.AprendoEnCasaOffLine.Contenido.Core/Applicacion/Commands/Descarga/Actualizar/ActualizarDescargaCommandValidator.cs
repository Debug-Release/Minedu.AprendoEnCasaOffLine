using FluentValidation;

namespace Minedu.AprendoEnCasaOffLine.Contenido.Core.Commands
{
    public class ActualizarDescargaCommandValidator : AbstractValidator<ActualizarDescargaCommand>
    {
        public ActualizarDescargaCommandValidator()
        {
            RuleFor(x => x)
                .NotNull().WithMessage("El request no debe ser nulo")
                .DependentRules(() =>
                {
                    //idDescarga
                    
                    RuleFor(x => x.idDescarga).NotEmpty().WithMessage("El id de descarga es requerido");

                    When(x => !string.IsNullOrEmpty(x.idDescarga), () =>
                    {
                        RuleFor(x => x.idDescarga).Matches(@"[0-9a-fA-F]{24}").WithMessage("El formato del id de descarga no es válido.");
                    });                                        

                });
        }
    }
}
