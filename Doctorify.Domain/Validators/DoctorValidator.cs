using Doctorify.Domain.Models.Dtos;
using Doctorify.Domain.Models.Enums;
using FluentValidation;

namespace Doctorify.Domain.Validators;

public class DoctorValidator : AbstractValidator<DoctorRequestDto>
{
    public DoctorValidator()
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
        RuleFor(x => x.Specialization)
           .NotEmpty().WithMessage("Specialization is required")
           .Length(7, 50).WithMessage("Specialization must be between 1 and 50 characters")
           .IsEnumName(typeof(DoctorSpecialization), false);
        RuleFor(x => x.TelephoneNumberId)
          .NotNull().WithMessage("Telephone number is required");
        RuleFor(x => x.MedicalInstitutionId)
          .NotNull().WithMessage("MedicalInstitution is required");
    }
}