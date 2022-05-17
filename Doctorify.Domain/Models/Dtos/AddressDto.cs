﻿namespace Doctorify.Domain.Models.Dtos;

public class AddressDto
{
    public string Country { get; set; }
    public string State { get; set; }
    public string City { get; set; }
    public string Street { get; set; }
    public string PostalCode { get; set; }
}