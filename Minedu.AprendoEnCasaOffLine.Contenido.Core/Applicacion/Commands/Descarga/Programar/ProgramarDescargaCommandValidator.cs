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
                    //When(x => !string.IsNullOrEmpty(x.idContenido), () =>
                    //{
                    //    RuleFor(x => x.idContenido).Matches(@"[0-9a-fA-F]{24}").WithMessage("El formato del id del contenido no es válido.");
                    //});

                    //Mac Servidor
                    RuleFor(x => x.macServidor).NotEmpty().WithMessage("La dirección mac del servidor es requerida");

                    When(x => !string.IsNullOrEmpty(x.macServidor), () =>
                    {
                        RuleFor(x => x.macServidor).MaximumLength(30).WithMessage("La dirección mac del servidor no puede tener más de 30 caracteres.");
                    });

                });
        }
    }
}
