using Doctorify.Domain.Models.Entities;

namespace Doctorify.Domain.Utils.Specifications;

public class MedInstWithTelNumAndAddressAndDoctors : BaseSpecification<MedicalInstitution>
{
    public MedInstWithTelNumAndAddressAndDoctors(MedInstSpecParams specParams) :
        base(x =>
                 (string.IsNullOrEmpty(specParams.Search) || x.FullName.ToLower().Contains(specParams.Search)) &&
                 (!specParams.TelNumId.HasValue || x.TelephoneNumberId == specParams.TelNumId) &&
                 (!specParams.AddressId.HasValue || x.AddressId == specParams.AddressId)
        )
    {
        AddInclude(x => x.TelephoneNumber);
        AddInclude(x => x.Address);
        AddInclude(x => x.Doctors);
        ApplyPaging(specParams.PageSize * (specParams.PageIndex - 1), specParams.PageSize);

        if (string.IsNullOrEmpty(specParams.Sort)) return;
        switch (specParams.Sort)
        {
            case "nameAsc":
                AddOrderBy(p => p.FullName);
                break;
            case "nameDesc":
                AddOrderByDesc(p => p.FullName);
                break;
            default:
                AddOrderBy(n => n.Id);
                break;
        }
    }

    public MedInstWithTelNumAndAddressAndDoctors(long id) : base(x => x.Id == id)
    {
        AddInclude(x => x.TelephoneNumber);
        AddInclude(x => x.Address);
        AddInclude(x => x.Doctors);
    }
}