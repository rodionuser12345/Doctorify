using Doctorify.Domain.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;

namespace Doctorify.Infrastructure.Services.Abstractions;

public interface ITelephoneService
{
    Task<EntityEntry<TelephoneNumber>> CreateTelephoneAsync(TelephoneNumber telephone);
    Task<int> CreateTelephoneBatchAsync(IEnumerable<TelephoneNumber> telephoneList);
    Task UpdateTelephoneAsync(TelephoneNumber updatedTelephone);
    Task<ActionResult<long>> DeleteTelephoneByIdAsync(long id);
    Task<TelephoneNumber> GetTelephoneByIdAsync(long id);
    Task<IEnumerable<TelephoneNumber>> GetTelephonesAsync();
    Task<IDbContextTransaction> StartTransactionAsync();
}