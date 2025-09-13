using MotorDiniz.Domain.Entities;

namespace MotorDiniz.Domain.Interfaces.Repository
{
    public interface IDeliveryRiderRepository
    {
        Task<bool> CnpjExistsAsync(string cnpj, CancellationToken cancellationToken);
        Task<bool> CnhExistsAsync(string cnhNumber, CancellationToken cancellationToken);

        Task AddAsync(DeliveryRider entity, CancellationToken cancellationToken);
        Task<DeliveryRider?> GetByIdentifierAsync(string identifier, CancellationToken cancellationToken);
        Task UpdateAsync(DeliveryRider entity, CancellationToken cancellationToken);
    }
}
