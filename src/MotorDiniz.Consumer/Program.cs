using MassTransit;
using Microsoft.EntityFrameworkCore;
using MotorDiniz.Consumer.Consumer;
using MotorDiniz.Consumer.Interfaces;
using MotorDiniz.Consumer.Services;
using MotorDiniz.Infra.Data.Context;
using MotorDiniz.Producer.Events;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((ctx, services) =>
    {
        var cs = Environment.GetEnvironmentVariable("Connection_String")
                 ?? ctx.Configuration.GetConnectionString("Postgres")
                 ?? "Host=postgres;Port=5432;Database=MotorDiniz;Username=postgres;Password=motor123";

        services.AddDbContext<ApplicationDbContext>(opt =>
            opt.UseNpgsql(cs, b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

        services.AddScoped<IMotorcycleEventStore, MotorcycleEventStore>();

        services.AddMassTransit(x =>
        {
            x.AddConsumer<MotorcycleCreatedConsumer>();

            x.UsingRabbitMq((context, cfg) =>
            {
                var server = Environment.GetEnvironmentVariable("MassTransit_Server") ?? string.Empty;
                var user = Environment.GetEnvironmentVariable("MassTransit_User") ?? string.Empty;
                var password = Environment.GetEnvironmentVariable("MassTransit_Password") ?? string.Empty;


                cfg.Host(server, "/", h =>
                {
                    h.Username(user);
                    h.Password(password);
                });

                var baseQueue = Environment.GetEnvironmentVariable("MassTransit_Queue_MotorcycleQueue") ?? "motorcycle-queue";
                var queueName = $"{baseQueue}-{MotorcycleCreatedEvent.EventType}";

                cfg.ReceiveEndpoint(queueName, e =>
                {
                    e.ConfigureConsumer<MotorcycleCreatedConsumer>(context);                   
                });
            });
        });
    })
    .Build();

await host.RunAsync();
