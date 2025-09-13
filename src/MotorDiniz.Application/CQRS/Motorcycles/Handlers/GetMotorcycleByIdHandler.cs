using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using MotorDiniz.Application.CQRS.Motorcycles.Queries;
using MotorDiniz.Application.DTOs.Motorcycle;
using MotorDiniz.Application.Extensions;
using MotorDiniz.Domain.Interfaces.Repository;

namespace MotorDiniz.Application.CQRS.Motorcycles.Handlers
{
    public sealed class GetMotorcycleByIdHandler : IRequestHandler<GetMotorcycleByIdQuery, MotorcycleViewDto?>
    {

        private readonly IMotorcycleRepository _motorcycleRepository;

        public GetMotorcycleByIdHandler(IMotorcycleRepository motorcycleRepository)
        {
            _motorcycleRepository = motorcycleRepository;
        }

        public async Task<MotorcycleViewDto?> Handle(GetMotorcycleByIdQuery request, CancellationToken cancellationToken)
        {
            var motorcycle = await _motorcycleRepository.GetByIdentifierAsync(request.Identifier, cancellationToken);

            return motorcycle.ToDto();
        }
    }
}
