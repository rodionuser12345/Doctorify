using AutoMapper;
using Doctorify.Domain.Models.Dtos;
using Doctorify.Domain.Models.Entities;
using Doctorify.Domain.Utils.Specifications;
using Doctorify.Infrastructure.Data.Repositories.Abstractions;
using Doctorify.Infrastructure.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace Doctorify.Controllers;

public class MedicalInstitutionController : BaseApiController
{
    private readonly IMedicalInstitutionService _medicalInstitutionService;
    private readonly ITelephoneService _telephoneService;
    private readonly IAddressService _addressService;
    private readonly IMapper _mapper;

    public MedicalInstitutionController(IMapper mapper, IMedicalInstitutionService medicalInstitutionService,
                                        ITelephoneService telephoneService,
                                        IAddressService addressService)
    {
        _mapper = mapper;
        _medicalInstitutionService = medicalInstitutionService;
        _telephoneService = telephoneService;
        _addressService = addressService;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<MedicalInstitutionResponseDto>>> GetMedicalInstitutionsAsync()
    {
        var medicalInstitutions = await _medicalInstitutionService.GetMedicalInstitutionsAsync();
        var medicalInstitutionsDto = _mapper.Map<IReadOnlyList<MedicalInstitutionResponseDto>>(medicalInstitutions);
        return Ok(medicalInstitutionsDto);
    }

    [HttpGet("{id:long}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<MedicalInstitutionResponseDto>> GetMedicalInstitutionByIdAsync(long id)
    {
        var medicalInstitution = await _medicalInstitutionService.GetMedicalInstitutionByIdAsync(id);
        var medicalInstitutionDto = _mapper.Map<MedicalInstitutionResponseDto>(medicalInstitution);
        return Ok(medicalInstitutionDto);
    }

    // [HttpGet("{id:long}/spec")]
    // [ProducesResponseType(StatusCodes.Status200OK)]
    // [ProducesResponseType(StatusCodes.Status404NotFound)]
    // public async Task<ActionResult<MedicalInstitutionResponseDto>> GetMedicalInstitutionByIdWithSpecAsync(long id)
    // {
    //     var spec = new MedInstWithTelNumAndAddressAndDoctors(id);
    //     var medicalInstitution = await _medicalInstitutionService.FindByIdWithSpecAsync(spec);
    //     if (medicalInstitution is null) return NotFound();
    //     return Ok(_mapper.Map<MedicalInstitutionResponseDto>(medicalInstitution));
    // }

    // [HttpGet("{country:alpha}")]
    // [ProducesResponseType(StatusCodes.Status200OK)]
    // public async Task<ActionResult<MedicalInstitutionResponseDto>> GetMedicalInstitutionByCountryAsync(string country)
    // {
    //     var medicalInstitution = await _medicalInstitutionService.GetByCountryAsync(country);
    //     var medicalInstitutionDto = _mapper.Map<MedicalInstitutionResponseDto>(medicalInstitution);
    //     return Ok(medicalInstitutionDto);
    // }

    // [HttpGet]
    // [Route("country/{country:alpha}")]
    // [ProducesResponseType(StatusCodes.Status200OK)]
    // public async Task<ActionResult<IReadOnlyList<MedicalInstitutionResponseDto>>> GetMedicalInstitutionsByCountryAsync(string country)
    // {
    //     var medicalInstitutions = await _medicalInstitutionService.GetAllByCountryAsync(country);
    //     var medicalInstitutionsDto = _mapper.Map<List<MedicalInstitutionResponseDto>>(medicalInstitutions);
    //     return Ok(medicalInstitutionsDto);
    // }

    [HttpPost]
    public async Task<ActionResult<MedicalInstitutionResponseDto>> CreateMedicalInstitutionAsync(
        MedicalInstitutionRequestDto medicalInstitutionDto)
    {
        await using var transaction = await _medicalInstitutionService.StartTransactionAsync();
        try
        {
            var medicalInstitution = await _medicalInstitutionService.CreateMedicalInstitutionAsync(_mapper.Map<MedicalInstitution>(medicalInstitutionDto));
            var result = medicalInstitution.Entity;
            result.Address = await _addressService.GetAddressByIdAsync(result.AddressId);
            result.TelephoneNumber = await _telephoneService.GetTelephoneByIdAsync(result.TelephoneNumberId);
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
    public async Task<ActionResult<int>> CreateMedicalInstitutionBatchAsync(
        IEnumerable<MedicalInstitutionRequestDto> medicalInstitutionDtoList)
    {
        await using var transaction = await _medicalInstitutionService.StartTransactionAsync();
        try
        {
            var value = await _medicalInstitutionService.CreateMedicalInstitutionBatchAsync(
                            _mapper.Map<IReadOnlyList<MedicalInstitution>>(medicalInstitutionDtoList));
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
        var validMedicalInstitution = await _medicalInstitutionService.GetMedicalInstitutionByIdAsync(id);
        var result = validMedicalInstitution ?? null;
        if (result is null)
            return NotFound();
        _mapper.Map(updateDto, result);
        await _medicalInstitutionService.UpdateMedicalInstitutionAsync(result);
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
        var medicalInstitutionToDelete = await _medicalInstitutionService.GetMedicalInstitutionByIdAsync(id);
        var result = medicalInstitutionToDelete ?? null;
        if (result is null)
        {
            return NotFound(id);
        }

        await _medicalInstitutionService.DeleteMedicalInstitutionByIdAsync(id);
        return Ok(id);
    }
}