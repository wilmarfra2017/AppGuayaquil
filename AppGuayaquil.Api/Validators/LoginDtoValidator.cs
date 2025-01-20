using AppGuayaquil.Api.Dtos;
using FluentValidation;

namespace AppGuayaquil.Api.Validators;

public class LoginDtoValidator : AbstractValidator<LoginRequestDto>
{
    public LoginDtoValidator()
    {
        RuleFor(x => x.UserName).NotEmpty().WithMessage("UserName No puede ser vacio");
        RuleFor(x => x.Password).NotEmpty().WithMessage("Password No puede ser vacio");
    }
}
