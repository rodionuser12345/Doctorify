using Doctorify.Domain.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;

namespace Doctorify.Infrastructure.Services.Abstractions;

public interface IAppointmentService
{
    Task<EntityEntry<Appointment>> CreateAppointmentAsync(Appointment appointment);
    Task<int> CreateAppointmentBatchAsync(IEnumerable<Appointment> appointmentList);
    Task UpdateAppointmentAsync(Appointment updatedAppointment);
    Task<ActionResult<long>> DeleteAppointmentByIdAsync(long id);
    Task<Appointment> GetAppointmentByIdAsync(long id);
    Task<IEnumerable<Appointment>> GetAppointmentsAsync();
    Task<IDbContextTransaction> StartTransactionAsync();
}