using FluentValidation;

namespace Minedu.AprendoEnCasaOffLine.Contenido.Core.Commands
{
    public class RegistrarContenidoCommandValidator : AbstractValidator<RegistrarContenidoCommand>
    {
        public RegistrarContenidoCommandValidator()
        {
            RuleFor(x => x)
                .NotNull().WithMessage("El request no debe ser nulo")
                .DependentRules(() =>
                {
                    //nombre
                    RuleFor(x => x.nombre).NotEmpty().WithMessage("El nombre del contenido es requerido");
                    RuleFor(x => x.nombre).MaximumLength(100).WithMessage("El nombre del contenido no puede tener más de 100 caracteres.");

                    //rutaOrigen
                    RuleFor(x => x.archivo).NotEmpty().WithMessage("La ruta del archivo del contenido es requerida");
                    RuleFor(x => x.archivo).MaximumLength(100).WithMessage("La ruta del archivo del contenido no puede tener más de 100 caracteres.");

                    //descripcion
                    When(x => !string.IsNullOrEmpty(x.descripcion), () =>
                    {
                        RuleFor(x => x.descripcion).MaximumLength(250).WithMessage("La descripción del contenido no puede tener más de 250 caracteres.");
                    });

                    //pesoMb
                    When(x => !string.IsNullOrEmpty(x.pesoMb), () =>
                    {
                        RuleFor(x => x.pesoMb).MaximumLength(10).WithMessage("El pesoMb del contenido no puede tener más de 10 caracteres.");
                    });
                });
        }
    }
}
