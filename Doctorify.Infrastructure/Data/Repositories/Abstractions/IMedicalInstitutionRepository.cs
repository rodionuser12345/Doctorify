using Doctorify.Domain.Models.Entities;
using Doctorify.Domain.Utils.Specifications;

namespace Doctorify.Infrastructure.Data.Repositories.Abstractions;

public interface IMedicalInstitutionRepository : IGenericRepository<MedicalInstitution, long>
{
    Task<MedicalInstitution> GetByCountryAsync(string country);
    Task<MedicalInstitution> FindByIdWithSpecAsync(ISpecification<MedicalInstitution> spec);
    Task<List<MedicalInstitution>> GetAllByCountryAsync(string country);
    MedicalInstitution GetByFullname(string fullName);
    MedicalInstitution GetByCountryAndFullname(string country, string fullName);
    MedicalInstitution InsertWithAddress(long addressId);
    MedicalInstitution InsertWithTelephoneNumber(long telId);
    MedicalInstitution InsertWithAddressAndTelephoneNumber(long addressId, long telId);
}