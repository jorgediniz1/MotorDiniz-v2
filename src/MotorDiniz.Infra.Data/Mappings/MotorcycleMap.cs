using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MotorDiniz.Domain.Entities;

namespace MotorDiniz.Infra.Data.Mappings
{
    public class MotorcycleMap : IEntityTypeConfiguration<Motorcycle>
    {
        public void Configure(EntityTypeBuilder<Motorcycle> builder)
        {
            builder.ToTable("motorcycles");

            builder.HasKey(x => x.Id);
            builder.HasAlternateKey(x => x.Identifier);

            builder.Property(x => x.Id).HasColumnName("id");

            builder.Property(x => x.Identifier)
                .HasColumnName("identifier")
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(x => x.Year)
                .HasColumnName("year")
                .IsRequired();

            builder.Property(x => x.Model)
                .HasColumnName("model")
                .HasMaxLength(120)
                .IsRequired();

            builder.Property(x => x.Plate)
                .HasColumnName("plate")
                .HasMaxLength(12)
                .IsRequired();

            builder.HasIndex(x => x.Plate).IsUnique();

            builder.Property(x => x.CreatedAt)
                .HasColumnName("created_at")
                .HasColumnType("timestamptz")
                .HasDefaultValueSql("timezone('utc', now())");

            builder.Property(x => x.UpdatedAt)
                .HasColumnName("updated_at")
                .HasColumnType("timestamptz");
        }
    }
}
