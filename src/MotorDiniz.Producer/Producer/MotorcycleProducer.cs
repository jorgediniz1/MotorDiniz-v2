using MotorDiniz.Producer.Events;
using MotorDiniz.Producer.Interface;

namespace MotorDiniz.Producer.Producer
{
    public sealed class MotorcycleProducer : IMotorcycleProducer
    {

        public readonly IProducerService _producerService;
        private readonly string _queueName;

        public MotorcycleProducer(IProducerService producerService)
        {
            _producerService = producerService;
            _queueName = Environment.GetEnvironmentVariable("MassTransit_Queue_MotorcycleQueue") ?? string.Empty;
        }
        public async Task PublishCreatedAsync(MotorcycleCreatedEvent message, CancellationToken cancellationToken)
        {
            await _producerService.SendAsync(message, $"{_queueName}-{MotorcycleCreatedEvent.EventType}");
        }
    }
}
