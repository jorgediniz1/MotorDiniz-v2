using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using MotorDiniz.Infra.Data.EntitieEvents;

namespace MotorDiniz.Infra.Data.Mappings
{
    public sealed class MotorcycleEventRecordMap : IEntityTypeConfiguration<MotorcycleEventRecord>
    {
        public void Configure(EntityTypeBuilder<MotorcycleEventRecord> b)
        {
            b.ToTable("motorcycle_events");
            b.HasKey(x => x.Id);

            b.Property(x => x.EventType).IsRequired();
            b.Property(x => x.Identifier).IsRequired();
            b.Property(x => x.Model).IsRequired();
            b.Property(x => x.Plate).IsRequired();
            b.Property(x => x.Year).IsRequired();
            b.Property(x => x.ReceivedAt).IsRequired();

            b.Property(x => x.PayloadJson)
             .IsRequired()
             .HasColumnType("jsonb");

            b.HasIndex(x => new { x.EventType, x.Identifier });
            b.HasIndex(x => x.ReceivedAt);
        }
    }
}
