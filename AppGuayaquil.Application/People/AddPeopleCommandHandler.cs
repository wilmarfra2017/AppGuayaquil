using AppGuayaquil.Domain.Ports;
using MediatR;

namespace AppGuayaquil.Application.People.Commands;

public class AddPeopleCommandHandler : IRequestHandler<AddPeopleCommand, bool>
{
    private readonly IPeopleRepository _repository;

    public AddPeopleCommandHandler(IPeopleRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public async Task<bool> Handle(AddPeopleCommand request, CancellationToken cancellationToken)
    {
        var people = new Domain.Entities.People
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            IdentificationNumber = request.IdentificationNumber,
            Email = request.Email,
            IdentificationType = request.IdentificationType,
            CreationDate = DateTime.UtcNow
        };

        return await _repository.AddPeopleAsync(people);
    }
}
