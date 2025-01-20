using AppGuayaquil.Domain.Entities;

namespace AppGuayaquil.Domain.Ports;

public interface IPeopleRepository
{
    Task<List<People>> GetPeopleAllAsync();
    Task<People> GetPeopleByIdAsync(Guid peopleId);
    Task<bool> AddPeopleAsync(People people);
    Task<bool> UpdatePeopleAsync(People people);
    Task<bool> DeletePeopleAsync(Guid peopleId);
}
