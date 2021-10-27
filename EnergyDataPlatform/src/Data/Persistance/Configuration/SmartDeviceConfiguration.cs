using EnergyDataPlatform.src.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnergyDataPlatform.src.Data.Persistance.Configuration
{
    public class SmartDeviceConfiguration : IEntityTypeConfiguration<SmartDevice>
    {
        public void Configure(EntityTypeBuilder<SmartDevice> builder)
        {
            builder.HasMany(e => e.SensorMeasurements).WithOne(e => e.SmartDevice).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
