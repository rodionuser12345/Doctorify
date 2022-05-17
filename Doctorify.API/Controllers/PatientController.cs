using AutoMapper;
using Doctorify.Domain.Models.Dtos;
using Doctorify.Domain.Models.Entities;
using Doctorify.Infrastructure.Data.Repositories.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace Doctorify.Controllers;

public class PatientController : BaseApiController
{
    private readonly IPatientRepository _repository;
    private readonly IMapper _mapper;

    public PatientController(IMapper mapper, IPatientRepository repository)
    {
        _mapper = mapper;
        _repository = repository;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<PatientResponseDto>>> GetPatientsAsync()
    {
        var patients = await _repository.GetAllAsync();
        var patientsDto = _mapper.Map<IReadOnlyList<PatientResponseDto>>(patients);
        return Ok(patientsDto);
    }

    [HttpGet("{id:long}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<PatientResponseDto>> GetPatientByIdAsync(long id)
    {
        var patient = await _repository.GetByIdAsync(id);
        var patientDto = _mapper.Map<PatientResponseDto>(patient);
        return Ok(patientDto);
    }

    [HttpPost]
    public async Task<ActionResult<PatientResponseDto>> CreatePatientAsync(PatientRequestDto patientDto)
    {
        await using var transaction = await _repository.StartTransactionAsync();
        try
        {
            var patient = await _repository.Insert(_mapper.Map<Patient>(patientDto));
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
        await using var transaction = await _repository.StartTransactionAsync();
        try
        {
            var value = await _repository.BulkInsert(_mapper.Map<IReadOnlyList<Patient>>(patientDtoList));
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
        var validPatient = await _repository.GetByIdAsync(id);
        var result = validPatient ?? null;
        if (result is null)
            return NotFound();
        _mapper.Map(updateDto, result);
        await _repository.Update(result);
        return NoContent();
    }

    // [HttpPatch("{id:long}")]
    // public async Task<ActionResult> PartialUpdatePatient(long id, [FromBody] JsonPatchDocument<PatientRequestDto> jsonPatchDoc)
    // {
    //     var validPatient = await _repository.GetByIdAsync(id);
    //     var result = validPatient ?? null;
    //     if (result is null)
    //         return NotFound();
    //     var patientToPatch = _mapper.Map<PatientResponseDto>(result);
    //     jsonPatchDoc.ApplyTo(patientToPatch, ModelState);
    //     if (!TryValidateModel(patientToPatch))
    //         return ValidationProblem(ModelState);
    //     _mapper.Map(patientToPatch, validPatient);
    //     await _repository.Update(result);
    //     return NoContent();
    // }

    [HttpDelete("{id:long}")]
    public async Task<ActionResult<long>> DeletePatientByIdAsync(long id)
    {
        var patientToDelete = await _repository.GetByIdAsync(id);
        var result = patientToDelete ?? null;
        if (result is null)
        {
            return NotFound(id);
        }

        await _repository.Delete(id);
        return Ok(id);
    }
}