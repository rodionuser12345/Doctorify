using Doctorify.Domain.Models.Entities;
using Doctorify.Infrastructure.Data.Context;
using Doctorify.Infrastructure.Data.Repositories.Abstractions;

namespace Doctorify.Infrastructure.Data.Repositories.Implementation;

public class AddressRepository : GenericRepository<Address, long>, IAddressRepository
{
    public AddressRepository(DoctorifyContext doctorCabContext) : base(doctorCabContext)
    {
    }
}