using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Testcontainers.PostgreSql;
using Testcontainers.RabbitMq;

namespace MotorDiniz.API.Integration.Tests.Configurations
{
    public sealed class ContainersFixture : IAsyncLifetime
    {
        public PostgreSqlContainer Postgres { get; private set; } = default!;
        public RabbitMqContainer Rabbit { get; private set; } = default!;

        public string ConnectionString => Postgres.GetConnectionString();
        public string RabbitHost => "localhost"; 
        public int RabbitPort => 5672;          

        public Task InitializeAsync()
        {
            Postgres = new PostgreSqlBuilder()
                .WithImage("postgres:16")
                .WithUsername("postgres")
                .WithPassword("motor123")
                .WithDatabase("MotorDiniz_Test")
                .WithPortBinding(5432, 5432) 
                .Build();

            Rabbit = new RabbitMqBuilder()
                .WithImage("rabbitmq:3.13-management")
                .WithUsername("guest")
                .WithPassword("guest")
                .WithPortBinding(5672, 5672)   
                .WithPortBinding(15672, 15672) 
                .Build();

            return Task.WhenAll(Postgres.StartAsync(), Rabbit.StartAsync());
        }

        public Task DisposeAsync() =>
            Task.WhenAll(Postgres.DisposeAsync().AsTask(), Rabbit.DisposeAsync().AsTask());
    }
}
