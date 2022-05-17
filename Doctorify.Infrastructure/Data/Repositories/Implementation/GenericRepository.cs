using System.Data.Entity.Validation;
using Doctorify.Domain.Models.Entities;
using Doctorify.Infrastructure.Data.Context;
using Doctorify.Infrastructure.Data.Repositories.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;
using EntityState = Microsoft.EntityFrameworkCore.EntityState;

namespace Doctorify.Infrastructure.Data.Repositories.Implementation;

public class GenericRepository<T, TId> : IGenericRepository<T, TId> where T : BaseEntity where TId : struct
{
    private readonly DbSet<T> _table;
    private string _errorMessage;

    public GenericRepository(DoctorifyContext doctorCabContext)
    {
        Context = doctorCabContext;
        _table = Context.Set<T>();
    }

    public DoctorifyContext Context { get; }

    public async Task<T> GetByIdAsync(TId id)
    {
        return await _table.FindAsync(id);
    }       

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _table.ToListAsync();
    }

    public async Task<EntityEntry<T>> Insert(T entity)
    {
        try
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            var entityEntry = await _table.AddAsync(entity);
            await Context.SaveChangesAsync();
            return entityEntry;
        }
        catch (DbEntityValidationException dbEx)
        {
            foreach (var validationErrors in dbEx.EntityValidationErrors)
            foreach (var validationError in validationErrors.ValidationErrors)
                _errorMessage += $"Property: {validationError.PropertyName} Error: {validationError.ErrorMessage}" + Environment.NewLine;
            throw new Exception(_errorMessage, dbEx);
        }
    }

    public async Task<int> BulkInsert(IEnumerable<T> entities)
    {
        try
        {
            if (entities == null)
            {
                throw new ArgumentNullException(nameof(entities));
            }

            await _table.AddRangeAsync(entities);
            return await Save();
        }
        catch (DbEntityValidationException dbEx)
        {
            foreach (var validationErrors in dbEx.EntityValidationErrors)
            {
                foreach (var validationError in validationErrors.ValidationErrors)
                {
                    _errorMessage += $"Property: {validationError.PropertyName} Error: {validationError.ErrorMessage}" +
                                     Environment.NewLine;
                }
            }

            throw new Exception(_errorMessage, dbEx);
        }
    }

    public virtual async Task Update(T entity)
    {
        try
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            SetEntryModified(entity);
            await Save();
        }
        catch (DbEntityValidationException dbEx)
        {
            foreach (var validationErrors in dbEx.EntityValidationErrors)
            foreach (var validationError in validationErrors.ValidationErrors)
                _errorMessage += Environment.NewLine +
                                 $"Property: {validationError.PropertyName} Error: {validationError.ErrorMessage}";
            throw new Exception(_errorMessage, dbEx);
        }
    }

    public virtual async Task<long> Delete(TId id)
    {
        try
        {
            var entity = GetByIdAsync(id).Result;
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            var entityId = _table.Remove(entity).Entity.Id;
            await Save();
            return entityId;
        }
        catch (DbEntityValidationException dbEx)
        {
            foreach (var validationErrors in dbEx.EntityValidationErrors)
            foreach (var validationError in validationErrors.ValidationErrors)
                _errorMessage += Environment.NewLine + $"Property: {validationError.PropertyName} Error: {validationError.ErrorMessage}";
            throw new Exception(_errorMessage, dbEx);
        }
    }

    private void SetEntryModified(T entity)
    {
        _table.Attach(entity);
        Context.Entry(entity).State = EntityState.Modified;
    }

    private async Task<int> Save()
    {
        return await Context.SaveChangesAsync();
    }

    public async Task<IDbContextTransaction> StartTransactionAsync()
    {
        return await Context.Database.BeginTransactionAsync();
    }
}