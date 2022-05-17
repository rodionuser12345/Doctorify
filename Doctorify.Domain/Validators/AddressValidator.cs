using Doctorify.Domain.Models.Dtos;
using FluentValidation;

namespace Doctorify.Domain.Validators;

public class AddressValidator : AbstractValidator<AddressDto>
{
    // create a validator for the address class
    public AddressValidator()
    {
        RuleFor(x => x.Street)
           .NotEmpty().WithMessage("Street is required")
           .MaximumLength(50).WithMessage("Street cannot be more than 50 characters");
        RuleFor(x => x.City)
           .NotEmpty().WithMessage("City is required")
           .MaximumLength(50).WithMessage("City cannot be more than 50 characters");
        RuleFor(x => x.State)
           .NotEmpty().WithMessage("State is required")
           .MaximumLength(50).WithMessage("State cannot be more than 50 characters");
        RuleFor(x => x.Country)
           .NotEmpty().WithMessage("Country is required")
           .MaximumLength(50).WithMessage("Country cannot be more than 50 characters");
        RuleFor(x => x.PostalCode)
          .NotEmpty().WithMessage("PostalCode is required")
          .MaximumLength(10).WithMessage("PostalCode cannot be more than 10 characters");
    }
}