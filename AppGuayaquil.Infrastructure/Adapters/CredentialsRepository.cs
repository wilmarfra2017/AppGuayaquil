using AppGuayaquil.Domain.Entities;
using AppGuayaquil.Domain.Ports;
using AppGuayaquil.Infrastructure.DataSource;
using Microsoft.EntityFrameworkCore;

namespace AppGuayaquil.Infrastructure.Adapters;

public class CredentialsRepository : ICredentialsRepository
{
    private readonly DataContext _context;

    public CredentialsRepository(DataContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }
    public async Task<User> GetCredentialsAsync(string username, string password)
    {
        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
        {
            throw new ArgumentException("Username and password NO pueden ser vacios");
        }

        var user = await _context.Users
        .AsNoTracking()
        .FirstOrDefaultAsync(u => u.UserName == username && u.Password == password);

        if (user == null)
        {
            throw new InvalidOperationException("No se encontró ningún usuario con las credenciales especificadas.");
        }

        return user;
    }

    public async Task<bool> AddUserCredentialAsync(User user)
    {
        if (user == null)
        {
            throw new ArgumentNullException(nameof(user), "El usuario no puede ser nulo.");
        }

        user.UserId = Guid.NewGuid();
        user.Id = Guid.NewGuid();
        user.CreatedOn = DateTime.UtcNow;
        user.LastModifiedOn = DateTime.UtcNow;

        _context.Users.Add(user);
        var result = await _context.SaveChangesAsync();
        return result > 0;
    }

}
