using EnergyDataPlatform.src.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnergyDataPlatform.src.Application.Services.Interfaces
{
    public interface ISmartDeviceService
    {
        List<SmartDeviceDashboardModel> GetAllDevicesForUser(string userId);
        void AddDevice(SmartDeviceDashboardModel smartDevice);
        void UpdateDevice(SmartDeviceDashboardModel smartDevice);
        void RemoveDevice(Guid id);
        List<SmartDeviceDashboardModel> GetAllSmartDevicesForCurrentUser(string userName);
    }
}