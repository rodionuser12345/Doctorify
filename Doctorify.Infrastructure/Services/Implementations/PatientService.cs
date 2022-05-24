using Doctorify.Domain.Models.Entities;
using Doctorify.Infrastructure.Data.Repositories.Abstractions;
using Doctorify.Infrastructure.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;

namespace Doctorify.Infrastructure.Services.Implementations;

public class PatientService : IPatientService
{
    private readonly IPatientRepository _repository;

    public PatientService(IPatientRepository repository)
    {
        _repository = repository;
    }
    
    public async Task<EntityEntry<Patient>> CreatePatientAsync(Patient patientNumber)
    {
        return await _repository.Insert(patientNumber);
    }

    public async Task<int> CreatePatientBatchAsync(IEnumerable<Patient> patientNumbers)
    {
        return await _repository.BulkInsert(patientNumbers);
    }

    public async Task UpdatePatientAsync(Patient number)
    {
        await _repository.Update(number);
    }

    public async Task<ActionResult<long>> DeletePatientByIdAsync(long id)
    {
        return await _repository.Delete(id);
    }

    public async Task<Patient> GetPatientByIdAsync(long id)
    {
        return await _repository.GetByIdAsync(id);
    }

    public async Task<IEnumerable<Patient>> GetPatientsAsync()
    {
        return await _repository.GetAllAsync();
    }

    public async Task<IDbContextTransaction> StartTransactionAsync()
    {
        return await _repository.StartTransactionAsync();
    }
}