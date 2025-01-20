using AppGuayaquil.Domain.Entities;
using AppGuayaquil.Domain.Ports;
using MediatR;

namespace AppGuayaquil.Application.Users;

public class AddUserCredentialCommandHandler : IRequestHandler<AddUserCredentialCommand, bool>
{
    private readonly ICredentialsRepository _credentialsRepository;

    public AddUserCredentialCommandHandler(ICredentialsRepository credentialsRepository)
    {
        _credentialsRepository = credentialsRepository ?? throw new ArgumentNullException(nameof(credentialsRepository));
    }

    public async Task<bool> Handle(AddUserCredentialCommand request, CancellationToken cancellationToken)
    {        
        if (string.IsNullOrWhiteSpace(request.UserName) || string.IsNullOrWhiteSpace(request.Password))
        {
            throw new ArgumentException("El nombre de usuario y la contraseña no pueden estar vacíos.");
        }

        var user = new User
        {
            UserName = request.UserName,
            Password = request.Password,
            CreationDate = DateTime.UtcNow,
            Id = Guid.NewGuid()
        };

        return await _credentialsRepository.AddUserCredentialAsync(user);
    }
}
