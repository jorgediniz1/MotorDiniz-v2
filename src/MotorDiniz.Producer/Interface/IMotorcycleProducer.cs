using MotorDiniz.Producer.Events;

namespace MotorDiniz.Producer.Interface
{
    public interface IMotorcycleProducer
    {
        Task PublishCreatedAsync(MotorcycleCreatedEvent message, CancellationToken cancellationToken);
    }
}
