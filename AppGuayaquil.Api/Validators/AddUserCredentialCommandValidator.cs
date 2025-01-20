using AppGuayaquil.Application.Users;
using FluentValidation;

namespace AppGuayaquil.Api.Validators;

public class AddUserCredentialCommandValidator : AbstractValidator<AddUserCredentialCommand>
{

    public AddUserCredentialCommandValidator()
    {
        RuleFor(x => x.UserName)
           .NotEmpty().WithMessage("El campo 'UserName' no puede estar vacío.");

        RuleFor(x => x.Password)
           .NotEmpty().WithMessage("El campo 'Password' no puede estar vacío.");
    }
                
}
