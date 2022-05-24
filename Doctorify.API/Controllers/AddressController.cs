using AutoMapper;
using Doctorify.Domain.Models.Dtos;
using Doctorify.Domain.Models.Entities;
using Doctorify.Infrastructure.Data.Repositories.Abstractions;
using Doctorify.Infrastructure.Services.Abstractions;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace Doctorify.Controllers;

public class AddressController : BaseApiController
{
    private readonly IAddressService _addressService;
    private readonly IMapper _mapper;

    public AddressController(IMapper mapper, IAddressService service)
    {
        _mapper = mapper;
        _addressService = service;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<AddressDto>>> GetAddressesAsync()
    {
        var addresses = await _addressService.GetAddressesAsync();
        var addressesDto = _mapper.Map<IReadOnlyList<AddressDto>>(addresses);
        return Ok(addressesDto);
    }

    [HttpGet("{id:long}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<AddressDto>> GetAddressByIdAsync(long id)
    {
        var address = await _addressService.GetAddressByIdAsync(id);
        var addressDto = _mapper.Map<AddressDto>(address);
        return Ok(addressDto);
    }

    [HttpPost]
    public async Task<ActionResult<AddressDto>> CreateAddressAsync(AddressDto addressDto)
    {
        await using var transaction = await _addressService.StartTransactionAsync();
        try
        {
            var address = await _addressService.CreateAddressAsync(_mapper.Map<Address>(addressDto));
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
        await using var transaction = await _addressService.StartTransactionAsync();
        try
        {
            var value = await _addressService.CreateAddressBatchAsync(_mapper.Map<IReadOnlyList<Address>>(addressDtoList));
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
        var validAddress = await _addressService.GetAddressByIdAsync(id);
        var result = validAddress ?? null;
        if (result is null)
            return NotFound();
        _mapper.Map(updateDto, result);
        await _addressService.UpdateAddressAsync(result);
        return NoContent();
    }

    [HttpPatch("{id:long}")]
    public async Task<ActionResult> PartialUpdateAddressAsync(long id, [FromBody] JsonPatchDocument<AddressDto> jsonPatchDoc)
    {
        var validAddress = await _addressService.GetAddressByIdAsync(id);
        var result = validAddress ?? null;
        if (result is null)
            return NotFound();
        var addressToPatch = _mapper.Map<AddressDto>(result);
        jsonPatchDoc.ApplyTo(addressToPatch, ModelState);
        if (!TryValidateModel(addressToPatch))
            return ValidationProblem(ModelState);
        _mapper.Map(addressToPatch, validAddress);
        await _addressService.UpdateAddressAsync(result);
        return NoContent();
    }

    [HttpDelete("{id:long}")]
    public async Task<ActionResult<long>> DeleteAddressByIdAsync(long id)
    {
        var addressToDelete = await _addressService.GetAddressByIdAsync(id);
        var result = addressToDelete ?? null;
        if (result is null)
        {
            return NotFound(id);
        }

        await _addressService.DeleteAddressByIdAsync(id);
        return Ok(id);
    }
}