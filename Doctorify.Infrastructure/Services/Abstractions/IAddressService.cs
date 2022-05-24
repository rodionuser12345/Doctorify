using Doctorify.Domain.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;

namespace Doctorify.Infrastructure.Services.Abstractions;

public interface IAddressService
{
    Task<EntityEntry<Address>> CreateAddressAsync(Address address);
    Task<int> CreateAddressBatchAsync(IEnumerable<Address> addressList);
    Task UpdateAddressAsync(Address updatedAddress);
    Task<ActionResult<long>> DeleteAddressByIdAsync(long id);
    Task<Address> GetAddressByIdAsync(long id);
    Task<IEnumerable<Address>> GetAddressesAsync();
    Task<IDbContextTransaction> StartTransactionAsync();
}