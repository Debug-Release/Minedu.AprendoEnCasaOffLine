using FluentValidation;

namespace Minedu.AprendoEnCasaOffLine.Contenido.Core.Commands
{
    public class InsertContenidoCommandValidator : AbstractValidator<InsertContenidoCommand>
    {
        public InsertContenidoCommandValidator()
        {
            RuleFor(x => x)
                .NotNull().WithMessage("El request no debe ser nulo")
                .DependentRules(() =>
                {

                    RuleFor(x => x.nombre)
                        .NotEmpty().WithMessage("El nombre del servidor es requerido");

                    RuleFor(x => x.rutaOrigen)
                        .NotEmpty().WithMessage("La ruta de origen del contenido es requerida");                                        
                });
        }
    }
}
