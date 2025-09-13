using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MassTransit;
using MotorDiniz.Producer.Interface;

namespace MotorDiniz.Producer.Service
{
    public sealed class ProducerService : IProducerService
    {
        private IBus _bus;

        public ProducerService(IBus bus)
        {
            _bus = bus;
        }
        public async Task SendAsync<T>(T message, string queueName) where T : class
        {
            var endpoint = await _bus.GetSendEndpoint(new Uri($"queue:{queueName}"));
            await endpoint.Send<T>(message);
        }
    }
}
