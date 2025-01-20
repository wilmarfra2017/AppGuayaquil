using AppGuayaquil.Domain.Entities;
using AppGuayaquil.Domain.Ports;
using AppGuayaquil.Infrastructure.DataSource;
using Microsoft.EntityFrameworkCore;

namespace AppGuayaquil.Infrastructure.Adapters;

public class PeopleRepository : IPeopleRepository
{
    private readonly DataContext _context;

    public PeopleRepository(DataContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }
  
    public async Task<List<People>> GetPeopleAllAsync()
    {
        return await _context.Peoples
        .FromSqlRaw("EXEC GetAllPeople")
        .AsNoTracking()
        .ToListAsync();
    }
    
    public async Task<People> GetPeopleByIdAsync(Guid peopleId)
    {
        var people = await _context.Peoples.AsNoTracking().FirstOrDefaultAsync(p => p.PeopleId == peopleId);

        if (people == null)
        {
            throw new InvalidOperationException($"No se encontró la peoplea con ID {peopleId}");
        }

        return people;
    }
    
    public async Task<bool> AddPeopleAsync(People people)
    {
        if (people == null)
        {
            throw new ArgumentNullException(nameof(people), "La peoplea no puede ser nula.");
        }

        people.PeopleId = Guid.NewGuid();
        people.Id = Guid.NewGuid();
        people.CreatedOn = DateTime.UtcNow;
        people.LastModifiedOn = DateTime.UtcNow;

        _context.Peoples.Add(people);
        var result = await _context.SaveChangesAsync();
        return result > 0;
    }
    
    public async Task<bool> UpdatePeopleAsync(People people)
    {
        if (people == null)
        {
            throw new ArgumentNullException(nameof(people), "La peoplea no puede ser nula.");
        }

        var existingpeople = await _context.Peoples.FirstOrDefaultAsync(p => p.PeopleId == people.PeopleId);

        if (existingpeople == null)
        {
            throw new InvalidOperationException($"No se encontró la people con ID {people.PeopleId}");
        }

        existingpeople.FirstName = people.FirstName;
        existingpeople.LastName = people.LastName;
        existingpeople.IdentificationNumber = people.IdentificationNumber;
        existingpeople.Email = people.Email;
        existingpeople.IdentificationType = people.IdentificationType;
        existingpeople.LastModifiedOn = DateTime.UtcNow;

        _context.Peoples.Update(existingpeople);
        var result = await _context.SaveChangesAsync();
        return result > 0;
    }
    
    public async Task<bool> DeletePeopleAsync(Guid peopleId)
    {
        var people = await _context.Peoples.FirstOrDefaultAsync(p => p.PeopleId == peopleId);

        if (people == null)
        {
            throw new InvalidOperationException($"No se encontró la people con ID {peopleId}");
        }

        _context.Peoples.Remove(people);
        var result = await _context.SaveChangesAsync();
        return result > 0;
    }
}
