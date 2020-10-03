using FluentValidation;

namespace Minedu.AprendoEnCasaOffLine.Contenido.Core.Commands
{
    public class InsertServidorCommandValidator : AbstractValidator<InsertServidorCommand>
    {
        public InsertServidorCommandValidator()
        {
            RuleFor(x => x)
                .NotNull().WithMessage("El request no debe ser nulo")
                .DependentRules(() =>
                {
                    RuleFor(x => x.ip)
                        .NotEmpty().WithMessage("La ip del servidor es requerida");

                    RuleFor(x => x.nombre)
                        .NotEmpty().WithMessage("El nombre del servidor es requerido");
                });
        }
    }
}
