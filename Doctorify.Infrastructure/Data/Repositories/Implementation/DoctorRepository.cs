using Doctorify.Domain.Models.Entities;
using Doctorify.Infrastructure.Data.Context;
using Doctorify.Infrastructure.Data.Repositories.Abstractions;

namespace Doctorify.Infrastructure.Data.Repositories.Implementation;

public class DoctorRepository : GenericRepository<Doctor, long>, IDoctorRepository
{
    public DoctorRepository(DoctorifyContext doctorCabContext) : base(doctorCabContext)
    {
    }
}