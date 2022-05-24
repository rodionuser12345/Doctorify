using AutoMapper;
using Doctorify.Domain.Models.Dtos;
using Doctorify.Domain.Models.Entities;

namespace Doctorify.Domain.Utils;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<Doctor, DoctorResponseDto>()
           .ForMember(d => d.FullName,
                      o => o.MapFrom(s => $"{s.Prefix} {s.FirstName} {s.LastName}"))
           .ForMember(d => d.Specialization,
                      o => o.MapFrom(s => s.Specialization))
           .ForMember(d => d.DateOfBirth,
                      o => o.MapFrom(s => s.DateOfBirth))
           .ForMember(d => d.TelephoneNumber,
                      o => o.MapFrom(s => s.TelephoneNumber))
           .ForMember(d => d.MedicalInstitution,
                      o => o.MapFrom(s => s.MedicalInstitution));

        CreateMap<DoctorRequestDto, Doctor>()
           .ForMember(d => d.FirstName,
                      o => o.MapFrom(s => s.FirstName))
           .ForMember(d => d.LastName,
                      o => o.MapFrom(s => s.LastName))
           .ForMember(d => d.Specialization,
                      o => o.MapFrom(s => s.Specialization))
           .ForMember(d => d.DateOfBirth,
                      o => o.MapFrom(s => s.DateOfBirth))
           .ForMember(d => d.TelephoneNumberId,
                      o => o.MapFrom(s => s.TelephoneNumberId))
           .ForMember(d => d.MedicalInstitutionId,
                      o => o.MapFrom(s => s.MedicalInstitutionId));

        CreateMap<Doctor, DoctorForPatientResponseDto>()
           .ForMember(d => d.FullName,
                      o => o.MapFrom(s => $"{s.Prefix} {s.FirstName} {s.LastName}"))
           .ForMember(d => d.Specialization,
                      o => o.MapFrom(s => s.Specialization))
           .ForMember(d => d.TelephoneNumber,
                      o => o.MapFrom(s => s.TelephoneNumber))
           .ForMember(d => d.MedicalInstitution,
                      o => o.MapFrom(s => s.MedicalInstitution.FullName));

        CreateMap<Address, AddressDto>();
        CreateMap<AddressDto, Address>();

        CreateMap<MedicalInstitution, MedicalInstitutionResponseDto>()
           .ForMember(d => d.FullName,
                      o => o.MapFrom(s => s.FullName))
           .ForMember(d => d.Address,
                      o => o.MapFrom(s => s.Address))
           .ForMember(d => d.TelephoneNumber,
                      o => o.MapFrom(s => s.TelephoneNumber));

        CreateMap<MedicalInstitutionRequestDto, MedicalInstitution>()
           .ForMember(d => d.FullName,
                      o => o.MapFrom(s => s.FullName))
           .ForMember(d => d.AddressId,
                      o => o.MapFrom(s => s.AddressId))
           .ForMember(d => d.TelephoneNumberId,
                      o => o.MapFrom(s => s.TelephoneNumberId));

        CreateMap<TelephoneNumber, TelephoneNumberDto>()
           .ForMember(d => d.Number,
                      o => o.MapFrom(s => s.Number));
        CreateMap<TelephoneNumberDto, TelephoneNumber>()
           .ForMember(d => d.Number,
                      o => o.MapFrom(s => s.Number));

        CreateMap<PatientRequestDto, Patient>()
           .ForMember(d => d.FirstName,
                      o => o.MapFrom(s => s.FirstName))
           .ForMember(d => d.LastName,
                      o => o.MapFrom(s => s.LastName))
           .ForMember(d => d.Diagnose,
                      o => o.MapFrom(s => s.Diagnose))
           .ForMember(d => d.BloodType,
                      o => o.MapFrom(s => s.BloodType))
           .ForMember(d => d.DateOfBirth,
                      o => o.MapFrom(s => s.DateOfBirth))
           .ForMember(d => d.TelephoneNumberId,
                      o => o.MapFrom(s => s.TelephoneId));

        CreateMap<Patient, PatientResponseDto>()
           .ForMember(d => d.FirstName,
                      o => o.MapFrom(s => s.FirstName))
           .ForMember(d => d.LastName,
                      o => o.MapFrom(s => s.LastName))
           .ForMember(d => d.Diagnose,
                      o => o.MapFrom(s => s.Diagnose))
           .ForMember(d => d.BloodType,
                      o => o.MapFrom(s => s.BloodType))
           .ForMember(d => d.DateOfBirth,
                      o => o.MapFrom(s => s.DateOfBirth))
           .ForMember(d => d.Doctors,
                      o => o.MapFrom(s => s.Doctors))
           .ForMember(d => d.Telephone,
                      o => o.MapFrom(s => s.TelephoneNumber));
    }
}