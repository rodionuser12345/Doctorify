using AutoMapper;
using Doctorify.Domain.Models.Dtos;
using Doctorify.Domain.Models.Entities;
using Doctorify.Domain.Utils.Specifications;
using Doctorify.Infrastructure.Data.Repositories.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace Doctorify.Controllers;

public class MedicalInstitutionController : BaseApiController
{
    private readonly IMedicalInstitutionRepository _repository;
    private readonly ITelephoneNumberRepository _telRepository;
    private readonly IAddressRepository _adrRepository;
    private readonly IMapper _mapper;

    public MedicalInstitutionController(IMapper mapper, IMedicalInstitutionRepository repository, ITelephoneNumberRepository telRepository,
                                        IAddressRepository adrRepository)
    {
        _mapper = mapper;
        _repository = repository;
        _telRepository = telRepository;
        _adrRepository = adrRepository;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<MedicalInstitutionResponseDto>>> GetMedicalInstitutionsAsync()
    {
        var medicalInstitutions = await _repository.GetAllAsync();
        var medicalInstitutionsDto = _mapper.Map<IReadOnlyList<MedicalInstitutionResponseDto>>(medicalInstitutions);
        return Ok(medicalInstitutionsDto);
    }
    
    [HttpGet("{id:long}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<MedicalInstitutionResponseDto>> GetMedicalInstitutionByIdAsync(long id)
    {
        var medicalInstitution = await _repository.GetByIdAsync(id);
        var medicalInstitutionDto = _mapper.Map<MedicalInstitutionResponseDto>(medicalInstitution);
        return Ok(medicalInstitutionDto);
    }

    [HttpGet("{id:long}/spec")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<MedicalInstitutionResponseDto>> GetMedicalInstitutionByIdWithSpecAsync(long id)
    {
        var spec = new MedInstWithTelNumAndAddressAndDoctors(id);
        var medicalInstitution = await _repository.FindByIdWithSpecAsync(spec);
        if (medicalInstitution is null) return NotFound();
        return Ok(_mapper.Map<MedicalInstitutionResponseDto>(medicalInstitution));
    }

    [HttpGet("{country:alpha}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<MedicalInstitutionResponseDto>> GetMedicalInstitutionByCountryAsync(string country)
    {
        var medicalInstitution = await _repository.GetByCountryAsync(country);
        var medicalInstitutionDto = _mapper.Map<MedicalInstitutionResponseDto>(medicalInstitution);
        return Ok(medicalInstitutionDto);
    }

    [HttpGet]
    [Route("country/{country:alpha}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<MedicalInstitutionResponseDto>>> GetMedicalInstitutionsByCountryAsync(string country)
    {
        var medicalInstitutions = await _repository.GetAllByCountryAsync(country);
        var medicalInstitutionsDto = _mapper.Map<List<MedicalInstitutionResponseDto>>(medicalInstitutions);
        return Ok(medicalInstitutionsDto);
    }

    // POST

    [HttpPost]
    public async Task<ActionResult<MedicalInstitutionResponseDto>> CreateMedicalInstitutionAsync(
        MedicalInstitutionRequestDto medicalInstitutionDto)
    {
        await using var transaction = await _repository.StartTransactionAsync();
        try
        {
            var medicalInstitution = await _repository.Insert(_mapper.Map<MedicalInstitution>(medicalInstitutionDto));
            var result = medicalInstitution.Entity;
            result.Address = await _adrRepository.GetByIdAsync(result.AddressId);
            result.TelephoneNumber = await _telRepository.GetByIdAsync(result.TelephoneNumberId);
            await transaction.CommitAsync();
            var value = _mapper.Map<MedicalInstitutionResponseDto>(medicalInstitution.Entity);
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
    public async Task<ActionResult<int>> CreateMedicalInstitutionBatchAsync(IEnumerable<MedicalInstitutionRequestDto> medicalInstitutionDtoList)
    {
        await using var transaction = await _repository.StartTransactionAsync();
        try
        {
            var value = await _repository.BulkInsert(_mapper.Map<IReadOnlyList<MedicalInstitution>>(medicalInstitutionDtoList));
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
    public async Task<ActionResult> UpdateMedicalInstitutionAsync(long id, MedicalInstitutionRequestDto updateDto)
    {
        var validMedicalInstitution = await _repository.GetByIdAsync(id);
        var result = validMedicalInstitution ?? null;
        if (result is null)
            return NotFound();
        _mapper.Map(updateDto, result);
        await _repository.Update(result);
        return NoContent();
    }

    // [HttpPatch("{id:long}")]
    // public async Task<ActionResult> PartialUpdateMedicalInstitution(
    //     long id, [FromBody] JsonPatchDocument<MedicalInstitutionRequestDto> jsonPatchDoc)
    // {
    //     var validMedicalInstitution = await _repository.GetByIdAsync(id);
    //     var result = validMedicalInstitution ?? null;
    //     if (result is null)
    //         return NotFound();
    //     var medicalInstitutionToPatch = _mapper.Map<MedicalInstitutionResponseDto>(result);
    //     jsonPatchDoc.ApplyTo(medicalInstitutionToPatch, ModelState);
    //     if (!TryValidateModel(medicalInstitutionToPatch))
    //         return ValidationProblem(ModelState);
    //     _mapper.Map(medicalInstitutionToPatch, validMedicalInstitution);
    //     await _repository.Update(result);
    //     return NoContent();
    // }

    [HttpDelete("{id:long}")]
    public async Task<ActionResult<long>> DeleteMedicalInstitutionByIdAsync(long id)
    {
        var medicalInstitutionToDelete = await _repository.GetByIdAsync(id);
        var result = medicalInstitutionToDelete ?? null;
        if (result is null)
        {
            return NotFound(id);
        }

        await _repository.Delete(id);
        return Ok(id);
    }
}