using System.Text.Json;
using MotorDiniz.Consumer.Interfaces;
using MotorDiniz.Infra.Data.Context;
using MotorDiniz.Infra.Data.EntitieEvents;
using MotorDiniz.Producer.Events;

namespace MotorDiniz.Consumer.Services
{
    public sealed class MotorcycleEventStore : IMotorcycleEventStore
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<MotorcycleEventStore> _logger;

        public MotorcycleEventStore(ApplicationDbContext context, ILogger<MotorcycleEventStore> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task SaveMotorcycle2024Async(MotorcycleCreatedEvent evt, CancellationToken cancellationToken)
        {

            if (evt.Year != 2024)
            {
                _logger.LogInformation("Ignored motorcycle {Identfier} - year {Year} != 2024.", evt.Identifier, evt.Year);
                return;
            }

            var eventRecord = new MotorcycleEventRecord
            {
                EventType = MotorcycleCreatedEvent.EventType,
                Identifier = evt.Identifier,
                Year = evt.Year,
                Model = evt.Model,
                Plate = evt.Plate,
                ReceivedAt = DateTime.UtcNow,
                PayloadJson = JsonSerializer.Serialize(evt)
            };

            _context.MotorcycleEvents.Add(eventRecord);
            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Saved notification for motorcycle {Identifier} ({Plate}) - year 2024.", evt.Identifier, evt.Plate);

        }
    }
}
