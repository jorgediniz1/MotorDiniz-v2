using Microsoft.EntityFrameworkCore;
using MotorDiniz.Domain.Entities;
using MotorDiniz.Infra.Data.EntitieEvents;

namespace MotorDiniz.Infra.Data.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Motorcycle> Motorcycles { get; set; }
        public DbSet<DeliveryRider> DeliveryRiders { get; set; }
        public DbSet<Rental> Rentals { get; set; }

        public DbSet<MotorcycleEventRecord> MotorcycleEvents { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        }

    }
}
