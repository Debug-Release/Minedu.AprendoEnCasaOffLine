using FluentValidation;

namespace Minedu.AprendoEnCasaOffLine.Contenido.Core.Commands
{
    public class RegistrarServidorCommandValidator : AbstractValidator<RegistrarServidorCommand>
    {
        public RegistrarServidorCommandValidator()
        {
            RuleFor(x => x)
                .NotNull().WithMessage("El request no debe ser nulo")
                .DependentRules(() =>
                {
                    //ip4
                    RuleFor(x => x.ip).NotEmpty().WithMessage("La dirección ip del servidor es requerida");
                    RuleFor(x => x.ip).MaximumLength(15).WithMessage("La dirección ip del servidor no puede tener más de 15 caracteres.");
                   
                    RuleFor(x => x.ip).Matches(@"^(([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])\.){3}([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])$").WithMessage("La dirección ip del servidor no es válido.");
                    
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
