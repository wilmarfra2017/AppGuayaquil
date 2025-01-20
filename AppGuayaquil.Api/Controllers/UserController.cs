using AppGuayaquil.Api.Dtos;
using AppGuayaquil.Api.Validators;
using AppGuayaquil.Application.Users;
using LogConfig;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AppGuayaquil.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class UserController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IConfiguration _configuration;

    public UserController(IMediator mediator, IConfiguration configuration)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequestDto)
    {
        ModelState.Clear();
        LoggerHelper.LogInfo("Método Login - UserController iniciado");

        if (loginRequestDto == null)
        {
            LoggerHelper.LogInfo("Solicitud de login inválida.");
            return BadRequest("Solicitud de login inválida.");
        }

        var validationResult = await new LoginDtoValidator().ValidateAsync(loginRequestDto);
        if (!validationResult.IsValid)
        {            
            return BadRequest(new { Errors = validationResult.Errors.Select(failure => failure.ErrorMessage) });
        }


        var user = await _mediator.Send(new GetUserCredentialsQuery(loginRequestDto.UserName, loginRequestDto.Password));

        var token = GenerateJwtToken(loginRequestDto.UserName);
        LoggerHelper.LogInfo($"Inicio de sesión exitoso para el usuario {loginRequestDto.UserName}.");
        return Ok(new { Token = token });
    }

    [HttpPost("add-user")]
    public async Task<IActionResult> AddUserCredential([FromBody] AddUserCredentialCommand addUserCredentialCommand)
    {
        LoggerHelper.LogInfo("Método AddUserCredential - UserController iniciado");

        if (addUserCredentialCommand == null)
        {
            LoggerHelper.LogInfo("Solicitud de agregar usuario inválida.");
            return BadRequest("Solicitud de agregar usuario inválida.");
        }

        var validationResult = await new AddUserCredentialCommandValidator().ValidateAsync(addUserCredentialCommand);
        if (!validationResult.IsValid)
        {
            return BadRequest(new { Errors = validationResult.Errors.Select(failure => failure.ErrorMessage) });
        }

        try
        {
            var result = await _mediator.Send(addUserCredentialCommand);

            if (!result)
            {
                LoggerHelper.LogError("Ocurrió un error al intentar agregar el usuario.", new Exception("Resultado de la operación fallido"));
                return StatusCode(500, "No se pudo agregar el usuario.");
            }

            LoggerHelper.LogInfo($"Usuario {addUserCredentialCommand.UserName} agregado exitosamente.");
            return Ok("Usuario agregado exitosamente.");
        }
        catch (ArgumentException ex)
        {
            LoggerHelper.LogError("Argumentos inválidos al intentar agregar usuario.", ex);
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            LoggerHelper.LogError("Error interno al intentar agregar usuario.", ex);
            return StatusCode(500, "Se produjo un error interno del servidor.");
        }
    }




    private string GenerateJwtToken(string userName)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtConfig:Key"]!));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, userName),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var tokenExpirationInHours = int.Parse(_configuration["JwtConfig:JwtTokenExpirationInHours"]!);

        var token = new JwtSecurityToken(
            issuer: _configuration["JwtConfig:Issuer"],
            audience: _configuration["JwtConfig:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(tokenExpirationInHours),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
