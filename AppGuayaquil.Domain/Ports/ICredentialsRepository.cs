using AppGuayaquil.Domain.Entities;

namespace AppGuayaquil.Domain.Ports;

public interface ICredentialsRepository
{
    Task<User> GetCredentialsAsync(string username, string password);
    Task<bool> AddUserCredentialAsync(User user);
}
