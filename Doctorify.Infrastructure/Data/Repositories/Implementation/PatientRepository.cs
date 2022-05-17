using Doctorify.Domain.Models.Entities;
using Doctorify.Infrastructure.Data.Context;
using Doctorify.Infrastructure.Data.Repositories.Abstractions;

namespace Doctorify.Infrastructure.Data.Repositories.Implementation;

public class PatientRepository : GenericRepository<Patient, long>, IPatientRepository
{
    public PatientRepository(DoctorifyContext doctorCabContext) : base(doctorCabContext)
    {
    }
}