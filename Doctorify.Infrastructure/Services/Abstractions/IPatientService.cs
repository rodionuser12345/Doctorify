using Doctorify.Domain.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;

namespace Doctorify.Infrastructure.Services.Abstractions;

public interface IPatientService
{
    Task<EntityEntry<Patient>> CreatePatientAsync(Patient patient);
    Task<int> CreatePatientBatchAsync(IEnumerable<Patient> patientList);
    Task UpdatePatientAsync(Patient updatedPatient);
    Task<ActionResult<long>> DeletePatientByIdAsync(long id);
    Task<Patient> GetPatientByIdAsync(long id);
    Task<IEnumerable<Patient>> GetPatientsAsync();
    Task<IDbContextTransaction> StartTransactionAsync();
}