using BilleteraApp.Dtos;
using FluentValidation;

namespace BilleteraApp.Validators
{
    public class GastoDtoValidator : AbstractValidator<GastoDto>
    {
        public GastoDtoValidator()
        {
            RuleFor(x => x.Monto).NotEmpty();
            RuleFor(x => x.CategoriaNombre).NotEmpty();
            RuleFor(x => x.Descripcion).NotEmpty();
        }
    }
}
