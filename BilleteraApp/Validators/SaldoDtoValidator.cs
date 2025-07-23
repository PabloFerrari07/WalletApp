using BilleteraApp.Dtos;
using FluentValidation;

namespace BilleteraApp.Validators
{
    public class SaldoDtoValidator : AbstractValidator<SaldoDto>
    {
        public SaldoDtoValidator()
        {
            RuleFor(x => x.MontoActual).NotEmpty();
        }
    }
}
