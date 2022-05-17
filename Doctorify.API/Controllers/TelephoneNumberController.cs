using AutoMapper;
using Doctorify.Domain.Models.Dtos;
using Doctorify.Domain.Models.Entities;
using Doctorify.Infrastructure.Data.Repositories.Abstractions;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace Doctorify.Controllers;

public class TelephoneNumberController : BaseApiController
{
    private readonly ITelephoneNumberRepository _repository;
    private readonly IMapper _mapper;

    public TelephoneNumberController(IMapper mapper, ITelephoneNumberRepository repository)
    {
        _mapper = mapper;
        _repository = repository;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<TelephoneNumberDto>>> GetTelephonesAsync()
    {
        var telephones = await _repository.GetAllAsync();
        var telephonesDto = _mapper.Map<IReadOnlyList<TelephoneNumberDto>>(telephones);
        return Ok(telephonesDto);
    }

    [HttpGet("{id:long}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<TelephoneNumberDto>> GetTelephoneByIdAsync(long id)
    {
        var telephone = await _repository.GetByIdAsync(id);
        var telephoneDto = _mapper.Map<TelephoneNumberDto>(telephone);
        return Ok(telephoneDto);
    }

    [HttpPost]
    public async Task<ActionResult<TelephoneNumberDto>> CreateTelephoneAsync(TelephoneNumberDto telephoneDto)
    {
        await using var transaction = await _repository.StartTransactionAsync();
        try
        {
            var telephone = await _repository.Insert(_mapper.Map<TelephoneNumber>(telephoneDto));
            await transaction.CommitAsync();
            var value = _mapper.Map<TelephoneNumberDto>(telephone.Entity);
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
    public async Task<ActionResult<long>> CreateTelephoneBatchAsync(IList<TelephoneNumberDto> telephoneDtoList)
    {
        await using var transaction = await _repository.StartTransactionAsync();
        try
        {
            var value = await _repository.BulkInsert(_mapper.Map<IReadOnlyList<TelephoneNumber>>(telephoneDtoList));
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
    public async Task<ActionResult> UpdateTelephoneAsync(long id, TelephoneNumberDto updateDto)
    {
        var validTelephone = await _repository.GetByIdAsync(id);
        var result = validTelephone ?? null;
        if (result is null)
            return NotFound();
        _mapper.Map(updateDto, result);
        await _repository.Update(result);
        return NoContent();
    }

    [HttpPatch("{id:long}")]
    public async Task<ActionResult> PartialUpdateTelephoneAsync(long id, [FromBody] JsonPatchDocument<TelephoneNumberDto> jsonPatchDoc)
    {
        var validTelephone = await _repository.GetByIdAsync(id);
        var result = validTelephone ?? null;
        if (result is null)
            return NotFound();
        var telephoneNumberToPatch = _mapper.Map<TelephoneNumberDto>(result);
        jsonPatchDoc.ApplyTo(telephoneNumberToPatch, ModelState);
        if (!TryValidateModel(telephoneNumberToPatch))
            return ValidationProblem(ModelState);
        _mapper.Map(telephoneNumberToPatch, validTelephone);
        await _repository.Update(result);
        return NoContent();
    }

    [HttpDelete("{id:long}")]
    public async Task<ActionResult<long>> DeleteTelephoneByIdAsync(long id)
    {
        var telephoneToDelete = await _repository.GetByIdAsync(id);
        var result = telephoneToDelete ?? null;
        if (result is null)
        {
            return NotFound(id);
        }

        await _repository.Delete(id);
        return Ok(id);
    }
}