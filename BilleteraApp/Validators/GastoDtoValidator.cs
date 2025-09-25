using BilleteraApp.Dtos;
using FluentValidation;

namespace BilleteraApp.Validators
{
    public class GastoDtoValidator : AbstractValidator<GastoDto>
    {
        public GastoDtoValidator()
        {
            RuleFor(x => x.Monto)
                .NotEmpty().WithMessage("El monto es obligatorio.")
                .GreaterThan(0).WithMessage("El monto debe ser mayor a 0.");

            RuleFor(x => x.CategoriaNombre)
                .NotEmpty().WithMessage("La categoría es obligatoria.")
                .Matches("^[a-zA-ZáéíóúÁÉÍÓÚñÑ\\s]+$").WithMessage("La categoría solo puede contener letras.");

            RuleFor(x => x.Descripcion)
                .NotEmpty().WithMessage("La descripción es obligatoria.")
                .Matches("^[a-zA-Z0-9áéíóúÁÉÍÓÚñÑ\\s,.!?-]+$").WithMessage("La descripción contiene caracteres no válidos.");
        }
    }
}
