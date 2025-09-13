using MotorDiniz.Domain.Entities;

namespace MotorDiniz.Domain.Interfaces.Repository
{
    public interface IMotorcycleRepository
    {
        Task<bool> PlateExistsAsync(string plate, CancellationToken cancellationToken);
        Task AddAsync(Motorcycle motorcycle, CancellationToken cancellationToken);
        Task<Motorcycle?> GetByIdentifierAsync(string identifier, CancellationToken cancellationToken);
        Task<IReadOnlyList<Motorcycle>> SearchAsync(string? plate, CancellationToken cancellationToken);
        Task RemoveAsync(Motorcycle motorcycle, CancellationToken cancellationToken);
        Task<bool> HasRentalsAsync(int motorcycleId, CancellationToken cancellationToken);
        Task UpdateAsync(Motorcycle motorcycle, CancellationToken cancellationToken);
    }
}
