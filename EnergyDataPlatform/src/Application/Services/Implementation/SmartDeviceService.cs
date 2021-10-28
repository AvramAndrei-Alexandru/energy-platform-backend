using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EnergyDataPlatform.src.Application.Mappers;
using EnergyDataPlatform.src.Application.Models;
using EnergyDataPlatform.src.Application.Services.Interfaces;
using EnergyDataPlatform.src.Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace EnergyDataPlatform.src.Application.Services.Implementation
{
    public class SmartDeviceService : ISmartDeviceService
    {
        private readonly ISmartDeviceRepository _smartDeviceRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserRepository _userRepository;

        public SmartDeviceService(ISmartDeviceRepository smartDeviceRepository, IHttpContextAccessor httpContextAccessor, IUserRepository userRepository)
        {
            _smartDeviceRepository = smartDeviceRepository;
            _httpContextAccessor = httpContextAccessor;
            _userRepository = userRepository;
        }

        public void AddDevice(SmartDeviceDashboardModel smartDevice)
        {
            _smartDeviceRepository.AddDevice(SmartDeviceMapper.ToSmartDeviceEntity(smartDevice));
        }

        public List<SmartDeviceDashboardModel> GetAllDevicesForUser(string userId)
        {
            var deviceModels = _smartDeviceRepository.GetAllDevicesForUser(userId).Select(d => SmartDeviceMapper.ToSmartDeviceDashboardModel(d)).ToList();
            deviceModels.ForEach(d =>
            {
                d.TotalEnergyConsumption = _smartDeviceRepository.GetTotalEnergyConsumptionForDevice(d.Id.Value);
            });
            return deviceModels;
        }

        public List<SmartDeviceDashboardModel> GetAllSmartDevicesForCurrentUser(string userName)
        {
            var dbUser = _userRepository.GetUserByName(userName);
            var deviceModels = _smartDeviceRepository.GetAllDevicesForUser(dbUser.Id).Select(d => SmartDeviceMapper.ToSmartDeviceDashboardModel(d)).ToList();
            deviceModels.ForEach(d =>
            {
                d.TotalEnergyConsumption = _smartDeviceRepository.GetTotalEnergyConsumptionForDevice(d.Id.Value);
            });
            return deviceModels;
        }

        public void RemoveDevice(Guid id)
        {
            _smartDeviceRepository.RemoveDevice(_smartDeviceRepository.GetDeviceById(id));
        }

        public void UpdateDevice(SmartDeviceDashboardModel smartDevice)
        {
            _smartDeviceRepository.UpdateDevice(SmartDeviceMapper.ToSmartDeviceEntity(smartDevice));
        } 
    } 
}
