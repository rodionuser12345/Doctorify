using Doctorify.Domain.Models.Entities;
using Doctorify.Infrastructure.Data.Repositories.Abstractions;
using Doctorify.Infrastructure.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;

namespace Doctorify.Infrastructure.Services.Implementations;

public class TelephoneService : ITelephoneService
{
    private readonly ITelephoneNumberRepository _repository;

    public TelephoneService(ITelephoneNumberRepository repository)
    {
        _repository = repository;
    }
    
    public async Task<EntityEntry<TelephoneNumber>> CreateTelephoneAsync(TelephoneNumber telephoneNumber)
    {
        return await _repository.Insert(telephoneNumber);
    }

    public async Task<int> CreateTelephoneBatchAsync(IEnumerable<TelephoneNumber> telephoneNumbers)
    {
        return await _repository.BulkInsert(telephoneNumbers);
    }

    public async Task UpdateTelephoneAsync(TelephoneNumber number)
    {
        await _repository.Update(number);
    }

    public async Task<ActionResult<long>> DeleteTelephoneByIdAsync(long id)
    {
        return await _repository.Delete(id);
    }

    public async Task<TelephoneNumber> GetTelephoneByIdAsync(long id)
    {
        return await _repository.GetByIdAsync(id);
    }

    public async Task<IEnumerable<TelephoneNumber>> GetTelephonesAsync()
    {
        return await _repository.GetAllAsync();
    }

    public async Task<IDbContextTransaction> StartTransactionAsync()
    {
        return await _repository.StartTransactionAsync();
    }
}