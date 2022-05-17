using Doctorify.Infrastructure.Data.Context;
using Doctorify.Infrastructure.Data.Repositories.Abstractions;
using Doctorify.Infrastructure.Data.Repositories.Implementation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Doctorify.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<DoctorifyContext>(optionBuilder =>
        {
            optionBuilder.UseSqlServer(configuration.GetConnectionString("DoctorifyConnection"));
        });

        // services.AddIdentity<User, Role>(options =>
        //          {
        //              options.Password.RequiredLength = 8;
        //          })
        //         .AddEntityFrameworkStores<DoctorifyContext>();

        services.AddScoped(typeof(IGenericRepository<,>), typeof(GenericRepository<,>));
        services.AddScoped(typeof(ITelephoneNumberRepository), typeof(TelephoneNumberRepository));
        services.AddScoped(typeof(IAddressRepository), typeof(AddressRepository));
        services.AddScoped(typeof(IPatientRepository), typeof(PatientRepository));
        services.AddScoped(typeof(IDoctorRepository), typeof(DoctorRepository));
        services.AddScoped(typeof(IMedicalInstitutionRepository), typeof(MedicalInstitutionRepository));
        services.AddScoped(typeof(IMedicalInstitutionRepository), typeof(MedicalInstitutionRepository));
        services.AddScoped(typeof(IAppointmentRepository), typeof(AppointmentRepository));

        return services;
    }
}