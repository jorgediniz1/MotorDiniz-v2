using Microsoft.EntityFrameworkCore;
using MotorDiniz.Domain.Entities;
using MotorDiniz.Domain.Interfaces.Repository;
using MotorDiniz.Infra.Data.Context;

namespace MotorDiniz.Infra.Data.Repositories
{
    public sealed class MotorcycleRepository : IMotorcycleRepository
    {

        private readonly ApplicationDbContext _context;

        public MotorcycleRepository(ApplicationDbContext context) => _context = context;

        public Task AddAsync(Motorcycle motorcycle, CancellationToken cancellationToken)
        {
            return _context.Motorcycles.AddAsync(motorcycle, cancellationToken).AsTask();
        }

        public async Task<Motorcycle?> GetByIdentifierAsync(string identifier, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(identifier))
                return null;

            var id = identifier.Trim();

            return await _context.Motorcycles
                .SingleOrDefaultAsync(r => r.Identifier == id, cancellationToken);
        }

        public Task<bool> HasRentalsAsync(int motorcycleId, CancellationToken cancellationToken)
        {
            return _context.Rentals.AsNoTracking()
                .AnyAsync(r => r.MotorcycleId == motorcycleId, cancellationToken);
        }

        public async Task<bool> PlateExistsAsync(string plate, CancellationToken cancellationToken)
        {
            var plateExists = (plate ?? string.Empty).Trim().ToUpperInvariant();
            return await _context.Motorcycles.AsNoTracking()
                .AnyAsync(m => m.Plate == plateExists, cancellationToken);
        }

        public Task RemoveAsync(Motorcycle motorcycle, CancellationToken cancellationToken)
        {
            _context.Motorcycles.Remove(motorcycle);
            return Task.CompletedTask;
        }

        public async Task<IReadOnlyList<Motorcycle>> SearchAsync(string? plate, CancellationToken cancellationToken)
        {
            var query = _context.Motorcycles.AsNoTracking().AsQueryable();
            if (!string.IsNullOrWhiteSpace(plate))
            {
                var p = plate.Trim().ToUpperInvariant();
                query = query.Where(m => m.Plate == p);
            }
            return await query.ToListAsync(cancellationToken);
        }

        public Task UpdateAsync(Motorcycle motorcycle, CancellationToken cancellationToken)
        {
            _context.Motorcycles.Update(motorcycle);
            return Task.CompletedTask;
        }
    }
}
