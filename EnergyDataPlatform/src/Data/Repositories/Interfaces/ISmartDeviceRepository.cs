using EnergyDataPlatform.src.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnergyDataPlatform.src.Data.Repositories.Interfaces
{
    public interface ISmartDeviceRepository
    {
        List<SmartDevice> GetAllDevicesForUser(string userId);
        SmartDevice GetDeviceById(Guid id);
        void AddDevice(SmartDevice smartDevice);
        void UpdateDevice(SmartDevice smartDevice);
        void RemoveDevice(SmartDevice smartDevice);
    }
}
