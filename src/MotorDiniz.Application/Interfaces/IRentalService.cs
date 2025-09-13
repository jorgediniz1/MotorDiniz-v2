using MotorDiniz.Application.DTOs.Rental;

namespace MotorDiniz.Application.Interfaces
{
    public interface IRentalService
    {
        Task CreateAsync(RentalCreateDto dto, CancellationToken cancellationToken);
        Task<RentalViewDto?> GetByIdAsync(string identifier, CancellationToken cancellationToken);
        Task InformReturnAsync(string identifier, ReturnDateDto dto, CancellationToken cancellationToken);
    }
}
