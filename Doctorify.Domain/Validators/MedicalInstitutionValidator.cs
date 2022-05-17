using Doctorify.Domain.Models.Dtos;
using FluentValidation;

namespace Doctorify.Domain.Validators;

public class MedicalInstitutionValidator : AbstractValidator<MedicalInstitutionRequestDto>
{
    public MedicalInstitutionValidator()
    {
        RuleFor(x => x.FullName)
           .NotEmpty().WithMessage("FullName is required")
           .MaximumLength(100).WithMessage("FullName cannot be more than 50 characters");
        RuleFor(x => x.AddressId)
           .NotNull().WithMessage("AddressId is required");
        RuleFor(x => x.TelephoneNumberId)
           .NotNull().WithMessage("TelephoneNumberId is required");
    }
}