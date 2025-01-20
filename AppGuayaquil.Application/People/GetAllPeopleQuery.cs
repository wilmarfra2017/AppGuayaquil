using MediatR;

namespace AppGuayaquil.Application.People.Queries
{
    public class GetAllPeopleQuery : IRequest<List<Domain.Entities.People>>
    {
    }
}
