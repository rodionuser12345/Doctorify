using Doctorify.Domain.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;

namespace Doctorify.Infrastructure.Services.Abstractions;

public interface IDoctorService
{
    Task<EntityEntry<Doctor>> CreateDoctorAsync(Doctor doctor);
    Task<int> CreateDoctorBatchAsync(IEnumerable<Doctor> doctorList);
    Task UpdateDoctorAsync(Doctor updatedDoctor);
    Task<ActionResult<long>> DeleteDoctorByIdAsync(long id);
    Task<Doctor> GetDoctorByIdAsync(long id);
    Task<IEnumerable<Doctor>> GetDoctorsAsync();
    Task<IDbContextTransaction> StartTransactionAsync();
}