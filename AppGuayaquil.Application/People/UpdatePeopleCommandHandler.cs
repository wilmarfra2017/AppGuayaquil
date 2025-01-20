using AppGuayaquil.Domain.Ports;
using MediatR;

namespace AppGuayaquil.Application.People.Commands;

public class UpdatePeopleCommandHandler : IRequestHandler<UpdatePeopleCommand, bool>
{
    private readonly IPeopleRepository _repository;

    public UpdatePeopleCommandHandler(IPeopleRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public async Task<bool> Handle(UpdatePeopleCommand request, CancellationToken cancellationToken)
    {
        var people = new Domain.Entities.People
        {
            PeopleId = request.PeopleId,
            FirstName = request.FirstName,
            LastName = request.LastName,
            IdentificationNumber = request.IdentificationNumber,
            Email = request.Email,
            IdentificationType = request.IdentificationType,
            LastModifiedOn = DateTime.UtcNow
        };

        return await _repository.UpdatePeopleAsync(people);
    }
}
