using Microsoft.EntityFrameworkCore;
using MotorDiniz.Domain.Entities;
using MotorDiniz.Domain.Interfaces.Repository;
using MotorDiniz.Infra.Data.Context;

namespace MotorDiniz.Infra.Data.Repositories
{
    public class DeliveryRiderRepository : IDeliveryRiderRepository
    {
        private readonly ApplicationDbContext _context;

        public DeliveryRiderRepository(ApplicationDbContext context) => _context = context;

        public Task AddAsync(DeliveryRider entity, CancellationToken cancellationToken)
        {
            return _context.DeliveryRiders.AddAsync(entity, cancellationToken).AsTask();
        }

        public async Task<bool> CnhExistsAsync(string cnhNumber, CancellationToken cancellationToken)
        {
            var cnhExists = (cnhNumber ?? string.Empty).Trim();
            return await _context.DeliveryRiders.AsNoTracking()
                .AnyAsync(x => x.CnhNumber == cnhExists, cancellationToken);
        }

        public async Task<bool> CnpjExistsAsync(string cnpj, CancellationToken cancellationToken)
        {
            var cnpjExists = new string((cnpj ?? string.Empty).Where(char.IsDigit).ToArray());
            return await _context.DeliveryRiders.AsNoTracking()
                .AnyAsync(x => x.Cnpj == cnpjExists, cancellationToken);
        }

        public async Task<DeliveryRider?> GetByIdentifierAsync(string identifier, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(identifier))
                return null;

            var id = identifier.Trim();

            return await _context.DeliveryRiders             
                .SingleOrDefaultAsync(r => r.Identifier == id, cancellationToken);
        }

        public Task UpdateAsync(DeliveryRider entity, CancellationToken cancellationToken)
        {
            _context.DeliveryRiders.Update(entity);
            return Task.CompletedTask;
        }
    }
}
