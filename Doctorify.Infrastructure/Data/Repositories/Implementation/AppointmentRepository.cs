using Doctorify.Domain.Models.Entities;
using Doctorify.Infrastructure.Data.Context;
using Doctorify.Infrastructure.Data.Repositories.Abstractions;

namespace Doctorify.Infrastructure.Data.Repositories.Implementation;

public class AppointmentRepository : GenericRepository<Appointment, long>, IAppointmentRepository
{
    public AppointmentRepository(DoctorifyContext doctorCabContext) : base(doctorCabContext)
    {
    }
}