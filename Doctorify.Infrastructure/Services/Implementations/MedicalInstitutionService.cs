using Doctorify.Domain.Models.Entities;
using Doctorify.Infrastructure.Data.Repositories.Abstractions;
using Doctorify.Infrastructure.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;

namespace Doctorify.Infrastructure.Services.Implementations;

public class MedicalInstitutionService : IMedicalInstitutionService
{
    private readonly IMedicalInstitutionRepository _repository;

    public MedicalInstitutionService(IMedicalInstitutionRepository repository)
    {
        _repository = repository;
    }
    
    public async Task<EntityEntry<MedicalInstitution>> CreateMedicalInstitutionAsync(MedicalInstitution medicalInstitutionNumber)
    {
        return await _repository.Insert(medicalInstitutionNumber);
    }

    public async Task<int> CreateMedicalInstitutionBatchAsync(IEnumerable<MedicalInstitution> medicalInstitutionNumbers)
    {
        return await _repository.BulkInsert(medicalInstitutionNumbers);
    }

    public async Task UpdateMedicalInstitutionAsync(MedicalInstitution number)
    {
        await _repository.Update(number);
    }

    public async Task<ActionResult<long>> DeleteMedicalInstitutionByIdAsync(long id)
    {
        return await _repository.Delete(id);
    }

    public async Task<MedicalInstitution> GetMedicalInstitutionByIdAsync(long id)
    {
        return await _repository.GetByIdAsync(id);
    }

    public async Task<IEnumerable<MedicalInstitution>> GetMedicalInstitutionsAsync()
    {
        return await _repository.GetAllAsync();
    }

    public async Task<IDbContextTransaction> StartTransactionAsync()
    {
        return await _repository.StartTransactionAsync();
    }
}