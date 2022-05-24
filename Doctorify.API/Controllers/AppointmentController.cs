using AutoMapper;
using Doctorify.Domain.Models.Dtos;
using Doctorify.Domain.Models.Entities;
using Doctorify.Infrastructure.Data.Repositories.Abstractions;
using Doctorify.Infrastructure.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace Doctorify.Controllers;

public class AppointmentController : BaseApiController
{
    private readonly IAppointmentService _appointmentService;
    private readonly IDoctorService _doctorService;
    private readonly IPatientService _patientService;
    private readonly IMapper _mapper;


    public AppointmentController(IAppointmentService appointmentService, IDoctorService doctorService, IPatientService patientService, IMapper mapper)
    {
        _appointmentService = appointmentService;
        _doctorService = doctorService;
        _patientService = patientService;
        _mapper = mapper;
    }
    
    // GET: api/Appointment get all appointments asynchronously
    [HttpGet]
    public async Task<IActionResult> GetAppointmentsAsync()
    {
        var appointments = await _appointmentService.GetAppointmentsAsync();
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

        var appointment = await _appointmentService.GetAppointmentByIdAsync(id);

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

        var validAppointment = await _appointmentService.GetAppointmentByIdAsync(id);
        var result = validAppointment ?? null;
        if (result is null)
            return NotFound();
        
        await _appointmentService.UpdateAppointmentAsync(result);

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

        await using var transaction = await _appointmentService.StartTransactionAsync();
        try
        {
            var rEntry = await _appointmentService.CreateAppointmentAsync(appointment);
            var result = rEntry.Entity;
            result.Doctor = await _doctorService.GetDoctorByIdAsync(result.DoctorId);
            result.Patient = await _patientService.GetPatientByIdAsync(result.PatientId);
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

        await using var transaction = await _appointmentService.StartTransactionAsync();
        try
        {
            var value = await _appointmentService.CreateAppointmentBatchAsync(appointments);
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

        var appointment = await _appointmentService.GetAppointmentByIdAsync(id);
        if (appointment == null)
        {
            return NotFound();
        }

        await _appointmentService.DeleteAppointmentByIdAsync(id);

        return Ok(id);
    }
    
}