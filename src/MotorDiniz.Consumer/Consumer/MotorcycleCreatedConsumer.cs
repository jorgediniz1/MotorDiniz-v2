using MassTransit;
using MotorDiniz.Consumer.Interfaces;
using MotorDiniz.Producer.Events;

namespace MotorDiniz.Consumer.Consumer
{
    public class MotorcycleCreatedConsumer : IConsumer<MotorcycleCreatedEvent>
    {
        private readonly IMotorcycleEventStore _store;
        private readonly ILogger<MotorcycleCreatedConsumer> _logger;

        public MotorcycleCreatedConsumer(IMotorcycleEventStore store, ILogger<MotorcycleCreatedConsumer> logger)
        {
            _store = store;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<MotorcycleCreatedEvent> context)
        {
            var message = context.Message;

            if (message.Year == 2024)
            {
                await _store.SaveMotorcycle2024Async(message, context.CancellationToken);
                _logger.LogInformation("Motorcycle 2024 event stored: {Identifier}", message.Identifier);
            }
            else
            {
                _logger.LogInformation("Motorcycle event ignored (year != 2024): {Identifier}", message.Identifier);
            }
        }
    }
}
