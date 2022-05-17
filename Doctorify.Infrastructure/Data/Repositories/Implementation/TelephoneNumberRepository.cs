using Doctorify.Domain.Models.Entities;
using Doctorify.Infrastructure.Data.Context;
using Doctorify.Infrastructure.Data.Repositories.Abstractions;

namespace Doctorify.Infrastructure.Data.Repositories.Implementation;

public class TelephoneNumberRepository : GenericRepository<TelephoneNumber, long>, ITelephoneNumberRepository
{
    public TelephoneNumberRepository(DoctorifyContext doctorCabContext) : base(doctorCabContext)
    {
    }
}