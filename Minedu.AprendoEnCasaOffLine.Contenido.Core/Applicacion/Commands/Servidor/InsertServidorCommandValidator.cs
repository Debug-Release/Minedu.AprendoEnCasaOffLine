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
                    //ip4
                    RuleFor(x => x.ip).NotEmpty().WithMessage("La ip del servidor es requerida");
                    RuleFor(x => x.ip).MaximumLength(15).WithMessage("La ip del servidor no puede tener más de 15 caracteres.");

                    //nombre
                    RuleFor(x => x.nombre).NotEmpty().WithMessage("El nombre del servidor es requerido");
                    RuleFor(x => x.nombre).MaximumLength(50).WithMessage("El nombre del servidor no puede tener más de 50 caracteres.");

                    //mac
                    When(x => !string.IsNullOrEmpty(x.mac), () =>
                    {
                        RuleFor(x => x.mac).MaximumLength(30).WithMessage("La mac del servidor no puede tener más de 30 caracteres.");
                    });

                    //fqdn
                    When(x => !string.IsNullOrEmpty(x.fqdn), () =>
                    {
                        RuleFor(x => x.fqdn).MaximumLength(50).WithMessage("El fqdn del servidor no puede tener más de 50 caracteres.");
                    });
                });
        }
    }
}
