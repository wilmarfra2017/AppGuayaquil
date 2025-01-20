using MediatR;

namespace AppGuayaquil.Application.People.Commands;

public class DeletePeopleCommand : IRequest<bool>
{
    public Guid PeopleId { get; set; }

    public DeletePeopleCommand(Guid peopleId)
    {
        PeopleId = peopleId;
    }
}
