using Doctorify.Domain.Models.Dtos;
using Doctorify.Domain.Models.Enums;
using FluentValidation;

namespace Doctorify.Domain.Validators;

public class PatientValidator : AbstractValidator<PatientRequestDto>
{
    public PatientValidator()
    {
        RuleFor(x => x.FirstName)
           .NotEmpty().WithMessage("First name is required")
           .Length(1, 50).WithMessage("First name must be between 1 and 50 characters");
        RuleFor(x => x.LastName)
           .NotEmpty().WithMessage("Last name is required")
           .Length(1, 50).WithMessage("Last name must be between 1 and 50 characters");
        RuleFor(x => x.DateOfBirth)
           .NotEmpty().WithMessage("Date of birth is required")
           .Must(x => x.Year > 1900).WithMessage("Date of birth must be after 1900");
        RuleFor(x => x.BloodType)
           .NotEmpty().WithMessage("Blood type is required")
           .MinimumLength(13).WithMessage("Blood type must be at least 13 characters")
           .IsEnumName(typeof(BloodType), false);
        RuleFor(x => x.Diagnose)
           .NotEmpty().WithMessage("Diagnoses is required")
           .MaximumLength(250).WithMessage("Diagnoses must be maximum 250 character");
        RuleFor(x => x.TelephoneId)
           .NotNull().WithMessage("Telephone is required");
        RuleFor(x => x.AddressId)
           .NotNull().WithMessage("Address is required");
    }
}