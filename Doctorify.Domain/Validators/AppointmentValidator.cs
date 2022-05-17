using Doctorify.Domain.Models.Entities;
using FluentValidation;

namespace Doctorify.Domain.Validators;

public class AppointmentValidator : AbstractValidator<Appointment>
{
    public AppointmentValidator()
    {
        RuleFor(x => x.Description)
           .NotEmpty().WithMessage("Description is required")
           .Length(10, 200).WithMessage("Description must be between 10 and 200 characters");
        RuleFor(x => x.Date)
           .NotEmpty().WithMessage("Date is required")
           .Must(x => x.Date >= DateTime.Now + TimeSpan.FromDays(1)).WithMessage("Date must be at least 1 day ahead from now");
        RuleFor(x => x.Cabinet)
           .NotNull().WithMessage("Cabinet is required");
        RuleFor(x => x.DoctorId)
           .NotNull().WithMessage("Doctor is required");
        RuleFor(x => x.PatientId)
           .NotNull().WithMessage("Patient is required");
    }
}