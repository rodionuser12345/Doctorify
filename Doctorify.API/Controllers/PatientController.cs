using AutoMapper;
using Doctorify.Domain.Models.Dtos;
using Doctorify.Domain.Models.Entities;
using Doctorify.Infrastructure.Data.Repositories.Abstractions;
using Doctorify.Infrastructure.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace Doctorify.Controllers;

public class PatientController : BaseApiController
{
    private readonly IPatientService _patientService;
    private readonly IMapper _mapper;

    public PatientController(IMapper mapper, IPatientRepository repository, IPatientService patientService)
    {
        _mapper = mapper;
        _patientService = patientService;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<PatientResponseDto>>> GetPatientsAsync()
    {
        var patients = await _patientService.GetPatientsAsync();
        var patientsDto = _mapper.Map<IReadOnlyList<PatientResponseDto>>(patients);
        return Ok(patientsDto);
    }

    [HttpGet("{id:long}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<PatientResponseDto>> GetPatientByIdAsync(long id)
    {
        var patient = await _patientService.GetPatientByIdAsync(id);
        var patientDto = _mapper.Map<PatientResponseDto>(patient);
        return Ok(patientDto);
    }

    [HttpPost]
    public async Task<ActionResult<PatientResponseDto>> CreatePatientAsync(PatientRequestDto patientDto)
    {
        await using var transaction = await _patientService.StartTransactionAsync();
        try
        {
            var patient = await _patientService.CreatePatientAsync(_mapper.Map<Patient>(patientDto));
            await transaction.CommitAsync();
            var value = _mapper.Map<PatientResponseDto>(patient.Entity);
            return Ok(value);
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            return Conflict(ex.Message);
        }
    }

    [HttpPost]
    [Route("batch")]
    public async Task<ActionResult<long>> CreatePatientBatchAsync(IList<PatientRequestDto> patientDtoList)
    {
        await using var transaction = await _patientService.StartTransactionAsync();
        try
        {
            var value = await _patientService.CreatePatientBatchAsync(_mapper.Map<IReadOnlyList<Patient>>(patientDtoList));
            await transaction.CommitAsync();
            return Ok(value);
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            return Conflict();
        }
    }

    [HttpPut("{id:long}")]
    public async Task<ActionResult> UpdatePatientAsync(long id, PatientRequestDto updateDto)
    {
        var validPatient = await _patientService.GetPatientByIdAsync(id);
        var result = validPatient ?? null;
        if (result is null)
            return NotFound();
        _mapper.Map(updateDto, result);
        await _patientService.UpdatePatientAsync(result);
        return NoContent();
    }

    // [HttpPatch("{id:long}")]
    // public async Task<ActionResult> PartialUpdatePatient(long id, [FromBody] JsonPatchDocument<PatientRequestDto> jsonPatchDoc)
    // {
    //     var validPatient = await _patientService.GetByIdAsync(id);
    //     var result = validPatient ?? null;
    //     if (result is null)
    //         return NotFound();
    //     var patientToPatch = _mapper.Map<PatientResponseDto>(result);
    //     jsonPatchDoc.ApplyTo(patientToPatch, ModelState);
    //     if (!TryValidateModel(patientToPatch))
    //         return ValidationProblem(ModelState);
    //     _mapper.Map(patientToPatch, validPatient);
    //     await _patientService.Update(result);
    //     return NoContent();
    // }

    [HttpDelete("{id:long}")]
    public async Task<ActionResult<long>> DeletePatientByIdAsync(long id)
    {
        var patientToDelete = await _patientService.GetPatientByIdAsync(id);
        var result = patientToDelete ?? null;
        if (result is null)
        {
            return NotFound(id);
        }

        await _patientService.DeletePatientByIdAsync(id);
        return Ok(id);
    }
}