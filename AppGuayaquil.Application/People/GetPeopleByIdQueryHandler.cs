using AppGuayaquil.Domain.Entities;
using AppGuayaquil.Domain.Ports;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace AppGuayaquil.Application.People.Queries
{
    public class GetPeopleByIdQueryHandler : IRequestHandler<GetPeopleByIdQuery, Domain.Entities.People>
    {
        private readonly IPeopleRepository _repository;

        public GetPeopleByIdQueryHandler(IPeopleRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<Domain.Entities.People> Handle(GetPeopleByIdQuery request, CancellationToken cancellationToken)
        {
            return await _repository.GetPeopleByIdAsync(request.PeopleId);
        }
    }
}
