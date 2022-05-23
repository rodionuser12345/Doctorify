using Doctorify.Infrastructure.Data.Context;
using Doctorify.Infrastructure.Data.Context.Identity;
using Doctorify.Infrastructure.Data.Repositories.Abstractions;
using Doctorify.Infrastructure.Data.Repositories.Implementation;
using Microsoft.AspNetCore.Identity;
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
        
        services.AddDbContext<AppIdentityDbContext>(optionBuilder =>
        {
            optionBuilder.UseSqlServer(configuration.GetConnectionString("DoctorifyIdentityConnection"));
        });
        
        services.AddIdentity<IdentityUser, IdentityRole>()
               .AddEntityFrameworkStores<AppIdentityDbContext>()
               .AddDefaultTokenProviders();

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