using Doctorify.Domain.Models.Entities;
using Doctorify.Infrastructure.Data.Repositories.Abstractions;
using Doctorify.Infrastructure.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;

namespace Doctorify.Infrastructure.Services.Implementations;

public class DoctorService : IDoctorService
{
    private readonly IDoctorRepository _repository;

    public DoctorService(IDoctorRepository repository)
    {
        _repository = repository;
    }
    
    public async Task<EntityEntry<Doctor>> CreateDoctorAsync(Doctor doctorNumber)
    {
        return await _repository.Insert(doctorNumber);
    }

    public async Task<int> CreateDoctorBatchAsync(IEnumerable<Doctor> doctorNumbers)
    {
        return await _repository.BulkInsert(doctorNumbers);
    }

    public async Task UpdateDoctorAsync(Doctor number)
    {
        await _repository.Update(number);
    }

    public async Task<ActionResult<long>> DeleteDoctorByIdAsync(long id)
    {
        return await _repository.Delete(id);
    }

    public async Task<Doctor> GetDoctorByIdAsync(long id)
    {
        return await _repository.GetByIdAsync(id);
    }

    public async Task<IEnumerable<Doctor>> GetDoctorsAsync()
    {
        return await _repository.GetAllAsync();
    }

    public async Task<IDbContextTransaction> StartTransactionAsync()
    {
        return await _repository.StartTransactionAsync();
    }
}