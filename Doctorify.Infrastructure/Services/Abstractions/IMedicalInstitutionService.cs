using Doctorify.Domain.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;

namespace Doctorify.Infrastructure.Services.Abstractions;

public interface IMedicalInstitutionService
{
    Task<EntityEntry<MedicalInstitution>> CreateMedicalInstitutionAsync(MedicalInstitution medicalInstitution);
    Task<int> CreateMedicalInstitutionBatchAsync(IEnumerable<MedicalInstitution> medicalInstitutionList);
    Task UpdateMedicalInstitutionAsync(MedicalInstitution updatedMedicalInstitution);
    Task<ActionResult<long>> DeleteMedicalInstitutionByIdAsync(long id);
    Task<MedicalInstitution> GetMedicalInstitutionByIdAsync(long id);
    Task<IEnumerable<MedicalInstitution>> GetMedicalInstitutionsAsync();
    Task<IDbContextTransaction> StartTransactionAsync();   
}