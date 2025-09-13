using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Minio;
using MotorDiniz.Application.CQRS.Motorcycles.Commands;
using MotorDiniz.Application.Interfaces;
using MotorDiniz.Application.Services;
using MotorDiniz.Domain.Interfaces.Repository;
using MotorDiniz.Infra.Data.Context;
using MotorDiniz.Infra.Data.Repositories;
using MotorDiniz.Infra.Storage;
using MotorDiniz.Producer.Interface;
using MotorDiniz.Producer.Producer;
using MotorDiniz.Producer.Service;

namespace MotorDiniz.Infra.IoC
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {

            //Application -Services
 
            services.AddMediatR(cfg => {
                cfg.RegisterServicesFromAssembly(typeof(CreateMotorcycleCommand).Assembly);
            });

            services.AddScoped<IMotorcycleService, MotorcycleService>();
            services.AddScoped<IDeliveryRiderService, DeliveryRiderService>();
            services.AddScoped<IRentalService, RentalService>();
            services.AddSingleton<IRentalPricingService, RentalPricingService>();


            // Infra - Data

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<IMotorcycleRepository, MotorcycleRepository>();
            services.AddScoped<IDeliveryRiderRepository, DeliveryRiderRepository>();
            services.AddScoped<IRentalRepository, RentalRepository>();


            services.AddScoped<IProducerService, ProducerService>();
            services.AddScoped<IMotorcycleProducer, MotorcycleProducer>();

            //Connection-DB

             //var connectionString = Environment.GetEnvironmentVariable("Connection_String");
           
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            if (string.IsNullOrWhiteSpace(connectionString))
                throw new InvalidOperationException("Connection string not found.");

            services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(connectionString,
            b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

            return services;
        }

        public static IServiceCollection AddRabbitMQServices(this IServiceCollection services)
        {
            var server = Environment.GetEnvironmentVariable("MassTransit_Server") ?? string.Empty;
            var user = Environment.GetEnvironmentVariable("MassTransit_User") ?? string.Empty;
            var password = Environment.GetEnvironmentVariable("MassTransit_Password") ?? string.Empty;

            services.AddMassTransit(x =>
            {
                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(server, "/", h =>
                    {
                        h.Username(user);
                        h.Password(password);
                    });

                    cfg.ConfigureEndpoints(context);
                });
            });

            return services;

        }

        public static IServiceCollection AddMinioServices(this IServiceCollection services, IConfiguration configuration)
        {
            var endpoint = configuration["Minio:Endpoint"] ?? "minio:9000";
            var accessKey = configuration["Minio:AccessKey"] ?? "admin";
            var secretKey = configuration["Minio:SecretKey"] ?? "admin12345";
            var useSSL = bool.TryParse(configuration["Minio:UseSSL"], out var ssl) && ssl;

            services.AddSingleton<IMinioClient>(_ =>
                new MinioClient()
                    .WithEndpoint(endpoint)
                    .WithCredentials(accessKey, secretKey)
                    .WithSSL(useSSL)
                    .Build());

            
            services.AddSingleton<IObjectStorage, MinioStorageService>();

            return services;
        }

    }
}
