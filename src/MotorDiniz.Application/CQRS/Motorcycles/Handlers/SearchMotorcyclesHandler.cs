using MediatR;
using MotorDiniz.Application.CQRS.Motorcycles.Queries;
using MotorDiniz.Application.DTOs.Motorcycle;
using MotorDiniz.Application.Extensions;
using MotorDiniz.Domain.Interfaces.Repository;

namespace MotorDiniz.Application.CQRS.Motorcycles.Handlers
{
    public sealed class SearchMotorcyclesHandler : IRequestHandler<SearchMotorcyclesQuery, IReadOnlyList<MotorcycleViewDto>>
    {
        private readonly IMotorcycleRepository _motorcycleRepository;

        public SearchMotorcyclesHandler(IMotorcycleRepository motorcycleRepository)
        {
            _motorcycleRepository = motorcycleRepository;
        }
        public async Task<IReadOnlyList<MotorcycleViewDto>> Handle(SearchMotorcyclesQuery request, CancellationToken cancellationToken)
        {
            var list = await _motorcycleRepository.SearchAsync(request.Plate, cancellationToken);

            return list.ToDto().ToList();
        }
    }
}
