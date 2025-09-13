namespace MotorDiniz.Producer.Interface
{
    public interface IProducerService
    {
        Task SendAsync<T>(T message, string queueName) where T : class;
    }
}
