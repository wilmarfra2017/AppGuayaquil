using AppGuayaquil.Domain.Ports;
using MediatR;

namespace AppGuayaquil.Application.People.Queries
{
    public class GetAllPeopleQueryHandler : IRequestHandler<GetAllPeopleQuery, List<Domain.Entities.People>>
    {
        private readonly IPeopleRepository _repository;

        public GetAllPeopleQueryHandler(IPeopleRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<List<Domain.Entities.People>> Handle(GetAllPeopleQuery request, CancellationToken cancellationToken)
        {
            return await _repository.GetPeopleAllAsync();
        }
    }
}
