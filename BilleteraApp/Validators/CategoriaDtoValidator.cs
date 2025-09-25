using BilleteraApp.Dtos;
using FluentValidation;

namespace BilleteraApp.Validators
{
    public class CategoriaDtoValidator : AbstractValidator<CategoriaDto>
    {
        public CategoriaDtoValidator()
        {
            RuleFor(x => x.Nombre)
                .NotEmpty().WithMessage("El nombre de la categoría es obligatorio.")
                .MaximumLength(50).WithMessage("El nombre de la categoría no debe superar los 50 caracteres.");
        }
    }
}
