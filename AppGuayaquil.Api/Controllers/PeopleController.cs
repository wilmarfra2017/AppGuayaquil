using AppGuayaquil.Api.Validators;
using AppGuayaquil.Application.People.Commands;
using AppGuayaquil.Application.People.Queries;
using LogConfig;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AppGuayaquil.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class PeopleController : ControllerBase
{
    private readonly IMediator _mediator;    

    public PeopleController(IMediator mediator)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));        
    }

    [HttpGet("all")]
    public async Task<IActionResult> GetAllPeople()
    {
        LoggerHelper.LogInfo("Método GetAllPeople - PeopleController iniciado");

        try
        {
            var peopleList = await _mediator.Send(new GetAllPeopleQuery());
            var response = peopleList.Select(p => new
            {
                p.PeopleId,
                p.FirstName,
                p.LastName,
                p.IdentificationNumber,
                p.Email,
                p.IdentificationType,
                p.CreationDate,
                p.Id,
                p.CreatedOn,
                p.LastModifiedOn,
                p.FullIdentification,
                p.FullName
            });

            return Ok(response);
        }
        catch (Exception ex)
        {
            LoggerHelper.LogError("Error interno al intentar obtener todas las Peopleas.", ex);
            return StatusCode(500, "Se produjo un error interno del servidor.");
        }
    }


    [HttpGet("{id}")]
    public async Task<IActionResult> GetPeopleById(Guid id)
    {
        LoggerHelper.LogInfo($"Método GetPeopleById - PeopleController iniciado para ID {id}");

        try
        {
            var people = await _mediator.Send(new GetPeopleByIdQuery(id));

            var response = new
            {
                people.PeopleId,
                people.FirstName,
                people.LastName,
                people.IdentificationNumber,
                people.Email,
                people.IdentificationType,
                people.CreationDate,
                people.Id,
                people.CreatedOn,
                people.LastModifiedOn,
                people.FullIdentification,
                people.FullName
            };

            return Ok(response);
        }
        catch (InvalidOperationException ex)
        {
            LoggerHelper.LogError($"Peoplea no encontrada con ID {id}.", ex);
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            LoggerHelper.LogError("Error interno al intentar obtener una Peoplea por ID.", ex);
            return StatusCode(500, "Se produjo un error interno del servidor.");
        }
    }



    [HttpPost("add")]
    public async Task<IActionResult> AddPeople([FromBody] AddPeopleCommand command)
    {
        LoggerHelper.LogInfo("Método AddPeople - PeopleController iniciado");

        if (command == null)
        {
            LoggerHelper.LogInfo("Solicitud de agregar Peoplea inválida.");
            return BadRequest("Solicitud de agregar Peoplea inválida.");
        }

        var validationResult = await new AddPeopleCommandValidator().ValidateAsync(command);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors.Select(failure => failure.ErrorMessage));
        }

        try
        {
            var result = await _mediator.Send(command);

            if (!result)
            {
                LoggerHelper.LogError("Ocurrió un error al intentar agregar la Peoplea.", new Exception("Resultado de la operación fallido"));
                return StatusCode(500, "No se pudo agregar la Peoplea.");
            }

            LoggerHelper.LogInfo($"Peoplea {command.FirstName} {command.LastName} agregada exitosamente.");
            return Ok("Peoplea agregada exitosamente.");
        }
        catch (ArgumentException ex)
        {
            LoggerHelper.LogError("Argumentos inválidos al intentar agregar una Peoplea.", ex);
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            LoggerHelper.LogError("Error interno al intentar agregar una Peoplea.", ex);
            return StatusCode(500, "Se produjo un error interno del servidor.");
        }
    }

    [HttpPut("update")]
    public async Task<IActionResult> UpdatePeople([FromBody] UpdatePeopleCommand command)
    {
        LoggerHelper.LogInfo("Método UpdatePeople - PeopleController iniciado");

        if (command == null)
        {
            LoggerHelper.LogInfo("Solicitud de actualizar Peoplea inválida.");
            return BadRequest("Solicitud de actualizar Peoplea inválida.");
        }

        var validationResult = await new UpdatePeopleCommandValidator().ValidateAsync(command);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors.Select(failure => failure.ErrorMessage));
        }

        try
        {
            var result = await _mediator.Send(command);

            if (!result)
            {
                LoggerHelper.LogError("Ocurrió un error al intentar actualizar la Peoplea.", new Exception("Resultado de la operación fallido"));
                return StatusCode(500, "No se pudo actualizar la Peoplea.");
            }

            LoggerHelper.LogInfo($"Peoplea {command.FirstName} {command.LastName} actualizada exitosamente.");
            return Ok("Peoplea actualizada exitosamente.");
        }
        catch (InvalidOperationException ex)
        {
            LoggerHelper.LogError("Peoplea no encontrada al intentar actualizar.", ex);
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            LoggerHelper.LogError("Error interno al intentar actualizar una People.", ex);
            return StatusCode(500, "Se produjo un error interno del servidor.");
        }
    }

    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> DeletePeople(Guid id)
    {
        LoggerHelper.LogInfo($"Método DeletePeople - PeopleController iniciado para ID {id}");

        if (id == Guid.Empty)
        {
            LoggerHelper.LogInfo("ID inválido proporcionado para la eliminación.");
            return BadRequest("El ID proporcionado no es válido.");
        }

        try
        {
            var result = await _mediator.Send(new DeletePeopleCommand(id));

            if (!result)
            {
                LoggerHelper.LogError($"Error al intentar eliminar la Peoplea con ID {id}.", new Exception("Resultado de la operación fallido"));
                return StatusCode(500, "No se pudo eliminar la Peoplea.");
            }

            LoggerHelper.LogInfo($"Peoplea con ID {id} eliminada exitosamente.");
            return Ok("Peoplea eliminada exitosamente.");
        }
        catch (InvalidOperationException ex)
        {
            LoggerHelper.LogError($"Peoplea no encontrada al intentar eliminar con ID {id}.", ex);
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            LoggerHelper.LogError("Error interno al intentar eliminar una Peoplea.", ex);
            return StatusCode(500, "Se produjo un error interno del servidor.");
        }
    }
}
