using MediatR;

namespace AppGuayaquil.Application.People.Commands;

public class UpdatePeopleCommand : IRequest<bool>
{
    public Guid PeopleId { get; set; }
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string IdentificationNumber { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string IdentificationType { get; set; } = default!;
}
