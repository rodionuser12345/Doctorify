﻿using System.IdentityModel.Tokens.Jwt;

namespace Doctorify.Domain.Models.Dtos.Identity;

public class AuthResponseDto
{
    public string? Status { get; set; }
    public string? Message { get; set; }
    public string? Bearer { get; set; }
}