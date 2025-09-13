using MotorDiniz.Application.DTOs.Motorcycle;

namespace MotorDiniz.Application.Interfaces
{
    public interface IMotorcycleService
    {
        Task CreateAsync(MotorcycleCreateDto dto, CancellationToken cancellationToken);
        Task<IReadOnlyList<MotorcycleViewDto>> SearchAsync(string? plate, CancellationToken cancellationToken);
        Task<MotorcycleViewDto?> GetByIdAsync(string identifier, CancellationToken cancellationToken);
        Task UpdatePlateAsync(string identifier, UpdatePlateDto dto, CancellationToken cancellationToken);
        Task DeleteAsync(string identifier, CancellationToken cancellationToken);
    }
}
