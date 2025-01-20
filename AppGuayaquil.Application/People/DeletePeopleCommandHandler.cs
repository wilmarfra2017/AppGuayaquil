using AppGuayaquil.Domain.Ports;
using MediatR;

namespace AppGuayaquil.Application.People.Commands;

public class DeletePeopleCommandHandler : IRequestHandler<DeletePeopleCommand, bool>
{
    private readonly IPeopleRepository _repository;

    public DeletePeopleCommandHandler(IPeopleRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public async Task<bool> Handle(DeletePeopleCommand request, CancellationToken cancellationToken)
    {
        return await _repository.DeletePeopleAsync(request.PeopleId);
    }
}
