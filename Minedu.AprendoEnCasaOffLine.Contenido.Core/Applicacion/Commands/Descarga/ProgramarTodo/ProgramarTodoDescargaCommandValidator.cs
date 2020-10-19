using FluentValidation;

namespace Minedu.AprendoEnCasaOffLine.Contenido.Core.Commands
{
    public class ProgramarTodoDescargaCommandValidator : AbstractValidator<ProgramarTodoDescargaCommand>
    {
        public ProgramarTodoDescargaCommandValidator()
        {
            RuleFor(x => x)
                .NotNull().WithMessage("El request no debe ser nulo")
                .DependentRules(() =>
                {
                    //idContenido                   
                    When(x => !string.IsNullOrWhiteSpace(x.idContenido), () =>
                    {
                        RuleFor(x => x.idContenido).Matches(@"[0-9a-fA-F]{24}").WithMessage("El formato del id del contenido no es válido.");
                    });                   

                });
        }
    }
}
