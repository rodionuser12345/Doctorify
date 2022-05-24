using Doctorify.Domain.Models.Entities;
using Doctorify.Infrastructure.Data.Repositories.Abstractions;
using Doctorify.Infrastructure.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;

namespace Doctorify.Infrastructure.Services.Implementations;

public class AppointmentService : IAppointmentService
{
    private readonly IAppointmentRepository _repository;

    public AppointmentService(IAppointmentRepository repository)
    {
        _repository = repository;
    }
    
    public async Task<EntityEntry<Appointment>> CreateAppointmentAsync(Appointment appointmentNumber)
    {
        return await _repository.Insert(appointmentNumber);
    }

    public async Task<int> CreateAppointmentBatchAsync(IEnumerable<Appointment> appointmentNumbers)
    {
        return await _repository.BulkInsert(appointmentNumbers);
    }

    public async Task UpdateAppointmentAsync(Appointment number)
    {
        await _repository.Update(number);
    }

    public async Task<ActionResult<long>> DeleteAppointmentByIdAsync(long id)
    {
        return await _repository.Delete(id);
    }

    public async Task<Appointment> GetAppointmentByIdAsync(long id)
    {
        return await _repository.GetByIdAsync(id);
    }

    public async Task<IEnumerable<Appointment>> GetAppointmentsAsync()
    {
        return await _repository.GetAllAsync();
    }

    public async Task<IDbContextTransaction> StartTransactionAsync()
    {
        return await _repository.StartTransactionAsync();
    }
}