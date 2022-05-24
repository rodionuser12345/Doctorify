using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Doctorify.Domain.Models.Auth;
using Doctorify.Domain.Models.Dtos.Identity;
using Doctorify.Infrastructure.Data.Repositories.Abstractions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Doctorify.Controllers.Auth;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IConfiguration _configuration;

    private readonly IAddressRepository _addressRepository;
    private readonly ITelephoneNumberRepository _telephoneNumberRepository;
    private readonly IPatientRepository _patientRepository;
    private readonly IMapper _mapper;

    public AuthController(
        UserManager<IdentityUser> userManager,
        RoleManager<IdentityRole> roleManager,
        IConfiguration configuration, IMapper mapper, IAddressRepository addressRepository,
        ITelephoneNumberRepository telephoneNumberRepository, IPatientRepository patientRepository)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _configuration = configuration;
        _mapper = mapper;
        _addressRepository = addressRepository;
        _telephoneNumberRepository = telephoneNumberRepository;
        _patientRepository = patientRepository;
    }

    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> Login([FromBody] LoginModel model)
    {
        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password)) return Unauthorized();

        var userRoles = await _userManager.GetRolesAsync(user);

        var authClaims = new List<Claim>
                         {
                             new(ClaimTypes.Name, user.Email),
                             new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                         };

        authClaims.AddRange(userRoles.Select(userRole => new Claim(ClaimTypes.Role, userRole)));

        var token = GetToken(authClaims);

        return Ok(new
                  {
                      token = new JwtSecurityTokenHandler().WriteToken(token),
                      expiration = token.ValidTo
                  });
    }

    [HttpPost]
    [Route("register-patient")]
    public async Task<IActionResult> RegisterPatient([FromBody] RegisterModelPatient model)
    {
        var userExists = await _userManager.FindByEmailAsync(model.Email);
        if (userExists != null)
            return StatusCode(StatusCodes.Status500InternalServerError,
                              new AuthResponseDto {Status = "Error", Message = "User already exists!"});

        IdentityUser user = new()
                            {
                                UserName = model.Email,
                                Email = model.Email,
                                SecurityStamp = Guid.NewGuid().ToString(),
                            };

        var result = await _userManager.CreateAsync(user, model.Password);
        if (!result.Succeeded)
            return StatusCode(StatusCodes.Status500InternalServerError,
                              new AuthResponseDto
                              {Status = "Error", Message = "User creation failed! Please check user details and try again."});

        var userRoles = await _userManager.GetRolesAsync(user);
        var authClaims = new List<Claim>
                         {
                             new(ClaimTypes.Name, user.Email),
                             new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                         };

        authClaims.AddRange(userRoles.Select(userRole => new Claim(ClaimTypes.Role, userRole)));

        var token = GetToken(authClaims).EncodedPayload;

        return Ok(new AuthResponseDto {Status = "Success", Message = "User created successfully!", Bearer = token});
    }

    [HttpPost]
    [Route("register-doctor")]
    public async Task<IActionResult> RegisterDoctor([FromBody] RegisterModelDoctor model)
    {
        var userExists = await _userManager.FindByEmailAsync(model.Email);
        if (userExists != null)
            return StatusCode(StatusCodes.Status500InternalServerError,
                              new AuthResponseDto {Status = "Error", Message = "User already exists!"});

        IdentityUser user = new()
                            {
                                UserName = model.Email,
                                Email = model.Email,
                                SecurityStamp = Guid.NewGuid().ToString(),
                            };

        var result = await _userManager.CreateAsync(user, model.Password);

        if (!result.Succeeded)
            return StatusCode(StatusCodes.Status500InternalServerError,
                              new AuthResponseDto
                              {Status = "Error", Message = "User creation failed! Please check user details and try again."});

        if (!await _roleManager.RoleExistsAsync(UserRoles.Doctor))
            await _roleManager.CreateAsync(new IdentityRole(UserRoles.Doctor));

        if (!await _roleManager.RoleExistsAsync(UserRoles.Patient))
            await _roleManager.CreateAsync(new IdentityRole(UserRoles.Patient));

        if (await _roleManager.RoleExistsAsync(UserRoles.Doctor))
        {
            await _userManager.AddToRoleAsync(user, UserRoles.Doctor);
        }

        if (await _roleManager.RoleExistsAsync(UserRoles.Doctor))
        {
            await _userManager.AddToRoleAsync(user, UserRoles.Patient);
        }

        var userRoles = await _userManager.GetRolesAsync(user);
        var authClaims = new List<Claim>
                         {
                             new(ClaimTypes.Name, user.Email),
                             new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                         };

        authClaims.AddRange(userRoles.Select(userRole => new Claim(ClaimTypes.Role, userRole)));

        var token = GetToken(authClaims).EncodedPayload;

        return Ok(new AuthResponseDto {Status = "Success", Message = "User created successfully!", Bearer = token});
    }

    private JwtSecurityToken GetToken(IEnumerable<Claim> authClaims)
    {
        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

        var token = new JwtSecurityToken(
            issuer: _configuration["JWT:ValidIssuer"],
            audience: _configuration["JWT:ValidAudience"],
            expires: DateTime.Now.AddHours(3),
            claims: authClaims,
            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
        );

        return token;
    }
}