using Doctorify.Domain.Models.Entities;
using Doctorify.Domain.Utils.SpecificationEvaluators;
using Doctorify.Domain.Utils.Specifications;
using Doctorify.Infrastructure.Data.Context;
using Doctorify.Infrastructure.Data.Repositories.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Doctorify.Infrastructure.Data.Repositories.Implementation;

public class MedicalInstitutionRepository : GenericRepository<MedicalInstitution, long>, IMedicalInstitutionRepository
{
    public MedicalInstitutionRepository(DoctorifyContext doctorCabContext) : base(doctorCabContext)
    {
    }

    public Task<MedicalInstitution> GetByCountryAsync(string country)
    {
        return Context.MedicalInstitutions
                      .Include(institution => institution.Address)
                      .Include(institution => institution.TelephoneNumber)
                      .Where(institution => institution.Address.Country.Equals(country))
                      .FirstAsync();
    }

    public Task<List<MedicalInstitution>> GetAllByCountryAsync(string country)
    {
        return Context.MedicalInstitutions
                      .Include(institution => institution.Address)
                      .Include(institution => institution.TelephoneNumber)
                      .Where(institution => institution.Address.Country.Equals(country))
                      .ToListAsync();
    }
    
    public async Task<MedicalInstitution> FindByIdWithSpecAsync(ISpecification<MedicalInstitution> spec)
    {
        return await ApplySpecification(spec).FirstOrDefaultAsync();
    }
    

    public MedicalInstitution GetByFullname(string fullName)
    {
        return Context.MedicalInstitutions
                      .Include(institution => institution.Address)
                      .Include(institution => institution.TelephoneNumber)
                      .First(institution => institution.FullName.Equals(fullName));
    }

    public MedicalInstitution GetByCountryAndFullname(string country, string fullName)
    {
        return Context.MedicalInstitutions
                      .Include(institution => institution.Address)
                      .Include(institution => institution.TelephoneNumber)
                      .First(institution => institution.Address.Country.Equals(country) && institution.FullName.Equals(fullName));
    }

    public MedicalInstitution InsertWithAddress(long addressId)
    {
        throw new NotImplementedException();
    }

    public MedicalInstitution InsertWithTelephoneNumber(long telId)
    {
        throw new NotImplementedException();
    }

    public MedicalInstitution InsertWithAddressAndTelephoneNumber(long addressId, long telId)
    {
        throw new NotImplementedException();
    }
    
    private IQueryable<MedicalInstitution> ApplySpecification(ISpecification<MedicalInstitution> spec)
    {
        return SpecificationEvaluator<MedicalInstitution>.GetQuery(Context.Set<MedicalInstitution>().AsQueryable(), spec);
    }
}