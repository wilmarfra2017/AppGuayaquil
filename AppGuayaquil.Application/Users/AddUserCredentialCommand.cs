using MediatR;

namespace AppGuayaquil.Application.Users;

public class AddUserCredentialCommand : IRequest<bool>
{
    public string UserName { get; set; } = default!;
    public string Password { get; set; } = default!;
    public DateTime CreationDate { get; set; }

    public AddUserCredentialCommand(string userName, string password, DateTime creationDate)
    {
        UserName = userName;
        Password = password;
        CreationDate = creationDate;
    }
}
