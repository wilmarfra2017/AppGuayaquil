using MediatR;

namespace AppGuayaquil.Application.People.Queries
{
    public class GetPeopleByIdQuery : IRequest<Domain.Entities.People>
    {
        public Guid PeopleId { get; }

        public GetPeopleByIdQuery(Guid peopleId)
        {
            PeopleId = peopleId;
        }
    }
}
