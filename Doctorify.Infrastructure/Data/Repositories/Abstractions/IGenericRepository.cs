using Doctorify.Domain.Models.Entities;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;

namespace Doctorify.Infrastructure.Data.Repositories.Abstractions;

public interface IGenericRepository<T, TId> where T : BaseEntity where TId : struct
{
    Task<T> GetByIdAsync(TId id);
    Task<IEnumerable<T>> GetAllAsync();
    Task<EntityEntry<T>> Insert(T entity);
    public Task<int> BulkInsert(IEnumerable<T> entities);
    Task Update(T entity);
    Task<long> Delete(TId id);
    public Task<IDbContextTransaction> StartTransactionAsync();

}