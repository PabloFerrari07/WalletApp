using BilleteraApp.Dtos;
using FluentValidation;

namespace BilleteraApp.Validators
{
    public class SaldoDtoValidator : AbstractValidator<SaldoDto>
    {
        public SaldoDtoValidator()
        {
            RuleFor(x => x.MontoActual)
                .NotEmpty().WithMessage("El monto actual es obligatorio.")
                .Must(x => x.GetType() == typeof(decimal)
                           || x.GetType() == typeof(double)
                           || x.GetType() == typeof(int))
                .WithMessage("El monto debe ser numérico.")
                .GreaterThanOrEqualTo(0).WithMessage("El monto no puede ser negativo.");
        }
    }
}
