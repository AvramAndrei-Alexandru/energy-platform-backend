using EnergyDataPlatform.src.Application.Models;
using EnergyDataPlatform.src.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnergyDataPlatform.src.Application.Mappers
{
    public static class SmartDeviceMapper
    {
        public static SmartDevice ToSmartDeviceEntity(SmartDeviceDashboardModel smartDevice)
        {
            if(smartDevice == null)
            {
                return null;
            }

            return new SmartDevice
            {
                Id = smartDevice.Id != null ? smartDevice.Id.Value : Guid.NewGuid(),
                Description = smartDevice.Description,
                SensorDescription = smartDevice.SensorDescription,
                Address = smartDevice.Address,
                MaximumEnergyConsumption = smartDevice.MaximumEnergyConsumption,
                UserId = smartDevice.UserId
            };
        }

        public static SmartDeviceDashboardModel ToSmartDeviceDashboardModel(SmartDevice smartDevice)
        {
            if(smartDevice == null)
            {
                return null;
            }

            return new SmartDeviceDashboardModel
            {
                Id = smartDevice.Id,
                Description = smartDevice.Description,
                SensorDescription = smartDevice.SensorDescription,
                Address = smartDevice.Address,
                MaximumEnergyConsumption = smartDevice.MaximumEnergyConsumption,
                AverageEnergyConsumption = smartDevice.AverageEnergyConsumption,
                UserId = smartDevice.UserId
            };
        }
    }
}