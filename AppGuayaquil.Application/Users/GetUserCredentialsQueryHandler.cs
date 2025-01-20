using AppGuayaquil.Application.Security;
using AppGuayaquil.Domain.Ports;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace AppGuayaquil.Application.Users;

public class GetUserCredentialsQueryHandler : IRequestHandler<GetUserCredentialsQuery, string>
{
    private readonly ICredentialsRepository _credentialsRepository;
    private readonly IConfiguration _configuration;

    public GetUserCredentialsQueryHandler(ICredentialsRepository credentialsRepository, IConfiguration configuration)
    {
        _credentialsRepository = credentialsRepository ?? throw new ArgumentNullException(nameof(credentialsRepository));
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));

    }

    public async Task<string> Handle(GetUserCredentialsQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request, nameof(request));

        var user = await _credentialsRepository.GetCredentialsAsync(request.UserName, request.Password);
        if (user == null)
        {
            throw new UnauthorizedAccessException("Las credenciales de usuario no son válidas o el acceso no está permitido.");
        }
        
        var token = TokenGenerator.GenerateJwtToken(user, _configuration);
        return token;
    }

}
