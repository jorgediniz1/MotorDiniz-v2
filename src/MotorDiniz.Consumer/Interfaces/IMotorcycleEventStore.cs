using MotorDiniz.Producer.Events;

namespace MotorDiniz.Consumer.Interfaces
{
    public interface IMotorcycleEventStore
    {
        Task SaveMotorcycle2024Async(MotorcycleCreatedEvent evt, CancellationToken cancellationToken);
    }
}
