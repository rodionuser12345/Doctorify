using AutoMapper;
using Doctorify.Domain.Models.Dtos;
using Doctorify.Domain.Models.Entities;
using Doctorify.Infrastructure.Data.Repositories.Abstractions;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace Doctorify.Controllers;

public class AddressController : BaseApiController
{
    private readonly IAddressRepository _repository;
    private readonly IMapper _mapper;

    public AddressController(IMapper mapper, IAddressRepository repository)
    {
        _mapper = mapper;
        _repository = repository;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<AddressDto>>> GetAddressesAsync()
    {
        var addresses = await _repository.GetAllAsync();
        var addressesDto = _mapper.Map<IReadOnlyList<AddressDto>>(addresses);
        return Ok(addressesDto);
    }

    [HttpGet("{id:long}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<AddressDto>> GetAddressByIdAsync(long id)
    {
        var address = await _repository.GetByIdAsync(id);
        var addressDto = _mapper.Map<AddressDto>(address);
        return Ok(addressDto);
    }

    [HttpPost]
    public async Task<ActionResult<AddressDto>> CreateAddressAsync(AddressDto addressDto)
    {
        await using var transaction = await _repository.StartTransactionAsync();
        try
        {
            var address = await _repository.Insert(_mapper.Map<Address>(addressDto));
            await transaction.CommitAsync();
            var value = _mapper.Map<AddressDto>(address.Entity);
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
    public async Task<ActionResult<long>> CreateAddressBatchAsync(IList<AddressDto> addressDtoList)
    {
        await using var transaction = await _repository.StartTransactionAsync();
        try
        {
            var value = await _repository.BulkInsert(_mapper.Map<IReadOnlyList<Address>>(addressDtoList));
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
    public async Task<ActionResult> UpdateAddressAsync(long id, AddressDto updateDto)
    {
        var validAddress = await _repository.GetByIdAsync(id);
        var result = validAddress ?? null;
        if (result is null)
            return NotFound();
        _mapper.Map(updateDto, result);
        await _repository.Update(result);
        return NoContent();
    }

    [HttpPatch("{id:long}")]
    public async Task<ActionResult> PartialUpdateAddressAsync(long id, [FromBody] JsonPatchDocument<AddressDto> jsonPatchDoc)
    {
        var validAddress = await _repository.GetByIdAsync(id);
        var result = validAddress ?? null;
        if (result is null)
            return NotFound();
        var addressToPatch = _mapper.Map<AddressDto>(result);
        jsonPatchDoc.ApplyTo(addressToPatch, ModelState);
        if (!TryValidateModel(addressToPatch))
            return ValidationProblem(ModelState);
        _mapper.Map(addressToPatch, validAddress);
        await _repository.Update(result);
        return NoContent();
    }

    [HttpDelete("{id:long}")]
    public async Task<ActionResult<long>> DeleteAddressByIdAsync(long id)
    {
        var addressToDelete = await _repository.GetByIdAsync(id);
        var result = addressToDelete ?? null;
        if (result is null)
        {
            return NotFound(id);
        }

        await _repository.Delete(id);
        return Ok(id);
    }
}