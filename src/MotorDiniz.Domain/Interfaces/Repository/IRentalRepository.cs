using MotorDiniz.Domain.Entities;

namespace MotorDiniz.Domain.Interfaces.Repository
{
    public interface IRentalRepository
    {
        Task AddAsync(Rental entity, CancellationToken cancellationToken);
        Task<Rental?> GetByIdentifierAsync(string identifier, CancellationToken cancellationToken);
        Task<Rental?> GetByIdentifierWithIncludesAsync(string identifier, CancellationToken ct);
        Task UpdateAsync(Rental entity, CancellationToken cancellationToken);
    }
}
