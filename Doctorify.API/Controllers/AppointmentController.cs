using AutoMapper;
using Doctorify.Domain.Models.Dtos;
using Doctorify.Domain.Models.Entities;
using Doctorify.Infrastructure.Data.Repositories.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace Doctorify.Controllers;

public class AppointmentController : BaseApiController
{
    private readonly IAppointmentRepository _repository;
    private readonly IDoctorRepository _doctorRepository;
    private readonly IPatientRepository _patientRepository;
    private readonly IMapper _mapper;


    public AppointmentController(IAppointmentRepository appointmentRepository, IDoctorRepository doctorRepository, IPatientRepository patientRepository, IMapper mapper)
    {
        _repository = appointmentRepository;
        _doctorRepository = doctorRepository;
        _patientRepository = patientRepository;
        _mapper = mapper;
    }
    
    // GET: api/Appointment get all appointments asynchronously
    [HttpGet]
    public async Task<IActionResult> GetAppointmentsAsync()
    {
        var appointments = await _repository.GetAllAsync();
        return Ok(appointments);
    }
    
    // GET: api/Appointment/5 get appointment by id asynchronously
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetAppointmentAsync([FromRoute] int id)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var appointment = await _repository.GetByIdAsync(id);

        if (appointment == null)
        {
            return NotFound();
        }

        return Ok(appointment);
    }
    
    // PUT: api/Appointment/5 update appointment asynchronously
    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateAppointmentAsync([FromRoute] int id, [FromBody] Appointment appointment)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        if (id != appointment.Id)
        {
            return BadRequest();
        }

        var validAppointment = await _repository.GetByIdAsync(id);
        var result = validAppointment ?? null;
        if (result is null)
            return NotFound();
        
        await _repository.Update(result);

        return NoContent();
    }
    
    // POST: api/Appointment create appointment asynchronously
    [HttpPost]
    public async Task<IActionResult> CreateAppointmentAsync([FromBody] Appointment appointment)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        await using var transaction = await _repository.StartTransactionAsync();
        try
        {
            var rEntry = await _repository.Insert(appointment);
            var result = rEntry.Entity;
            result.Doctor = await _doctorRepository.GetByIdAsync(result.DoctorId);
            result.Patient = await _patientRepository.GetByIdAsync(result.PatientId);
            await transaction.CommitAsync();
            return Ok(result);
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            return Conflict(ex.Message);
        }
    }
    
    // POST: api/Appointment create batch appointments asynchronously
    [HttpPost("batch")]
    public async Task<IActionResult> CreateAppointmentsAsync([FromBody] List<Appointment> appointments)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        await using var transaction = await _repository.StartTransactionAsync();
        try
        {
            var value = await _repository.BulkInsert(appointments);
            await transaction.CommitAsync();
            return Ok(value);
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            return Conflict(ex.Message);
        }
    }
    
    // DELETE: api/Appointment/5 delete appointment asynchronously
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteAppointmentAsync([FromRoute] int id)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var appointment = await _repository.GetByIdAsync(id);
        if (appointment == null)
        {
            return NotFound();
        }

        await _repository.Delete(id);

        return Ok(id);
    }
    
}