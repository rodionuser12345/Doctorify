using AutoMapper;
using Doctorify.Domain.Models.Dtos;
using Doctorify.Domain.Models.Entities;
using Doctorify.Infrastructure.Data.Repositories.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace Doctorify.Controllers;

public class DoctorController : BaseApiController
{
    private readonly IDoctorRepository _repository;
    private readonly ITelephoneNumberRepository _telRepository;
    private readonly IMedicalInstitutionRepository _medRepository;
    private readonly IAddressRepository _adrRepository;
    private readonly IMapper _mapper;

    public DoctorController(IMapper mapper, IDoctorRepository repository, ITelephoneNumberRepository telRepository,
                            IMedicalInstitutionRepository medRepository, IAddressRepository adrRepository)
    {
        _mapper = mapper;
        _repository = repository;
        _telRepository = telRepository;
        _medRepository = medRepository;
        _adrRepository = adrRepository;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<DoctorResponseDto>>> GetDoctorsAsync()
    {
        var doctors = await _repository.GetAllAsync();
        var doctorsDto = _mapper.Map<IReadOnlyList<DoctorResponseDto>>(doctors);
        return Ok(doctorsDto);
    }

    [HttpGet("{id:long}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<DoctorResponseDto>> GetDoctorByIdAsync(long id)
    {
        var doctor = await _repository.GetByIdAsync(id);
        var doctorDto = _mapper.Map<DoctorResponseDto>(doctor);
        return Ok(doctorDto);
    }

    [HttpPost]
    public async Task<ActionResult<DoctorResponseDto>> CreateDoctorAsync(DoctorRequestDto doctorDto)
    {
        await using var transaction = await _repository.StartTransactionAsync();
        try
        {
            var doctor = await _repository.Insert(_mapper.Map<Doctor>(doctorDto));
            var result = doctor.Entity;
            result.MedicalInstitution = await _medRepository.GetByIdAsync(result.MedicalInstitutionId);
            result.TelephoneNumber = await _telRepository.GetByIdAsync(result.TelephoneNumberId);
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
        await using var transaction = await _repository.StartTransactionAsync();
        try
        {
            var value = await _repository.BulkInsert(_mapper.Map<IReadOnlyList<Doctor>>(doctorDtoList));
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
        var validDoctor = await _repository.GetByIdAsync(id);
        var result = validDoctor ?? null;
        if (result is null)
            return NotFound();
        _mapper.Map(updateDto, result);
        await _repository.Update(result);
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
        var doctorToDelete = await _repository.GetByIdAsync(id);
        var result = doctorToDelete ?? null;
        if (result is null)
        {
            return NotFound(id);
        }

        await _repository.Delete(id);
        return Ok(id);
    }
}