using Doctorify.Domain.Models.Entities;
using Doctorify.Infrastructure.Data.Repositories.Abstractions;
using Doctorify.Infrastructure.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;

namespace Doctorify.Infrastructure.Services.Implementations;

public class AddressService : IAddressService

{
    private readonly IAddressRepository _repository;

    public AddressService(IAddressRepository repository)
    {
        _repository = repository;
    }
    
    public async Task<EntityEntry<Address>> CreateAddressAsync(Address addressNumber)
    {
        return await _repository.Insert(addressNumber);
    }

    public async Task<int> CreateAddressBatchAsync(IEnumerable<Address> addressNumbers)
    {
        return await _repository.BulkInsert(addressNumbers);
    }

    public async Task UpdateAddressAsync(Address number)
    {
        await _repository.Update(number);
    }

    public async Task<ActionResult<long>> DeleteAddressByIdAsync(long id)
    {
        return await _repository.Delete(id);
    }

    public async Task<Address> GetAddressByIdAsync(long id)
    {
        return await _repository.GetByIdAsync(id);
    }

    public async Task<IEnumerable<Address>> GetAddressesAsync()
    {
        return await _repository.GetAllAsync();
    }

    public async Task<IDbContextTransaction> StartTransactionAsync()
    {
        return await _repository.StartTransactionAsync();
    }
}