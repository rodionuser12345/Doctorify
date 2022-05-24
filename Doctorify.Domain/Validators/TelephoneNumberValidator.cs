using Doctorify.Domain.Models.Dtos;
using FluentValidation;

namespace Doctorify.Domain.Validators;

public class TelephoneNumberValidator : AbstractValidator<TelephoneNumberDto>
{
    // create a validator for the telephone number model for Home and Work fields
    public TelephoneNumberValidator()
    {
        RuleFor(x => x.Number)
           .NotEmpty()
           .WithMessage("Telephone number is required")
           .Length(9)
           .WithMessage("Telephone number must be 9 digits");
    }
}