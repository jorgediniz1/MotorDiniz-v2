using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MotorDiniz.Domain.Entities;

namespace MotorDiniz.Infra.Data.Mappings
{
    public sealed class RentalMap : IEntityTypeConfiguration<Rental>
    {
        public void Configure(EntityTypeBuilder<Rental> builder)
        {
            builder.ToTable("rentals");

            builder.HasKey(x => x.Id);
            builder.HasAlternateKey(x => x.Identifier);
            builder.Property(x => x.Identifier).HasMaxLength(120).IsRequired();

            builder.Property(x => x.DailyPrice).HasColumnType("numeric(12,2)").IsRequired();

            builder.Property(x => x.TotalAmount)
                   .HasColumnName("total_amount")
                   .HasColumnType("numeric(12,2)");

            builder.HasIndex(x => x.Identifier).IsUnique();
            builder.HasIndex(x => new { x.DeliveryRiderId, x.MotorcycleId });

            builder.HasOne(x => x.Motorcycle)
                .WithMany()
                .HasForeignKey(x => x.MotorcycleId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.DeliveryRider)
                .WithMany()
                .HasForeignKey(x => x.DeliveryRiderId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
