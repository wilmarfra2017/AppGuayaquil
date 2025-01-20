using AppGuayaquil.Application.People.Commands;
using FluentValidation;

namespace AppGuayaquil.Api.Validators;

public class UpdatePeopleCommandValidator : AbstractValidator<UpdatePeopleCommand>
{
    public UpdatePeopleCommandValidator()
    {
        RuleFor(x => x.FirstName)
           .NotEmpty().WithMessage("El campo 'FirstName' no puede estar vacío.")           
           .MaximumLength(50).WithMessage("El campo 'FirstName' no debe exceder los 50 caracteres.");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("El campo 'LastName' no puede estar vacío.")
            .MinimumLength(2).WithMessage("El campo 'LastName' debe tener al menos 2 caracteres.")
            .MaximumLength(50).WithMessage("El campo 'LastName' no debe exceder los 50 caracteres.");

        RuleFor(x => x.IdentificationNumber)
            .NotEmpty().WithMessage("El campo 'IdentificationNumber' no puede estar vacío.")
            .Matches(@"^\d+$").WithMessage("El campo 'IdentificationNumber' solo puede contener números.")
            .Length(8, 12).WithMessage("El campo 'IdentificationNumber' debe tener entre 8 y 12 caracteres.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("El campo 'Email' no puede estar vacío.")
            .EmailAddress().WithMessage("El campo 'Email' debe ser una dirección de correo electrónico válida.");

        RuleFor(x => x.IdentificationType)
            .NotEmpty().WithMessage("El campo 'IdentificationType' no puede estar vacío.");
    }
}
