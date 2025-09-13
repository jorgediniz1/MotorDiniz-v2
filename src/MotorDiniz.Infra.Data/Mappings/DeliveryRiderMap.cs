using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MotorDiniz.Domain.Entities;

namespace MotorDiniz.Infra.Data.Mappings
{
    public class DeliveryRiderMap : IEntityTypeConfiguration<DeliveryRider>
    {
        public void Configure(EntityTypeBuilder<DeliveryRider> builder)
        {
            builder.ToTable("delivery_riders");

            builder.HasKey(x => x.Id);
            builder.HasAlternateKey(x => x.Identifier); // unique business key

            builder.Property(x => x.Id).HasColumnName("id");
            builder.Property(x => x.Identifier)
                .HasColumnName("identifier")
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(x => x.Name)
                .HasColumnName("name")
                .HasMaxLength(120)
                .IsRequired();

            builder.Property(x => x.Cnpj)
                .HasColumnName("cnpj")
                .HasMaxLength(14)
                .IsRequired();

            builder.HasIndex(x => x.Cnpj).IsUnique();

            builder.Property(x => x.BirthDate)
                .HasColumnName("birth_date")
                .HasColumnType("timestamptz")
                .IsRequired();

            builder.Property(x => x.CnhNumber)
                .HasColumnName("cnh_number")
                .HasMaxLength(20)
                .IsRequired();

            builder.HasIndex(x => x.CnhNumber).IsUnique();

            builder.Property(x => x.CnhType)
                .HasColumnName("cnh_type")
                .HasConversion<string>()
                .HasMaxLength(3)
                .IsRequired();

            builder.Property(x => x.CnhImagePath)
                .HasColumnName("cnh_image_path")
                .HasMaxLength(500);

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
