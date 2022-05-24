using AutoMapper;
using Doctorify.Domain.Models.Dtos;
using Doctorify.Domain.Models.Entities;
using Doctorify.Infrastructure.Data.Repositories.Abstractions;
using Doctorify.Infrastructure.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace Doctorify.Controllers;

public class DoctorController : BaseApiController
{
    private readonly IDoctorService _doctorService;
    private readonly ITelephoneService _telephoneService;
    private readonly IMedicalInstitutionService _medicalInstitutionService;
    private readonly IAddressService _addressService;
    private readonly IMapper _mapper;

    public DoctorController(IMapper mapper, IDoctorService doctorService, ITelephoneService telephoneService,
                            IMedicalInstitutionService medicalInstitutionService, IAddressService addressService)
    {
        _mapper = mapper;
        _doctorService = doctorService;
        _telephoneService = telephoneService;
        _medicalInstitutionService = medicalInstitutionService;
        _addressService = addressService;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<DoctorResponseDto>>> GetDoctorsAsync()
    {
        var doctors = await _doctorService.GetDoctorsAsync();
        var doctorsDto = _mapper.Map<IReadOnlyList<DoctorResponseDto>>(doctors);
        return Ok(doctorsDto);
    }

    [HttpGet("{id:long}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<DoctorResponseDto>> GetDoctorByIdAsync(long id)
    {
        var doctor = await _doctorService.GetDoctorByIdAsync(id);
        var doctorDto = _mapper.Map<DoctorResponseDto>(doctor);
        return Ok(doctorDto);
    }

    [HttpPost]
    public async Task<ActionResult<DoctorResponseDto>> CreateDoctorAsync(DoctorRequestDto doctorDto)
    {
        await using var transaction = await _doctorService.StartTransactionAsync();
        try
        {
            var doctor = await _doctorService.CreateDoctorAsync(_mapper.Map<Doctor>(doctorDto));
            var result = doctor.Entity;
            result.MedicalInstitution = await _medicalInstitutionService.GetMedicalInstitutionByIdAsync(result.MedicalInstitutionId);
            result.TelephoneNumber = await _telephoneService.GetTelephoneByIdAsync(result.TelephoneNumberId);
            await transaction.CommitAsync();
            var value = _mapper.Map<DoctorResponseDto>(doctor.Entity);
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
    public async Task<ActionResult<long>> CreateDoctorBatchAsync(IList<DoctorRequestDto> doctorDtoList)
    {
        await using var transaction = await _doctorService.StartTransactionAsync();
        try
        {
            var value = await _doctorService.CreateDoctorBatchAsync(_mapper.Map<IReadOnlyList<Doctor>>(doctorDtoList));
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
    public async Task<ActionResult> UpdateDoctorAsync(long id, DoctorRequestDto updateDto)
    {
        var validDoctor = await _doctorService.GetDoctorByIdAsync(id);
        var result = validDoctor ?? null;
        if (result is null)
            return NotFound();
        _mapper.Map(updateDto, result);
        await _doctorService.UpdateDoctorAsync(result);
        return NoContent();
    }

    // [HttpPatch("{id:long}")]
    // public async Task<ActionResult> PartialUpdateDoctor(long id, [FromBody] JsonPatchDocument<DoctorRequestDto> jsonPatchDoc)
    // {
    //     var validDoctor = await _repository.GetByIdAsync(id);
    //     var result = validDoctor ?? null;
    //     if (result is null)
    //         return NotFound();
    //     var doctorToPatch = _mapper.Map<DoctorResponseDto>(result);
    //     jsonPatchDoc.ApplyTo(doctorToPatch, ModelState);
    //     if (!TryValidateModel(doctorToPatch))
    //         return ValidationProblem(ModelState);
    //     _mapper.Map(doctorToPatch, validDoctor);
    //     await _repository.Update(result);
    //     return NoContent();
    // }

    [HttpDelete("{id:long}")]
    public async Task<ActionResult<long>> DeleteDoctorByIdAsync(long id)
    {
        var doctorToDelete = await _doctorService.GetDoctorByIdAsync(id);
        var result = doctorToDelete ?? null;
        if (result is null)
        {
            return NotFound(id);
        }

        await _doctorService.DeleteDoctorByIdAsync(id);
        return Ok(id);
    }
}