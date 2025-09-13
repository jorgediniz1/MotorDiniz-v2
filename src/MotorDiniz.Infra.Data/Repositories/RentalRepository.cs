using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MotorDiniz.Domain.Entities;
using MotorDiniz.Domain.Interfaces.Repository;
using MotorDiniz.Infra.Data.Context;

namespace MotorDiniz.Infra.Data.Repositories
{
    public class RentalRepository : IRentalRepository
    {
        private readonly ApplicationDbContext _context;

        public RentalRepository(ApplicationDbContext context) => _context = context;

        public Task AddAsync(Rental entity, CancellationToken cancellationToken)
        {
            return _context.Rentals.AddAsync(entity, cancellationToken).AsTask();
        }

        public async Task<Rental?> GetByIdentifierAsync(string identifier, CancellationToken cancellationToken)
        {
            var id = (identifier ?? string.Empty).Trim();
           
            return await _context.Rentals
                .FirstOrDefaultAsync(r => r.Identifier == id, cancellationToken);
        }

        public async Task<Rental?> GetByIdentifierWithIncludesAsync(string identifier, CancellationToken ct)
        {
            var id = (identifier ?? string.Empty).Trim();
            return await _context.Rentals
                .Include(r => r.DeliveryRider)
                .Include(r => r.Motorcycle)
                .AsNoTracking()               
                .FirstOrDefaultAsync(r => r.Identifier == id, ct);
        }

        public Task UpdateAsync(Rental entity, CancellationToken cancellationToken)
        {
            _context.Rentals.Update(entity);
            return Task.CompletedTask;
        }
    }
}
