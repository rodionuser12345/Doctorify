using Doctorify.Domain.Models.Entities;

namespace Doctorify.Infrastructure.Data.Repositories.Abstractions;

public interface IAppointmentRepository : IGenericRepository<Appointment, long>
{
    
}