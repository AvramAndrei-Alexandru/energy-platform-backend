using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EnergyDataPlatform.src.Data.Entities;

namespace EnergyDataPlatform.src.Data.Persistance
{
    public class DatabaseContext : IdentityDbContext<User>
    {
        public DatabaseContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<SmartDevice> SmartDevices { get; set; }
        public DbSet<SensorMeasurement> SensorMeasurements { get; set; }
    }
}
