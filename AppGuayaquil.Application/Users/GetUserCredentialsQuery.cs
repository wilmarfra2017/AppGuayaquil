using MediatR;

namespace AppGuayaquil.Application.Users;

public class GetUserCredentialsQuery : IRequest<string>
{
    public string UserName { get; set; } = default!;
    public string Password { get; set; } = default!;

    public GetUserCredentialsQuery(string userName, string password)
    {
        UserName = userName;
        Password = password;
    }
}
