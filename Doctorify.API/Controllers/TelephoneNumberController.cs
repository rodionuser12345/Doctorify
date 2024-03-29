﻿using AutoMapper;
using Doctorify.Domain.Models.Dtos;
using Doctorify.Domain.Models.Entities;
using Doctorify.Infrastructure.Services.Abstractions;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage;

namespace Doctorify.Controllers;

public class TelephoneNumberController : BaseApiController
{
    private readonly ITelephoneService _telephoneService;
    private readonly IMapper _mapper;

    public TelephoneNumberController(IMapper mapper, ITelephoneService telephoneService)
    {
        _mapper = mapper;
        _telephoneService = telephoneService;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<TelephoneNumberDto>>> GetTelephonesAsync()
    {
        var telephones = _telephoneService.GetTelephonesAsync();
        var telephonesDto = _mapper.Map<IReadOnlyList<TelephoneNumberDto>>(telephones);
        return Ok(telephonesDto);
    }

    [HttpGet("{id:long}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<TelephoneNumberDto>> GetTelephoneByIdAsync(long id)
    {
        var telephone = await _telephoneService.GetTelephoneByIdAsync(id);
        var telephoneDto = _mapper.Map<TelephoneNumberDto>(telephone);
        return Ok(telephoneDto);
    }

    [HttpPost]
    public async Task<ActionResult<TelephoneNumberDto>> CreateTelephoneAsync(TelephoneNumberDto telephoneDto)
    {
        await using var transaction = await _telephoneService.StartTransactionAsync();
        try
        {
            var telephone = await _telephoneService.CreateTelephoneAsync(_mapper.Map<TelephoneNumber>(telephoneDto));
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
        await using var transaction = await _telephoneService.StartTransactionAsync();
        try
        {
            var value = await _telephoneService.CreateTelephoneBatchAsync(_mapper.Map<IEnumerable<TelephoneNumber>>(telephoneDtoList));
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
        var validTelephone = await _telephoneService.GetTelephoneByIdAsync(id);
        var result = validTelephone ?? null;
        if (result is null)
            return NotFound();
        _mapper.Map(updateDto, result);
        await _telephoneService.UpdateTelephoneAsync(result);
        return NoContent();
    }

    [HttpPatch("{id:long}")]
    public async Task<ActionResult> PartialUpdateTelephoneAsync(long id, [FromBody] JsonPatchDocument<TelephoneNumberDto> jsonPatchDoc)
    {
        var validTelephone = await _telephoneService.GetTelephoneByIdAsync(id);
        var result = validTelephone ?? null;
        if (result is null)
            return NotFound();
        var telephoneNumberToPatch = _mapper.Map<TelephoneNumberDto>(result);
        jsonPatchDoc.ApplyTo(telephoneNumberToPatch, ModelState);
        if (!TryValidateModel(telephoneNumberToPatch))
            return ValidationProblem(ModelState);
        _mapper.Map(telephoneNumberToPatch, validTelephone);
        await _telephoneService.UpdateTelephoneAsync(result);
        return NoContent();
    }

    [HttpDelete("{id:long}")]
    public async Task<ActionResult<long>> DeleteTelephoneByIdAsync(long id)
    {
        var telephoneToDelete = await _telephoneService.GetTelephoneByIdAsync(id);
        var result = telephoneToDelete ?? null;
        if (result is null)
        {
            return NotFound(id);
        }

        await _telephoneService.DeleteTelephoneByIdAsync(id);
        return Ok(id);
    }
}