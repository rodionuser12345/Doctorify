using Doctorify.Domain.Models.Entities;

namespace Doctorify.Infrastructure.Data.Repositories.Abstractions;

public interface IPatientRepository : IGenericRepository<Patient, long>
{
    
}