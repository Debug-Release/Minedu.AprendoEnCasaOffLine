using FluentValidation;

namespace Minedu.AprendoEnCasaOffLine.Contenido.Core.Commands
{
    public class TrazabilidadCommandValidator : AbstractValidator<TrazabilidadCommand>
    {
        public TrazabilidadCommandValidator()
        {
            RuleFor(x => x)
               .NotNull().WithMessage("El request no debe ser nulo")
               .DependentRules(() =>
               {
                   RuleFor(x => x.archivo).NotEmpty().WithMessage("El archivo de trazabilidad es requerido");
                   RuleFor(x => x.archivo).Must(x => x == null || x.Length == 0).WithMessage("El contenido del archivo de trazabilidad no es valido.");

               });
        }
    }
}
