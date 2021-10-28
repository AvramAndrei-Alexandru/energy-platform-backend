using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnergyDataPlatform.src.Application.Models
{
    public class SmartDeviceDashboardModel
    {
        public Guid? Id { get; set; }
        public string Description { get; set; }
        public string SensorDescription { get; set; }
        public string Address { get; set; }
        public decimal MaximumEnergyConsumption { get; set; }
        public decimal? AverageEnergyConsumption { get; set; }
        public string UserId { get; set; }
        public decimal TotalEnergyConsumption { get; set; }
    }
}
