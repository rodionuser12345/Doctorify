using Doctorify.Domain.Models.Dtos;
using FluentValidation;

namespace Doctorify.Domain.Validators;

public class TelephoneNumberValidator : AbstractValidator<TelephoneNumberDto>
{
    // create a validator for the telephone number model for Home and Work fields
    public TelephoneNumberValidator()
    {
        RuleFor(x => x.Home)
           .NotEmpty()
           .WithMessage("Home telephone number is required")
           .Length(9)
           .WithMessage("Home telephone number must be 9 digits");

        RuleFor(x => x.Work)
           .NotEmpty()
           .WithMessage("Work telephone number is required")
           .Length(10)
           .WithMessage("Work telephone number must be 10 digits");
    }
}